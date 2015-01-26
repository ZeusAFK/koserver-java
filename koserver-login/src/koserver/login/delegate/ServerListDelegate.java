package koserver.login.delegate;

import java.util.List;

import koserver.common.database.dao.DAOManager;
import koserver.login.database.dao.ServerListDAO;
import koserver.login.database.entity.ServerEntity;

public class ServerListDelegate {
	public static List<ServerEntity> getServers() {
		return DAOManager.getDAO(ServerListDAO.class).readAll();
	}
}
