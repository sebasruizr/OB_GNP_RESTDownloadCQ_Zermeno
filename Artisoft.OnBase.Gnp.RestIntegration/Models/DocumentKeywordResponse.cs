using System;

namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    public class DocumentKeywordResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public DocumentKeywordResponse()
        {
        }

        public DocumentKeywordResponse(long id, string name, object value)
        {
            Id = id;
            Name = name;
            Value = value;
        }
    }
}