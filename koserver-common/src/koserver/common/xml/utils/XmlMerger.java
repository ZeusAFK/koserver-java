package koserver.common.xml.utils;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Collection;

import javax.xml.namespace.QName;
import javax.xml.stream.XMLEventFactory;
import javax.xml.stream.XMLEventReader;
import javax.xml.stream.XMLEventWriter;
import javax.xml.stream.XMLInputFactory;
import javax.xml.stream.XMLOutputFactory;
import javax.xml.stream.XMLStreamException;
import javax.xml.stream.events.Comment;
import javax.xml.stream.events.StartElement;
import javax.xml.stream.events.XMLEvent;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.FilenameUtils;

public class XmlMerger {
    
    private static final XMLOutputFactory outputFactory = XMLOutputFactory.newInstance();
    private static final XMLInputFactory inputFactory = XMLInputFactory.newInstance();
    private static final XMLEventFactory eventFactory = XMLEventFactory.newInstance();
    
    public final static File processFile(File xml) throws IOException, XMLStreamException {
        File resultFile = File.createTempFile(FilenameUtils.removeExtension(xml.getName()), "");
        
        final XMLEventWriter writer = outputFactory.createXMLEventWriter(new BufferedWriter(new FileWriter(resultFile, false)));
        final XMLEventReader reader = inputFactory.createXMLEventReader(new FileReader(xml));
        while (reader.hasNext()) {
            final XMLEvent xmlEvent = reader.nextEvent();
            if (xmlEvent.isStartElement()) {
                if ("import".equals(xmlEvent.asStartElement().getName().getLocalPart())) {
                    File fileToImport = new File(xml.getParentFile()+File.separator+xmlEvent.asStartElement().getAttributeByName(new QName("file")).getValue());
                    Boolean skipRoot = Boolean.valueOf(xmlEvent.asStartElement().getAttributeByName(new QName("skipRoot")).getValue());
                    if (fileToImport.isDirectory()) {
                        Collection<File> files = FileUtils.listFiles(fileToImport, new String[]{"xml"}, true);
                        for (File file : files) {
                            processImport(file, writer, skipRoot);
                        }
                    } else if (fileToImport.isFile()) {
                        processImport(fileToImport, writer, skipRoot);
                    }
                    continue;
                }
            }
            
            if (xmlEvent.isEndElement()) {
                if ("import".equals(xmlEvent.asEndElement().getName().getLocalPart())) {
                    continue;
                }
            }
            
            // skip comments.
            if (xmlEvent instanceof Comment) {
                continue;
            }

            // skip whitespaces.
            if (xmlEvent.isCharacters()) {
                if (xmlEvent.asCharacters().isWhiteSpace() || xmlEvent.asCharacters().isIgnorableWhiteSpace()) {
                    continue;
                }
            }

            writer.add(xmlEvent);
        }
        
        reader.close();
        writer.close();
        
        return resultFile;
    }
    
    public final static void processImport(File file, XMLEventWriter writer, boolean skipRoot) throws FileNotFoundException, XMLStreamException {
        XMLEventReader reader = inputFactory.createXMLEventReader(new FileReader(file));
        QName firstTagQName = null;

        while (reader.hasNext()) {
            XMLEvent event = reader.nextEvent();
            if (event.isStartDocument() || event.isEndDocument()) {
                continue;
            }

            if (event instanceof Comment)
                continue;
            // skip white-spaces and all ignoreable white-spaces.
            if (event.isCharacters()) {
                if (event.asCharacters().isWhiteSpace() || event.asCharacters().isIgnorableWhiteSpace())
                    continue;
            }

            // modify root-tag of imported file.
            if (firstTagQName == null && event.isStartElement()) {
                firstTagQName = event.asStartElement().getName();

                if (skipRoot) {
                    continue;
                }
                else {
                    StartElement old = event.asStartElement();
                    event = eventFactory.createStartElement(old.getName(), old.getAttributes(), null);
                }
            }

            // if root was skipped - skip root end too.
            if (event.isEndElement() && skipRoot && event.asEndElement().getName().equals(firstTagQName))
                continue;

            // finally - write tag
            writer.add(event);
        }
        
        reader.close();
    }
}
