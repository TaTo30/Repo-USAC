var crypto = require('crypto');
const User = require('../models/user.model')
const AmazonCognitoIdentity = require('amazon-cognito-identity-js');
const { keys_cognito, keys_rek } = require('../configs/creds');
const aws = require('aws-sdk')
const rek = new aws.Rekognition(keys_rek);
const cognito = new AmazonCognitoIdentity.CognitoUserPool(keys_cognito);

module.exports = {

	async login(req, res) {

		var { nickname, password } = req.body;
		
		var hash = crypto.createHash('sha256').update(password).digest('hex');
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
			onSuccess: function (result) {
				res.status(200).json({login:true,result:result.idToken.payload}); //
			},
			onFailure: function (err) {
				// User authentication was not successful
				res.status(400).json(err);
			},
			mfaRequired: function (codeDeliveryDetails) {
				// MFA is required to complete user authentication.
				// Get the code from user and call
				cognitoUser.sendMFACode(verificationCode, this);
			},
		});
	},

	async recognition(req, res) {

		var { nickname, image } = req.body;
		const user = await User.findOne({nickname});
		if (!user) return res.status(400).json({message:"Usuario no encontrado"});

		var params = {
			SimilarityThreshold: 90, 
			SourceImage: {
				S3Object: {
					Bucket: process.env.BUCKET_S3, 
					Name: user.image
				}
			}, 
			TargetImage: {
				Bytes: Buffer.from(image.data, 'base64')
			}
		  };


		rek.compareFaces(params, function(err, data) {
			if (err) {res.json({mensaje: err})} 
			else {   
				if (data.FaceMatches.length===0)return res.status(400).json({login:false,mensaje:"No se reconoció la cara"});
				
				var authenticationData = {
					Username: user.nickname,
					Password: user.password+"D**"
				};
		
				var authenticationDetails = new AmazonCognitoIdentity.AuthenticationDetails(
					authenticationData
				);
		
				var userData = {
					Username: user.nickname,
					Pool: cognito,
				};
				var cognitoUser = new AmazonCognitoIdentity.CognitoUser(userData);
				cognitoUser.setAuthenticationFlowType('USER_PASSWORD_AUTH');
			  
				cognitoUser.authenticateUser(authenticationDetails, {
					onSuccess: function (result) {
						res.status(200).json({login:true,result:result.idToken.payload}); //
					},
					onFailure: function (err) {
						// User authentication was not successful
						res.status(400).json({login:false,mensaje:err});
					},
					mfaRequired: function (codeDeliveryDetails) {
						// MFA is required to complete user authentication.
						// Get the code from user and call
						cognitoUser.sendMFACode(verificationCode, this);
					},
				});	
				
				      
			}
		});
		
	},
	
};
