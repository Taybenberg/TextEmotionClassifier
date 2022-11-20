using TextPreprocessing;

namespace NeuralNetwork
{
    public class NeuralNetworkTeacher : NeuralNetworkEvaluator
    {
        public double LearningSpeed
        {
            get => Config.LearningSpeed;
            set => Config.LearningSpeed = value;
        }

        public NeuralNetworkTeacher(NeuralNet neuralNet, WordStorage wordStorage) : base(neuralNet, wordStorage) { }

        public void Train(string text, Sentiment sentiment, bool printError = false)
        {
            var outputHistory = GetNeuralNetOutput(text);

            if (outputHistory.Count > 0)
            {
                //expected (correct) output
                var expectedOutput = new double[Config.OutputSize];
                expectedOutput[(int)sentiment] = 1.0;

                if (printError)
                {
                    var actualOutput = outputHistory[^1][^1];

                    double error = 0.0;
                    int max = 0;

                    for (int i = 0; i < Config.OutputSize; i++)
                    {
                        error += ((expectedOutput[i] - actualOutput[i]) * (expectedOutput[i] - actualOutput[i])) / 2;

                        if (actualOutput[i] > actualOutput[max])
                            max = i;
                    }

                    Console.WriteLine($"Error: {error}; Expected output: {sentiment}; Actual output: {(Sentiment)max};");
                }

                //process history in reverse order
                foreach (var output in outputHistory.AsEnumerable().Reverse())
                {
                    //update neural network weights using back propagation method
                    expectedOutput = NeuralNet.BackPropagation(expectedOutput, output);
                }
            }
        }
    }
}
