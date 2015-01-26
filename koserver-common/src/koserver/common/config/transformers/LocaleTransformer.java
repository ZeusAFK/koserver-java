package koserver.common.config.transformers;

import java.lang.reflect.Field;
import java.util.Locale;

import org.apache.commons.lang3.LocaleUtils;

import koserver.common.config.PropertyTransformer;
import koserver.common.config.exception.TransformationException;

public class LocaleTransformer implements PropertyTransformer<Locale> {
    /**
     * Shared instance of this transformer. It's thread-safe so no need of multiple instances
     */
    public static final LocaleTransformer SHARED_INSTANCE = new LocaleTransformer();

    /**
     * Transforms String to the file
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return File object that represents string
     */
    public Locale transform(String value, Field field) throws TransformationException {
        return LocaleUtils.toLocale(value);
    }
}
