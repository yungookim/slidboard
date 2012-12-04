package edu.slidboard.client;

import java.io.File;
import java.io.IOException;

import android.util.Log;

public class FileUploader implements Runnable {
	private String result;
	private String senderId;
	
	public FileUploader(String senderId){
		this.senderId = senderId;
	}
	
	public void run() {
	    waiter();
	}
	private void waiter() {
		String res;
		while (true) {
			try {
				Log.e("REQUEST THREAD", "waiting");
				res = HTTPClient.POST("wait", this.senderId);
				Log.e("REQUEST THREAD", "accepted");
				res = res.trim();
				Log.e("REQUEST THREAD", "sending...");
				res = HTTPClient.uploadFile(new File(res.toString()));
				Log.e("REQUEST THREAD", res);
				Log.e("REQUEST THREAD", "done");
				res = "";
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	public String getResult() {
	    return result;
	}
	
}
