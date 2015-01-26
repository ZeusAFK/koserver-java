package koserver.login.delegate;

import koserver.common.database.dao.DAOManager;
import koserver.common.domain.mapper.MapperManager;
import koserver.login.database.dao.AccountDAO;
import koserver.login.database.entity.AccountEntity;
import koserver.login.domain.mapper.database.AccountMapper;
import koserver.login.models.Account;

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
    
    public static void createAccountModel(final Account model) {
        final AccountEntity accountEntity = MapperManager.getDatabaseMapper(AccountMapper.class).map(model);
        AccountDelegate.createAccountEntity(accountEntity);
        model.setId(accountEntity.getId());
    }
    
    public static Account readAccountModelById(final Integer id) {
        Account player = null;
        final AccountEntity accountEntity = AccountDelegate.readAccountEntityById(id);
        if (accountEntity != null) {
            player = MapperManager.getDatabaseMapper(AccountMapper.class).map(accountEntity);
        }
        return player;
    }
    
    public static Account readAccountModelByLogin(final String login) {
        Account account = null;
        final AccountEntity accountEntity = AccountDelegate.readAccountEntityByLogin(login);
        if (accountEntity != null) {
            account = MapperManager.getDatabaseMapper(AccountMapper.class).map(accountEntity);
        }
        return account;
    }
    
    public static void updateAccountModel(final Account model) {
        final AccountEntity accountEntity = MapperManager.getDatabaseMapper(AccountMapper.class).map(model);
        AccountDelegate.updateAccountEntity(accountEntity);
    }
}
