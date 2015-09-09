package koserver.game;

import koserver.common.utils.PrintUtils;
import koserver.game.configuration.GameConfig;
import koserver.game.services.ConfigurationService;
import koserver.game.services.DatabaseService;
import koserver.game.services.NetworkService;
import koserver.game.services.PlayerService;
import koserver.game.services.ThreadPoolService;

public class MainGame {

	public static void main(String[] args) {
		final long start = System.currentTimeMillis();

		PrintUtils.printSection("Services");
		ConfigurationService.getInstance().start();
		DatabaseService.getInstance().start();
		NetworkService.getInstance().start();
		PlayerService.getInstance().start();
		ThreadPoolService.getInstance().start();
		PrintUtils.printSection("Launching game server version: " + GameConfig.GAME_VERSION);

		System.out.println("Server started in " + ((System.currentTimeMillis() - start) / 1000) + " seconds");
	}
}
