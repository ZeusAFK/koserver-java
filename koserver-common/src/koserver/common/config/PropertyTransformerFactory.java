package koserver.common.config;

import java.io.File;
import java.net.InetSocketAddress;
import java.util.Locale;
import java.util.regex.Pattern;

import koserver.common.config.exception.TransformationException;
import koserver.common.config.transformers.BooleanTransformer;
import koserver.common.config.transformers.ByteTransformer;
import koserver.common.config.transformers.CharTransformer;
import koserver.common.config.transformers.ClassTransformer;
import koserver.common.config.transformers.DoubleTransformer;
import koserver.common.config.transformers.EnumTransformer;
import koserver.common.config.transformers.FileTransformer;
import koserver.common.config.transformers.FloatTransformer;
import koserver.common.config.transformers.InetSocketAddressTransformer;
import koserver.common.config.transformers.IntegerTransformer;
import koserver.common.config.transformers.LocaleTransformer;
import koserver.common.config.transformers.LongTransformer;
import koserver.common.config.transformers.PatternTransformer;
import koserver.common.config.transformers.ShortTransformer;
import koserver.common.config.transformers.StringArrayTransformer;
import koserver.common.config.transformers.StringTransformer;

public class PropertyTransformerFactory {
    /**
     * Returns property transformer or throws
     * {@link com.aionemu.commons.configuration.TransformationException} if
     * can't create new one.
     * 
     * @param clazzToTransform
     *            Class that will is going to be transformed
     * @param tc
     *            {@link com.aionemu.commons.configuration.PropertyTransformer}
     *            class that will be instantiated
     * @return instance of PropertyTransformer
     * @throws TransformationException
     *             if can't instantiate
     *             {@link com.aionemu.commons.configuration.PropertyTransformer}
     */
    @SuppressWarnings("rawtypes")
    public static PropertyTransformer newTransformer(Class clazzToTransform, Class<? extends PropertyTransformer> tc) throws TransformationException {

        // Just a hack, we can't set null to annotation value
        if (tc == PropertyTransformer.class) {
            tc = null;
        }

        if (tc != null) {
            try {
                return tc.newInstance();
            }
            catch (Exception e) {
                throw new TransformationException("Can't instantiate property transfromer", e);
            }
        }

        if (clazzToTransform == Boolean.class || clazzToTransform == Boolean.TYPE) {
            return BooleanTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Byte.class || clazzToTransform == Byte.TYPE) {
            return ByteTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Character.class || clazzToTransform == Character.TYPE) {
            return CharTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Double.class || clazzToTransform == Double.TYPE) {
            return DoubleTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Float.class || clazzToTransform == Float.TYPE) {
            return FloatTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Integer.class || clazzToTransform == Integer.TYPE) {
            return IntegerTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Long.class || clazzToTransform == Long.TYPE) {
            return LongTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Short.class || clazzToTransform == Short.TYPE) {
            return ShortTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == String.class) {
            return StringTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == String[].class) {
            return StringArrayTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform.isEnum()) {
            return EnumTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == File.class) {
            return FileTransformer.SHARED_INSTANCE;
        }
        else if (InetSocketAddress.class.isAssignableFrom(clazzToTransform)) {
            return InetSocketAddressTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Pattern.class) {
            return PatternTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Class.class) {
            return ClassTransformer.SHARED_INSTANCE;
        }
        else if (clazzToTransform == Locale.class) {
            return LocaleTransformer.SHARED_INSTANCE;
        }
        else {
            throw new TransformationException("Transformer not found for class " + clazzToTransform.getName());
        }
    }
}

