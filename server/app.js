var net = require('net'),
    PORT = 6060,
    APICalls = require('./api'),
    indexer = require('./indexer'),
    fs = require('fs');

//Using HTTP
var express = require('express'),
    app     = express.createServer();

app.configure(function(){
 	app.use(express.bodyParser());
});

//Test connection between a mobile device to the server
app.post('/init', function(req, res){
	console.log("/init==============================================");
	try {
		APICalls.parse(req.body.msg);
	} catch (e) {
		res.send(e);
	}
	res.send('ok');
	console.log("===================================================");
});

app.post('/saveFile', function(req, res){
	var decodedFile = new Buffer(req.body.msg, 'base64');
	fs.writeFile('./test.json', decodedFile, 'binary', function(err){
		if (err){
			console.log(err);
		}
	});

	res.send('ok');
});

app.post('/wait', function(req, res){
	console.log("/wait==============================================");
	try {
		var query = APICalls.parse(req.body.msg);
	} catch (e){
		res.send(e);
	}
	
	APICalls.mobiles.push({ deviceId : query.deviceId, response : res });

	setTimeout(function(){
		res.send('stillWaiting');
	}, 30000);

	console.log("===================================================");
});

app.post('/fileIndex', function(req, res){
	var data = req.body.msg.split('\n');
	//Stupid and slow implementation.
	for (i in data){
		try{
			var temp  = JSON.parse(data[i]);
			//Bit slower but, provides more cleaner output.
			var line = JSON.stringify(temp) + "\n";
			if (temp.type === "FILE"){		
				fs.appendFileSync('/data/indexFile.json', line, 'utf8', function (err) {
					if (err) throw err;
				});
			} else if (temp.type === "DIR") {
				fs.appendFileSync('/data/indexDir.json', line, 'utf8', function (err) {
					if (err) throw err;
				});
			}
		} catch (e){
			//Let it be
		}
	}
	res.send('ok');
	indexer.index_dir(function(){
		indexer.index_files(function(){
		})
	});
});

app.get('/init', function(req, res){
	console.log("===================================================");
	var query = APICalls.parse(req.query.msg);
	APICalls.exec(query, function(ret){
		res.send(ret);
	});
	console.log("===================================================");
});

app.get('/getIndex', function(req, res){
	console.log("===================================================");
	var query = APICalls.parse(req.query.msg);
	APICalls.exec(query, function(ret){
		res.send(ret);
		console.log("Query Sent");
		//console.log(ret);
	});
	console.log("===================================================");
});

app.get('/getFile', function(req, res){
	console.log("===================================================");
	var query = APICalls.parse(req.query.msg);
	APICalls.exec(query, function(ret){
		console.log(ret);
		res.send(ret);
		console.log("Query Sent");
	});
	console.log("===================================================");
});

app.listen(8081);
console.log("HTTP server running on 8081");
