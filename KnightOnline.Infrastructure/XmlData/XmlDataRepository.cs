using KnightOnline.Application.Contracts.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnightOnline.Infrastructure.XmlData
{
    public class XmlDataRepository<T> : IXmlDataRepository<T> where T : class
    {
        private readonly XmlDataParser _parser;
        private readonly string _filePath; // Specific XML file path for this repository type

        // Example: public XmlDataRepository(XmlDataParser parser, string filePath)
        // {
        //    _parser = parser;
        //    _filePath = filePath; // e.g., "data/items.xml"
        // }

        public XmlDataRepository(XmlDataParser parser)
        {
            _parser = parser;
            // TODO: Determine how filePath will be provided, perhaps via configuration
            // or specific derived classes for each XML data type.
            _filePath = string.Empty;
        }

        public Task<T> GetByIdAsync(int id)
        {
            // TODO: Implement loading all, then filtering by ID
            // This will depend on how data is structured in XML
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            // TODO: Implement using _parser to load all items from _filePath
            // Example: return Task.FromResult(_parser.Parse<List<T>>(_filePath));
            throw new System.NotImplementedException();
        }
    }
}
