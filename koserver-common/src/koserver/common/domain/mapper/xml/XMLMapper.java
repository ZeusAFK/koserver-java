package koserver.common.domain.mapper.xml;

import koserver.common.entity.AbstractEntity;
import koserver.common.models.AbstractModel;
import koserver.common.xml.entity.AbstractXMLEntity;

public abstract class XMLMapper<E extends AbstractXMLEntity, M extends AbstractModel> {
    
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
