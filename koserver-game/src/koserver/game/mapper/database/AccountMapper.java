package koserver.game.mapper.database;

import java.util.List;
import java.util.Set;

import javolution.util.FastList;
import javolution.util.FastSet;
import koserver.common.entity.AbstractEntity;
import koserver.common.enums.AccountAuthority;
import koserver.common.mapper.MapperManager;
import koserver.common.mapper.database.DatabaseMapper;
import koserver.common.models.AbstractModel;
import koserver.game.database.entity.AccountEntity;
import koserver.game.database.entity.PlayerEntity;
import koserver.game.models.account.Account;
import koserver.game.models.player.Player;

public class AccountMapper extends DatabaseMapper<AccountEntity, Account> {

	@Override
	public AccountEntity map(final Account model, final AbstractEntity linkedEntity) {
		final AccountEntity accountEntity = new AccountEntity(model.getId());

		// DIRECT
		accountEntity.setLogin(model.getLogin());
		accountEntity.setPassword(model.getPassword());
		accountEntity.setNation(model.getNation());
		accountEntity.setAuthority(model.getAuthority().getValue());
		accountEntity.setAccess(model.getAccess());
		accountEntity.setCreation(model.getCreation());

		// PLAYER
		final Set<PlayerEntity> players = new FastSet<>();
		for (final Player player : model.getPlayers()) {
			if (linkedEntity instanceof PlayerEntity && MapperManager.getDatabaseMapper(PlayerMapper.class).equals(player, (PlayerEntity) linkedEntity)) {
				players.add((PlayerEntity) linkedEntity);
			} else {
				players.add(MapperManager.getDatabaseMapper(PlayerMapper.class).map(player, accountEntity));
			}
		}
		accountEntity.setPlayers(players);

		return accountEntity;
	}

	@Override
	public Account map(final AccountEntity model, final AbstractModel linkedModel) {
		final Account account = new Account(model.getId());

		// DIRECT
		account.setLogin(model.getLogin());
		account.setPassword(model.getPassword());
		account.setNation(model.getNation());
		account.setAuthority(AccountAuthority.from(model.getAuthority()));
		account.setAccess(model.getAccess());
		account.setCreation(model.getCreation());

		// PLAYER
		final List<Player> players = new FastList<Player>();
		for (final PlayerEntity playerEntity : model.getPlayers()) {
			if (linkedModel instanceof Player && MapperManager.getDatabaseMapper(PlayerMapper.class).equals((Player) linkedModel, playerEntity)) {
				players.add((Player) linkedModel);
			} else {
				players.add(MapperManager.getDatabaseMapper(PlayerMapper.class).map(playerEntity, account));
			}

		}
		account.setPlayers(players);

		return account;
	}

	@Override
	public boolean equals(final Account model, final AccountEntity entity) {
		return model.getLogin().equals(entity.getLogin());
	}
}
