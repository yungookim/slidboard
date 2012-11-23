var indexer = require('./indexer'), 
    _ = require('underscore')._;



module.exports = {
	//Holds the main PixelSense
	pixelSense : null,
	//Holds smartphone devices
	mobiles : [],
	CLIENT_MOBILE : "MOBILE",
	CLIENT_PIXELSENSE : 'PixelSense',

	//Routes incoming request to appropriate handler
	exec : function(data, next){
		if (data.from === this.CLIENT_MOBILE){
			this.execMobile(data, next);
		} else if (data.from === this.CLIENT_PIXELSENSE){
			this.execPixelSense(data, next);
		}
	},

	//Hanlder for incoming mobile requests
	execMobile : function(data, next){
	},

	//Handler for incoming PixelSense handler
	execPixelSense : function(data, next){
	
		if (data.action === "end"){
			this.pixelSense = null;
			console.log("Teminating connection with PixelSense");
			return;
		}

		switch (data.action){
			case 'init':
				console.log("PixelSense Connected");
				next('ok');
				break;
			case 'getIndex':
				var query = JSON.parse(data.extraMsg);
				indexer.getIndex(query.requestingDevice, query.path, function(indexList){
					console.log('Query Done');
					//Serialize the returned list into a format that can be easily parsed
					//in PixelSense
					var list = "";
					for (i in indexList){
						list += JSON.stringify(indexList[i]) + "\n";
					}
					next(list);
				});
				break;
			case 'getFile':
				var query = JSON.parse(data.extraMsg);
				indexer.getFile(query.requestingDevice, query.path, function(ret){
					console.log('Query Done');
					next(ret);
				});
			default :
				console.log(JSON.parse(data.extraMsg));
				break;
		}
	}
}

module.exports.parse = function(query){
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
		throw e;
	}
	return parsed_data;
}
