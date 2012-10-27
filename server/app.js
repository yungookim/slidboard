var net = require('net');
var PORT = 6060,
    CLIENT_MOBILE = "MOBILE",
    CLIENT_PIXELSENSE = "PS",
    pixelSense,
    mobiles = [];



var server = net.createServer(function (socket){
	socket.setEncoding('utf-8');
	console.log('Accepting Connection from ' + socket.remoteAddress);

	socket.on('data', function(data){
		data = JSON.parse(data);
		//console.log(JSON.parse(data));
		if (data.type === CLIENT_MOBILE){
			var item = {
				socket : socket,
				uuid : data.uuid
			};
			mobiles.push(item);
		} else if (data.type === CLIENT_PIXELSENSE){
			pixelSense = socket;
		}
		socket.write('Hello!\n');
		console.log(mobiles);
		socket.end();
	});
	socket.on('end', function(){
		console.log('server disconnected');
	});
});

server.listen(PORT, function(){
	console.log('Server Listening on '+ PORT);
});

