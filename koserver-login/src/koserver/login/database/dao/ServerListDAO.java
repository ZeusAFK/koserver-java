package koserver.login.database.dao;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import koserver.common.database.dao.AbstractDAO;
import koserver.login.database.entity.ServerEntity;
import koserver.login.services.DatabaseService;

import org.hibernate.Session;
import org.hibernate.Transaction;

public class ServerListDAO extends AbstractDAO<ServerEntity> {

	@Override
	public void create(final ServerEntity entity) {
		throw new RuntimeException("Can't create a server !");
	}

	@Override
	public ServerEntity read(final Integer id) {
		ServerEntity serverList = null;

		final Transaction transaction = session.beginTransaction();

		serverList = (ServerEntity) session.get(ServerEntity.class, id);

		transaction.commit();

		return serverList;
	}

	public List<ServerEntity> readAll() {
		final List<ServerEntity> serverEntities = new ArrayList<ServerEntity>();

		session.beginTransaction();
		@SuppressWarnings("unchecked")
		final Collection<? extends ServerEntity> servers = session.createQuery("FROM ServerEntity").list();
		if (servers != null) {
			serverEntities.addAll(servers);
		}
		session.clear();
		session.getTransaction().commit();

		return serverEntities;
	}

	@Override
	public void update(final ServerEntity entity) {
		throw new RuntimeException("Can't update a server !");
	}

	@Override
	public void delete(final ServerEntity entity) {
		throw new RuntimeException("Can't delete a server !");
	}

	@Override
	protected Session getSession() {
		return DatabaseService.getInstance().getSessionFactory().openSession();
	}
}
