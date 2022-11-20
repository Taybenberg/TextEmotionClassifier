using TextPreprocessing;

namespace NeuralNetwork
{
    public class NeuralNetworkEvaluator
    {
        public NeuralNet NeuralNet { get; protected set; }
        public WordStorage Storage { get; protected set; }

        public NeuralNetworkEvaluator(NeuralNet neuralNet, WordStorage storage)
        {
            NeuralNet = neuralNet;
            Storage = storage;
        }

        public IEnumerable<(Sentiment Sentiment, double Score)> Evaluate(string text)
        {
            //remove mentions, urls and punctuation symbols
            var filteredText = text.Filter();

            var output = GetNeuralNetOutput(filteredText);

            //get last vector of output
            var result = output[^1][^1];

            //order results by score
            return result.Select((x, i) => ((Sentiment)i, x)).OrderByDescending(o => o.x);
        }

        protected List<List<double[]>> GetNeuralNetOutput(string text)
        {
            //split text by spaces
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            //neural network processing history
            var outputHistory = new List<List<double[]>>();

            //output of previous step (used as recurrent input)
            var prevOutput = new double[Config.OutputSize];

            foreach (var word in words)
            {
                //input vector
                var input = new double[Config.InputSize];

                //set TF-IDF as 0-th input element
                input[0] = Storage.GetTfIdf(word, words.Count(w => w == word), words.Length);

                //then SentimentFrequencies 
                Storage.GetCounter(word).SentimentFrequency.CopyTo(input, 1);

                //then previous output
                prevOutput.CopyTo(input, input.Length - prevOutput.Length);

                //get NeuralNetwork output
                var output = NeuralNet.GetOutput(input);

                //store whole output
                outputHistory.Add(output);

                //copy last output layer
                prevOutput = output[^1];
            }

            return outputHistory;
        }
    }
}
