var net = require('net');
var PORT = 6060;

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');	
	console.log('Accepting Connection');
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

server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});
