package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class ShortTransformer implements PropertyTransformer<Short> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final ShortTransformer SHARED_INSTANCE = new ShortTransformer();

    /**
     * Transforms value to short
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Short object that represents value
     * @throws TransformationException if something went wrong
     */
    public Short transform(String value, Field field) throws TransformationException {
        try {
            return Short.decode(value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}
