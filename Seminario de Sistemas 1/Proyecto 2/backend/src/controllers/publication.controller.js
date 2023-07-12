const Friend = require('../models/friend.model')
const Publication = require('../models/publication.model')
const { keys_rek, keys_s3, keys_translate } = require('../configs/creds');
const aws = require('aws-sdk');
const s3 = new aws.S3(keys_s3)
const { v4: uuidv4 } = require('uuid')
const rek = new aws.Rekognition(keys_rek);
const translate = new aws.Translate(keys_translate);

module.exports = {

    async publications(req, res) {

		var nickname = req.params.nickname;
		let friend = await Friend.aggregate([
            {
                $match:{
                    $and:[
                        {
                            $or :[
                                {"send": { $eq: nickname }},
                                {"reciver": { $eq: nickname }}
                            ]
                        },
                        {"status": { $eq: 1 }}
                    ]   
                }
            },
            {
                $project: {    
                    friend: { $cond: { if: { $eq: [ "$send" , nickname ] }, then: "$reciver", else: "$send" } },
                }
            }, 
            { 
                $lookup: {
                    from: 'publications',
                    localField: 'friend',
                    foreignField: 'owner',
                    as: 'publications'
                }
            },
            {
                $unwind: "$publications"
            }            
        ])

        friend = friend.map(value => {
            return {
                _id: value.publications._id,
                owner: value.friend,
                text: value.publications.text,
                image: `${process.env.BUCKET_URI}${value.publications.image}`,
                createdAt: value.publications.createdAt,
            }
        })

        const publications = await Publication.aggregate([
            {
                $match:{
                    owner:nickname
                }
            },
            {
                $project:{
                    id:1,
                    owner:1,
                    text:1,
                    image:{ $concat: [ process.env.BUCKET_URI,"$image" ] },
                    createdAt:1
                }
            }
        ]);

        var publicaciones = [...friend,...publications];

        publicaciones.sort((a, b) => {if (a.createdAt < b.createdAt) return 1;
        else if (a.createdAt > b.createdAt) return -1;
        return 0;});
   
        for (let index = 0; index < publicaciones.length; index++) {
            let p = publicaciones[index];
            
            var params = {
                Image: {
                    S3Object: {
                        Bucket: process.env.BUCKET_S3, 
                        Name: p.image.split(".com/").pop()
                    
                    }
                }, 
                MaxLabels: 123, 
                MinConfidence: 70
            };
            //console.log(params)

            p.labels = (await rek.detectLabels(params).promise()).Labels
        }
        return res.status(200).json(publicaciones)
    },

    async create(req, res){
        const { nickname, text, image } = req.body

        const nombre = `publicacion/${uuidv4().replace('-', '')}.${image.extension}`
        const bytes = new Buffer.from(image.data, 'base64')
        await s3.upload({
            Bucket: process.env.BUCKET_S3,
            Key: nombre,
            Body: bytes,
            ACL: 'public-read'
        }).promise()

        const publication = new Publication({
            owner:nickname,
            image:nombre,
            text: text
        });
        
        await publication.save();

        res.status(200).json({ message: 'Publicacion guardada'});
		

    },

    async translate(req,res){

        let { text } = req.body
      
        let params = {
            SourceLanguageCode: 'auto',
            TargetLanguageCode: 'es',
            Text: text
        };
        
        translate.translateText(params, function (err, data) {
            if (err) {
                //console.log(err, err.stack);
                res.status(500).json({ error: err })
            } else {
                //console.log(data);
                res.status(200).json({ message: data })
            }
        });
    }
	
};
