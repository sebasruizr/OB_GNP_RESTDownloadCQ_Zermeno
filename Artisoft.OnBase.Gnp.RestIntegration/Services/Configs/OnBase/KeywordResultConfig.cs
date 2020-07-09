using System;
using System.Configuration;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase
{
    public class KeywordResultConfig : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = false)]
        public long Id => (long)base["id"];

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name => base["name"] as string;
    }
}