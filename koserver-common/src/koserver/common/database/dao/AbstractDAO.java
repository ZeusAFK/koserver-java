package koserver.common.database.dao;

import koserver.common.database.entity.AbstractDatabaseEntity;

import org.hibernate.Session;

public abstract class AbstractDAO<E extends AbstractDatabaseEntity> {
    
    protected final Session session = this.getSession();
    
    public abstract void create(E entity);
    public abstract E read(Integer id);
    public abstract void update(E entity);
    public abstract void delete(E entity);
    
    protected abstract Session getSession();
}
