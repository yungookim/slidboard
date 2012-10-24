var net = require('net');

var server = net.createServer(function (socket){
	console.log('server running');
	socket.on('end', function(){
		console.log('server disconnected');
	});
	socket.write('hello\n');
	socket.pipe(socket);
});

var PORT = 6060;
server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});
