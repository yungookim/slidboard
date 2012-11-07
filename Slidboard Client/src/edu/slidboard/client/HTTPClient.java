package edu.slidboard.client;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;

import android.util.Log;

public class HTTPClient {
	
	
	public static void POST(String data, String path) throws IOException{
		String targetURL = "http://69.164.219.86:8081/" + path; 
		String msg = "msg=" + URLEncoder.encode(data, "UTF-8");
		
		URL url;
		HttpURLConnection connection = null;  
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
			
			//Send request
			DataOutputStream wr = new DataOutputStream(connection.getOutputStream ());
			wr.writeBytes (msg);
			wr.flush ();
			wr.close ();
			
			//Get Response	
			InputStream is = connection.getInputStream();
			BufferedReader rd = new BufferedReader(new InputStreamReader(is));
			String line;
			StringBuffer response = new StringBuffer(); 
			while((line = rd.readLine()) != null) {
				response.append(line);
				response.append('\r');
			}
			rd.close();
			Log.v("FROM SERVER ", response.toString());
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			if(connection != null) {
			connection.disconnect(); 
			}
		}
	}
}
