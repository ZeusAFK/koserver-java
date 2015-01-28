package koserver.game.delegate;

import java.lang.reflect.InvocationTargetException;

import org.apache.commons.beanutils.BeanUtils;

import koserver.common.database.dao.DAOManager;
import koserver.common.mapper.MapperManager;
import koserver.game.database.dao.PlayerDAO;
import koserver.game.database.entity.PlayerEntity;
import koserver.game.mapper.database.PlayerMapper;
import koserver.game.models.player.Player;
import koserver.game.services.AccountService;

public class PlayerDelegate {

	// ------------------------------------------------------------
	// PlayerEntity
	// ------------------------------------------------------------
	public static void createPlayerEntity(final PlayerEntity entity) {
		DAOManager.getDAO(PlayerDAO.class).create(entity);
	}

	public static PlayerEntity readPlayerEntityById(final Integer id) {
		return DAOManager.getDAO(PlayerDAO.class).read(id);
	}

	public static PlayerEntity readPlayerEntityByName(final String name) {
		return DAOManager.getDAO(PlayerDAO.class).readByName(name);
	}

	public static void updatePlayerEntity(final PlayerEntity entity) {
		DAOManager.getDAO(PlayerDAO.class).update(entity);
	}

	public static void deletePlayerEntity(final PlayerEntity entity) {
		DAOManager.getDAO(PlayerDAO.class).delete(entity);
	}

	// ------------------------------------------------------------
	// Player
	// ------------------------------------------------------------
	public static void createPlayerModel(final Player model) {
		final PlayerEntity playerEntity = MapperManager.getDatabaseMapper(PlayerMapper.class).map(model);
		PlayerDelegate.createPlayerEntity(playerEntity);

		PlayerDelegate.copy(model, MapperManager.getDatabaseMapper(PlayerMapper.class).map(playerEntity));
	}

	public static Player readPlayerModelById(final Integer id) {
		Player player = null;
		final PlayerEntity playerEntity = PlayerDelegate.readPlayerEntityById(id);
		if (playerEntity != null) {
			player = MapperManager.getDatabaseMapper(PlayerMapper.class).map(playerEntity);
		}
		return player;
	}

	public static Player readPlayerModelByName(final String name) {
		Player player = null;
		final PlayerEntity playerEntity = PlayerDelegate.readPlayerEntityByName(name);
		if (playerEntity != null) {
			player = MapperManager.getDatabaseMapper(PlayerMapper.class).map(playerEntity);
		}
		return player;
	}

	public static void updatePlayerModel(final Player model) {
		final PlayerEntity playerEntity = MapperManager.getDatabaseMapper(PlayerMapper.class).map(model);
		PlayerDelegate.updatePlayerEntity(playerEntity);

		// TODO we should call each DAO for each subdata
		PlayerDelegate.copy(model, MapperManager.getDatabaseMapper(PlayerMapper.class).map(playerEntity));
	}

	public static void deletePlayerModel(final Player model) {
		final PlayerEntity playerEntity = MapperManager.getDatabaseMapper(PlayerMapper.class).map(model);
		PlayerDelegate.deletePlayerEntity(playerEntity);
	}

	// ------------------------------------------------------------
	// Common
	// ------------------------------------------------------------
	private static void copy(final Player modelDest, final Player modelOrig) {
		final AccountService connection = modelDest.getConnection();

		try {
			BeanUtils.copyProperties(modelDest, modelOrig);
		} catch (IllegalAccessException | InvocationTargetException e) {
			e.printStackTrace();
		}

		modelDest.getAccount().setConnection(connection);
	}
}
