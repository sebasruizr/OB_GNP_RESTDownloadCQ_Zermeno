using System.IO;

namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    public class File
    {
        public string Name { get; set; }
        public MemoryStream Content { get; set; }
    }
}