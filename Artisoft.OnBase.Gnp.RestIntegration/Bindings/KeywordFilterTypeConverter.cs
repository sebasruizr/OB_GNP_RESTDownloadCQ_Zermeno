using System;
using System.ComponentModel;
using System.Globalization;
using Artisoft.OnBase.Gnp.RestIntegration.Models;
using Hyland.Unity;

namespace Artisoft.OnBase.Gnp.RestIntegration.Bindings
{
    public class KeywordFilterTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
                return base.ConvertFrom(context, culture, value);
            var filterValue = (string) value;
            KeywordFilter filter = null;
            if (!string.IsNullOrEmpty(filterValue))
            {
                var tokens = filterValue.Split(',');
                if (tokens.Length == 3)
                {
                    KeywordOperator kwOperator;
                    KeywordRelation kwRelation;

                    filter = new KeywordFilter
                    {
                        //Por el momento la operación la dejamos siempre en ADN
                        Relation = KeywordRelation.And, //Enum.TryParse(tokens[0], true, out kwRelation) ? kwRelation : (KeywordRelation?) null,
                        KeywordId = Int64.Parse(tokens[0]),
                        Operator = Enum.TryParse(tokens[1], true, out kwOperator) ? kwOperator : (KeywordOperator?) null,
                        Value = tokens[2]
                    };
                }
            }

            return filter ?? base.ConvertFrom(context, culture, value);
        }
    }
}