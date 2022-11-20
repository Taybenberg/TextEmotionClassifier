using TextPreprocessing;

namespace AppTests
{
    public class Tests
    {
        [Test]
        public void TestStringFilter()
        {
            var str = "hElLO WoRld!";

            var filteredStr = StringExtensions.Filter(str);

            Assert.That(filteredStr, Is.EqualTo("hello world "));
        }

        [Test]
        public void TestStringFilterMentionRemoval()
        {
            var str = "hE1l0 @W0R1d";

            var filteredStr = StringExtensions.Filter(str);

            Assert.That(filteredStr, Is.EqualTo("he1l0 "));
        }

        [Test]
        public void TestStringFilterHyperlinkRemoval()
        {
            var str = "hE1l0 https://world.com?say=hello";

            var filteredStr = StringExtensions.Filter(str);

            Assert.That(filteredStr, Is.EqualTo("he1l0 "));
        }

        [Test]
        public void TestStringToSentiment()
        {
            var str = "fun";

            var sentiment = StringExtensions.ToSentiment(str);

            Assert.That(sentiment, Is.EqualTo(Sentiment.Fun));
        }

        [Test]
        public void TestStringToSentimentException()
        {
            var str = "world";

            Assert.Throws<ArgumentException>(() =>
                StringExtensions.ToSentiment(str)
            );
        }

        [Test]
        public void TestWordAppearanceCounterInitialization()
        {
            var counter = new WordAppearanceCounter();

            Assert.That(counter.SentimentCounter.Length, Is.EqualTo(13));
            Assert.True(counter.SentimentCounter.All(x => x == 0));
        }

        [Test]
        public void TestWordAppearanceCounterSentimentFrequencyInitialization()
        {
            var counter = new WordAppearanceCounter();

            var sf = counter.SentimentFrequency;

            Assert.That(sf.Length, Is.EqualTo(13));
            Assert.True(sf.All(x => x == 0));
        }

        [Test]
        public void TestWordAppearanceCounterSentimentFrequency()
        {
            var counter = new WordAppearanceCounter();
            counter.SentimentCounter[0] = 5;
            counter.TotalFrequency = 10;

            var sf = counter.SentimentFrequency;

            Assert.That(sf[0], Is.EqualTo(0.5));
        }

        [Test]
        public void TestTF()
        {
            var storage = new WordStorage();

            var tf = storage.GetTF(1, 2);

            Assert.That(tf, Is.EqualTo(0.5));
        }

        [Test]
        public void TestIDF()
        {
            var storage = new WordStorage();

            var idf = storage.GetIDF("test");

            Assert.That(idf, Is.EqualTo(0));
        }

        [Test]
        public void TestTfIdf()
        {
            var storage = new WordStorage();

            var tfIdf = storage.GetTfIdf("word", 1, 2);

            Assert.That(tfIdf, Is.EqualTo(0));
        }
    }
}