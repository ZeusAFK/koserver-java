package koserver.login.domain.mapper.database;

import koserver.common.domain.mapper.database.DatabaseMapper;
import koserver.common.entity.AbstractEntity;
import koserver.common.models.AbstractModel;
import koserver.login.database.entity.AccountEntity;
import koserver.login.enums.AccountAuthority;
import koserver.login.models.Account;

public class AccountMapper extends DatabaseMapper<AccountEntity, Account> {

	@Override
	public AccountEntity map(final Account model, final AbstractEntity linkedEntity) {
		final AccountEntity accountEntity = new AccountEntity(model.getId());

		// DIRECT
		accountEntity.setLogin(model.getLogin());
		accountEntity.setPassword(model.getPassword());
		accountEntity.setAuthority(model.getAuthority().getValue());
		accountEntity.setAccess(model.getAccess());
		accountEntity.setCreation(model.getCreation());

		return accountEntity;
	}

	@Override
	public Account map(final AccountEntity model, final AbstractModel linkedModel) {
		final Account account = new Account(model.getId());

		// DIRECT
		account.setLogin(model.getLogin());
		account.setPassword(model.getPassword());
		account.setAuthority(AccountAuthority.from(model.getAuthority()));
		account.setAccess(model.getAccess());
		account.setCreation(model.getCreation());

		return account;
	}

	@Override
	public boolean equals(final Account model, final AccountEntity entity) {
		return model.getLogin().equals(entity.getLogin());
	}
}
