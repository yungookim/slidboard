//Index given file system listing using Mongodb.
var fs = require('fs'),
		_ = require('underscore')._,
		 mongodb = require('mongodb'),
    server = new mongodb.Server("127.0.0.1", 27017, 
						{ 'auto_reconnect': true, 'poolSize': 2 }
						),
    db = new mongodb.Db('slidboard', server, {safe:true});

var RAW_INDEX_DIR = '/data/indexDir.json';
var RAW_INDEX_FILE = '/data/indexFile.json';
var RAW_INDEX_TEST = '/data/test.json';
 
module.exports.indexer = {
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
				//[0] == '', [1] == root, [2] == sdcard, and so on
				var dir_array = fullPath.split('/');
				collection.findAndModify({ id : id },[], 
					{$set : 
						{
							deviceId : deviceId,
							id : id,
							name : dirName,
							fullPath : fullPath,
							parent : dir_array[dir_array.length-2]
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
						}
						return;
					}
				);
				}
			}
		});
	},

	index_files : function(){

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
					var collection = new mongodb.Collection(client, 'index_file');

					var raw = JSON.parse(data[i]);
					var fileName = raw.name;
					var deviceId = raw.device_uuid;
					var id = raw.id;
					var fullPath = raw.fullPath;
					var MD5 = raw.MD5;
					var size = raw.size;
					try {	
						var path_array = fullPath.split('/');
						collection.findAndModify({id : id}, [], 
						{$set : 
							{
								fileName : fileName,
								deviceId : deviceId,
								id : id,
								fullPath : fullPath,
								MD5 : MD5,
								size : size,
								parent : path_array[path_array.length-2]
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
							}
							return;
						}
					);
					} catch (err){
						//Files in other language sometimes causes problems.
						//Skip them
						numbInsertions++;
					}
				}
			}
		});
	}

}

module.exports.indexer.index_files();
//module.exports.indexer.index_dir();
