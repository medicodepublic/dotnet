using System;
using System.Collections.Generic;
using System.IO;

namespace Structurizr.Documentation
{
    internal class FormatFinder
    {
        private static readonly ISet<string> MARKDOWN_EXTENSIONS = new HashSet<string>
        {
            ".md", ".markdown", ".text"
        };

        private static readonly ISet<string> ASCIIDOC_EXTENSIONS = new HashSet<string>
        {
            ".asciidoc", ".adoc", ".asc"
        };

        internal static Format FindFormat(FileSystemInfo file)
        {
            if (file == null) throw new ArgumentException("A file must be specified.");

            if (MARKDOWN_EXTENSIONS.Contains(file.Extension))
                return Format.Markdown;
            if (ASCIIDOC_EXTENSIONS.Contains(file.Extension))
                return Format.AsciiDoc;
            return Format.Markdown;
        }
    }
}