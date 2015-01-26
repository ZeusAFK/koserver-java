package koserver.common.config.transformers;

import java.io.File;
import java.lang.reflect.Field;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class FileTransformer implements PropertyTransformer<File> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final FileTransformer SHARED_INSTANCE = new FileTransformer();

    /**
     * Transforms String to the file
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return File object that represents string
     */
    public File transform(String value, Field field) throws TransformationException {
        return new File(value);
    }
}
