package koserver.login.delegate;

import java.util.List;

import koserver.common.database.dao.DAOManager;
import koserver.login.database.dao.PatchListDAO;
import koserver.login.database.entity.PatchEntity;

public class PatchListDelegate {
	public static List<PatchEntity> getPatchs() {
		return DAOManager.getDAO(PatchListDAO.class).readAll();
	}
}
