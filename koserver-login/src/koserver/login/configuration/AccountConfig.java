package koserver.login.configuration;

import koserver.common.config.Property;

public class AccountConfig {
    
    @Property(key = "account.autocreate", defaultValue = "true")
    public static boolean ACCOUNT_AUTOCREATE;
}
