package koserver.common.config.transformers;

import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class ByteTransformer implements PropertyTransformer<Byte> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final ByteTransformer SHARED_INSTANCE = new ByteTransformer();

    /**
     * Tansforms string to byte
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return Byte object that represents value
     * @throws TransformationException if something went wrong
     */
    public Byte transform(String value, Field field) throws TransformationException {
        try {
            return Byte.decode(value);
        }
        catch (Exception e) {
            throw new TransformationException(e);
        }
    }
}

