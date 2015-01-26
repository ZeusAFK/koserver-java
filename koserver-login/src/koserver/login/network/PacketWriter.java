package koserver.login.network;

import java.io.BufferedOutputStream;
import java.io.IOException;

import org.apache.log4j.Logger;

import koserver.common.network.Packet;
import koserver.common.utils.ArrayUtils;
import koserver.login.services.AccountService;

public class PacketWriter {

	private static Logger log = Logger.getLogger(PacketWriter.class.getName());
	private AccountService clientService;
	private BufferedOutputStream bout;

	public PacketWriter(AccountService clientService) throws IOException {
		this.clientService = clientService;
		this.bout = new BufferedOutputStream(clientService.getSocket().getOutputStream());
	}

	public boolean sendPacket(Packet packet) {
		try {
			byte[] header = new byte[] { -86, 85 };
			byte[] data = packet.getData();
			byte[] opcodeBytes = new byte[] { (byte) packet.getOpcode() };
			byte[] tail = new byte[] { 85, -86 };
			
			packet.setData(opcodeBytes);
			packet.appendData(data);
			data = packet.getData();
			
			if (clientService.isCryptoEnabled()) {
				packet.setData(ArrayUtils.shortToByteArray((short) 0x1EFC));
				packet.appendShort(clientService.getPacketSecuenceId());
				packet.appendInt8(0);
				packet.appendData(data);
				
				data = clientService.getCryptor().EncryptionWithCRC32(packet.getData());
				if (data != null) {
					packet.setData(data);
				}
			}
			
			byte[] length = ArrayUtils.shortToByteArray((short) (data.length));

			packet.setData(header);
			packet.appendData(length);
			packet.appendData(data);
			packet.appendData(tail);

			byte[] completePacketData = packet.getData();

			bout.write(completePacketData);
			bout.flush();
		} catch (IOException e) {
			log.warn("Failed to send packet: " + e.getMessage());
			return false;
		}

		return true;
	}
}
