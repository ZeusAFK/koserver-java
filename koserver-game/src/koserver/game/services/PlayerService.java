package koserver.game.services;

import java.util.Date;

import koserver.common.services.AbstractService;
import koserver.game.delegate.PlayerDelegate;
import koserver.game.models.account.Account;
import koserver.game.models.player.Player;

import org.apache.log4j.Logger;

public class PlayerService extends AbstractService {

	/** LOGGER */
	private static Logger log = Logger.getLogger(PlayerService.class.getName());

	private PlayerService() {
	}

	@Override
	public void onInit() {
		log.info("PlayerService started");
	}

	@Override
	public void onDestroy() {
		log.info("PlayerService stopped");
	}

	// ---- EVENT ---- //
	public final void onPlayerCreate(final Player player, final boolean creationSuccess) {
		// player.getConnection().sendPacket(new
		// SM_CHARACTER_CREATE(creationSuccess));
	}

	public final void onPlayerDelete(final Player player) {

	}

	public final void onPlayerConnect(final Player player) {
		player.setOnline(true);

		// ThreadPoolService.getInstance().scheduleTask(new
		// PlayerAutoSaveTask(player), PlayerConfig.PLAYER_AUTO_SAVE_DELAY,
		// TimeUnit.SECONDS);
	}

	public void onPlayerDisconnect(final Player player) {
		player.setOnline(false);
		player.setLastOnlineTime(new Date());

		// ThreadPoolService.getInstance().cancelAllTasksByLinkedObject(player);
		// ObjectIDService.getInstance().releaseId(ObjectFamilyEnum.fromClass(player.getClass()),
		// player.getUid());
	}

	public void onPlayerUpdate(final Player player) {
		PlayerDelegate.updatePlayerModel(player);
	}

	public void onPlayerCheckNameUsed(final AccountService connection, final short type, final String name) {
		// connection.sendPacket(new
		// SM_CHARACTER_USERNAME_CHECK((this.findPlayerByName(name) == null)));
	}

	// ---- EVENT ---- //

	public void registerPlayer(final AccountService connection, final Player player) {
		connection.setActivePlayer(player);
		player.setAccount(connection.getAccount());

		this.onPlayerConnect(player);
	}

	/** CRUD OPERATIONS */
	public void createPlayer(final AccountService connection, final Player player) {
		final Account account = connection.getAccount();
		player.setAccount(account);
		account.addPlayer(player);

		final boolean creationSuccess = checkPlayerRequiremens(account, player);
		if (creationSuccess) {
			PlayerDelegate.createPlayerModel(player);
		}

		this.onPlayerCreate(player, creationSuccess);
	}

	public void deletePlayer(final int playerId) {
		final Player player = PlayerDelegate.readPlayerModelById(playerId);
		if (player == null) {
			return;
		}

		player.setDeletionTime(new Date()); // TODO
		// ThreadPoolService.getInstance().scheduleTask(new
		// PlayerDeleteTask(player), PlayerConfig.PLAYER_DELETE_TIMEOUT,
		// TimeUnit.SECONDS);
		this.onPlayerDelete(player);
	}

	public Player findPlayerById(final int playerId) {
		return PlayerDelegate.readPlayerModelById(playerId);
	}

	public Player findPlayerByName(final String name) {
		return PlayerDelegate.readPlayerModelByName(name);
	}

	private boolean checkPlayerRequiremens(final Account account, final Player player) {
		boolean creationSuccess = true;

		// Admin accounts doesn't have any restrictions
		/*
		 * if (account.getAccess() == 0) { if (player.getPlayerClass() ==
		 * PlayerClassEnum.REAPER) { if
		 * (!account.haveCharacterWithLevel(PlayerConfig
		 * .PLAYER_REAPER_REQUIRE_PLAYER_MIN_LEVEL)) { creationSuccess = false;
		 * } } }
		 */
		return creationSuccess;
	}

	/** SINGLETON */
	public static PlayerService getInstance() {
		return SingletonHolder.instance;
	}

	private static class SingletonHolder {
		protected static final PlayerService instance = new PlayerService();
	}
}
