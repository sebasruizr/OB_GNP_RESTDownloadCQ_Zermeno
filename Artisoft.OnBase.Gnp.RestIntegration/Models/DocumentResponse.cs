using System.Collections.Generic;

namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    public class DocumentResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DocumentKeywordResponse> Keywords { get; set; }

        public DocumentResponse()
        {
            Keywords = new List<DocumentKeywordResponse>();
        }

        public DocumentResponse(long id, string name) : this()
        {
            Id = id;
            Name = name;
        }
    }
}