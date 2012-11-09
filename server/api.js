var indexer = require('./indexer');

module.exports = {
  CLIENT_MOBILE : "MOBILE",
  CLIENT_PIXELSENSE : "PixelSense",
  pixelSense : null,
  mobiles : [],

	exec : function(data, socket){
	
		if (data.from === this.CLIENT_MOBILE){
			this.execMobile(data, socket);
		} else if (data.from == this.CLIENT_PIXELSENSE){
			this.execPixelSense(data, socket);
		}
	},

	execMobile : function(data, socket){
		
		switch (data.action)
		{
			case "INIT": 
				var item = {
					socket : socket,
					uuid : data.uuid
				};
				this.mobiles.push(item);
				break;
			case "INDEX":
				console.log(data.name);
				break;
		}
//		socket.end();
	},

	execPixelSense : function(data, socket){
		if (data.action === "end"){
			socket.write("end");
			this.pixelSense = null;
			console.log("Teminating connection with PixelSense");
			socket.end();
			socket.destroy();
			return;
		}

		switch (data.action){
			case 'init':
				this.pixelSense = socket;
				console.log("PixelSense Connected");
				socket.end('ok');
				break;
			case 'getIndex':
				var query = JSON.parse(data.extraMsg);
				indexer.getIndex(query.requestingDevice, query.dir, function(indexList){
					console.log(indexList);
					console.log('Sending response');
				
					socket.end(indexList);
				});
				break;
			default :
				console.log(JSON.parse(data.msg));
				
				break;
		}
	}
}
