const pool = require('../MySql/connection');
const client = require('../Mongo/database')
const moment = require('moment');

let sockets;
let goTime=0;
let pyTime=0;
let ruTime=0;

exports.getTiempos = () => {
    return {"go": goTime, "python": pyTime, "rust": ruTime}
}

exports.sockets = async (socket) => {

    sockets=socket;

    socket.on('getReports', async (msg)=> {
        goTime=goTime+msg.data.go.tiempo;
        pyTime=pyTime+msg.data.python.tiempo;
        ruTime=ruTime+msg.data.rust.tiempo;

        if(msg.data.db.rep==='cosmo'){
            let datos = JSON.stringify(await getReportsMongo(msg.fechaI, msg.fechaF))
            socket.emit('AddReports', {"data": datos})
        }else if(msg.data.db.rep==='sql'){
            let datos = JSON.stringify(await getReportsMySql(msg.fechaI, msg.fechaF))
            socket.emit('AddReports', {"data": datos})
        }
    })

    socket.on('getTwits', async (msg)=> {
        if(msg.db.db==='cosmo'){
            let movie = []
            try {
                await client.connect();
                console.log('conected')
                const database = client.db("sopes1");
                const movies = database.collection("proyecto1");
                // Query for a movie that has the title 'The Room'
                const query = {};
                const options = {};

                movie = await movies.find(query, options).sort({$natural:-1}).limit((msg.rust.cosmo+msg.python.cosmo+msg.go.cosmo)).toArray();
                
                // since this method returns the matched document, not a cursor, print it directly
                console.log('finish')
            } finally {
                await client.close();
                socket.emit('AddTwit', {"data": JSON.stringify(movie), "long": movie.length})
            }
            
        }else if(msg.db.db==='sql'){
            let result = await pool.query('(SELECT t.*, REPLACE(GROUP_CONCAT(distinct h.hashtag), \',\', \' #\') hashtags FROM hash_twit ht INNER JOIN twit t ON t.id=ht.idTwit INNER JOIN hashtags h ON h.id=ht.idHash WHERE t.api=\'rust\' GROUP BY t.id ORDER BY t.id DESC LIMIT ?)\n'
            +'UNION\n'
            +'(SELECT t.*, REPLACE(GROUP_CONCAT(distinct h.hashtag), \',\', \' #\') hashtags FROM hash_twit ht INNER JOIN twit t ON t.id=ht.idTwit INNER JOIN hashtags h ON h.id=ht.idHash WHERE t.api=\'python\' GROUP BY t.id ORDER BY t.id DESC LIMIT ?)\n'
            +'UNION\n'
            +'(SELECT t.*, REPLACE(GROUP_CONCAT(distinct h.hashtag), \',\', \' #\') hashtags FROM hash_twit ht INNER JOIN twit t ON t.id=ht.idTwit INNER JOIN hashtags h ON h.id=ht.idHash WHERE t.api=\'go\' GROUP BY t.id ORDER BY t.id DESC LIMIT ?)\n', [msg.rust.sql, msg.python.sql, msg.go.sql]);

            let results = [];
            results = result.map((data) => {

                let array = {
                    "id": data["id"],
                    "nombre": data["nombre"],
                    "comentario": data["comentario"],
                    "fecha": data["fecha"],
                    "upvotes": data["upvotes"],
                    "downvotes": data["downvotes"],
                    "hashtags": "#"+data["hashtags"]
                }
                return array
            })

            if (results.length != 0){ 
                socket.emit('AddTwit', {"data": JSON.stringify(results), "long": results.length})
            }
        }
    })
}

async function getReportsMySql(fechaI, fechaF){
    let reporte = {
        "twits": 0,
        "hashtags": 0,
        "upvotes": 0,
        "tophash": [],
        "tophashData": [],
        "days": [],
        "upvotesByDay": [],
        "downvotesByDay": [],
        "last100entrys": [],
        "times": [goTime, pyTime, ruTime]
    }

    let noticias = await pool.query('SELECT COUNT(id) total FROM twit');
     
    reporte.twits = noticias[0]["total"]

    let hashtags = await pool.query('SELECT COUNT(id) total FROM hashtags');

    reporte.hashtags = hashtags[0]["total"]

    let upvotes = await pool.query('SELECT SUM(upvotes) total FROM twit');

    reporte.upvotes = upvotes[0]["total"]

    let top5Hash = await pool.query('SELECT SUM(t.upvotes) total, h.hashtag FROM hash_twit ht INNER JOIN twit t ON t.id=ht.idTwit INNER JOIN hashtags h ON h.id=ht.idHash GROUP BY idHash ORDER BY total DESC LIMIT 5');
        
    let resultsTopH = [];
    let dataTopH = [];

    top5Hash.map((data) => {     
        resultsTopH.push(data["hashtag"])
        dataTopH.push(data["total"])
    })   

    reporte.tophash = (resultsTopH)
    reporte.tophashData = (dataTopH)

    let lastEntries = await pool.query('SELECT t.id, t.nombre, t.comentario, t.fecha, t.upvotes, t.downvotes, REPLACE(GROUP_CONCAT(distinct h.hashtag), \',\', \' #\') hashtags FROM hash_twit ht INNER JOIN twit t ON t.id=ht.idTwit INNER JOIN hashtags h ON h.id=ht.idHash GROUP BY t.id LIMIT 100');
        
    let resultsEntries = [];
    resultsEntries = lastEntries.map((data) => { 
        
        let array = [
            data["nombre"],
            data["fecha"],
            data["comentario"],            
            data["upvotes"],
            data["downvotes"],
            "#"+data["hashtags"]
        ]
        return array
    }) 

    reporte.last100entrys = (resultsEntries)

    var actual = moment(new Date(fechaI)).format('YYYY-MM-DD')
    var llegar = moment(new Date(fechaF)).format('YYYY-MM-DD')

    if(!moment(new Date(fechaI)).isValid()) actual = moment(new Date()).format('YYYY-MM-DD')
    if(!moment(new Date(fechaF)).isValid()) llegar = moment(new Date(actual)).add(7, 'days').format('YYYY-MM-DD')
    
    let votesByDate = await pool.query('SELECT SUM(upvotes) upvotes, SUM(downvotes) downvotes, fecha FROM twit WHERE fecha BETWEEN ? AND ? GROUP BY fecha', [actual, llegar]);
        
    let labelsVotes = [];
    let numUpvotes = [];
    let numDownVotes = [];

    votesByDate.map((data) => {        
        labelsVotes.push(moment(new Date(data["fecha"])).format('YYYY-MM-DD'))
        numUpvotes.push(data["upvotes"])
        numDownVotes.push(data["downvotes"]*-1)
    }) 

    reporte.days = labelsVotes
    reporte.upvotesByDay = numUpvotes
    reporte.downvotesByDay = numDownVotes


    return reporte
}

async function getReportsMongo(fechaI, fechaF){
    let reporte = {
        "twits": 0,
        "hashtags": 0,
        "upvotes": 0,
        "tophash": [],
        "tophashData": [],
        "days": [],
        "upvotesByDay": [],
        "downvotesByDay": [],
        "last100entrys": [],
        "times": [goTime, pyTime, ruTime]
    }

    try {
        await client.connect();
        console.log('conected')
        const database = client.db("sopes1");
        const collection = database.collection("proyecto1");

        // Query for a movie that has the title 'The Room'
        const queryTweets = {};
        const optionsTweets = {};
        let totalTweets = await collection.find(queryTweets, optionsTweets).count();
        console.log(totalTweets)

        //Total De Hashtags
        const queryHash = {};
        const optionsHash = {};
        let hashtags = await collection.find(queryHash, optionsHash).toArray();        

        var diasReporte = function(inicio, fin){
            var actual = moment(new Date(inicio))
            var llegar = moment(new Date(fin))

            if(!actual.isValid()) actual = moment(new Date())
            if(!llegar.isValid()) llegar = moment(new Date(actual)).add(7, 'days')

            var fechas = []
            
            while(actual.isSameOrBefore(llegar)){
                fechas.push(actual.format('DD/MM/YYYY'))
                actual.add(1, 'days')
            }
            return fechas
        }

        var dias = diasReporte(fechaI, fechaF)

        var valores = new Array(dias.length)
        var valDown = new Array(dias.length)
        valores.fill(0)
        valDown.fill(0)

        let fechasConsulta = []

        dias.forEach(fecha0=>{
            fechasConsulta.push({fecha: fecha0})
        })

        const queryDate = {$or: fechasConsulta};
        const optionDate = {}
        let sig = false
        let byDay = await collection.find(queryDate, optionDate).toArray().then(sig=true);

        byDay.forEach(day => {
            var index = dias.indexOf(day.fecha)
            valores[index] = valores[index]+day.upvotes
            valDown[index] = valores[index]+day.downvotes
        })         
        
        //Ultimas 100
        let ultimas100 = []

        hashtags.forEach(hash => {
            if(ultimas100.length!=100){
                ultimas100.push([hash.nombre, hash.fecha, hash.comentario, hash.upvotes, hash.downvotes, "#"+hash.hashtags.join(" #")])
            }
        })

        //Consultas
        let hashtagsArray = []
        let votes = []
        
        let upvotesTotal = 0
        let downvotesTotal = 0

        hashtags.forEach(element => {
            let array = element.hashtags
            upvotesTotal = upvotesTotal + element.upvotes
            downvotesTotal = downvotesTotal + element.downvotes

            array.forEach(hash => {
                if(!hashtagsArray.includes(hash)){
                    hashtagsArray.push(hash)

                    var myHash = {"hash": hash, "votes": element.upvotes} 
                    votes.push(myHash)                 
                }else{
                    var pos = hashtagsArray.indexOf(hash)
                    votes[pos].votes=votes[pos].votes+element.upvotes
                }
            })
        });

        votes.sort(function(a, b) {
            return b.votes - a.votes;
        })

        let top5Hash = []
        let topHashCant = []

        for(var i=0; i<5; i++){

            if(top5Hash.length==5 || i>=votes.length) break

            let element = votes[i]

            if(element!==undefined && element!==null){
                top5Hash.push(element.hash)
                topHashCant.push(element.votes)
            }                  
        }        

        reporte = {
            "twits": totalTweets,
            "hashtags": hashtagsArray.length,
            "upvotes": upvotesTotal,
            "tophash": top5Hash,
            "tophashData": topHashCant,
            "days": dias,
            "upvotesByDay": valores,
            "downvotesByDay": valDown,
            "last100entrys": ultimas100,
            "times": [goTime, pyTime, ruTime]
        }
        
        // since this method returns the matched document, not a cursor, print it directly
        console.log('finish')
    }catch(err){
        console.log(err)
    }finally {             
        return reporte
    }
}

exports.getSocket = () => {
    return sockets
}