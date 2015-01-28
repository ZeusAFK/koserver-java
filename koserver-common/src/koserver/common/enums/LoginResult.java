package koserver.common.enums;

public class LoginResult {

    public static final int AUTH_SUCCESS = 0x01;
    public static final int AUTH_NOT_FOUND = 0x02;
    public static final int AUTH_INVALID = 0x03;
    public static final int AUTH_BANNED = 0x04;
    public static final int AUTH_IN_GAME = 0x05;
    public static final int AUTH_ERROR = 0x06;
    public static final int AUTH_AGREEMENT = 0xF;
    public static final int AUTH_FAILED = 0xF;
}