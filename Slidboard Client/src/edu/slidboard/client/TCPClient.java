package edu.slidboard.client;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
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
	private PrintWriter  outToServer = null;
	private BufferedReader inFromServer = null;
	
	public TCPClient(String ip, int port){	
		this.port = port;
		this.ip = ip;
	}
	
	public void connect(){
		try {
			//Get all the streams ready
			this.clientSocket = new Socket(ip, port);
			this.outToServer = new PrintWriter(this.clientSocket.getOutputStream());
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
		this.outToServer.println(input + "\r\n");
		this.outToServer.flush();
	}
	
	public void sendFile(File file) throws IOException{
		byte[] buf = new byte[1024];
	    OutputStream os = this.clientSocket.getOutputStream();
	    BufferedOutputStream out = new BufferedOutputStream(os, 1024);
	    FileInputStream in = new FileInputStream(file);
	    int i = 0;
	    int bytecount = 1024;
	    while ((i = in.read(buf, 0, 1024)) != -1) {
	      bytecount = bytecount + 1024;
	      out.write(buf, 0, i);
	    }
	    out.flush();
	    this.clientSocket.shutdownOutput(); /* important */
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
