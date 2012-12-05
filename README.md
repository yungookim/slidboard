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

app.js : Interface layer of the server. Although some of the functionalities are included here, they should be moved to either api.js or indexer.js.

api.js : Handlers for device communications. Parses incoming request and invokes the appropriate function.

indexer.js : Handlers for indexing file system of Android in MongoDB.

###Android Clientc
Almost down to nothing skeleton work for Android client.

MainActivity : Initializes, and manages the client and FileUploader worker thread.

FileWalker : Recursively indexes the file system and sends the result to the server via HTTPClient. Each entity holds following information about a file.   
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

HTTPClient : Static handlers for HTTP requests. Responsible for communicating with and sending files to the server. (No compression algorithm is used for the current prototype)  


