package koserver.game.models.player.enums;

import org.apache.log4j.Logger;

public enum PlayerClassEnum {
	KARUS_WARRIOR_BEGGINER(101), KARUS_ROGUE_BEGGINER(102), KARUS_MAGICIAN_BEGGINER(103), KARUS_PRIEST_BEGGINER(104), KARUS_WARRIOR_SKILLED(105), KARUS_WARRIOR_MASTERED(106), KARUS_ROGUE_SKILLED(107), KARUS_ROGUE_MASTERED(108), KARUS_MAGICIAN_SKILLED(
			109), KARUS_MAGICIAN_MASTERED(110), KARUS_PRIEST_SKILLED(111), KARUS_PRIEST_MASTERED(112), EL_MORAD_WARRIOR_BEGGINER(201), EL_MORAD_ROGUE_BEGGINER(202), EL_MORAD_MAGICIAN_BEGGINER(203), EL_MORAD_PRIEST_BEGGINER(204), EL_MORAD_WARRIOR_SKILLED(
			205), EL_MORAD_WARRIOR_MASTERED(206), EL_MORAD_ROGUE_SKILLED(207), EL_MORAD_ROGUE_MASTERED(208), EL_MORAD_MAGICIAN_SKILLED(209), EL_MORAD_MAGICIAN_MASTERED(210), EL_MORAD_PRIEST_SKILLED(211), EL_MORAD_PRIEST_MASTERED(212);

	public final int value;

	PlayerClassEnum(final int value) {
		this.value = value;
	}

	public static PlayerClassEnum fromValue(final int value) {
		for (final PlayerClassEnum playerClass : PlayerClassEnum.values()) {
			if (playerClass.value == value) {
				return playerClass;
			}
		}

		Logger.getLogger(PlayerClassEnum.class.getName()).error("Can't find PlayerClassEnum with value " + value);
		return null;
	}
}
