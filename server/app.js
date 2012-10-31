var net = require('net'),
    PORT = 6060,
    APICalls = require('./api');

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');
	socket.setNoDelay(true);
	socket.setKeepAlive(true);
	console.log('Accepting Connection from ' + socket.remoteAddress);

	socket.write("hello");
	socket.write("hello2");
	socket.end();
	
	socket.on('data', function(data){
		//APICalls.exec(data, socket);
		console.log(data);		
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

