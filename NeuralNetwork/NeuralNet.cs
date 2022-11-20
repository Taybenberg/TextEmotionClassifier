namespace NeuralNetwork
{
    public class NeuralNet
    {
        /* Weights are three-dimensional arrays, where
         * 
         * the first dimension indicates the placement of the weights
         * for ex. 0-th index contains the weights of the input layer with 0-th hidden layer
         * 
         * the second dimension indicates the index of the neuron to which the weights are attached
         * 
         * the third dimension contains a weight value
         * the index is equal to the index of the neuron of the previous layer from which the connection originates
         */
        public double[][][] Weights { get; set; }

        /* BiasWeights keep the weight value for all neurons of the desired layer
        */
        public double[] BiasWeights { get; set; }

        //Weights initialization
        public NeuralNet()
        {

            Weights = new double[Config.HiddenLayersSizes.Length + 1][][];

            BiasWeights = new double[Config.HiddenLayersSizes.Length + 1];

            //if we have 2 hidden layers, then we have to contain weights for 3 layers
            //[input -- 0th hidden layer], [0th hidden layer -- 1st hidden layer], [1st hidden layer -- output]
            for (int i = 0; i < Weights.Length; i++)
            {
                BiasWeights[i] = Config.DoubleRandom(); //random initial value

                //next layer will contain weights from current layer
                Weights[i] = new double[Config.GetLayerSize(i + 1)][];

                for (int j = 0; j < Weights[i].Length; j++)
                {
                    //weights from current layer
                    Weights[i][j] = new double[Config.GetLayerSize(i)];

                    for (int z = 0; z < Weights[i][j].Length; z++)
                        Weights[i][j][z] = Config.DoubleRandom(); //random initial value
                }
            }
        }


        public List<double[]> GetOutput(double[] input)
        {
            List<double[]> history = new()
            {
                input
            };

            for (int i = 0; i < Weights.Length; i++)
            {
                var neuronOutputs = new double[Weights[i].Length];

                //get output for each neuron in parallel
                Parallel.For(0, Weights[i].Length, (j) =>
                {
                    double sum = Config.Bias * BiasWeights[i];

                    for (int z = 0; z < Weights[i][j].Length; z++)
                        sum += history[^1][z] * Weights[i][j][z];

                    neuronOutputs[j] = Config.ActivationFunction(sum);
                });

                history.Add(neuronOutputs);
            }

            return history;
        }

        public double[] BackPropagation(double[] correctOutput, List<double[]> history)
        {
            var deltaLayer = new double[Config.OutputSize];
            double deltaBias = 0;

            //calculate gradient descent
            for (int i = 0; i < correctOutput.Length; i++)
            {
                deltaLayer[i] = -(correctOutput[i] - history[^1][i]) * (history[^1][i] * (1.0 - history[^1][i]));
                deltaBias += deltaLayer[i];
            }

            for (int i = Weights.Length - 1; i >= 0; i--)
            {
                //remove last element in history
                history.RemoveAt(history.Count - 1);

                //decrease bias error
                BiasWeights[i] -= Config.LearningSpeed * deltaBias * Config.Bias;

                var hiddenLayerError = new double[history[^1].Length];

                Parallel.For(0, Config.GetLayerSize(i + 1), (j) =>
                {
                    for (int z = 0; z < Config.GetLayerSize(i); z++)
                    {
                        hiddenLayerError[z] += deltaLayer[j] * Weights[i][j][z];
                        //decrease weights error
                        Weights[i][j][z] -= Config.LearningSpeed * deltaLayer[j] * history[^1][z];
                    }
                });

                deltaLayer = new double[hiddenLayerError.Length];
                deltaBias = 0;

                //calculate gradient descent
                for (int j = 0; j < deltaLayer.Length; j++)
                {
                    deltaLayer[j] = hiddenLayerError[j] * (history[^1][j] * (1.0 - history[^1][j]));
                    deltaBias += deltaLayer[j];
                }
            }

            //reduce the error of the input
            return deltaLayer[^Config.OutputSize..].Zip(history[0][^Config.OutputSize..], (a, b) => a + b).ToArray();
        }
    }
}