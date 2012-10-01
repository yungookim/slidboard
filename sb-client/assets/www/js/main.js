var MainView = Backbone.View.extend({
    el : '#container',

    events: {
        'click .dir_goto' : 'goToDir',
        'click .file_goto' : 'goToFile'
    },

    initialize: function() {
        //DirectoryEntries
        this.currentDir = null;
        this.previousDir = null;
    },

    readFileSystem : function(next){
        var self = this;
        //Create callbacks
        var onSuccess = function(fileSystem) {
            // console.log(fileSystem.name);
            // console.log(fileSystem.root.name);
            self.currentDir = fileSystem.root;
            $("#path").html(self.currentDir.fullPath);
            next();
        }
        var fail = function(evt) {
            console.log(evt.target.error.code);
            next('error initializing : ' + evt.target.error.code);
        }

        // request the persistent file system
        window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, onSuccess, fail);
    },

    //Read and render all the files in the current directory
    readDirectory : function(){
        var self = this;
        // Get a directory reader
        var directoryReader = self.currentDir.createReader();

        //Create call backs
        var success = function(entries){

            self.$el.html('');

            var dir_template = $("#directory_row_template").html();
            var file_template = $("#file_row_template").html();

            _.each(entries, function(each){
                var temp = new window.FileSystemModel();
                temp.setObject(each);
                self.collection.add(temp);

                if (each.isFile){
                    self.$el.append(Mustache.render(file_template, each));
                } else {
                    self.$el.append(Mustache.render(dir_template, each));
                }
            });

            $("#path").html(self.currentDir.fullPath);
        }

        // Get a list of all the entries in the directory
        directoryReader.readEntries(success,self.FileSystemError);
    },

    goToDir : function(e){
        var self = this;

        var targetPath = $(e.currentTarget).attr("data-path");

        var nextDirectoryModel;

        _.each(self.collection.models, function(model){
            if (model.get("fullPath") === targetPath){
                nextDirectoryModel = model;
            }
        });

        //clear collection
        self.collection.reset();
        self.previousDir = self.currentDir;
        self.currentDir = nextDirectoryModel.get('object');
        self.readDirectory();
    },

    goToFile : function(e){
        var self = this;

        var targetPath = $(e.currentTarget).attr("data-path");

        var nextDirectoryModel;

        _.each(self.collection.models, function(model){
            if (model.get("fullPath") === targetPath){
                nextDirectoryModel = model;
            }
        });

        if (nextDirectoryModel.get('isFile') === true){
            window.resolveLocalFileSystemURI(nextDirectoryModel.get('fullPath'), 
                //onSuccess
                function(fileEntry){
                    function win(file) {
                        var reader = new FileReader();
                        reader.onloadend = function(evt) {
                            console.log(evt.target.result);
                        };
                        reader.readAsText(file);
                    };

                    var fail = function(evt) {
                        console.log(error.code);
                    };

                    fileEntry.file(win, fail);
                }, 
                //onFail
                self.FileSystemError
            );
        }
    },

    FileSystemError : function(error){
        alert("Failed to list directory contents: " + error.code);
        console.log("Failed to list directory contents: " + error.code);
    }
});



window.addEventListener('load', function () {
    document.addEventListener('deviceready', function(){
        var file_system_collection = new FileSystemCollection();
        var main_view = new MainView({collection : file_system_collection});
        main_view.readFileSystem(function(err){
            if(err){
                return;
            }
            main_view.readDirectory();
        });
    }, false);
}, false);