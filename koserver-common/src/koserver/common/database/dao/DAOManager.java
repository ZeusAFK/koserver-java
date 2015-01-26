package koserver.common.database.dao;

import java.util.Map;

import org.apache.log4j.Logger;

import javolution.util.FastMap;
import koserver.common.database.dao.exception.DAONotFoundException;
import koserver.common.database.entity.AbstractDatabaseEntity;

public class DAOManager {

	/** LOGGER */
	private static final Logger log = Logger.getLogger(DAOManager.class.getName());

	/** Collection of registered DAOs */
	private static final Map<String, AbstractDAO<? extends AbstractDatabaseEntity>> daoMap = new FastMap<String, AbstractDAO<? extends AbstractDatabaseEntity>>();

	private DAOManager() {
		// empty
	}

	public static <T extends AbstractDAO<? extends AbstractDatabaseEntity>> T getDAO(Class<T> clazz) {
		DAOManager.ensureDAO(clazz);

		@SuppressWarnings("unchecked")
		T result = (T) daoMap.get(clazz.getName());

		if (result == null) {
			throw new DAONotFoundException("DAO Not found " + clazz.getName());
		}

		return result;
	}

	private static final <T extends AbstractDAO<? extends AbstractDatabaseEntity>> void ensureDAO(Class<T> clazz) {
		AbstractDAO<? extends AbstractDatabaseEntity> result = daoMap.get(clazz.getName());
		if (result == null) {
			try {
				daoMap.put(clazz.getName(), clazz.newInstance());
				log.info("Initiated DAO: " + clazz.getName());
			} catch (Exception e) {
				throw new RuntimeException(e.getMessage(), e);
			}
		}
	}
}
