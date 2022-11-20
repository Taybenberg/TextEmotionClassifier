namespace TextPreprocessing
{
    public class WordStorage
    {
        public int DocumentCount { get; set; }
        public Dictionary<string, WordAppearanceCounter> Tokens { get; set; } = new();

        public WordAppearanceCounter GetCounter(string word)
        {
            if (Tokens.ContainsKey(word))
                return Tokens[word];

            return new WordAppearanceCounter();
        }

        public double GetIDF(string word)
        {
            if (Tokens.ContainsKey(word))
                return Math.Log(DocumentCount / (1.0 + Tokens[word].DocumentFrequency));

            return 0;
        }

        public double GetTF(double wordOccurences, double totalWords)
        {
            return wordOccurences / totalWords;
        }

        public double GetTfIdf(string word, int wordOccurences, int totalWords)
        {
            return GetTF(wordOccurences, totalWords) * GetIDF(word);
        }
    }
}
