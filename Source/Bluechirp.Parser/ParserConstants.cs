namespace Bluechirp.Parser
{
    internal class ParserConstants
    {
        public const char PARAGRAPH_TAG = 'p';
        public const char LINK_TAG = 'a';
        public const char TAG_START_CHARACTER = '<';
        public const char TAG_END_CHARACTER = '>';
        public const char TAG_CLOSE_CHARAACTER = '/';
        public const char ATTRIBUTE_VALUE_CHARACTER = '"';
        public const char ATTRIBUTE_CHARACTER = '=';

        public const string BREAK_TAG = "br";
        public const string SPAN_TAG = "span";

        public const string MENTION_CLASS = "u-url mention";
        public const string PLAIN_HASHTAG_CLASS = "hashtag";
        public const string HASHTAG_CLASS = "mention hashtag";
        public const string CLASS_ATTRIBUTE = "class";
        public const string LINK_HREF = "href";
    }
}