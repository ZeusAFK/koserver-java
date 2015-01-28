package koserver.game.models.player.enums;

import org.apache.log4j.Logger;

public enum PlayerRaceEnum {
	UNKNOW(0), EL_MORAD_BARBARIAN(11), EL_MORAD_MALE(12), EL_MORAD_FEMALE(13), KARUS_ARCH_TUAREK(1), KARUS_TUAREK(2), KARUS_WRINKLE_TUAREK(3), KARUS_PURI_TUAREK(4);

	public final int value;

	PlayerRaceEnum(final int value) {
		this.value = value;
	}

	public static PlayerRaceEnum fromValue(final int value) {
		for (final PlayerRaceEnum race : PlayerRaceEnum.values()) {
			if (race.value == value) {
				return race;
			}
		}

		Logger.getLogger(PlayerRaceEnum.class.getName()).error("Can't find NationEnum with value " + value);
		return null;
	}
}
