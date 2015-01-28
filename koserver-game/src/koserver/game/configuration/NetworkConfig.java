package koserver.game.configuration;

import koserver.common.config.Property;

public class NetworkConfig {
    @Property(key = "gameserver.network.bind.host", defaultValue = "*")
    public static String GAME_BIND_ADDRESS;

    @Property(key = "gameserver.network.bind.port", defaultValue = "80")
    public static int GAME_BIND_PORT;
}
