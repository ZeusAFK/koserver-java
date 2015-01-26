package koserver.common.network.crypt;

import java.math.BigInteger;
import java.util.Random;

public class CryptorKey {

    public static BigInteger generateKey() {
        while (true) {
            BigInteger n = new BigInteger(64, new Random(System.nanoTime()));
            if (n.toString().length() == 20) {
                return n;
            }
        }
    }
}
