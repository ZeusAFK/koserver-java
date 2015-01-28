package koserver.game.models.player;

import java.util.Date;

import koserver.common.models.AbstractModel;
import koserver.game.models.account.Account;
import koserver.game.models.player.enums.PlayerRaceEnum;
import koserver.game.models.player.enums.PlayerClassEnum;
import koserver.game.services.AccountService;
import static koserver.game.enums.GameSystem.*;

public class Player extends AbstractModel {

	private int characterSlot;
	private String name;
	private PlayerRaceEnum race;
	private PlayerClassEnum playerClass;
	private int hair;
	private int face;
	private int level;
	private long experience;
	private int money;
	private int hp;
	private int mp;
	private short strength;
	private short stamina;
	private short dextery;
	private short intelligence;
	private short magic;
	private Date creationTime;
	private Date deletionTime;
	private Date lastOnlineTime;
	private boolean online;
	private byte[] items;
	private int mapId;
	private float x;
	private float y;
	private float z;
	private Account account;

	public Player(Integer id) {
		super(id);
	}

	public Player() {
		this(null);
	}

	public String getName() {
		return name;
	}

	public void setName(final String name) {
		this.name = name;
	}

	public PlayerRaceEnum getRace() {
		return race;
	}

	public void setRace(final PlayerRaceEnum race) {
		this.race = race;
	}

	public PlayerClassEnum getPlayerClass() {
		return playerClass;
	}

	public void setPlayerClass(final PlayerClassEnum playerClass) {
		this.playerClass = playerClass;
	}

	public long getExperience() {
		return experience;
	}

	public void setExperience(final long experience) {
		this.experience = experience;
	}

	public void addExperience(final int experience) {
		this.experience += experience;
	}

	public int getMoney() {
		return money;
	}

	public void setMoney(final int money) {
		this.money = money;
	}

	public void addMoney(final int money) {
		this.money += money;
	}

	public void removeMoney(final int money) {
		this.money -= money;
	}

	public Date getCreationTime() {
		return creationTime;
	}

	public void setCreationTime(final Date creationTime) {
		this.creationTime = creationTime;
	}

	public Date getDeletionTime() {
		return deletionTime;
	}

	public void setDeletionTime(final Date deletionTime) {
		this.deletionTime = deletionTime;
	}

	public Date getLastOnlineTime() {
		return lastOnlineTime;
	}

	public void setLastOnlineTime(final Date lastOnlineTime) {
		this.lastOnlineTime = lastOnlineTime;
	}

	public boolean isOnline() {
		return online;
	}

	public void setOnline(final boolean online) {
		this.online = online;
	}

	public byte[] getItems() {
		if (items == null) {
			return new byte[INVENTARY_TOTAL_SLOTS * 8];
		}
		return items;
	}

	public void setItems(final byte[] items) {
		this.items = items;
	}

	public Account getAccount() {
		return account;
	}

	public void setAccount(final Account account) {
		this.account = account;
	}

	public int getHp() {
		return hp;
	}

	public void setHp(int hp) {
		this.hp = hp;
	}

	public int getMp() {
		return mp;
	}

	public void setMp(int mp) {
		this.mp = mp;
	}

	public short getStrength() {
		return strength;
	}

	public void setStrength(short strength) {
		this.strength = strength;
	}

	public short getStamina() {
		return stamina;
	}

	public void setStamina(short stamina) {
		this.stamina = stamina;
	}

	public short getDextery() {
		return dextery;
	}

	public void setDextery(short dextery) {
		this.dextery = dextery;
	}

	public short getIntelligence() {
		return intelligence;
	}

	public void setIntelligence(short intelligence) {
		this.intelligence = intelligence;
	}

	public short getMagic() {
		return magic;
	}

	public void setMagic(short magic) {
		this.magic = magic;
	}

	public final AccountService getConnection() {
		return this.getAccount().getConnection();
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		return result;
	}

	@Override
	public boolean equals(final Object obj) {
		if (this == obj) {
			return true;
		}
		if (!super.equals(obj)) {
			return false;
		}
		if (obj == null) {
			return false;
		}
		if (!(obj instanceof Player)) {
			return false;
		}
		final Player other = (Player) obj;
		if (!name.equals(other.name)) {
			return false;
		}
		return true;
	}

	public int getLevel() {
		return level;
	}

	public void setLevel(int level) {
		this.level = level;
	}

	public int getHair() {
		return hair;
	}

	public void setHair(int hair) {
		this.hair = hair;
	}

	public int getFace() {
		return face;
	}

	public void setFace(int face) {
		this.face = face;
	}

	public int getCharacterSlot() {
		return characterSlot;
	}

	public void setCharacterSlot(int characterSlot) {
		this.characterSlot = characterSlot;
	}

	public int getMapId() {
		return mapId;
	}

	public void setMapId(int mapId) {
		this.mapId = mapId;
	}

	public float getX() {
		return x;
	}

	public void setX(float x) {
		this.x = x;
	}

	public float getY() {
		return y;
	}

	public void setY(float y) {
		this.y = y;
	}

	public float getZ() {
		return z;
	}

	public void setZ(float z) {
		this.z = z;
	}
}
