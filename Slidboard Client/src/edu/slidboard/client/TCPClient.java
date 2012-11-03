package edu.slidboard.client;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;
import java.net.UnknownHostException;

import android.util.Log;

/**
 * @author Dave
 *
 */
public class TCPClient {

	private int port;
	private String ip;
	private Socket clientSocket = null;
	private DataOutputStream outToServer = null;
	private BufferedReader inFromServer = null;
	
	public TCPClient(String ip, int port){	
		this.port = port;
		this.ip = ip;
	}
	
	public void connect(){
		try {
			//Get all the streams ready
			this.clientSocket = new Socket(ip, port);
			this.outToServer = new DataOutputStream(clientSocket.getOutputStream());
			this.inFromServer = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
		} catch (UnknownHostException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public void write(String input){
		try {
			this.outToServer.writeBytes(input);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	
	//Start listening to the incoming message. 
	//Blocking function.
	public void read(){
		try {
			Log.v("MSG", "Start listening.");
			Log.v("MSG", "Going to wait.");
			while (!this.inFromServer.ready()){}
			
			Log.v("MSG", "Wait done, receiving.");
			//Blocks until messages arrive
			String t = this.inFromServer.readLine();
			Log.v("From Server", t);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public void close(){
		try {
			this.outToServer.close();
			this.clientSocket.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
}
