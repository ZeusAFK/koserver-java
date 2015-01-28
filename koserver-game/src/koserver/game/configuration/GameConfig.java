package koserver.game.configuration;

import koserver.common.config.Property;

public class GameConfig {
	@Property(key = "gameserver.version", defaultValue = "1860")
    public static int GAME_VERSION;
}
