package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class DoubleTransformer implements PropertyTransformer<Double> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final DoubleTransformer SHARED_INSTANCE = new DoubleTransformer();

    /**
     * Transforms string to required double
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Double that represents transformed string
     * @throws TransformationException if something went wrong
     */
    public Double transform(String value, Field field) throws TransformationException {
        try {
            return Double.parseDouble(value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}

