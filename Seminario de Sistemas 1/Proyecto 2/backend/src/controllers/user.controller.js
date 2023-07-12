var crypto = require('crypto');
const User = require('../models/user.model')
const { v4: uuidv4 } = require('uuid')
const AmazonCognitoIdentity = require('amazon-cognito-identity-js');
const { keys_cognito, keys_s3 } = require('../configs/creds');
const aws = require('aws-sdk')
const s3 = new aws.S3(keys_s3)
const cognito = new AmazonCognitoIdentity.CognitoUserPool(keys_cognito);

module.exports = {

    async signup(req, res) {

		var { name, nickname, password, email, image } = req.body;

		const nombre = `perfil/${uuidv4().replace('-', '')}.${image.extension}`
        const bytes = new Buffer.from(image.data, 'base64')
        const uploadResult = await s3.upload({
            Bucket: process.env.BUCKET_S3,
            Key: nombre,
            Body: bytes,
            ACL: 'public-read'
        }).promise()
        const location = uploadResult.Location
		
		var attributelist = [];

		var dataname = {
			Name: 'name',
			Value: name,
		};

		var attributename = new AmazonCognitoIdentity.CognitoUserAttribute(dataname);

		attributelist.push(attributename);

		var datanickname = {
			Name: 'nickname',
			Value: nickname,
		};
		var attributenickname = new AmazonCognitoIdentity.CognitoUserAttribute(datanickname);
	  
		attributelist.push(attributenickname);

		var dataemail = {
			Name: 'email',
			Value: email,
		};
		var attributeemail = new AmazonCognitoIdentity.CognitoUserAttribute(dataemail);
	  
		attributelist.push(attributeemail);

		var dataimage = {
			Name: 'custom:imageProfile',
			Value: location,
		};
		var attributeimage = new AmazonCognitoIdentity.CognitoUserAttribute(dataimage);
	  
		attributelist.push(attributeimage);

		var datamode = {
			Name: 'custom:botMode',
			Value: '0',
		};

		var attributemode = new AmazonCognitoIdentity.CognitoUserAttribute(datamode);
	  
		attributelist.push(attributemode);

		var hash = crypto.createHash('sha256').update(password).digest('hex');
  		//console.log(attributelist);
		//console.log(nickname, hash+"D**")

		cognito.signUp(nickname, hash+"D**", attributelist, null, async (err, data) => {
      
			if (err) {
				console.log("Error: "+ err);

				return res.status(500).json({ message: err.message || err});
				
			}
			//console.log(data);
			const user = new User({
				name,
				nickname:nickname,
				image:nombre,
				password: hash,
				email,
				bot:0

			});
			await user.save();
			res.status(200).json({ message: name+' registrado'});
		});

	},
	async edit(req, res) {

		var { name, nickname, password, bot, image } = req.body;

		var hash = crypto.createHash('sha256').update(password).digest('hex');

		var user = await User.findOne({nickname});
		if (!user) return res.status(400).json({mensaje:"Usuario incorrecto"})
		if (user.password !== hash) return res.status(400).json({mensaje:"Constraseña incorrecta"})
		var attributelist = [];

		var dataname = {
			Name: 'name',
			Value: name,
		};

		var attributename = new AmazonCognitoIdentity.CognitoUserAttribute(dataname);

		attributelist.push(attributename);

		if (image){
			const nombre = `perfil/${uuidv4().replace('-', '')}.${image.extension}`
			const bytes = new Buffer.from(image.data, 'base64')
			const uploadResult = await s3.upload({
				Bucket: process.env.BUCKET_S3,
				Key: nombre,
				Body: bytes,
				ACL: 'public-read'
			}).promise()
			const location = uploadResult.Location
			
			var dataimage = {
				Name: 'custom:imageProfile',
				Value: location,
			};
			var attributeimage = new AmazonCognitoIdentity.CognitoUserAttribute(dataimage);
		
			attributelist.push(attributeimage);	
			
			await User.findOneAndUpdate({ nickname }, { image: nombre }, { new: true });
	
		}
		
		var datamode = {
			Name: 'custom:botMode',
			Value: bot+'',
		};

		var attributemode = new AmazonCognitoIdentity.CognitoUserAttribute(datamode);
	  
		attributelist.push(attributemode);

  		//console.log(attributelist);
		//console.log(nickname, hash+"D**")
		var authenticationData = {
			Username: nickname,
			Password: hash+"D**"
		};

		var authenticationDetails = new AmazonCognitoIdentity.AuthenticationDetails(
			authenticationData
		);

		var userData = {
			Username: nickname,
			Pool: cognito,
		};
		var cognitoUser = new AmazonCognitoIdentity.CognitoUser(userData);
		cognitoUser.setAuthenticationFlowType('USER_PASSWORD_AUTH');
	  
		cognitoUser.authenticateUser(authenticationDetails, {
			onSuccess: async function (result) {
				
				cognitoUser.updateAttributes(attributelist, async function(err, result) {
					if (err) {
						
						return res.status(500).json({ message: err.message || err});
						
					}
					await User.findOneAndUpdate({ nickname }, { name, bot }, { new: true });
					return res.status(200).json({ result });
				});
				
			},
			onFailure: function (err) {
				res.status(400).json(err);
			},
			mfaRequired: function (codeDeliveryDetails) {
				cognitoUser.sendMFACode(verificationCode, this);
			},
		});

	},
	async user(req, res) {

		const id = req.params.id;

		var user = await User.findOne({nickname:id});
		if (!user) return res.status(400).json({mensaje:"Usuario incorrecto"})
		user.image = process.env.BUCKET_URI + user.image
		res.status(200).json({usuario:user})

	},
};
