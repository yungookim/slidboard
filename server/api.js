var indexer = require('./indexer');


module.exports = {
	pixelSense : null,
	mobiles : [],
	CLIENT_MOBILE : "MOBILE",
	CLIENT_PIXELSENSE : 'PixelSense',

	exec : function(data, next){
		if (data.from === this.CLIENT_MOBILE){
			this.execMobile(data, next);
		} else if (data.from === this.CLIENT_PIXELSENSE){
			this.execPixelSense(data, next);
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

	execPixelSense : function(data, next){
	
		if (data.action === "end"){
			this.pixelSense = null;
			console.log("Teminating connection with PixelSense");
			return;
		}

		console.log(data.action);

		switch (data.action){
			case 'init':
				console.log("PixelSense Connected");
				next('ok');
				break;
			case 'getIndex':
				var query = JSON.parse(data.extraMsg);
				indexer.getIndex(query.requestingDevice, query.dir, function(indexList){
					console.log('Query Done');
					next(indexList);
				});
				break;
			default :
				console.log(JSON.parse(data.msg));
				break;
		}
	}
}

module.exports.parse = function(query){
	console.log("===================================================");
	console.log("Raw Message : " + query);
	//check if data is JSON
	try {
		var parsed_data = JSON.parse(query);
		device = parsed_data.from;
		console.log("Message parsed");
		console.log(parsed_data);
	} catch (e){
		console.log("Wrong data time : app.js.socket.on.data");
		console.log(e);
		console.log(data);
		socket.end('WDT');
		return;
	}
	return parsed_data;
}
