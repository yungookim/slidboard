module.exports = {
	CLIENT_MOBILE : "MOBILE",
  CLIENT_PIXELSENSE : "PixelSense",
  pixelSense : null,
  mobiles : [],

	exec : function(data, socket){
		//check if data is JSON
		try {
			data = JSON.parse(data);
		}	catch (e){
			return 'Wrong Object Type.';
		}

		console.log(data);
		if (data.type === this.CLIENT_MOBILE){
			execMobile(data, socket);
		} else if (data.FROM === this.CLIENT_PIXELSENSE){
			this.execPixelSense(data, socket);
		}
	},

	execMobile : function(data, socket){
		var item = {
			socket : socket,
			uuid : data.uuid
		};
		this.mobiles.push(item);
	},

	execPixelSense : function(data, socket){
		if (data.msg === "END"){
			socket.write("END");
			this.pixelSense = null;
			console.log("Teminating connection with PixelSense");
			return;
		}

		data = JSON.parse(data.msg);
		//console.log(data);
		switch (data.ACTION){
			case 'INIT':
				this.pixelSense = socket;
				socket.write('ok');
				//socket.pipe(socket);
				break;
			default :
				console.log("Error");
				console.log(data);
				break;
		}
	}
}