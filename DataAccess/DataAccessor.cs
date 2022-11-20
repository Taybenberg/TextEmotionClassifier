using NeuralNetwork;
using System.Text;
using System.Text.Json;
using TextPreprocessing;

namespace DataAccess
{
    public static class DataAccessor
    {
        const string WordStoragePath = "WordStorage.json";
        const string NeuralNetworkPath = "NeuralNet.json";

        public static void WriteWordStorage(WordStorage storage)
        {
            var json = JsonSerializer.Serialize(storage);
            File.WriteAllText(WordStoragePath, json, Encoding.UTF8);
        }

        public static WordStorage ReadWordStorage()
        {
            var json = File.ReadAllText(WordStoragePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<WordStorage>(json);
        }

        public static NeuralNetworkEvaluator GetNeuralNetworkEvaluator(WordStorage wordStorage = null)
        {
            (var storage, var neuralNet) = LoadData(wordStorage);
            return new(neuralNet, storage);
        }

        public static NeuralNetworkTeacher GetNeuralNetworkTeacher(WordStorage wordStorage = null)
        {
            (var storage, var neuralNet) = LoadData(wordStorage);
            return new(neuralNet, storage);
        }

        public static void Backup(this NeuralNetworkTeacher teacher) => WriteNeuralNetwork(teacher.NeuralNet);

        internal static (WordStorage, NeuralNet) LoadData(WordStorage wordStorage)
        {
            var storage = wordStorage ?? ReadWordStorage();
            var neuralNet = ReadNeuralNetwork();
            return (storage, neuralNet);
        }

        internal static void WriteNeuralNetwork(NeuralNet net)
        {
            var json = JsonSerializer.Serialize(net);
            File.WriteAllText(NeuralNetworkPath, json, Encoding.UTF8);
        }

        internal static NeuralNet ReadNeuralNetwork()
        {
            if (File.Exists(NeuralNetworkPath))
            {
                var json = File.ReadAllText(NeuralNetworkPath, Encoding.UTF8);
                return JsonSerializer.Deserialize<NeuralNet>(json);
            }
            return new();
        }
    }
}
