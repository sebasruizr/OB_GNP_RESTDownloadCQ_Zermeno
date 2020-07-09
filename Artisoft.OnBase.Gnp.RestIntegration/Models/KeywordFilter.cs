using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Artisoft.OnBase.Gnp.RestIntegration.Bindings;
using Hyland.Unity;

namespace Artisoft.OnBase.Gnp.RestIntegration.Models
{
    [TypeConverter(typeof(KeywordFilterTypeConverter))]
    [ValidateInput(true)]
    public class KeywordFilter
    {
        [Required] public Int64 KeywordId { get; set; }
        [Required] public KeywordOperator? Operator { get; set; }
        [Required] public KeywordRelation? Relation { get; set; }
        [Required] public string Value { get; set; }
    }
}