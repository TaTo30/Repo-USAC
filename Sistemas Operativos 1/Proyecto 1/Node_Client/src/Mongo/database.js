var MongoClient = require ('mongodb').MongoClient; 

const uri = 'mongodb://sopes1-g24-2021:kxeCcSywgmVVNUgN2vuDMPKwULZ01ZryPyJQm3R8SjfJeG2WB3pBd7BmwI8pA3nnd28No0gJIUOBLnK5JoNWdw==@sopes1-g24-2021.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@sopes1-g24-2021@'

const client = new MongoClient(uri);

module.exports = client
/*
async function run() {
  try {
    await client.connect();
    console.log('conected')
    const database = client.db("sopes1");
    const movies = database.collection("proyecto1");
    // Query for a movie that has the title 'The Room'
    const query = {};
    const options = {};
    const movie = await movies.find(query, options).toArray();
    // since this method returns the matched document, not a cursor, print it directly
    console.log(movie);
    console.log('finish')
  } finally {
    await client.close();
  }
}
run().catch(console.dir);
*/