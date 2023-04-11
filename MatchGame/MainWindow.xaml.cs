using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MatchGame
{
    using System.CodeDom.Compiler;
    using System.Windows.Media.Converters;
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double[] results = new double[5];
        int amountOfResults = 0;
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        public MainWindow()
        {
            
                InitializeComponent();
                timer.Interval = TimeSpan.FromSeconds(.1);
                timer.Tick += Timer_Tick;
                SetUpGame();
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
              "🐙", "🐙",
              "🐳", "🐳",
              "🐘", "🐘",
              "🐡", "🐡",
              "🐫", "🐫",
              "🦕", "🦕",
              "🦘", "🦘",
              "🦔", "🦔",
              "🐷", "🐷",
              "🐔", "🐔",
            };
            Random random = new Random();

            if (amountOfResults < results.Length)
            {
                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Name != "timeTextBlock")
                    {
                        textBlock.Visibility = Visibility.Visible;
                        int index = random.Next(animalEmoji.Count);
                        string nextEmoji = animalEmoji[index];
                        textBlock.Text = nextEmoji;
                        animalEmoji.RemoveAt(index);

                    }
                }
                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }
            else timeTextBlock.Text = "Finish";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
           
                tenthsOfSecondsElapsed++;
                timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
                if (matchesFound == 10)
                {
                    timer.Stop();
                   
                timeTextBlock.Text = $"{timeTextBlock.Text} The best result is " +
                    $"{FindingTheBestResult(tenthsOfSecondsElapsed / 10F).ToString("0.0s")} - Play again?";
                }
                 
        }

        public double FindingTheBestResult(double res)
        {
            double bestResult;
            if (amountOfResults < results.Length)
            {
                results[amountOfResults] = res;
                amountOfResults++;
            }
                double[] arr = new double[amountOfResults];
                for (int i = 0; i < amountOfResults; i++)
                {
                    arr[i] = results[i];
                }
                bestResult = arr[0];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] < bestResult) bestResult = arr[i];
                }

                return bestResult;
            }

        TextBlock lastTextBlockClicked = new TextBlock();
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is TextBlock textBlock)
            {
                //TextBlock textBlock = sender as TextBlock;
                if (findingMatch == false)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lastTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lastTextBlockClicked.Text)
                {
                    matchesFound++;
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lastTextBlockClicked.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 10)
            {
                SetUpGame();
            }
        }
    }
}
