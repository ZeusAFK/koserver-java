package koserver.common.mapper.database;

import koserver.common.database.entity.AbstractDatabaseEntity;
import koserver.common.entity.AbstractEntity;
import koserver.common.models.AbstractModel;


public abstract class DatabaseMapper<E extends AbstractDatabaseEntity, M extends AbstractModel> {
    
    public final E map(final M model) {
        return this.map(model, null);
    }
    
    public final M map(final E entity) {
        return this.map(entity, null);
    }
    
    public abstract E map(final M model, AbstractEntity linkedEntity);
    public abstract M map(final E entity, AbstractModel linkedModel);
    public abstract boolean equals(M model, E entity);
}
