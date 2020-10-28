namespace Structurizr.Documentation
{
    internal class FormattedContent
    {
        internal FormattedContent(string content, Format format)
        {
            Content = content;
            Format = format;
        }

        internal string Content { get; }
        internal Format Format { get; }
    }
}