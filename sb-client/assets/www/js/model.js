window.FileSystemModel = Backbone.Model.extend({
	defaults: {
		"isFile": null,
	    "isDirectory" : null,
	    "name" : null,
	    "fullPath" : null,
        "filesystem" : null,
        "object" : null  //directoryEntry/fileEntry object
	  },

      setObject : function(obj){
        var self = this;

        if(obj.isDirectory === true ){
            self.set("object", obj);
        } 

        self.set("fullPath", obj.fullPath);
        self.set("name", obj.name);
        self.set("isFile", obj.isFile);
        self.set("isDirectory", obj.isDirectory);
        self.set("filesystem", obj.filesystem);        
      }
});


window.FileSystemCollection = Backbone.Collection.extend({
    model: window.FileSystemModel,
});
