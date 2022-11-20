using DataAccess;
using NeuralNetwork;
using System;
using System.Linq;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace TextEmotionClassifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        NeuralNetworkEvaluator _evaluator;

        public MainWindow()
        {
            _evaluator = DataAccessor.GetNeuralNetworkEvaluator();

            InitializeComponent();
        }

        //Change app theme
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            Theme.Apply(Theme.GetAppTheme() is ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
        }

        //Calculate results
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var text = TextForRecognition.Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                var results = _evaluator.Evaluate(text);
                var result = results.First();

                RecognitionResult.Text = $"{result.Sentiment}; {Math.Round(result.Score, 2)}";
                RecognitionResults.Text = string.Join(", ", results.Select(x => $"({x.Sentiment}; {Math.Round(x.Score, 2)})"));
            }
        }
    }
}
