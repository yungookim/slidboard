var net = require('net');

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');	
	console.log('server running');
	socket.on('data', function(data){
		console.log(data);
		socket.write("Accepted ");
		socket.pipe(socket);
		socket.end();
	});
	socket.on('end', function(){
		console.log('server disconnected');
	});
});

var PORT = 6060;
server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});
