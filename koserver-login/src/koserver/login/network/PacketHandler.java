package koserver.login.network;

import java.io.IOException;
import java.math.BigInteger;
import java.util.List;

import org.apache.log4j.Logger;

import koserver.common.network.Packet;
import koserver.common.network.crypt.CryptorKey;
import koserver.common.utils.ArrayUtils;
import koserver.common.utils.HexDump;
import koserver.login.configuration.LoginConfig;
import koserver.login.database.entity.PatchEntity;
import koserver.login.database.entity.ServerEntity;
import koserver.login.delegate.PatchListDelegate;
import koserver.login.delegate.ServerListDelegate;
import koserver.login.services.AccountService;
import static koserver.login.enums.Opcodes.*;

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
		case LS_VERSION_REQ:
			VersionRequestHandler(packet);
			break;
		case LS_DOWNLOADINFO_REQ:
			DownloadInfoRequestHandler(packet);
			break;
		case LS_CRYPTION:
			EncriptionPublicKeyRequestHandler(packet);
			break;
		case LS_LOGIN:
			LoginRequestHandler(packet);
			break;
		case LS_SERVERLIST:
			ServerListRequestHandler(packet);
			break;
		case LS_NEWS:
			NewsHandler(packet);
			break;
		case LS_UNKNOW_F7:
			UnknowF7Handler(packet);
			break;
		default:
			log.warn("Unknow packet opcode> " + packet.getOpcode());
			log.warn(HexDump.dumpHexString(packet.getData()));
		}
	}

	private void VersionRequestHandler(Packet packet) {
		packet = new Packet(LS_VERSION_REQ);
		packet.appendShort((short) LoginConfig.LOGIN_VERSION);
		packetWriter.sendPacket(packet);
	}

	private void DownloadInfoRequestHandler(Packet packet) {
		int version = ArrayUtils.byteArrayToShort(packet.getData());
		packet = new Packet(LS_DOWNLOADINFO_REQ);
		packet.appendString(LoginConfig.LOGIN_FTP_URL);
		packet.appendString(LoginConfig.LOGIN_FTP_PATH);
		List<PatchEntity> patchList = PatchListDelegate.getPatchs();
		for (PatchEntity patch : patchList) {
			if (patch.getVersion() < version) {
				patchList.remove(patch);
			}
		}
		packet.appendShort((short) patchList.size());
		for (PatchEntity patch : patchList) {
			packet.appendString(patch.getFileName());
		}
		packetWriter.sendPacket(packet);
	}

	private void EncriptionPublicKeyRequestHandler(Packet packet) {
		BigInteger publicKey = CryptorKey.generateKey();
		String keyString = publicKey.toString(16);
		byte[] keyData = ArrayUtils.hexStringToByteArray(keyString);
		keyData = ArrayUtils.byteArrayReverse(keyData);
		packet = new Packet(LS_CRYPTION);
		packet.appendData(keyData);
		packetWriter.sendPacket(packet);
		accountService.setPublicKey(publicKey);
	}

	private void LoginRequestHandler(Packet packet) {
		String login = packet.getString();
		String password = packet.getString();
		int result = accountService.authorizeAccount(login, password);
		packet = new Packet(LS_LOGIN);
		packet.appendInt8(result);
		packet.appendShort((short) 7857);
		packet.appendString(login);
		packetWriter.sendPacket(packet);
	}

	private void ServerListRequestHandler(Packet packet) {
		short echo = packet.getShort();
		List<ServerEntity> serverList = ServerListDelegate.getServers();
		packet = new Packet(LS_SERVERLIST);
		packet.appendShort(echo);
		packet.appendInt8(serverList.size());
		for (ServerEntity server : serverList) {
			packet.appendString(server.getIp());
			packet.appendString(server.getName());
			packet.appendShort(server.getUserCount());
			packet.appendShort((short) (int) server.getId());
			packet.appendShort((short) server.getCategory());
			packet.appendShort(server.getUserMax());
			packet.appendShort(server.getUserMaxFree());
			packet.appendInt8(0);
			// TODO: King name and notice not implemented
			packet.appendString("");
			packet.appendString("");
			packet.appendString("");
			packet.appendString("");
		}
		packetWriter.sendPacket(packet);
	}

	private void NewsHandler(Packet packet) {
		packet = new Packet(LS_NEWS);
		packet.appendString("LoginNotice");
		packet.appendString("<empty>");
		packetWriter.sendPacket(packet);
	}

	private void UnknowF7Handler(Packet packet) {
		packet = new Packet(LS_UNKNOW_F7);
		packet.appendInt8(0);
		packetWriter.sendPacket(packet);
	}
}
