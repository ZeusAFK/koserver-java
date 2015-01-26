package koserver.common.network;

import koserver.common.utils.ArrayUtils;

public class Packet {

	private int opcode;
	private byte[] data;
	private int position;

	public Packet(int opcode) {
		this.opcode = opcode;
		data = new byte[0];
		position = 0;
	}

	public void setData(byte[] data) {
		this.data = data;
	}

	public byte[] getData() {
		return data;
	}

	public int getOpcode() {
		return opcode;
	}

	public void appendData(byte[] data) {
		byte[] newData = new byte[this.data.length + data.length];
		System.arraycopy(this.data, 0, newData, 0, this.data.length);
		System.arraycopy(data, 0, newData, this.data.length, data.length);
		this.data = newData;
	}

	public void appendByteArray(byte[] objectData) {
		appendData(ArrayUtils.intToByteArray(objectData.length));
		appendData(objectData);
	}

	public byte[] getByteArray() {
		try {
			int length = ArrayUtils.byteArrayToInt(data, position, position + 4);
			position += 4;
			byte[] byteArray = ArrayUtils.arraySubset(data, position, position + length);
			position += length;
			
			return byteArray;
		} catch (Exception e) {
			return null;
		}
	}

	public void appendString(String data) {
		appendData(ArrayUtils.shortToByteArray((short) data.length()));
		appendData(data.getBytes());
	}

	public String getString() {
		try {
			short length = ArrayUtils.byteArrayToShort(data, position, position + 2);
			position += 2;
			String value = ArrayUtils.hexToString(ArrayUtils.byteArrayToHex(data, position, position + length));
			position += length;

			return value;
		} catch (Exception ex) {
			return "";
		}
	}

	public void appendShort(short data) {
		appendData(ArrayUtils.shortToByteArray(data));
	}

	public short getShort() {
		try {
			short value = ArrayUtils.byteArrayToShort(data, position, position + 2);
			position += 2;
			return value;
		} catch (Exception ex) {
			return -1;
		}
	}

	public void appendInt(int data) {
		appendData(ArrayUtils.intToByteArray(data));
	}

	public void appendInt8(int data) {
		appendData(new byte[] { (byte) data });
	}
}
