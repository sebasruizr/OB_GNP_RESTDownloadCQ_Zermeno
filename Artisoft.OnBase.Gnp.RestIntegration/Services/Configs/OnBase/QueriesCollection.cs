using System;
using System.Configuration;
using Microsoft.Ajax.Utilities;

namespace Artisoft.OnBase.Gnp.RestIntegration.Services.Configs.OnBase
{
    public class QueriesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new QueryConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueryConfig) element).Id;
        }

        public void Add(QueryConfig queryConfig)
        {
            BaseAdd(queryConfig);
        }

        public QueryConfig this[long id]
        {
            get { return (QueryConfig) BaseGet(id); }
        }
        
        public QueryConfig this[int index]
        {
            get { return (QueryConfig) BaseGet(index); }
        }
    }
}