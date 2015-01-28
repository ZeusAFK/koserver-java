package koserver.game.configuration;

import koserver.common.config.Property;

public class PlayerConfig {

	@Property(key = "player.name.pattern", defaultValue = "[A-Za-z]*")
	public static String PLAYER_NAME_PATTERN;

	@Property(key = "player.max.level", defaultValue = "60")
	public static int PLAYER_MAX_LEVEL;

	@Property(key = "player.auto.save.delay", defaultValue = "900")
	public static int PLAYER_AUTO_SAVE_DELAY;

	@Property(key = "player.gain.hp.delay", defaultValue = "5")
	public static int PLAYER_GAIN_HP_DELAY;

	@Property(key = "player.gain.mp.delay", defaultValue = "5")
	public static int PLAYER_GAIN_MP_DELAY;
}