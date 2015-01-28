package koserver.login.models;

import java.util.Date;

import koserver.common.enums.AccountAuthority;
import koserver.common.models.AbstractModel;
import koserver.login.services.AccountService;

public class Account extends AbstractModel {

    private String login;
    private String password;
    private AccountAuthority authority;
    private Date access;
    private Date creation;
    private AccountService connection;

    public Account(final Integer id) {
        super(id);
    }

    public Account() {
        super(null);
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(final String login) {
        this.login = login;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(final String password) {
        this.password = password;
    }
    
    public AccountAuthority getAuthority() {
		return authority;
	}

	public void setAuthority(AccountAuthority authority) {
		this.authority = authority;
	}

    public boolean isBanned() {
        return authority == AccountAuthority.BANNED;
    }

    public void setBanned(final boolean banned) {
        authority = AccountAuthority.BANNED;
    }

    public Date getAccess() {
        return access;
    }

    public void setAccess(final Date access) {
        this.access = access;
    }

    public AccountService getConnection() {
        return connection;
    }

    public void setConnection(final AccountService connection) {
        this.connection = connection;
    }
    
    public Date getCreation() {
		return creation;
	}

	public void setCreation(Date creation) {
		this.creation = creation;
	}
    
    @Override
    public int hashCode() {
        return 0;
    }

    @Override
    public boolean equals(final Object obj) {
        if (this == obj) {
            return true;
        }
        if (!super.equals(obj)) {
            return false;
        }
        if (!(obj instanceof Account)) {
            return false;
        }
        final Account other = (Account) obj;
        if (access != other.access) {
            return false;
        }
        if (connection == null) {
            if (other.connection != null) {
                return false;
            }
        }
        else if (!connection.equals(other.connection)) {
            return false;
        }
        if (login == null) {
            if (other.login != null) {
                return false;
            }
        }
        else if (!login.equals(other.login)) {
            return false;
        }
        if (password == null) {
            if (other.password != null) {
                return false;
            }
        }
        else if (!password.equals(other.password)) {
            return false;
        }
        return true;
    }
}
