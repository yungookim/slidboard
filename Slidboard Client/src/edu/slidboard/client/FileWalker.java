package edu.slidboard.client;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import org.json.simple.JSONObject;

import android.util.Log;

public class FileWalker {
	
	//Path to the external storage
	private String external;
	
	//Object holding index.json
	private File indexFileJSON;
	private File indexDirJSON;
	
	//Path to slidboard
	private String dir;
	
	private FileWriter fileJsonFstream;
	private FileWriter dirJsonFstream;
	
	public FileWalker(String external) throws Exception{
		this.external = external;
		this.dir = external + "/slidboard";
		File root_dir = new File(this.dir);
		//Ensure that the index.json file exists.
		
		//Check if the directory already exists
		if(root_dir.exists()){
			this.createIndexJSON();
		} else {
			//Dir DNE. Create new.
			if(root_dir.mkdir()){
				//Dir created. Create index.json to index file system.
				this.createIndexJSON();
			} else {
				//Error
				Log.e("Error", "Directory could not be created. Abort.");
				throw new Exception();	
			}
		}
		
		//index.json file is created.
		this.fileJsonFstream = new FileWriter(this.indexFileJSON);
		this.dirJsonFstream = new FileWriter(this.indexDirJSON);
	}
	
	public void createIndexJSON() throws IOException{
		this.indexFileJSON = new File(this.dir + "/indexFile.json");
		this.indexDirJSON = new File(this.dir + "/indexDir.json");
		if (!this.indexFileJSON.exists()){
			this.indexFileJSON.createNewFile();
		}
		if (!this.indexDirJSON.exists()){
			this.indexDirJSON.createNewFile();
		}
	}
	
	//Simply list all the files in the external storage. 
	//Let the server and the PixelSense do the heavy work
    @SuppressWarnings("unchecked")
	public void walk(String path) throws IOException{
        File root = new File(path);
        File[] list = root.listFiles();
        
        try {
        	for (File f : list) {
        		Log.i("Walker", "Walking on " + f.getName());
        		
                if (f.isDirectory()) {
                	JSONObject _json = new JSONObject();
            		_json.put("name", f.getName());
        			_json.put("fullPath", f.getAbsolutePath());
        			_json.put("size", f.length());
                	_json.put("type", "DIR");
                	
                	String temp = _json + "\n";
                	this.dirJsonFstream.write(temp);
                	
                	//Look into subdirectories
                	this.walk(f.getAbsolutePath());
                } else {
                	JSONObject _json = new JSONObject();
            		_json.put("name", f.getName());
        			_json.put("fullPath", f.getAbsolutePath());
        			_json.put("size", f.length());
                	_json.put("type", "FILE");
                	
                	String temp = _json + "\n";
                	this.fileJsonFstream.write(temp);
                }
            }
        } catch (Exception e){
        	//Let it be! .... for now.
        }
    }
    
    public void done() throws IOException{
    	this.dirJsonFstream.close();
    	this.fileJsonFstream.close();
    }
}
