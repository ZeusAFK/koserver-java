package koserver.common.database.entity;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.MappedSuperclass;

import koserver.common.entity.AbstractEntity;

@MappedSuperclass
public abstract class AbstractDatabaseEntity extends AbstractEntity implements Serializable {

    private static final long serialVersionUID = -779623826754385140L;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", unique = true, updatable = false, nullable = false)
    protected Integer id;

    public AbstractDatabaseEntity(Integer id) {
        this.id = id;
    }

    public AbstractDatabaseEntity() {}

    public final Integer getId() {
        return id;
    }

    public final void setId(Integer id) {
        this.id = id;
    }

    @Override
    public int hashCode() {
        final int prime = 31;
        int result = 1;
        result = prime * result + ((id == null) ? 0 : id.hashCode());
        return result;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null) {
            return false;
        }
        if (!(obj instanceof AbstractDatabaseEntity)) {
            return false;
        }
        AbstractDatabaseEntity other = (AbstractDatabaseEntity) obj;
        if (id == null) {
            if (other.id != null) {
                return false;
            }
        }
        else if (!id.equals(other.id)) {
            return false;
        }
        return true;
    }
}
