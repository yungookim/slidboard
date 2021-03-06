package edu.slidboard.client;

import org.json.simple.JSONObject;

import java.io.File;
import java.io.IOException;
import java.util.UUID;
import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.os.Environment;
import android.os.PowerManager;
import android.os.PowerManager.WakeLock;
import android.provider.Settings;
import android.util.Log;
import android.view.Menu;
import android.widget.Toast;

public class MainActivity extends Activity {

	//public UUID CLIENT_UUID;
	public String CLIENT_UUID;
	
	public final String CLIENT_TYPE = "MOBILE";
	
	//File uploader
	private Thread thread;

	private WakeLock mWakeLock;
	
    @SuppressWarnings("unchecked")
	@Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        
        String device_id = Settings.Secure.ANDROID_ID;
        
        //Generate the device's UUID
        //CLIENT_UUID = new UUID(device_id.hashCode(), device_id.hashCode()*device_id.hashCode());
        
        //HACK : FOR DEMO PURPOSE
        CLIENT_UUID = "24973f10-3dab-11e2-a25f-0800200c9a66";
        
        
        //Get path to an external storage
        String storage = Environment.getExternalStorageDirectory().getPath();
        
        JSONObject obj = new JSONObject();
		obj.put("from","MOBILE");
		obj.put("deviceId", CLIENT_UUID.toString());

		try {
			HTTPClient.POST("init", obj.toJSONString());
		} catch (Exception e){
			Log.e("Err while init", e.toString());
		}
		
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
        
        final PowerManager pm = (PowerManager) getSystemService(Context.POWER_SERVICE);
        this.mWakeLock = pm.newWakeLock(PowerManager.SCREEN_DIM_WAKE_LOCK, "My Tag");
        this.mWakeLock.acquire();
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
    	this.mWakeLock.release();
        super.onDestroy();
    	//System.exit(0);
    }
}


