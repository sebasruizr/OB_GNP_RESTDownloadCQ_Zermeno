using System.Configuration;
using System.Xml;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase
{
    public class OnBaseConfig : ConfigurationSection
    {
        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint => this["endpoint"] as string;

        [ConfigurationProperty("dataSource", IsRequired = true)]
        public string DataSource => this["dataSource"] as string;

        [ConfigurationProperty("keywordDateTimeFormat", IsRequired = true)]
        public string KeywordDateTimeFormat => this["keywordDateTimeFormat"] as string;
        
        [ConfigurationProperty("queries", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(QueriesCollection), AddItemName = "add")]
        public QueriesCollection Queries => (QueriesCollection) base["queries"];
    }
}