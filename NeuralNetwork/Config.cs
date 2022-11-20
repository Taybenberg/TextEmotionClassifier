using TextPreprocessing;

namespace NeuralNetwork
{
    internal static class Config
    {
        //Weight change factor
        public static double LearningSpeed = 0.5;

        public static readonly double Bias = 1.0;

        public static readonly Random R = new Random();

        //Size of Output vector
        public static readonly int OutputSize = Enum.GetNames(typeof(Sentiment)).Length; //13

        //Size of Input vector
        public static readonly int InputSize = 1 + OutputSize + OutputSize; //TF-IDF score unit, Input SentimentFrequencies, Context

        public static readonly int[] HiddenLayersSizes = new int[]
        {
            110,
            50,
            90,
        };

        public static int GetLayerSize(int index)
        {
            if (index == 0)
                return InputSize;
            else if (index > HiddenLayersSizes.Length)
                return OutputSize;
            else
                return HiddenLayersSizes[index - 1];
        }

        //Random double from -1.0 to 1.0
        public static double DoubleRandom()
        {
            if (R.Next(0, 2) > 0)
                return R.NextDouble();

            return -R.NextDouble();
        }

        //Sigmoidal activation function
        public static double ActivationFunction(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}
