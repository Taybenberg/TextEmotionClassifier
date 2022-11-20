namespace TextPreprocessing
{
    public class WordAppearanceCounter
    {
        private static readonly int _sentimentsLength = Enum.GetNames(typeof(Sentiment)).Length;

        public int[] SentimentCounter { get; set; } = new int[_sentimentsLength];
        public int TotalFrequency { get; set; } = 0;
        public int DocumentFrequency { get; set; } = 0;

        private double[] _sentimentFrequecy;
        public double[] SentimentFrequency
        {
            get
            {
                if (_sentimentFrequecy is null)
                {
                    _sentimentFrequecy = new double[_sentimentsLength];

                    if (TotalFrequency > 0)
                    {
                        for (int i = 0; i < _sentimentsLength; i++)
                            _sentimentFrequecy[i] = SentimentCounter[i] / (double)TotalFrequency;
                    }
                }

                return _sentimentFrequecy;
            }
        }
    }
}
