package koserver.game.network;

import java.io.IOException;
import java.math.BigInteger;

import org.apache.log4j.Logger;

import koserver.common.network.Packet;
import koserver.common.network.crypt.CryptorKey;
import koserver.common.utils.ArrayUtils;
import koserver.common.utils.ByteArrayReader;
import koserver.common.utils.HexDump;
import koserver.game.configuration.GameConfig;
import koserver.game.delegate.AccountDelegate;
import koserver.game.enums.GameSystem;
import koserver.game.models.player.Player;
import koserver.game.models.player.enums.PlayerRaceEnum;
import koserver.game.models.player.enums.PlayerClassEnum;
import koserver.game.services.AccountService;
import koserver.game.services.PlayerService;
import static koserver.game.enums.Opcodes.*;
import static koserver.game.enums.GameSystem.*;
import static koserver.common.enums.LoginResult.*;

public class PacketHandler {

	private static Logger log = Logger.getLogger(PacketHandler.class.getName());

	private AccountService accountService;
	private PacketWriter packetWriter;

	public void Initialize(AccountService accountService) {
		this.accountService = accountService;
		try {
			packetWriter = new PacketWriter(accountService);
		} catch (IOException e) {
			log.warn("Failed to prepare client output stream");
		}
	}

	public void handlePacket(Packet packet) {
		switch (packet.getOpcode()) {
		case GS_VERSION_CHECK:
			ClientVersionCheckHandler(packet);
			break;
		case GS_LOGIN:
			LoginRequestHandler(packet);
			break;
		case GS_NATION_SELECT:
			SelectNationRequestHandler(packet);
			break;
		case GS_CREATE_CHARACTER:
			CharacterCreationHandler(packet);
			break;
		case GS_ALLCHAR_INFO_REQ:
			AllCharacterInforRequestHandler(packet);
			break;
		case GS_SPEEDHACK_CHECK:
			SpeedHackCheckRequestHandler(packet);
			break;
		case GS_SELECT_CHARACTER:
			SelectCharacterRequestHandler(packet);
		default:
			log.warn("Unknow packet opcode> " + packet.getOpcode());
			log.warn(HexDump.dumpHexString(packet.getData()));
		}
	}

	private void ClientVersionCheckHandler(Packet packet) {
		packet = new Packet(GS_VERSION_CHECK);
		packet.appendInt8(0);
		packet.appendShort((short) GameConfig.GAME_VERSION);
		BigInteger publicKey = CryptorKey.generateKey();
		String keyString = publicKey.toString(16);
		byte[] keyData = ArrayUtils.hexStringToByteArray(keyString);
		keyData = ArrayUtils.byteArrayReverse(keyData);
		packet.appendData(keyData);
		packet.appendInt8(0);
		packetWriter.sendPacket(packet);
		accountService.setPublicKey(publicKey);
	}

	private void LoginRequestHandler(Packet packet) {
		if (accountService.getAccount() != null && !accountService.getAccount().getLogin().isEmpty()) {
			return;
		}
		String login = packet.getString();
		String password = packet.getString();
		if (accountService.authorizeAccount(login, password) != AUTH_SUCCESS) {
			return;
		}
		int nation = accountService.getAccount().getNation();
		packet = new Packet(GS_LOGIN);
		packet.appendInt8(nation);
		packetWriter.sendPacket(packet);
	}

	private void SelectNationRequestHandler(Packet packet) {
		short nation = packet.getInt8();
		packet = new Packet(GS_NATION_SELECT);
		if (nation == GameSystem.ELMORAD_ID || nation == GameSystem.KARUS_ID) {
			accountService.getAccount().setNation(nation);
			packet.appendInt8(nation);
		} else {
			packet.appendInt8(0);
		}
		AccountDelegate.updateAccountModel(accountService.getAccount());
		packetWriter.sendPacket(packet);
	}

	private void AllCharacterInforRequestHandler(Packet packet) {
		packet = new Packet(GS_ALLCHAR_INFO_REQ);
		packet.appendInt8(1);

		int loadedPlayers = 0;
		for (Player player : accountService.getAccount().getPlayers()) {
			loadedPlayers++;
			packet.appendString(player.getName());
			packet.appendInt8(player.getRace().value);
			packet.appendShort((short) player.getPlayerClass().value);
			packet.appendInt8(player.getLevel());
			packet.appendInt8(player.getFace());
			packet.appendInt(player.getHair());
			packet.appendInt8(player.getMapId());

			ByteArrayReader baReader = new ByteArrayReader(player.getItems());
			for (int i = 0; i < EQUIPED_SLOTS; i++) {
				int itemId = baReader.getInt();
				short durability = baReader.getShort();
				@SuppressWarnings("unused")
				short count = baReader.getShort();
				if (i == HEAD || i == BREAST || i == SHOULDER || i == LEG || i == GLOVE || i == FOOT || i == RIGHTHAND || i == LEFTHAND) {
					packet.appendInt(itemId);
					packet.appendShort(durability);
				}
			}
		}
		packet.appendData(new byte[60 * (3 - loadedPlayers)]);
		packetWriter.sendPacket(packet);
	}

	private void CharacterCreationHandler(Packet packet) {
		int slot = packet.getInt8();
		String name = packet.getString();
		short race = packet.getInt8();
		short playerClass = packet.getShort();
		short face = packet.getInt8();
		int hair = packet.getInt();
		short strength = packet.getInt8();
		short stamina = packet.getInt8();
		short dextery = packet.getInt8();
		short intelligence = packet.getInt8();
		short magic = packet.getInt8();

		Player player = new Player();
		player.setCharacterSlot(slot);
		player.setName(name);
		player.setRace(PlayerRaceEnum.fromValue(race));
		player.setPlayerClass(PlayerClassEnum.fromValue(playerClass));
		player.setFace(face);
		player.setHair(hair);
		player.setStrength(strength);
		player.setStamina(stamina);
		player.setDextery(dextery);
		player.setIntelligence(intelligence);
		player.setMagic(magic);
		// TODO: Add player starting level to config file
		player.setLevel(1);

		PlayerService.getInstance().createPlayer(accountService, player);

		int result = 0;
		packet = new Packet(GS_CREATE_CHARACTER);
		packet.appendInt8(result);
		packetWriter.sendPacket(packet);
	}

	private void SpeedHackCheckRequestHandler(Packet packet) {
		// TODO: Implement speck hack check handler
	}

	private void SelectCharacterRequestHandler(Packet packet) {
		String AccountId = packet.getString();
		String UserId = packet.getString();
		short bInit = packet.getShort();
		packet = new Packet(GS_SELECT_CHARACTER);
		for (Player player : accountService.getAccount().getPlayers()) {
			Player connectedPlayer = PlayerService.getInstance().findPlayerByName(UserId);
			if (connectedPlayer != null && connectedPlayer.isOnline() && !connectedPlayer.getAccount().getConnection().getSocket().equals(accountService.getSocket())) {
				PlayerService.getInstance().registerPlayer(accountService, player);
				packet.appendShort(bInit);
				packetWriter.sendPacket(packet);
				return;
			} else if (player.getAccount().getLogin().equals(AccountId) && player.getName().equals(UserId)) {
				packet.appendString(UserId);
				packet.appendShort(bInit);
				packetWriter.sendPacket(packet);
				return;
			}
		}
	}
}
