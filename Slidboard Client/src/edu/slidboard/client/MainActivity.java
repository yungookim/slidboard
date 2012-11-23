package edu.slidboard.client;

import org.json.simple.JSONObject;

import java.io.File;
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
//        try {        	
//        	JSONObject obj = new JSONObject();
//        	obj.put("from","MOBILE");
//        	obj.put("deviceId", CLIENT_UUID.toString());
//			String res = HTTPClient.POST("init", obj.toJSONString());
//			Log.e("MAIN", res);
//			if (!res.toString().equals("ok")){
//				Toast.makeText(this, "Error. Check server log", Toast.LENGTH_LONG).show();
//			} else {
//			}
//		} catch (IOException e1) {
//			// TODO Auto-generated catch block
//			e1.printStackTrace();
//			return;//Let the system die
//		}
        
        //Scan the contents of the external storage
        //FileWalker fw;
//		try {
//			//fw = new FileWalker(storage);
//			//fw.walk(storage, client, this.CLIENT_UUID);
//			//HTTPClient.POST("fileIndex", fw.getRawIndex());
//		} catch (IOException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
		
//		try {
//			JSONObject obj = new JSONObject();
//	    	obj.put("from","MOBILE");
//	    	obj.put("deviceId", CLIENT_UUID.toString());
//			String res = HTTPClient.POST("wait", obj.toJSONString());
//			
//		} catch (IOException e){
//			e.printStackTrace();
//		}
        
        try {
			HTTPClient.uploadFile(new File(storage + "/slidboard/1.png"));
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
}
