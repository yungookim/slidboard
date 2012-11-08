var net = require('net'),
    PORT = 6060,
    APICalls = require('./api');

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');
	//socket.setNoDelay(true);
	socket.setKeepAlive(true);
	console.log('Accepting Connection from ' + socket.remoteAddress);

	//socket.write("Welcome!\n");
	
	socket.on('data', function(data){
		console.log(data + "\n");
		//check if data is JSON
		try {
			var parsed_data = JSON.parse(data);
			APICalls.exec(parsed_data, socket);
		} catch (e){
			console("Wrong data time : app.js.socket.on.data")
		}
		//console.log(data);		
	});
	socket.on('end', function(){
		console.log('server disconnected');
		socket.end();
		socket.destroy();
	});
	socket.on('timeout', function(){
		socket.end();
		socket.destroy();
	});
	socket.on('error', function(e){
		console.log(e);
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
