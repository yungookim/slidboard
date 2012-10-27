package edu.slidboard.client;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.UUID;

import org.json.simple.JSONObject;


import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;

public class MainActivity extends Activity {

	private UUID CLIENT_UUID = UUID.randomUUID();
	private String CLIENT_TYPE = "MOBILE";
	
	
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
       
        //Only read one specific file for now.
        //TODO: Improve this
        String str = readFiles();
        //Creates a connection to the server
        createConnection();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.activity_main, menu);
        return true;
    }
    
    @SuppressWarnings("unchecked")
	private void createConnection(){
		TCPClient client = new TCPClient("69.164.219.86", 6060);
    	client.connect();
    	
    	//Tell the server who am I.
    	JSONObject startString = new JSONObject();
    	startString.put("uuid", CLIENT_UUID.toString());
    	startString.put("type", CLIENT_TYPE.toString());
		client.write(startString.toString());
		
    }
    
    private String readFiles(){
    	FileInputStream fstream;
		try {
			fstream = new FileInputStream("/sdcard/slidboard/randomString");
			DataInputStream in = new DataInputStream(fstream);
			BufferedReader br = new BufferedReader(new InputStreamReader(in));
			String strLine;
			String str = "";
			while ((strLine = br.readLine()) != null)   {
				str = str + strLine + "\n";
			}
			
			in.close();
			
			Log.v("File reads", str);
			return str;
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return "";
    }
}
