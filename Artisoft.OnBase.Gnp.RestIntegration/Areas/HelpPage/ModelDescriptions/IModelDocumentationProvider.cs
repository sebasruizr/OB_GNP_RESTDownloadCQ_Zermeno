using System;
using System.Reflection;

namespace Artisoft.OnBase.Gnp.RestIntegration.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}