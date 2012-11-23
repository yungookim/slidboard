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
//	private File indexFileJSON;
//	private File indexDirJSON;
	
	//index.json file streams
//	private FileWriter fileJsonFstream;
//	private FileWriter dirJsonFstream;
		
	//Path to slidboard
	private String dir;
	
	//Directories within this list will not be indexed
	private ArrayList<String> whiteList = new ArrayList<String>();
	private ArrayList<String> extAllowedList = new ArrayList<String>();

	//Raw index is saved here
	private StringBuilder fileRawIndex = new StringBuilder();
	private StringBuilder dirRawIndex = new StringBuilder();
	
	
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
		//this.fileJsonFstream = new FileWriter(this.indexFileJSON);
		//this.dirJsonFstream = new FileWriter(this.indexDirJSON);
		
		this.prepBlackList();
	}
	
	private void prepBlackList(){
		//Just hardcode for now :p
		//Should provide a regex too later.
		this.whiteList.add("download");
		this.whiteList.add("bluetooth");
		this.whiteList.add("slidboard");
		
		extAllowedList.add("jpg");
		extAllowedList.add("jpeg");
		extAllowedList.add("png");
		extAllowedList.add("bmp");
		//extAllowedList.add("txt");
		//extAllowedList.add("mp3");
	}
	
	public void createIndexJSON() throws IOException{
//		this.indexFileJSON = new File(this.dir + "/indexFile.json");
//		if (this.indexFileJSON.exists()){
//			this.indexFileJSON.delete();
//		}
//		if (!this.indexFileJSON.exists()){
//			this.indexFileJSON.createNewFile();
//		}
//		
//		this.indexDirJSON = new File(this.dir + "/indexDir.json");
//		if (this.indexDirJSON.exists()){
//			this.indexDirJSON.delete();
//		}
//		if (!this.indexDirJSON.exists()){
//			this.indexDirJSON.createNewFile();
//		}
		
	}
	
	//Simply list all the files in the external storage. 
	//Let the server and the PixelSense do the heavy work
    @SuppressWarnings("unchecked")
	public void walk(String path, TCPClient client, UUID device_uuid) throws IOException{
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
        		
                if (f.isDirectory() && this.whiteList.contains(f.getName())) {
                	                	
                	JSONObject _json = new JSONObject();
            		_json.put("name", f.getName());
        			_json.put("fullPath", f.getAbsolutePath());
                	_json.put("type", "DIR");
                	_json.put("device_uuid", device_uuid.toString());
                	_json.put("id", UUID.randomUUID().toString());
                	
                	//this.dirJsonFstream.write(_json + "\r\n");
                	dirRawIndex.append(_json + "\r\n");
                	
                	//Look into subdirectories
                	this.walk(f.getAbsolutePath(), client, device_uuid);
                	
                } else if (
                		f.isFile() && ext.length() > 1 
                		&& extAllowedList.contains(ext) 
                		&& first_dot != 0
                		&& f.length() < 5242880){
                	
                	JSONObject _json = new JSONObject();
            		_json.put("name", f.getName().toString());
        			_json.put("fullPath", f.getAbsolutePath());
        			_json.put("size", f.length());
                	_json.put("type", "FILE");
                	_json.put("id", UUID.randomUUID().toString());
                	_json.put("device_uuid", device_uuid.toString());
                	_json.put("MD5", createMD5Checksum(f));
                	
                	fileRawIndex.append(_json + "\r\n");
                	
                	HTTPClient.POSTFile("saveFile", f);
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
    
  //Close the file streams
    public void closeFileOutputStreams(){
//    	try {
//			this.fileJsonFstream.close();
//			this.dirJsonFstream.close();
//		} catch (IOException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
    }
    
    public File getIndexFile(){
//    	return this.indexFileJSON;
    	return null;
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

	public String getRawIndex() {
		return this.fileRawIndex.append(this.dirRawIndex.toString()).toString();
	}
}
