using System.Text.RegularExpressions;

namespace TextPreprocessing
{
    public static class StringExtensions
    {
        //remove mentions, urls and punctuation symbols
        public static string Filter(this string str)
        {
            var tmpText = Regex.Replace(str.ToLower(), "(@|http)\\S+|[']", string.Empty);
            return Regex.Replace(tmpText, "[^a-zA-Z0-9 ]", " ");
        }

        //convert string to enum
        public static Sentiment ToSentiment(this string str) =>
            str switch
            {
                "anger" => Sentiment.Anger,
                "boredom" => Sentiment.Boredom,
                "empty" => Sentiment.Empty,
                "enthusiasm" => Sentiment.Enthusiasm,
                "fun" => Sentiment.Fun,
                "happiness" => Sentiment.Happiness,
                "hate" => Sentiment.Hate,
                "love" => Sentiment.Love,
                "neutral" => Sentiment.Neutral,
                "relief" => Sentiment.Relief,
                "sadness" => Sentiment.Sadness,
                "surprise" => Sentiment.Surprise,
                "worry" => Sentiment.Worry,
                _ => throw new ArgumentException(nameof(str))
            };
    }
}
