package koserver.login.database.entity;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

import koserver.common.database.entity.AbstractDatabaseEntity;

@Entity
@Table(name="server")
public class ServerEntity extends AbstractDatabaseEntity {

    private static final long serialVersionUID = -1506148037250430137L;

    @Column(name = "ip")
    private String ip;

    @Column(name = "category")
    private int category;

    @Column(name = "name")
    private String name;
    
    @Column(name = "userCount")
    private short userCount;

	@Column(name = "userMax")
    private short userMax;

	@Column(name = "userMaxFree")
    private short userMaxFree;
    
    public String getIp() {
        return ip;
    }

    public void setIp(final String ip) {
        this.ip = ip;
    }
    
    public int getCategory() {
        return category;
    }

    public void setCategory(final int category) {
        this.category = category;
    }

    public String getName() {
        return name;
    }

    public void setName(final String name) {
        this.name = name;
    }
    
    public short getUserCount() {
		return userCount;
	}

	public void setUserCount(short userCount) {
		this.userCount = userCount;
	}
    
    public short getUserMax() {
		return userMax;
	}

	public void setUserMax(short userMax) {
		this.userMax = userMax;
	}

	public short getUserMaxFree() {
		return userMaxFree;
	}

	public void setUserMaxFree(short userMaxFree) {
		this.userMaxFree = userMaxFree;
	}
}
