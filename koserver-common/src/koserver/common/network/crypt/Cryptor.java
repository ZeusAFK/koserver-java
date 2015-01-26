package koserver.common.network.crypt;

import java.math.BigInteger;

import koserver.common.utils.ArrayUtils;

public class Cryptor {

    public static BigInteger privateKey = new BigInteger("1207500120128966", 16);
    BigInteger publicKey;
    BigInteger tKey;

    public Cryptor(BigInteger publicKey) {
        this.publicKey = publicKey;
        tKey = publicKey.xor(privateKey);
    }

    public byte[] EncryptionWithCRC32(byte[] data) {
        byte[] result = EncryptionFast(data);
        return result;
    }

    private byte[] EncryptionFast(byte[] data) {
        int length = data.length;
        byte[] result = new byte[length];
        int lKey, rsk;
        int rKey = 2157;

        char[] pKey = new StringBuilder(ArrayUtils.hexToString(tKey.toString(16))).reverse().toString().toCharArray();
        lKey = (length * 157) & 0xFF;

        for (int i = 0; i < length; i++) {
            rsk = (rKey >> 8) & 0xFF;
            result[i] = (byte) (((data[i] ^ rsk) ^ (int) pKey[(i % 8)]) ^ lKey);
            rKey *= 2171;
        }

        return result;
    }
}