package edu.slidboard.client;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.net.URLEncoder;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.FileEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;

import android.provider.MediaStore.Files;
import android.util.Log;

public class HTTPClient {
	
	//POST messages to the server
	public static String POST(String path, String data) throws IOException{
		String targetURL = "http://69.164.219.86:8081/" + path; 
		String msg = "msg=" + URLEncoder.encode(data, "UTF-8");
		URL url;
		HttpURLConnection connection = null;  
		String response = "err";
		try {
			//Create connection
			url = new URL(targetURL);
			connection = (HttpURLConnection)url.openConnection();
			connection.setRequestMethod("POST");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
			connection.setRequestProperty("Content-Length", "" + Integer.toString(msg.getBytes().length));
			connection.setRequestProperty("Content-Language", "en-US");  
			connection.setUseCaches (false);
			connection.setDoInput(true);
			connection.setDoOutput(true);
			connection.setConnectTimeout(3600000);
			
			//Send request
			DataOutputStream wr = new DataOutputStream(connection.getOutputStream ());
			wr.writeBytes (msg);
			wr.flush ();
			wr.close ();
			
			//Get Response	
			InputStream is = connection.getInputStream();
			BufferedReader rd = new BufferedReader(new InputStreamReader(is));
			String line;
			StringBuffer _response = new StringBuffer(); 
			while((line = rd.readLine()) != null) {
				_response.append(line);
				_response.append('\r');
			}
			rd.close();
			Log.v("FROM SERVER ", _response.toString());
			response = _response.toString();
		} catch (Exception e) {
			e.printStackTrace();
			
		} finally {
			if(connection != null) {
				connection.disconnect(); 
				Log.v("HTTP Client", "Connection closed");
			}
		}
		return response;
	}
	
	public static String POSTFile(String path, File file) throws IOException{
		byte[] data = toByteArray(file);
		return POST(path, new String(data, "UTF-8"));
	}
	
private static byte[] toByteArray(File file) throws FileNotFoundException, IOException{  
	int length = (int) file.length();  
	byte[] array = new byte[length];  
	InputStream in = new FileInputStream(file);  
	int offset = 0;  
	while (offset < length) {  
		offset += in.read(array, offset, (length - offset));  
	}  
	in.close();  
	return array;  
	}  
}
