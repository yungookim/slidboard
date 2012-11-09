var net = require('net'),
    PORT = 6060,
    APICalls = require('./api');

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
			console("Wrong data time : app.js.socket.on.data")
		}
		APICalls.exec(parsed_data, socket);
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

//To recieve a larger chunk of data such as an entire index file
var express = require('express'),
    app     = express.createServer();

app.configure(function(){
 	app.use(express.bodyParser());
});

app.post('/fileIndex', function(req, res){
	console.log(req.body.msg);
	res.send('ok');
});

app.listen(8081);
console.log("HTTP server running on 8081");
