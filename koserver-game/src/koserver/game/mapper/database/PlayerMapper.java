package koserver.game.mapper.database;

import koserver.common.entity.AbstractEntity;
import koserver.common.mapper.MapperManager;
import koserver.common.mapper.database.DatabaseMapper;
import koserver.common.models.AbstractModel;
import koserver.game.database.entity.AccountEntity;
import koserver.game.database.entity.PlayerEntity;
import koserver.game.models.account.Account;
import koserver.game.models.player.Player;

public class PlayerMapper extends DatabaseMapper<PlayerEntity, Player> {

	@Override
	public PlayerEntity map(final Player model, final AbstractEntity linkedEntity) {
		final PlayerEntity playerEntity = new PlayerEntity(model.getId());

		// DIRECT
		playerEntity.setCharacterSlot(model.getCharacterSlot());
		playerEntity.setName(model.getName());
		playerEntity.setRace(model.getRace());
		playerEntity.setPlayerClass(model.getPlayerClass());
		playerEntity.setLevel(model.getLevel());
		playerEntity.setHair(model.getHair());
		playerEntity.setFace(model.getFace());
		playerEntity.setExperience(model.getExperience());
		playerEntity.setMoney(model.getMoney());
		playerEntity.setHp(model.getHp());
		playerEntity.setMp(model.getMp());
		playerEntity.setStrength(model.getStrength());
		playerEntity.setStamina(model.getStamina());
		playerEntity.setDextery(model.getDextery());
		playerEntity.setIntelligence(model.getIntelligence());
		playerEntity.setMagic(model.getMagic());
		playerEntity.setCreationTime(model.getCreationTime());
		playerEntity.setDeletionTime(model.getDeletionTime());
		playerEntity.setLastOnlineTime(model.getLastOnlineTime());
		playerEntity.setOnline(model.isOnline());
		playerEntity.setItems(model.getItems());
		playerEntity.setMapId(model.getMapId());
		playerEntity.setX(model.getX());
		playerEntity.setY(model.getY());
		playerEntity.setZ(model.getZ());

		// ACCOUNT
		if (linkedEntity instanceof AccountEntity) {
			playerEntity.setAccount((AccountEntity) linkedEntity);
		} else {
			playerEntity.setAccount(MapperManager.getDatabaseMapper(AccountMapper.class).map(model.getAccount(), playerEntity));
		}

		return playerEntity;
	}

	@Override
	public Player map(final PlayerEntity entity, final AbstractModel linkedModel) {
		final Player player = new Player(entity.getId());

		// DIRECT
		player.setCharacterSlot(entity.getCharacterSlot());
		player.setName(entity.getName());
		player.setRace(entity.getRace());
		player.setPlayerClass(entity.getPlayerClass());
		player.setLevel(entity.getLevel());
		player.setHair(entity.getHair());
		player.setFace(entity.getFace());
		player.setExperience(entity.getExperience());
		player.setMoney(entity.getMoney());
		player.setHp(entity.getHp());
		player.setMp(entity.getMp());
		player.setStrength(entity.getStrength());
		player.setStamina(entity.getStamina());
		player.setDextery(entity.getDextery());
		player.setIntelligence(entity.getIntelligence());
		player.setMagic(entity.getMagic());
		player.setCreationTime(entity.getCreationTime());
		player.setDeletionTime(entity.getDeletionTime());
		player.setLastOnlineTime(entity.getLastOnlineTime());
		player.setOnline(entity.isOnline());
		player.setItems(entity.getItems());
		player.setMapId(entity.getMapId());
		player.setX(entity.getX());
		player.setY(entity.getY());
		player.setZ(entity.getZ());

		// ACCOUNT
		if (linkedModel instanceof Account) {
			player.setAccount((Account) linkedModel);
		} else {
			player.setAccount(MapperManager.getDatabaseMapper(AccountMapper.class).map(entity.getAccount()));
		}

		return player;
	}

	@Override
	public boolean equals(final Player model, final PlayerEntity entity) {
		return model.getName().equals(entity.getName());
	}
}
