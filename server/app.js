var net = require('net'),
    PORT = 6060,
    APICalls = require('./api'),
    indexer = require('./indexer'),
    fs = require('fs'),
    BufferedWriter = require('buffered-writer');



//Using HTTP
var express = require('express'),
    app     = express.createServer();

app.configure(function(){
 	app.use(express.bodyParser());
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
	
	indexer.index_dir(function(){
		indexer.index_files(function(){
			res.send('ok');
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





//Using TCP. DEPRECATED
var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');
	socket.setNoDelay(true);
	socket.setKeepAlive(true);
	var remoteAddress = socket.remoteAddress;
	var device = "";

	console.log('Accepting Connection from ' + socket.remoteAddress);
	
	socket.on('data', function(data){
		console.log("===================================================");
		console.log("Raw Message : " + data);
		//check if data is JSON
		try {
			var parsed_data = JSON.parse(data);
			device = parsed_data.from;
			console.log("Message parsed");
			console.log(parsed_data);
		} catch (e){
			console.log("Wrong data time : app.js.socket.on.data");
			console.log(e);
			console.log(data);
			socket.end('WDT');
			return;
		}
		
		APICalls.exec(parsed_data, function(ret){
			if (ret === "PS.init") {
				APICalls.pixelSense = socket;
			} else if (ret === "Device.init") {
				APICalls.mobiles.push(socket);
				socket.end('ok');
			}
		});
		console.log("===================================================");
	});
	socket.on('end', function(){
		console.log('Connection half closed with ' + device + "(" + remoteAddress + ")");
		socket.destroy();
	});
	socket.on('timeout', function(){
		socket.end();
		socket.destroy();
	});
	socket.on('error', function(e){
		console.log(e);
	});
	socket.on('close', function(){
		console.log('Connection fully closed with ' +
				device + "(" + remoteAddress + ")");
	});
});

server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});
