var MainView = Backbone.View.extend({
    el : '#container',

    initialize: function() {
        this.currentDir = null;
    },

    render: function() {


    },

    readFileSystem : function(next){
        var self = this;
        //Create callbacks
        var onSuccess = function(fileSystem) {
            console.log(fileSystem.name);
            console.log(fileSystem.root.name);
            self.currentDir = fileSystem.root;
            next();
        }
        var fail = function(evt) {
            console.log(evt.target.error.code);
            next('err');
        }

        // request the persistent file system
        window.requestFileSystem(LocalFileSystem.PERSISTENT, 0, onSuccess, fail);
    },

    //Read all the files in current directory
    readDirectory : function(){
        var self = this;
        // Get a directory reader
        var directoryReader = self.currentDir.createReader();

        //Create call backs
        var success = function(entries){
            var i;
            for (i=0; i<entries.length; i++) {
                console.log(entries[i].name);
            }

            var template = $("#directory_row_template").html();
            var html = "";
            _.each(entries, function(each){
                html += Mustache.render(template, each);
            });

            self.$el.html(html);
        }

        var fail = function(error) {
            console.log("Failed to list directory contents: " + error.code);
        }


        // Get a list of all the entries in the directory
        directoryReader.readEntries(success,fail);
    }
});

window.addEventListener('load', function () {
    document.addEventListener('deviceready', function(){
        var main_view = new MainView();

        main_view.readFileSystem(function(err){
            if(err){
                return;
            }
            main_view.readDirectory();
        });
        

    }, false);
}, false);