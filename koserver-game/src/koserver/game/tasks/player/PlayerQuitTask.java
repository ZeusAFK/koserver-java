package koserver.game.tasks.player;

import koserver.game.models.account.Account;
import koserver.game.models.player.Player;
import koserver.game.services.AccountService;
import koserver.game.services.PlayerService;
import koserver.game.tasks.AbstractTask;
import koserver.game.tasks.TaskTypeEnum;

public class PlayerQuitTask extends AbstractTask<Player> {

	public PlayerQuitTask(final Player linkedObject) {
		super(linkedObject, TaskTypeEnum.PLAYER_QUIT);
	}

	@Override
	public void execute() {
		final Player player = this.linkedObject;
		if (player == null) {
			return;
		}

		final Account account = player.getAccount();
		if (account == null) {
			return;
		}

		final AccountService connection = account.getConnection();
		if (connection == null) {
			return;
		}

		PlayerService.getInstance().onPlayerDisconnect(player);
	}
}
