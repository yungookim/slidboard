package edu.slidboard.client;

import org.json.simple.JSONObject;

import java.io.File;
import java.io.IOException;
import java.util.UUID;
import android.app.Activity;
import android.os.Bundle;
import android.os.Environment;
import android.provider.Settings;
import android.util.Log;
import android.view.Menu;
import android.widget.Toast;

public class MainActivity extends Activity {

	public UUID CLIENT_UUID;
	
	public final String CLIENT_TYPE = "MOBILE";
	
	//File uploader
	private Thread thread;
	
    @SuppressWarnings("unchecked")
	@Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        
        String device_id = Settings.Secure.ANDROID_ID;
        
        //Generate the device's UUID
        CLIENT_UUID = new UUID(device_id.hashCode(), device_id.hashCode()*device_id.hashCode());
        
        //Get path to an external storage
        String storage = Environment.getExternalStorageDirectory().getPath();
        
        JSONObject obj = new JSONObject();
		obj.put("from","MOBILE");
		obj.put("deviceId", CLIENT_UUID.toString());

        //Scan the contents of the external storage
		try {
			FileWalker fw;
	    	fw = new FileWalker(storage);
			fw.walk(storage, this.CLIENT_UUID);
			HTTPClient.POST("fileIndex", fw.getRawIndex());
		} catch (IOException e){
			
		}
		
		//Initiate a file upload request listener
		FileUploader worker = new FileUploader(obj.toJSONString());
        thread = new Thread(worker);
        thread.start();
        
    }
    
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.activity_main, menu);
        return true;
    }
    
    public void onDestroy() {
        // Stop the thread
    	try {
			thread.join();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        //thread.interrupt();
        
    }
}


