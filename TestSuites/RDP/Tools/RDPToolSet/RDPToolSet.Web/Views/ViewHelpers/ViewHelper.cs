// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace RDPToolSet.Web
{
    public static class ViewHelper
    {
        public static string FormatId(this IHtmlHelper htmlHelper, params string[] strings)
        {
            return string.Join("-", strings).Replace(" ", string.Empty).ToLowerInvariant();
        }

        public static HtmlString ButtonFor(this IHtmlHelper htmlHelper, string name,
            string id = null, string @class = null)
        {
            if (id == null) id = string.Empty;
            if (@class == null) @class = string.Empty;

            return new HtmlString(string.Format(
                "<button type=\"button\" id=\"{0}\" " +
                "class=\"btn btn-primary {1}\">{2}" +
                "</button>", id, @class, name));
        }

        public static IDisposable BeginPanel(this IHtmlHelper htmlHelper, string id, string title)
        {
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine(
                @"<div class=""panel panel-default""><div class=""panel-heading"">" + // panel heading
                @"<h4 class=""panel-title""><a data-toggle=""collapse"" href=""#{0}"">{1}</a></h4></div>" +
                @"<div id=""{0}"" class=""panel-collapse collapse""><div class=""panel-body"">", // panel body
                id, title);

            return new PanelBodyContainer(writer);
        }

        private class PanelBodyContainer : IDisposable
        {
            private readonly TextWriter writer;

            public PanelBodyContainer(TextWriter writer)
            {
                this.writer = writer;
            }

            public void Dispose()
            {
                // div end of panel-body
                // div end of panel-collapse
                // div end of panel
                writer.Write(@"</div></div></div>");
            }
        }

        public static IDisposable BeginInOutTabs(this IHtmlHelper htmlHelper, string inputId, string outputId)
        {
            var writer = htmlHelper.ViewContext.Writer;

            writer.WriteLine(
                @"<div class=""tabs"">" +
                @"<ul class=""nav nav-tabs nav-justified"">" +
                @"<li>" +
                @"<a href=""#{0}"" data-toggle=""tab"">Input</a>" +
                @"</li>" +
                @"<li class=""active"">" +
                @"<a href=""#{1}"" data-toggle=""tab"">Ouput</a>" +
                @"</li>" +
                @"</ul>" +
                @"<div class=""tab-content"">",
                inputId, outputId
            );
            return new InOutTabsContainer(writer);
        }

        private class InOutTabsContainer : IDisposable
        {
            private readonly TextWriter writer;

            public InOutTabsContainer(TextWriter writer)
            {
                this.writer = writer;
            }

            public void Dispose()
            {
                // div end of tabs
                // div end of tab-content
                writer.Write(@"</div></div>");
            }
        }

    }
}
