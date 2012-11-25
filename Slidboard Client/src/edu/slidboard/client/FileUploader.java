package edu.slidboard.client;

import java.io.File;
import java.io.IOException;

public class FileUploader implements Runnable {
	private String result;
	private String senderId;
	
	public FileUploader(String senderId){
		this.senderId = senderId;
	}
	
	public void run() {
	    waiter();
	}
	private String waiter() {
		String res;
		try {
			while (true){
				res = HTTPClient.POST("wait", this.senderId);
				res = res.trim();
				HTTPClient.uploadFile(new File(res.toString()));
			}
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return "ok";
	}
	
	public String getResult() {
	    return result;
	}
	
}
