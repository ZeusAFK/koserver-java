package koserver.common.config;

import java.lang.reflect.Field;

import koserver.common.config.exception.TransformationException;

public interface PropertyTransformer<T> {

    /**
     * This method actually transforms value to object instance
     *
     * @param value value that will be transformed
     * @param field value will be assigned to this field
     * @return result of transformation
     * @throws TransformationException if something went wrong
     */
    public T transform(String value, Field field) throws TransformationException;
}

