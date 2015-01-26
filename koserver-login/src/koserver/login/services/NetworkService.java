package koserver.login.services;

import java.io.IOException;

import koserver.common.network.SocketServer;
import koserver.common.network.config.ServerConfig;
import koserver.common.services.AbstractService;
import koserver.login.configuration.NetworkConfig;
import koserver.login.network.PacketHandler;

import org.apache.log4j.Logger;

public class NetworkService extends AbstractService {

	private static Logger log = Logger.getLogger(NetworkService.class.getName());

	private SocketServer server;

	private NetworkService() {
	}

	@Override
	public void onInit() {
		server = new SocketServer(new ServerConfig(NetworkConfig.LOGIN_BIND_ADDRESS, NetworkConfig.LOGIN_BIND_PORT));
		try {
			server.open();
		} catch (IOException e) {
			log.error("Error on sockets server startup: " + e.getMessage());
			System.exit(0);
		}
		ClientsCollector clientsCollector = new ClientsCollector(server, new PacketHandler());
		Thread clientsCollectorThread = new Thread(clientsCollector);
		clientsCollectorThread.start();
		log.info("NetworkService started");
	}

	@Override
	public void onDestroy() {
		log.info("NetworkService stopped");
	}

	public static NetworkService getInstance() {
		return SingletonHolder.instance;
	}

	private static class SingletonHolder {
		protected static final NetworkService instance = new NetworkService();
	}
}
