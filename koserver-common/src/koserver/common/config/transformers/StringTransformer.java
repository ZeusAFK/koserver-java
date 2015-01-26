package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class StringTransformer implements PropertyTransformer<String> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final StringTransformer SHARED_INSTANCE = new StringTransformer();

    /**
     * Just returns value object
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return return value object
     * @throws TransformationException never thrown
     */
    public String transform(String value, Field field) throws TransformationException {
        return value;
    }
}

