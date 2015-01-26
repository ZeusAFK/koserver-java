package koserver.common.domain.mapper;

import java.util.Map;

import javolution.util.FastMap;
import koserver.common.database.dao.exception.DAONotFoundException;
import koserver.common.domain.mapper.database.DatabaseMapper;
import koserver.common.domain.mapper.xml.XMLListMapper;
import koserver.common.domain.mapper.xml.XMLMapper;

import org.apache.log4j.Logger;

public class MapperManager {
    
    /** LOGGER */
    private static Logger log = Logger.getLogger(MapperManager.class.getName());
    
    /** Collection of registered DAOs */
    private static final Map<String, DatabaseMapper<?,?>> databaseMappersMap = new FastMap<>();
    private static final Map<String, XMLMapper<?,?>> xmlMappersMap = new FastMap<>();
    private static final Map<String, XMLListMapper<?,?>> xmlListMappersMap = new FastMap<>();
    
    private MapperManager() {
        // empty
    }
    
    public static <T extends DatabaseMapper<?,?>> T getDatabaseMapper(Class<T> clazz) {
        MapperManager.ensureDatabaseMapper(clazz);
        
        @SuppressWarnings("unchecked")
        T result = (T) databaseMappersMap.get(clazz.getName());
        
        if (result == null) {
            throw new DAONotFoundException("Mapper Not found "+clazz.getName());
        }
        
        return result;
    }
    
    public static <T extends XMLMapper<?,?>> T getXMLMapper(Class<T> clazz) {
        MapperManager.ensureXMLMapper(clazz);
        
        @SuppressWarnings("unchecked")
        T result = (T) xmlMappersMap.get(clazz.getName());
        
        if (result == null) {
            throw new DAONotFoundException("Mapper Not found "+clazz.getName());
        }
        
        return result;
    }
    
    public static <T extends XMLListMapper<?,?>> T getXMLListMapper(Class<T> clazz) {
        MapperManager.ensureXMLListMapper(clazz);
        
        @SuppressWarnings("unchecked")
        T result = (T) xmlListMappersMap.get(clazz.getName());
        
        if (result == null) {
            throw new DAONotFoundException("Mapper Not found "+clazz.getName());
        }
        
        return result;
    }
    
    private static final <T extends DatabaseMapper<?,?>> void ensureDatabaseMapper(Class<T> clazz) {
        DatabaseMapper<?,?> result = databaseMappersMap.get(clazz.getName());
        if (result == null) {
            try {
                databaseMappersMap.put(clazz.getName(), clazz.newInstance());
                log.debug("Initiated DatabaseMapper: "+clazz.getName());
            }
            catch (Exception e) {}
        }
    }
    
    private static final <T extends XMLMapper<?,?>> void ensureXMLMapper(Class<T> clazz) {
        XMLMapper<?,?> result = xmlMappersMap.get(clazz.getName());
        if (result == null) {
            try {
                xmlMappersMap.put(clazz.getName(), clazz.newInstance());
                log.debug("Initiated XMLMapper: "+clazz.getName());
            }
            catch (Exception e) {}
        }
    }
    
    private static final <T extends XMLListMapper<?,?>> void ensureXMLListMapper(Class<T> clazz) {
        XMLListMapper<?,?> result = xmlListMappersMap.get(clazz.getName());
        if (result == null) {
            try {
                xmlListMappersMap.put(clazz.getName(), clazz.newInstance());
                log.debug("Initiated XMLListDatabaseMapper: "+clazz.getName());
            }
            catch (Exception e) {}
        }
    }
}
