package koserver.game.database.entity;

import java.util.Date;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;
import javax.persistence.Temporal;
import javax.persistence.TemporalType;

import koserver.common.database.entity.AbstractDatabaseEntity;
import koserver.game.models.player.enums.PlayerRaceEnum;
import koserver.game.models.player.enums.PlayerClassEnum;

@Entity
@Table(name = "player")
public class PlayerEntity extends AbstractDatabaseEntity {

	private static final long serialVersionUID = 1630861678711998407L;

	@Column(name = "character_slot")
	private int characterSlot;

	@Column(unique = true)
	private String name;

	@Column
	private short race;

	@Column(name = "player_class")
	private short playerClass;

	@Column
	private int hair;

	@Column
	private int face;

	@Column(name = "player_level")
	private int level;

	@Column
	private long experience;

	@Column(name = "player_money")
	private int money;

	@Column
	private int hp;

	@Column
	private int mp;

	@Column
	private short strength;

	@Column
	private short stamina;

	@Column
	private short dextery;

	@Column
	private short intelligence;

	@Column
	private short magic;

	@Column(columnDefinition = "TIMESTAMP", name = "creation_time")
	@Temporal(TemporalType.TIMESTAMP)
	private Date creationTime;

	@Column(columnDefinition = "TIMESTAMP", name = "deletion_time")
	@Temporal(TemporalType.TIMESTAMP)
	private Date deletionTime;

	@Column(columnDefinition = "TIMESTAMP", name = "last_online_time")
	@Temporal(TemporalType.TIMESTAMP)
	private Date lastOnlineTime;

	@Column(length = 1000, name = "player_items")
	private byte[] items;

	@Column(name="map_id")
	private int mapId;

	@Column(name = "player_x")
	private float x;

	@Column(name = "player_y")
	private float y;

	@Column(name = "player_z")
	private float z;

	@Column(name = "player_online")
	private boolean online;

	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "account_id", nullable = false)
	private AccountEntity account;

	public PlayerEntity(final Integer id) {
		super(id);
	}

	public PlayerEntity() {
		super();
	}

	public int getCharacterSlot() {
		return characterSlot;
	}

	public void setCharacterSlot(int characterSlot) {
		this.characterSlot = characterSlot;
	}

	public String getName() {
		return name;
	}

	public void setName(final String name) {
		this.name = name;
	}

	public PlayerRaceEnum getRace() {
		return PlayerRaceEnum.fromValue(race);
	}

	public void setRace(final PlayerRaceEnum race) {
		this.race = (short) race.value;
	}

	public PlayerClassEnum getPlayerClass() {
		return PlayerClassEnum.fromValue(playerClass);
	}

	public void setPlayerClass(final PlayerClassEnum playerClass) {
		this.playerClass = (short) playerClass.value;
	}

	public int getLevel() {
		return level;
	}

	public void setLevel(final int i) {
		this.level = i;
	}

	public long getExperience() {
		return experience;
	}

	public void setExperience(final long experience) {
		this.experience = experience;
	}

	public int getMoney() {
		return money;
	}

	public void setMoney(final int money) {
		this.money = money;
	}

	public int getHp() {
		return hp;
	}

	public void setHp(final int hp) {
		this.hp = hp;
	}

	public int getMp() {
		return mp;
	}

	public void setMp(final int mp) {
		this.mp = mp;
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

	public byte[] getItems() {
		return items;
	}

	public void setItems(final byte[] items) {
		this.items = items;
	}

	public int getMapId() {
		return mapId;
	}

	public void setMapId(final int mapId) {
		this.mapId = mapId;
	}

	public float getX() {
		return x;
	}

	public void setX(final float x) {
		this.x = x;
	}

	public float getY() {
		return y;
	}

	public void setY(final float y) {
		this.y = y;
	}

	public float getZ() {
		return z;
	}

	public void setZ(final float z) {
		this.z = z;
	}

	public boolean isOnline() {
		return online;
	}

	public void setOnline(final boolean online) {
		this.online = online;
	}

	public AccountEntity getAccount() {
		return account;
	}

	public void setAccount(final AccountEntity account) {
		this.account = account;
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

	@Override
	public int hashCode() {
		final int prime = 31;
		final int result = prime * ((name == null) ? 0 : name.hashCode());
		return result;
	}

	@Override
	public boolean equals(final Object obj) {
		if (this == obj) {
			return true;
		}
		if (!(obj instanceof PlayerEntity)) {
			return false;
		}
		final PlayerEntity other = (PlayerEntity) obj;
		if (name == null) {
			if (other.name != null) {
				return false;
			}
		} else if (!name.equals(other.name)) {
			return false;
		}
		return true;
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
}
