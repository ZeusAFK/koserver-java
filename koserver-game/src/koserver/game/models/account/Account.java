package koserver.game.models.account;

import java.util.Date;
import java.util.List;

import javolution.util.FastList;
import koserver.common.enums.AccountAuthority;
import koserver.common.models.AbstractModel;
import koserver.game.models.player.Player;
import koserver.game.services.AccountService;

public class Account extends AbstractModel {

	private String login;
	private String password;
	private int nation;
	private AccountAuthority authority;
	private Date access;
	private Date creation;
	private List<Player> players;
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

	public int getNation() {
		return nation;
	}

	public void setNation(int nation) {
		this.nation = nation;
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
	
	public List<Player> getPlayers() {
        if (players == null) {
            players = new FastList<>();
        }
        return players;
    }
	
	public Player getPlayerById(final int playerId) {
        for (final Player player : this.getPlayers()) {
            if (player.getId() == playerId) {
                return player;
            }
        }
        
        return null;
    }

    public void setPlayers(final List<Player> players) {
        this.players = players;
    }
    
    public void addPlayer(final Player player) {
        this.getPlayers().add(player);
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
		} else if (!connection.equals(other.connection)) {
			return false;
		}
		if (login == null) {
			if (other.login != null) {
				return false;
			}
		} else if (!login.equals(other.login)) {
			return false;
		}
		if (password == null) {
			if (other.password != null) {
				return false;
			}
		} else if (!password.equals(other.password)) {
			return false;
		}
		return true;
	}
}
