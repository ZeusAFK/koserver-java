package koserver.login.delegate;

import koserver.common.database.dao.DAOManager;
import koserver.login.database.dao.AccountDAO;
import koserver.login.database.entity.AccountEntity;

public class AccountDelegate {

	public static void createAccountEntity(final AccountEntity entity) {
		DAOManager.getDAO(AccountDAO.class).create(entity);
	}

	public static AccountEntity readAccountEntityById(final Integer id) {
		return DAOManager.getDAO(AccountDAO.class).read(id);
	}

	public static AccountEntity readAccountEntityByLogin(final String login) {
		return DAOManager.getDAO(AccountDAO.class).readByLogin(login);
	}

	public static void updateAccountEntity(final AccountEntity entity) {
		DAOManager.getDAO(AccountDAO.class).update(entity);
	}
}
