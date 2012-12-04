//Index given file system listing using Mongodb.
var fs      = require('fs'),
    mongodb = require('mongodb'),
    server  = new mongodb.Server("127.0.0.1", 27017, 
						{'auto_reconnect': true}),
    db      = new mongodb.Db('slidboard', server, {safe:true});

var RAW_INDEX_DIR  = '/data/indexDir.json';
var RAW_INDEX_FILE = '/data/indexFile.json';
var RAW_INDEX_TEST = '/data/test.json';
 
module.exports = {

	index_dir : function(next) {
		var self = this;
		//Synchronous file read.
		var data = fs.readFileSync(RAW_INDEX_DIR);
		//have the raw index interatable
		data = data.toString().split('\n');
		var numbLines = data.length;
		var numbInsertions = 0;
		db.open(function (error, client) {
			if (error) throw error;
			console.log("Indexing " + numbLines + " items");
			for (i in data){
				if (data[i].length === 0){ numbLines--;  }
				else {
				var collection = new mongodb.Collection(client, 'index_dir');
				var indexEntry = JSON.parse(data[i]);
				var fullPath = indexEntry.fullPath;
				var dirName = indexEntry.name;
				var deviceId = indexEntry.device_uuid;
				var id = indexEntry.id;
				var type = indexEntry.type;

				//[0] == '', [1] == root, [2] == sdcard, and so on
				var dir_array = fullPath.split('/');
				dir_array.splice(dir_array.length-1, 1);
				var parent = dir_array.join('/');

				collection.findAndModify({ fullPath : fullPath },[], 
					{$set : 
						{
							deviceId : deviceId,
							id : id,
							name : dirName,
							fullPath : fullPath,
							type : type,
							parent : parent
						} 
					}, 
					{ upsert : true }, 
					function(err, ret){
						numbInsertions++;
						if (err) {console.log(err); return};
						//Make sure all lines are inserted 
						if (numbLines - numbInsertions < 1) {
							console.log("Indexed " + numbLines + " items.");
							db.close();
							next();
						}
						return;
					}
				);
				}
			}
		});
	},

	index_files : function(next){
		//Synchronous file read.
		var data = fs.readFileSync(RAW_INDEX_FILE);
		//have the raw index interatable
		data = data.toString().split('\n');
		var numbLines = data.length;
		var numbInsertions = 0;
		db.open(function (error, client) {
			if (error) throw error;
			console.log("Indexing " + numbLines + " items");
			for (i in data){
				if (data[i].length === 0){ numbLines--;  }
				else {
					try {	
						var collection = new mongodb.Collection(client, 'index_file');

						var raw = JSON.parse(data[i]);
						var fileName = raw.name;
						var deviceId = raw.device_uuid;
						var id = raw.id;
						var fullPath = raw.fullPath;
						var MD5 = raw.MD5;
						var size = raw.size;
						var type = raw.type;
						//[0] == '', [1] == root, [2] == sdcard, and so on
						var dir_array = fullPath.split('/');
						dir_array.splice(dir_array.length-1, 1);
						var parent = dir_array.join('/');

						var path_array = fullPath.split('/');
						collection.findAndModify({fullPath : fullPath}, [], 
						{$set : 
							{
								fileName : fileName,
								deviceId : deviceId,
								id : id,
								fullPath : fullPath,
								MD5 : MD5,
								size : size,
								type : type,
								parent : parent
							}
						}, 
						{ upsert : true },
						function(err, ret){
							numbInsertions++;
							if (err) {console.log(err); return;}
							//Make sure all lines are inserted 
							if (numbLines - numbInsertions < 1) {
								console.log("Indexed " + numbLines + " items.");
								db.close();
								next();
							}
							return;
						});
					} catch (err){
						//Files in other language sometimes causes problems.
						//Skip them
						numbInsertions++;
					}
				}
			}
		});
	},

	getIndex : function(deviceId, dir, next){
		db.open(function (error, client) {
			if (error) throw error;
					var collection = new mongodb.Collection(client, 'index_dir');				
					collection.find({ deviceId : deviceId, parent : dir }, {})
						.toArray(function(err, dirIndexes){
						if (err) {console.log(err); return;}
							collection = new mongodb.Collection(client, 'index_file');
							collection.find({ deviceId : deviceId, parent : dir }, {})
								.toArray(function(err, fileIndexes){
									//Join the two results
									var index = dirIndexes.concat(fileIndexes);
									db.close();
									console.log(index);
									next(index);
								});					
					});
		});
	},

	getFile : function(deviceId, path, next){
		db.open(function(err, client){
			if (err) throw err;
				var collection = new mongodb.Collection(client, 'index_file');

				collection.findOne({deviceId : deviceId, fullPath : path}, function(err, ret){
					if (err) throw err;
					db.close();
					next(ret);
				});
		});
	},

	removeIndex : function(){
		db.open(function(err, client){
			if (err) throw err;
			var collection = new mongodb.Collection(client, 'index_file');
			collection.drop(function(){
				collection = new mongodb.Collection(client, 'index_dir');				
				collection.drop(function(){
					db.close();
				});
				
			});
		});
	}
}
/*
module.exports.index_files(function(){
	module.exports.index_dir(function(){});
});*/
