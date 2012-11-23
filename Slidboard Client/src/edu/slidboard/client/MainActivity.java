package edu.slidboard.client;

import org.json.simple.JSONObject;

import java.io.IOException;
import java.util.UUID;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.os.Bundle;
import android.os.Environment;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.Menu;
import android.widget.Toast;

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
        
        //Initialize connection.
        try {        	
        	JSONObject obj = new JSONObject();
        	obj.put("from","MOBILE");
        	obj.put("deviceId", CLIENT_UUID.toString());
			String res = HTTPClient.POST("init", obj.toJSONString());
			if (!res.equals("ok")){
				Toast.makeText(this, "Error. Check server log", Toast.LENGTH_LONG).show();
			}
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
			return;//Let the system die
		}
        
        //Scan the contents of the external storage
        FileWalker fw;
		try {
			fw = new FileWalker(storage);
			fw.walk(storage, client, this.CLIENT_UUID);
			HTTPClient.POST("fileIndex", fw.getRawIndex());
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		JSONObject obj = new JSONObject();
		obj.put("from","MOBILE");
		obj.put("deviceId", CLIENT_UUID.toString());
		//String res = HTTPClient.POST("wait", obj.toJSONString());
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
    	startString.put("action", "init");
    	startString.put("uuid", CLIENT_UUID.toString());
    	startString.put("from", CLIENT_TYPE.toString());
    	this.client.write(startString.toString());
    	//this.client.close();
    }
}
