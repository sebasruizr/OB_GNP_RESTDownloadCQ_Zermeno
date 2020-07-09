using System;
using System.Configuration;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase
{
    public class QueryConfig : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public long Id => (long) base["id"];

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name => base["name"] as string;

        [ConfigurationProperty("results", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KeywordResultsCollection), AddItemName = "add")]
        public KeywordResultsCollection Results => (KeywordResultsCollection) base["results"];
    }
}