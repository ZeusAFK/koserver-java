package koserver.login.enums;

import java.util.HashMap;
import java.util.Map;

public enum AccountAuthority {
    NORMAL(1),
    BANNED(0)
    ;
    
    public final int value;
    
    AccountAuthority(final int value) {
        this.value = value;
    }
    
    public int getValue() {
        return value;
    }
    
    private static final Map<Integer, AccountAuthority> _map = new HashMap<Integer, AccountAuthority>();
    static
    {
        for (AccountAuthority authority : AccountAuthority.values())
            _map.put(authority.value, authority);
    }
    
    public static AccountAuthority from(int value)
    {
        return _map.get(value);
    }
}
