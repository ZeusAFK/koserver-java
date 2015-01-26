package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class EnumTransformer implements PropertyTransformer<Enum<?>> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final EnumTransformer SHARED_INSTANCE = new EnumTransformer();

    /**
     * Trnasforms string to enum
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Enum object representing the value
     * @throws TransformationException if somehting went wrong
     */
    @SuppressWarnings({"unchecked", "rawtypes"})
    public Enum<?> transform(String value, Field field) throws TransformationException {
        Class<? extends Enum> clazz = (Class<? extends Enum>) field.getType();

        try {
            return Enum.valueOf(clazz, value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}
