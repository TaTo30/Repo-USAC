const Friend = require('../models/friend.model')
const Conversation = require('../models/conversation.model')
const User = require('../models/user.model')

module.exports = {

    async friends(req, res) {

		var nickname = req.params.nickname;
		const user = await User.aggregate([
            { 
                $lookup: {
                    from: 'friends',
                    let: {
                        nk: "$nickname"
                    },
                    pipeline: [
                        {
                            $match: {
                                $and:[
                                    {$expr: {
                                        $eq: [
                                            "$send",
                                            nickname
                                        ]
                                    }},
                                   { $expr: {
                                        $eq: [
                                            "$reciver",
                                            "$$nk"
                                        ]
                                    }}
                                ]
                               
                            }
                        }
                    ],
                    as: 'send'
               }
            },
            { 
                $lookup: {
                    from: 'friends',
                    let: {
                        nk: "$nickname"
                    },
                    pipeline: [
                        {
                            $match: {
                                $and:[
                                    {$expr: {
                                        $eq: [
                                            "$reciver",
                                            nickname
                                        ]
                                    }},
                                   { $expr: {
                                        $eq: [
                                            "$send",
                                            "$$nk"
                                        ]
                                    }}
                                ]
                               
                            }
                        }
                    ],
                    as: 'reciver'
               }
            }, 
            {
                $match:{
                    $and:[
                        {"nickname": { $ne: nickname }},
                        {
                            $or:[
                                {"send":{ $size: 0 }},
                                {"send.status": 1},
                            ]
                        }
                    ]
                }
            },
            {
                $project: {
                    nickname:1,
                    image:{ $concat: [ process.env.BUCKET_URI,"$image" ] },
                    email:1,
                    name:1,
                    bot:1,
                    is_friend: { 
                        $cond: { 
                            if: { $eq: [ { $size: "$reciver"} , 0 ] }, //si no recibí una solicitud de este usuario
                            
                            then:{ 
                                $cond:{ 
                                    if: { $eq: [ { $size: "$send"} , 0 ] }, //si no envie una solicitud de este usuario
                                    then: -1,  
                                    else:1 //si envié una solicitud de este usuario y aceptó
                                }
                            },
                            
                            else:{ //si recibí una solicitud de este usuario
                                $first: "$reciver.status" 
                            }
                        } 
                    },
               }
           }
        ])
		res.status(200).json(user)
	},
    async friend(req, res) {

		var {nickname,friend} = req.body;
		const newfriend = new Friend({
            send:nickname,
            reciver:friend,
            status: 0
        });
        await newfriend.save();
           
		res.status(200).json({mensaje:"Solicitud enviada"})
	},

    async accept(req, res) {

		var {nickname,friend,accept} = req.body;

		if(accept === 1 ){
            await Friend.findOneAndUpdate({send:friend,reciver:nickname},{status: 1},{new:true});
            const conversation = new Conversation({
                members: [nickname,friend]
            });
            await conversation.save();
            return res.status(200).json({mensaje:"Solicitud aceptada"})
        }else {
            await Friend.findOneAndDelete({send:friend,reciver:nickname});
            return res.status(200).json({mensaje:"Solicitud eliminada"})
        }
           
	},


	
};
