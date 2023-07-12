var app = require('./app');
var mongoose = require('mongoose');
var config = require('config');

mongoose.Promise = global.Promise;
const PORT = process.env.PORT || config.get('app.port');



mongoose.connect(config.get('db.StringConnection'), {
    useNewUrlParser: true
  })
  .then(() => {
    app.listen(PORT,  () => {
      console.log('Listening on port:  ' + PORT);
    });

  }).catch(err => console.log(err));