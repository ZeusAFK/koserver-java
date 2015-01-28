package koserver.game.database.dao;

import koserver.common.database.dao.AbstractDAO;
import koserver.game.database.entity.PlayerEntity;
import koserver.game.services.DatabaseService;

import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.Transaction;

public class PlayerDAO extends AbstractDAO<PlayerEntity> {

	@Override
	public void create(final PlayerEntity entity) {
		final Transaction transaction = session.beginTransaction();

		final Integer id = (Integer) session.save(entity);
		entity.setId(id);

		transaction.commit();
	}

	@Override
	public PlayerEntity read(final Integer id) {
		PlayerEntity player = null;

		final Transaction transaction = session.beginTransaction();

		player = (PlayerEntity) session.get(PlayerEntity.class, id);

		transaction.commit();

		return player;
	}

	public PlayerEntity readByName(final String name) {
		PlayerEntity player = null;

		final Transaction transaction = session.beginTransaction();

		final Query query = session.createQuery("from PlayerEntity where name = :name");
		query.setString("name", name);

		player = (PlayerEntity) query.uniqueResult();

		transaction.commit();

		return player;
	}

	@Override
	public void update(final PlayerEntity entity) {
		final Transaction transaction = session.beginTransaction();

		session.update(session.merge(entity));

		transaction.commit();
	}

	@Override
	public void delete(final PlayerEntity entity) {
		final Transaction transaction = session.beginTransaction();

		session.delete(session.merge(entity));

		transaction.commit();
	}

	@Override
	protected Session getSession() {
		return DatabaseService.getInstance().getSessionFactory().openSession();
	}
}
