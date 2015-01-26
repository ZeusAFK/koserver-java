package koserver.common.utils;

import java.io.ByteArrayOutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.nio.ByteBuffer;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.zip.GZIPOutputStream;

import org.apache.log4j.Logger;

public class ArrayUtils {
	private static Logger log = Logger.getLogger(ArrayUtils.class.getName());

	public static String byteArrayToHex(byte[] a) {
		StringBuilder sb = new StringBuilder();
		for (byte b : a) {
			sb.append(String.format("%02x", b & 0xff));
		}
		return sb.toString().toUpperCase();
	}

	public static byte[] byteArrayReverse(byte[] data) {
		int i = 0;
		int j = data.length - 1;
		byte tmp;
		while (j > i) {
			tmp = data[j];
			data[j] = data[i];
			data[i] = tmp;
			j--;
			i++;
		}
		return data;
	}

	public static String byteToHex(byte b) {
		return String.format("%02X", b);
	}

	public static byte intToByte(int n) {
		return (byte) (n & 0xFF);
	}

	public static String intToHex(int n) {
		return Integer.toHexString(n);
	}

	public static String byteArrayToString(byte[] b) {
		try {
			return (new String(b, "UTF-8"));
		} catch (UnsupportedEncodingException ex) {
			return new String();
		}
	}

	public static String byteArrayToHex(byte[] data, int start, int end) {
		StringBuilder sb = new StringBuilder();
		for (int i = start; i < end; i++) {
			sb.append(String.format("%02x", data[i] & 0xFF));
		}
		return sb.toString().toUpperCase();
	}

	public static short byteArrayToShort(byte[] data) {
		return (short) ((data[1] << 8) + (data[0] & 0xff));
	}

	public static short byteArrayToShort(byte[] data, int startPosition) {
		short value = 0;
		for (int i = 0; i < data.length - startPosition; i++) {
			value += ((short) data[i + startPosition] & 0xffL) << (8 * i);
		}
		return value;
	}

	public static byte byteArrayToByte(byte[] data, int position) {
		return data[position - 1];
	}

	public static short byteArrayToShort(byte[] data, int start, int end) {
		short value = 0;
		int n = 0;
		for (int i = start; i < end; i++) {
			value += ((short) data[i] & 0xFFL) << (8 * n++);
		}
		return value;
	}

	public static int byteArrayToInt(byte[] data, int start, int end) {
		int value = 0;
		int n = 0;
		for (int i = start; i < end; i++) {
			value += (data[i] & 0xFFL) << (8 * n++);
		}
		return value;
	}

	public static int byteArrayToIntRight(byte[] data) {
		int value = 0;
		for (int i = 0; i < data.length; i++) {
			value = (value << 8) + (data[i] & 0xff);
		}
		return value;
	}

	public static int byteArrayToInt(byte[] b) {
		return b[0] & 0xFF | (b[1] & 0xFF) << 8 | (b[2] & 0xFF) << 16 | (b[3] & 0xFF) << 24;
	}

	public static byte[] shortToByteArray(short value) {
		byte[] ret = new byte[2];
		ret[0] = (byte) (value & 0xff);
		ret[1] = (byte) ((value >> 8) & 0xff);
		return ret;
	}

	public static byte[] intToByteArray(int value) {
		byte[] ret = new byte[4];
		ret[0] = (byte) (value & 0xff);
		ret[1] = (byte) ((value >> 8) & 0xff);
		ret[2] = (byte) ((value >> 16) & 0xff);
		ret[3] = (byte) ((value >> 24) & 0xff);
		return ret;
	}

	public static byte[] longToByteArray(long value) {
		ByteBuffer buffer = ByteBuffer.allocate(8);
		buffer.putLong(value);
		return buffer.array();
	}

	public static String hexToString(String hex) {
		StringBuilder output = new StringBuilder();
		for (int i = 0; i < hex.length(); i += 2) {
			String str = hex.substring(i, i + 2);
			output.append((char) Integer.parseInt(str, 16));
		}
		return output.toString();
	}

	public static byte[] hexStringToByteArray(String s) {
		int len = s.length();
		byte[] data = new byte[len / 2];
		for (int i = 0; i < len; i += 2) {
			data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4) + Character.digit(s.charAt(i + 1), 16));
		}
		return data;
	}

	public static byte[] arrayCopy(byte[] source, byte[] target, int start, int length) {
		System.arraycopy(source, start, target, 0, length);
		return target;
	}

	public static byte[] arraySubset(byte[] source, int srcBegin, int srcEnd) {
		byte destination[];

		destination = new byte[srcEnd - srcBegin];
		getBytes(source, srcBegin, srcEnd, destination, 0);

		return destination;
	}

	public static void getBytes(byte[] source, int srcBegin, int srcEnd, byte[] destination, int dstBegin) {
		System.arraycopy(source, srcBegin, destination, dstBegin, srcEnd - srcBegin);
	}

	public static byte[] compressByteArray(byte[] stream) {
		ByteArrayOutputStream compressedStream = new ByteArrayOutputStream(stream.length);
		try {
			GZIPOutputStream zipStream = new GZIPOutputStream(compressedStream);
			zipStream.write(stream);
			zipStream.close();
			compressedStream.close();
			return compressedStream.toByteArray();
		} catch (IOException e) {
			return null;
		}
	}

	public static String SHAsum(byte[] data) {
		try {
			MessageDigest md = MessageDigest.getInstance("SHA-1");
			return byteArrayToHex(md.digest(data));
		} catch (NoSuchAlgorithmException e) {
			return "";
		}
	}

	public static boolean storeToFile(String file, byte[] data) {
		try {
			OutputStream out = new FileOutputStream(file);
			out.write(data);
			out.close();
			return true;
		} catch (IOException e) {
			log.warn("ArrayUtils::storeToFile error: " + e.getMessage());
			return false;
		}
	}

	public static byte[] readFromFile(String file) {
		try {
			// File fi = new File(file);
			// return Files.readAllBytes(fi.toPath());

			InputStream in = new FileInputStream(file);
			byte[] buffer = new byte[in.available()];
			in.read(buffer);
			in.close();
			return buffer;
		} catch (IOException e) {
			log.warn("ArrayUtils::readFromFile error: " + e.getMessage());
			return null;
		}
	}
}