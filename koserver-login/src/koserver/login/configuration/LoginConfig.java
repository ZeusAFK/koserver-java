package koserver.login.configuration;

import koserver.common.config.Property;

public class LoginConfig {
	@Property(key = "loginserver.version", defaultValue = "1860")
    public static int LOGIN_VERSION;
	
	@Property(key = "loginserver.ftp.url", defaultValue = "127.0.0.1")
	public static String LOGIN_FTP_URL;
	
	@Property(key = "loginserver.ftp.path", defaultValue = "/")
	public static String LOGIN_FTP_PATH;
}
