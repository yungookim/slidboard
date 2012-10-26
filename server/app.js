var net = require('net');
var PORT = 6060;

var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');

	var pixelSense = socket.remoteAddress;

	console.log('Accepting Connection from ' + socket.remoteAddress);
	socket.on('data', function(data){
		console.log(JSON.parse(data));


		socket.write("Accepted ");
		socket.pipe(socket);

		socket.end();
		
		setTimeout(function(){	
				var newSocket = new net.Socket();
				console.log('starting');
				newSocket.connect(6060, pixelSense, function(_socket){
					console.log("writing to socket");
					_socket.write("Hello PixelSense!\n");
					_socket.pipe(_socket);
				});
		}, 1000);


	});
	socket.on('end', function(){
		console.log('server disconnected');
	});
});

server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});

