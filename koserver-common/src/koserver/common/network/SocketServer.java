package koserver.common.network;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

import koserver.common.network.config.ServerConfig;

public class SocketServer {

	private ServerSocket socket;
	private int port;

	public SocketServer(ServerConfig config) {
		this.port = config.getPort();
	}

	public void close() throws IOException {
		socket.close();
	}

	public void open() throws IOException {
		socket = new ServerSocket(port);
	}

	public Socket waitForConnection() {
		try {
			return socket.accept();
		} catch (IOException e) {
			return null;
		}
	}
	
	public boolean isConnected(){
		return !socket.isClosed();
	}
	
	public int getPort(){
		return socket.getLocalPort();
	}
}