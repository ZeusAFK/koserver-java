package koserver.common.network;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;

import koserver.common.utils.ArrayUtils;

public class PacketListener {

	private BufferedInputStream bin;
	private InputStream in;

	private byte[] header;
	private byte[] length;
	private byte[] data;
	private byte[] tail;

	public PacketListener(InputStream in) {
		this.in = in;
		this.bin = new BufferedInputStream(this.in);
	}

	public Packet waitForPacket() throws IOException, Exception {
		int bytesReaded = 0;

		header = new byte[2];
		bytesReaded = bin.read(header);

		if (bytesReaded == 0 || bytesReaded == -1)
			throw new IOException("Connection closed.");

		if (ArrayUtils.byteArrayToShort(header) != 0x55AA) {
			long skippedBytesCount = bin.skip(bin.available());
			throw new Exception("Packet without header or invalid, skipping " + skippedBytesCount + " bytes remaining in buffer");
		}

		length = new byte[2];
		bin.read(length);
		int dataLength = ArrayUtils.byteArrayToShort(length);

		if (dataLength > Short.MAX_VALUE) {
			throw new Exception("Packet maximum size excceded: " + dataLength);
		}

		data = new byte[dataLength];
		int offset = 0;
		offset = bin.read(data, offset, data.length - offset);

		while (true) {
			if (offset >= dataLength) {
				break;
			}
			int readed = bin.read(data, offset, data.length - offset);
			offset += readed;
		}

		tail = new byte[2];
		bin.read(tail);

		if (tail[0] != header[1] || tail[1] != header[0]) {
			throw new Exception("Packet without tail");
		}

		Packet packet = new Packet(ArrayUtils.byteArrayToShort(new byte[]{data[0]}, 0));
		packet.setData(data);

		return packet;
	}
}