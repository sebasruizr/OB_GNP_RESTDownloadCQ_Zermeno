using System;
using System.Configuration;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase
{
    public class KeywordResultsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeywordResultConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KeywordResultConfig) element).Id;
        }

        public void Add(KeywordResultConfig keywordResultConfig)
        {
            BaseAdd(keywordResultConfig);
        }
        
        public KeywordResultConfig this[long id]
        {
            get { return (KeywordResultConfig) BaseGet(id); }
        }
        
        public KeywordResultConfig this[int index]
        {
            get { return (KeywordResultConfig) BaseGet(index); }
        }
    }
}