using DataAccess;
using TextPreprocessing;

//skip header
var lines = File.ReadAllLines("tweet_emotions.csv").Skip(1);
Console.WriteLine($"Loaded {lines.Count()} lines");

//tokenized words with features
var storage = new WordStorage
{
    DocumentCount = lines.Count()
};

List<(string line, Sentiment sentiment)> labeledLines = new();

//preprocess lines
foreach (var line in lines)
{
    //skip tweet id
    var lineData = line[11..];

    //find delimeter between sentiment and tweet
    var delimeterIndex = lineData.IndexOf(',');

    //get tweet sentiment
    var sentiment = lineData[..delimeterIndex].ToSentiment();

    //get raw tweet text
    var tweetText = lineData[(delimeterIndex + 1)..];

    //remove mentions, urls and punctuation symbols
    var filteredText = tweetText.Filter();

    labeledLines.Add((filteredText, sentiment));

    //split text by spaces
    var words = filteredText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    //process each word
    for (int i = 0; i < words.Length; i++)
    {
        var word = words[i];

        if (!storage.Tokens.ContainsKey(word))
            storage.Tokens.Add(word, new());

        storage.Tokens[word].TotalFrequency++;
        storage.Tokens[word].SentimentCounter[(int)sentiment]++;

        //increase document frequency only single time for this word per document
        for (int j = words.Length - 1; j >= i; j--)
        {
            if (words[j] == word)
            {
                if (j == i)
                    storage.Tokens[word].DocumentFrequency++;

                break;
            }
        }
    }
}

Console.WriteLine($"Extracted {storage.Tokens.Count} words");
DataAccessor.WriteWordStorage(storage);
Console.WriteLine("Stored WordStorage");

var teacher = DataAccessor.GetNeuralNetworkTeacher(storage);

Console.WriteLine("Training Neural Network");

var r = new Random();

for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"Starting training epoch {i}");

    teacher.LearningSpeed = 1.0 - 0.1 * i; //reduce learning speed each epoch

    int count = 0;

    foreach (var ll in labeledLines.OrderBy(x => r.Next()))
    {
        if (++count % 1000 == 0)
        {
            teacher.Train(ll.line, ll.sentiment, printError: true);
            Console.WriteLine($"Epoch {i}; Trained {count}/{labeledLines.Count} lines");
        }
        else
        {
            teacher.Train(ll.line, ll.sentiment);
        }
    }

    Console.WriteLine($"Finished training epoch {i}");
}




Console.WriteLine("Finished training");
teacher.Backup();