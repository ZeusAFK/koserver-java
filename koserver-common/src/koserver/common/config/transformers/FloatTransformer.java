package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class FloatTransformer implements PropertyTransformer<Float> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final FloatTransformer SHARED_INSTANCE = new FloatTransformer();

    /**
     * Thransforms string to float
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Float that represents value
     * @throws TransformationException if something went wrong
     */
    public Float transform(String value, Field field) throws TransformationException {
        try {
            return Float.parseFloat(value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}

