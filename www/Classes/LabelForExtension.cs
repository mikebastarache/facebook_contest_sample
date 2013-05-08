using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

public static class LabelForExtension {

    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string prefix, string postfix, object htmlAttributes)
    {
        return LabelFor(html, expression, prefix, postfix, new RouteValueDictionary(htmlAttributes));
    }
    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string prefix, string postfix, IDictionary<string, object> htmlAttributes)
    {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName;
        if (String.IsNullOrEmpty(labelText))
        {
            return MvcHtmlString.Empty;
        }

        TagBuilder tag = new TagBuilder("label");
        tag.MergeAttributes(htmlAttributes);
        tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
        tag.InnerHtml = prefix;
        return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
    }
}