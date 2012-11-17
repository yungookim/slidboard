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
import android.content.Context;
import android.os.Bundle;
import android.os.Environment;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.Menu;

public class MainActivity extends Activity {

	public UUID CLIENT_UUID;
	
	public final String CLIENT_TYPE = "MOBILE";
	
	//TCP Client
	private TCPClient client;
	
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        
        String device_id = Settings.Secure.ANDROID_ID;
        
        //Generate the device's UUID
        CLIENT_UUID = new UUID(device_id.hashCode(), device_id.hashCode()*device_id.hashCode());
        
        //Get path to an external storage
        String storage = Environment.getExternalStorageDirectory().getPath();
        
        //Scan the contents of the external storage
        FileWalker fw;
		try {
			//Connect to server and send the index files
			this.createConnection(client);
			
			//TODO : as for the testing phase, only walk in a small dir.
			fw = new FileWalker(storage);
			
			fw.walk(storage, client, this.CLIENT_UUID);
			
			HTTPClient.POST(fw.getRawIndex(), "fileIndex");
			
			//TODO: should close here, but should start to listen
			this.client.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.activity_main, menu);
        return true;
    }
    
    @SuppressWarnings("unchecked")
	private void createConnection(TCPClient client){
		this.client = new TCPClient("69.164.219.86", 6060);
    	this.client.connect();
    	
    	//Tell the server who am I.
    	JSONObject startString = new JSONObject();
    	startString.put("action", "INIT");
    	startString.put("uuid", CLIENT_UUID.toString());
    	startString.put("from", CLIENT_TYPE.toString());
    	this.client.write(startString.toString());
    }
}
