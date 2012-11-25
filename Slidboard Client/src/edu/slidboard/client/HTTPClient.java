package edu.slidboard.client;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import android.util.Base64;
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
	
	public static void uploadFile(File file) throws IOException
    {
		InputStream is = new BufferedInputStream(new FileInputStream(file));
		ByteArrayOutputStream bos = new ByteArrayOutputStream();
		while (is.available() > 0) {
			bos.write(is.read());
		}
		byte[] bytes = bos.toByteArray();
		String encodedItem = Base64.encodeToString(bytes, Base64.DEFAULT);
		
		Log.e("FILE READY", encodedItem);
		
		HTTPClient.POST("uploadFile", encodedItem);
		
    }
}
