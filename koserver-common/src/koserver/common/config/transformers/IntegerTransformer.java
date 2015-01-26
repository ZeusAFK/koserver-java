package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class IntegerTransformer implements PropertyTransformer<Integer> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final IntegerTransformer SHARED_INSTANCE = new IntegerTransformer();

    /**
     * Transforms value to integer
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Integer object that represents value
     * @throws TransformationException if something went wrong
     */
    public Integer transform(String value, Field field) throws TransformationException {
        try {
            return Integer.decode(value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}
