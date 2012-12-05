SlidBoard
=============

##Description

A fully functioning conceptual work for file system synchronization between Microsoft PixelSense and Android systems.

The working prototype fully implements following scenario.  
  
* The User downloads, installs, and runs the client app from    
https://dl.dropbox.com/u/36220055/edu.slidboard.client.apk  

* The user simply places his/her device on the screen of MS PixelSense.  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.05.25.jpg

* The PixelSense will then display the "white-listed" directories in Android system as shown in the link.  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.06.49.jpg  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.07.00.jpg  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.07.20.jpg

* All the elements shown can be freely moved, located, resized, and flicked.  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.08.28.jpg

* When working with peers, simply rotating the physical device will rotate every elements by the corresponding degree.  
https://dl.dropbox.com/u/36220055/CSC494/2012-12-04%2023.08.55.jpg  
https://dl.dropbox.com/u/36220055/CSC494/final_browsing.png

* As for the working prototype, only images and mp3 files are supported on PixelSense.

##Components

Slidboard has three components, namely, Slidboard (runs on PixelSense), Server, and Android Client.
Any message exchanges in the network are done via HTTP and should be in JSON format.

###Server

Server is written in Nodejs to simplify the communication handling with other multi-threaded clients. Also, please be advised that the server utilizes MongoDB for indexing file system.

#####app.js
Interface layer of the server. Although some of the functionalities are included here, they should be moved to either api.js or indexer.js.

#####api.js
Handlers for device communications. Parses incoming request and invokes the appropriate function.

#####indexer.js
Handlers for indexing file system of Android in MongoDB.

###Android Client
Almost down to nothing skeleton work for Android client.

#####MainActivity  
Initializes, and manages the client and FileUploader worker thread.

#####FileWalker  
Recursively indexes the file system and sends the result to the server via HTTPClient. Each entity holds following information about a file.   
```
{
"name" : Name of a file/directory,  
"fullPath", Absolute path of a file/directory,  
"size", Size of a file,  
"type", "FILE" || "DIR",  
"id", UUID of a file/directory,  
"device_uuid", Current device's UUID , 
"MD5", chasum of a file  
}
```  

#####HTTPClient  
Static handlers for HTTP requests. Responsible for communicating with and sending files to the server (No compression algorithm is used for the current prototype) .

#####FileUploader
A worker thread class. Runs in background and waits for file request from the server. When the request is received, invokes HTTPClient.uploadFile(...).

###PixelSense
The main dish. Fetches, and renders the file system from the mounted device.

#####SurfaceWindow1
Controller for Slidboard. Initializes the main view and handles events.

#####SlidboardView
Creates, and handles elements dynamically created via TagVisualization event from SurfaceWindow1. From IndexObjects, the class can generate file system structure view, render images, and play audio files(MediaElement). Any items generated here are wrapped inside a ScatterViewItem, otherwise, the system will throw an Exception. Also, to ensure a non-blocking UX, any NetworkIO should create a background thread. When the thread finishes the IO, it creates then delete the view elements to the main view for render.

#####IndexObject
Model for incoming indexed file system for Android Client.

#####HTTPClient
Similar to Android Client, the class holds all the static methods for communicating with the server.

#####JSONMessage , JSONMessageWrapper, Parser
An extension of Newtonsoft.Json tailor specifically for this application. Any messages sent or received to the server should be firstly wrapped or parsed with the current class.


##TO DO
The current components are just the ground works done in a few weeks during school term.
In the beginning, it was aimed to the point where the PixelSense can handle multiple devices simultaneously and supports file transmission in NHCI manner. However, due to some constraints (eg. Device availability, and time) the goal become unattainable. It would be interesting to see how peers can interact with the files within their personal devices on a on centralized view.
