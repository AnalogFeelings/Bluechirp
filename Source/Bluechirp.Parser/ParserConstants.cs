namespace Bluechirp.Parser
{
    /// <summary>
    /// Dummy class containing constants used by <see cref="TootParser"/>.
    /// <para/>
    /// The constants have self-documenting names.
    /// </summary>
    internal class ParserConstants
    {
        public const string PARAGRAPH_TAG = "p";
        public const string LINK_TAG = "a";
        public const string BREAK_TAG = "br";
        public const string SPAN_TAG = "span";
        public const string RAW_TEXT_TAG = "#text";

        public const string MENTION_CLASS = "mention";
        public const string HASHTAG_CLASS = "hashtag";
        public const string MENTION_SPAN_CLASS = "h-card";
    }
}