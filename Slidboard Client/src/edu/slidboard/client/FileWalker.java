package edu.slidboard.client;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.UUID;
import org.json.simple.JSONObject;

import android.util.Log;

public class FileWalker {
	
	//Object holding index.json
	private File indexFileJSON;
//	private File indexDirJSON;
	
	//index.json file streams
	private FileWriter fileJsonFstream;
//	private FileWriter dirJsonFstream;
		
	//Path to slidboard
	private String dir;
	
	//Directories within this list will not be indexed
	private ArrayList<String> blackList = new ArrayList<String>();
	private ArrayList<String> extBlackList = new ArrayList<String>();
	
	public FileWalker(String external) throws IOException{
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
				throw new FileNotFoundException();	
			}
		}
		
		//index.json file is created.
		this.fileJsonFstream = new FileWriter(this.indexFileJSON);
//		this.dirJsonFstream = new FileWriter(this.indexDirJSON);
		
		this.prepBlackList();
	}
	
	private void prepBlackList(){
		//Just hardcode for now :p
		//Should provide a regex too later.
		this.blackList.add("Android");
		this.blackList.add(".");
		this.blackList.add("..");
		this.blackList.add("viber");
		this.blackList.add("data-app");
		this.blackList.add("bugreports");
		this.blackList.add("burstlyImageCache");
		this.blackList.add("applanet");
		this.blackList.add("slidboard");
		this.blackList.add("Music");
		
		extBlackList.add("log");
		extBlackList.add("apk");
	}
	
	public void createIndexJSON() throws IOException{
		this.indexFileJSON = new File(this.dir + "/indexFile.json");
		if (this.indexFileJSON.exists()){
			this.indexFileJSON.delete();
		}
		if (!this.indexFileJSON.exists()){
			this.indexFileJSON.createNewFile();
		}
		//this.indexFileJSON.deleteOnExit();
		
//		this.indexDirJSON = new File(this.dir + "/indexDir.json");		
//		if (!this.indexDirJSON.exists()){
//			this.indexDirJSON.createNewFile();
//		}
//		this.indexDirJSON.deleteOnExit();
		
	}
	
	//Simply list all the files in the external storage. 
	//Let the server and the PixelSense do the heavy work
    @SuppressWarnings("unchecked")
	public void walk(String path, TCPClient client, UUID uuid) throws IOException{
        File root = new File(path);
        File[] list = root.listFiles();
        try {
        	for (File f : list) {
        		Log.i("Walker", "Walking on " + f.getName());
        		
        		//Check file extension.
        		//If the file does not have an extension, ignore it.
        		int pos = f.getName().lastIndexOf('.');
        		String ext = f.getName().substring(pos+1);
        		
        		int first_dot = f.getName().indexOf('.');
        		
                if (f.isDirectory() && !this.blackList.contains(f.getName())) {
                	
                	//I don't need directory listings
                	
//                	JSONObject _json = new JSONObject();
//            		_json.put("name", f.getName());
//        			_json.put("fullPath", f.getAbsolutePath());
//        			_json.put("size", f.length());
//                	_json.put("type", "DIR");
//                	_json.put("id", UUID.randomUUID());
//                	
//                	String temp = _json + "\n";
//                	this.dirJsonFstream.write(temp);
                	
                	//Look into subdirectories
                	this.walk(f.getAbsolutePath(), client, uuid);
                } else if (
                		f.isFile() && ext.length() > 1 
                		&& !extBlackList.contains(ext) 
                		&& first_dot != 0
                		&& f.length() > 5242880){
                	
                	JSONObject _json = new JSONObject();
            		_json.put("name", f.getName().toString());
        			_json.put("fullPath", f.getAbsolutePath());
        			_json.put("size", f.length());
                	_json.put("type", "FILE");
                	_json.put("id", UUID.randomUUID().toString());
                	_json.put("from", "MOBILE");
                	_json.put("uuid", uuid.toString());
                	_json.put("MD5", createMD5Checksum(f));
                	this.fileJsonFstream.write(_json + "\r\n");
                }
            }
        } catch (Exception e){
        	//Let it be! .... for now.
        }
    }
    
    public String createMD5Checksum(File f){
    	InputStream fis;
		try {
			fis = new FileInputStream(f);
			byte[] buffer = new byte[1024];
	        MessageDigest complete = MessageDigest.getInstance("MD5");
	        int numRead;

	        do {
	            numRead = fis.read(buffer);
	            if (numRead > 0) {
	                complete.update(buffer, 0, numRead);
	            }
	        } while (numRead != -1);
	        fis.close();
	        
	        //return as string
	        return new BigInteger(1, complete.digest()).toString(16);
	        
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (NoSuchAlgorithmException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return null;
    }
    
    public void closeFileOutputStream(){
    	try {
			this.fileJsonFstream.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }
    
    public File getIndexFile(){
    	return this.indexFileJSON;
    }
    
    public String readFile(File index_file) throws IOException{
    	FileInputStream fstream;

		fstream = new FileInputStream(index_file);
		DataInputStream in = new DataInputStream(fstream);
		BufferedReader br = new BufferedReader(new InputStreamReader(in));
		String strLine;
		String str = "";
		while ((strLine = br.readLine()) != null)   {
			str = str + strLine + "\n";
		}
		in.close();
//		Log.v("File reads", str);
		return str;

    }
    
    //Close the file streams
    public void done() throws IOException{
//    	this.dirJsonFstream.close();
    	this.fileJsonFstream.close();
    }
}
