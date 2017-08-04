using System;
using System.Reflection;

namespace YC.Http.Formatting.Multipart.Sample.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}