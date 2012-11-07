//Index given file system listing using Mongodb.
var mongodb = require('mongodb'),
    server = new mongodb.Server("127.0.0.1", 27017, {}),
    db = new mongodb.Db('slidboard', server, {safe:false}),
    fs = require('fs'),
    byline = require('byline');
 
module.exports.indexer = {
	index : function() {
		//Start reading the raw index file line by line
		var stream = byline(fs.createReadStream('/data/test.json'));
		db.open(function (error, client) {
				if (error) throw error;
					stream.on('data', function(line) {
						var collection = new mongodb.Collection(client, 'index');
						collection.insert(JSON.parse(line), {safe:true}, function(err, objects) {
							if (err) console.warn(err.message);
							if (err && err.message.indexOf('E11000 ') !== -1) {}
      		});
			});
		});
	}
}

module.exports.indexer.index();
