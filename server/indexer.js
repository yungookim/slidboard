//Index given file system listing using Mongodb.
var mongodb = require('mongodb'),
    server = new mongodb.Server("127.0.0.1", 27017, 
						{ 'auto_reconnect': true, 'poolSize': 5 }
						),
    db = new mongodb.Db('slidboard', server, {safe:true}),
    fs = require('fs'),
    byline = require('byline'),
		_ = require('underscore')._;

var RAW_INDEX_DIR = '/data/indexDir.json';
var RAW_INDEX_FILE = '/data/indexFile.json';
var RAW_INDEX_TEST = '/data/test.json';
 
module.exports.indexer = {
	line_count : 0,
	index_dir : function(next) {
		var self = this;
		require('fs')
			.readFileSync(RAW_INDEX_DIR)
			.toString()
			.split('\n')
			.forEach(function (line) { 
				self.saveDirIndex(line);
			});
		
	},
	index_files : function(){
		var stream = byline(fs.createReadStream(RAW_INDEX_FILE));
		var collection = new mongodb.Collection(client, 'index_file');
		stream.on('data', function(line){
			var raw = JSON.parse(line);
			var fileName = raw.name;
			var deviceId = raw.device_uuid;
			var id = raw.id;
			var fullPath = raw.fullPath;
			var MD5 = raw.MD5;
			var size = raw.size;
			try {	
				var path_array = fullPath.splite('/');
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
						if (err) {console.log(err); return;}
		console.log('aeraer');
						return;
					}
				);
			} catch (err){
				//Files in other language sometimes causes problems.
				//Skip them
			}
		});
	},

	saveDirIndex : function(line){
		db.open(function (error, client) {
			if (error) throw error;
			var collection = new mongodb.Collection(client, 'index_dir');
			var indexEntry = JSON.parse(line);
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
					if (err) {console.log(err); return};
					db.close();
					next();
					return;
				}
			);
		});
	}
}

module.exports.indexer.index_dir(function(){
/*
	Start with directory indexing
	Note that even though the current execution line has reached
	the current line, it does not guarantee that all the insertions are done
*/
	eodule.exports.indexer.index_files();
});
