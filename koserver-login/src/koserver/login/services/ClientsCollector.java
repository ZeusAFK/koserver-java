package koserver.login.services;

import java.net.Socket;
import java.util.ArrayList;

import org.apache.log4j.Logger;

import koserver.common.network.SocketServer;
import koserver.common.services.AbstractService;
import koserver.login.network.PacketHandler;

public class ClientsCollector extends AbstractService implements Runnable {

	private static Logger log = Logger.getLogger(ClientsCollector.class.getName());

	private SocketServer server;
	private ArrayList<AccountService> clients;
	private PacketHandler packetHandler;

	public ClientsCollector(SocketServer server, PacketHandler packetHandler) {
		clients = new ArrayList<AccountService>();
		this.server = server;
		this.packetHandler = packetHandler;
	}

	@Override
	public void run() {
		this.onInit();
		while (server.isConnected()) {
			Socket clientSocket = server.waitForConnection();
			//log.info("Connection received on port " + clientSocket.getLocalPort() + " from " + clientSocket.getInetAddress().getHostAddress() + ":" + clientSocket.getPort());
			if (clientSocket != null) {
				AccountService client = new AccountService(clientSocket, packetHandler);
				clients.add(client);
				Thread clientThread = new Thread(client);
				clientThread.start();
			}
		}
		this.onDestroy();
	}

	public ArrayList<AccountService> getClients() {
		return clients;
	}

	@Override
	public void onInit() {
		//log.info("Clients connections service started on port " + server.getPort());
	}

	@Override
	public void onDestroy() {
		log.info("Stopping client connections service");
	}
}
