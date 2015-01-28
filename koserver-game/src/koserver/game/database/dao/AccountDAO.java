package koserver.game.database.dao;

import koserver.common.database.dao.AbstractDAO;
import koserver.game.database.entity.AccountEntity;
import koserver.game.services.DatabaseService;

import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.Transaction;

public class AccountDAO extends AbstractDAO<AccountEntity> {

	@Override
	public void create(final AccountEntity entity) {
		final Transaction transaction = session.beginTransaction();

		final Integer id = (Integer) session.save(entity);
		entity.setId(id);

		transaction.commit();
	}

	@Override
	public AccountEntity read(final Integer id) {
		AccountEntity account = null;

		final Transaction transaction = session.beginTransaction();

		account = (AccountEntity) session.get(AccountEntity.class, id);

		transaction.commit();

		return account;
	}

	public AccountEntity readByLogin(final String login) {
		AccountEntity account = null;

		final Transaction transaction = session.beginTransaction();

		final Query query = session.createQuery("from AccountEntity where login = :login");
		query.setString("login", login);

		account = (AccountEntity) query.uniqueResult();

		transaction.commit();

		return account;
	}

	@Override
	public void update(final AccountEntity entity) {
		final Transaction transaction = session.beginTransaction();

		session.update(session.merge(entity));

		transaction.commit();
	}

	@Override
	public void delete(final AccountEntity entity) {
		final Transaction transaction = session.beginTransaction();

		session.delete(session.merge(entity));

		transaction.commit();
	}

	@Override
	protected Session getSession() {
		return DatabaseService.getInstance().getSessionFactory().openSession();
	}
}
