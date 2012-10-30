var net = require('net'),
    PORT = 6060,
    APICalls = require('./api');

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');
	console.log('Accepting Connection from ' + socket.remoteAddress);

	socket.on('data', function(data){
		APICalls.exec(data, socket);
	});
	socket.on('end', function(){
		console.log('server disconnected');
		socket.end();
	});
	socket.on('timeout', function(){
		socket.end();
	});
});

server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});

