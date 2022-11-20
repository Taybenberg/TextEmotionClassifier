using System.Text.Json.Serialization;

namespace TextPreprocessing
{
    public class WordAppearanceCounter
    {
        private static readonly int _sentimentsLength = Enum.GetNames(typeof(Sentiment)).Length;

        public int[] SentimentCounter { get; set; } = new int[_sentimentsLength];
        public int TotalFrequency { get; set; }
        public int DocumentFrequency { get; set; }

        [JsonIgnore]
        public double[] SentimentFrequency
        {
            get
            {
                var sentimentFrequency = new double[_sentimentsLength];

                if (TotalFrequency > 0)
                {
                    for (int i = 0; i < _sentimentsLength; i++)
                        sentimentFrequency[i] = SentimentCounter[i] / (double)TotalFrequency;
                }

                return sentimentFrequency;
            }
        }
    }
}
