var MainView = Backbone.View.extend({
    el : '#container',

    events: {
        'click .dir_goto' : 'goToDir',
        'click .file_goto' : 'readFile'
    },

    initialize: function() {
        //DirectoryEntries
        this.currentDir = null;
        this.previousDir = null;
        this.root = null;
        this.dir_template = $("#directory_row_template").html();
        this.file_template = $("#file_row_template").html();
        var self = this;

        $('#root').bind('click', function(){
            self.currentDir = null;
            self.currentDir = self.root;
            self.readDirectory();
        });

        $('#up').bind('click', function(){
            self.currentDir.getParent(
                function(parent){
                    self.currentDir = null;
                    self.currentDir = parent;
                    self.readDirectory();
                },
                function(error){
                    console.log(error.code);
                }
            );
        });
    },

    readFileSystem : function(next){
        var self = this;
        //Create callbacks
        var onSuccess = function(fileSystem) {
            // console.log(fileSystem.name);
            self.root = fileSystem.root;
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

            _.each(entries, function(each){
                var temp = new window.FileSystemModel();
                temp.setObject(each);
                self.collection.add(temp);

                if (each.isFile){
                    self.$el.append(Mustache.render(self.file_template, each));
                } else {
                    self.$el.append(Mustache.render(self.dir_template, each));
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

    readFile : function(e){
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

var g_socketid = -1;
var g_bluetoothPlugin = null;

window.addEventListener('load', function () {
    document.addEventListener('deviceready', function(){
        g_bluetoothPlugin = cordova.require( 'cordova/plugin/bluetooth' );

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








function enableBT() {
    g_bluetoothPlugin.enable( function() {
        alert( 'Enabling successfull' );
    }, function(error) {
        alert( 'Error enabling BT: ' + error );
    } );
}

function disableBT() {
    g_bluetoothPlugin.disable( function() {
        alert( 'Disabling successfull' );
    }, function(error) {
        alert( 'Error disabling BT: ' + error );
    } );
}

function discoverDevices() {
    g_bluetoothPlugin.discoverDevices( function(devices) {
        $('#bt-devices-select').html('');
        
        for( var i = 0; i < devices.length; i++ ) {
            $('#bt-devices-select').append( $( '<option value="' + devices[i].address + '">' + devices[i].name + '</option>' ) );
        }
    }, function(error) { alert( 'Error: ' + error ); } );
}

function listUUIDs() {
    g_bluetoothPlugin.getUUIDs( function(uuids) {
        $('#bt-device-uuids').html('');

        for( var i = 0; i < uuids.length; i++ ) {
            $('#bt-device-uuids').append( $( '<option value="' + uuids[i] + '">' + uuids[i] + '</option>' ) );
        }
    }, function(error) { alert( 'Error: ' + error ); }, $( '#bt-devices-select' ).val() );
}

function openRfcomm() {
    g_bluetoothPlugin.connect( function(socketId) { g_socketid = socketId; console.log( 'Socket-id: ' + g_socketid ); }, function(error) { alert( 'Error: ' + error ); }, $( '#bt-devices-select' ).val(), $( '#bt-device-uuids' ).val() );
}

function readRfcomm() {
    g_bluetoothPlugin.read( bp_readSuccess, bp_readError, g_socketid );
}

function bp_readError( error ) {
    alert( 'Error: ' + error );
}

function bp_readSuccess( p_data ) {
    $( '#bt-data-dump' ).html( p_data );
}