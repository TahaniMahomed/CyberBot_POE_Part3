/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: How to create a Control Programmatically
AUTHOR: Microsoft WPF Team
DATE: 13 May 2026
VERSION: WPF Documentation
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-create-a-control-programmatically
*/

/* S!--CODE ATTRIBUTION
TITLE: WPF-Tutorial: The Border Control in Code
AUTHOR: WPF-Tutorial.com
DATE: 13 May 2026
VERSION: Tutorial Publication
AVAILABLE: https://www.wpftutorial.net/Border.html
*/

/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: DropShadowEffect Class
AUTHOR: Microsoft WPF Team
DATE: 13 May 2026
VERSION: WPF Documentation
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.effects.dropshadoweffect
*/

/* S!--CODE ATTRIBUTION
TITLE: Blog: Creating Neon Glow Effects in WPF
AUTHOR: Independent Developer Blog
DATE: 13 May 2026
VERSION: Article Publication
AVAILABLE: https://www.c-sharpcorner.com/article/wpf-glow-effects-guide
*/

/* S!--CODE ATTRIBUTION
TITLE: Stack Overflow: Changing Alignment Programmatically
AUTHOR: Stack Overflow Contributors
DATE: 13 May 2026
VERSION: Community Q&A
AVAILABLE: https://stackoverflow.com/questions/5299273/set-horizontalalignment-in-code-behind
*/

using System;
using System.Data; // Required for working with DataTables
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Linq;

namespace CyberBot_POE
{
    public partial class MainWindow : Window
    {
        // Connecting the UI to our ChatBot backend logic
        private ChatBot myBot = new ChatBot();
        private QuizManager _quizManager = new QuizManager();
        private string botDisplayName = "AEGIS-X";

        // Connecting the layout hooks to our secure local database manager
        private readonly DatabaseManager db = new DatabaseManager();

        // Defining a "Cyberpunk" theme using specific RGB values for a neon look
        private readonly Color AegisCyan = Color.FromRgb(0, 255, 255);
        private readonly Color UserPurple = Color.FromRgb(191, 64, 191);
        private readonly Color DeepDark = Color.FromRgb(10, 10, 20);

        public MainWindow()
        {
            InitializeComponent();
            // Starts the initial bot setup and greeting
            LoadTask1Requirements();
            // Auto-loads any existing tasks directly out of SQL upon startup
            RefreshTaskGrid();
        }
        // =========================================================================
        // --- QUIZ INTERFACE METHODS ---
        // =========================================================================

        // --- QUIZ INTERFACE METHODS ---

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowStartScreen();
        }

        private void ShowStartScreen()
        {
            QuizButtonContainer.Children.Clear();
            QuestionDisplay.Text = "Press START to begin the Cybersecurity Quiz";

            Button startBtn = new Button
            {
                Content = "START",
                Background = new SolidColorBrush(UserPurple),
                Foreground = Brushes.White,
                Width = 150,
                Height = 50,
                FontSize = 18
            };
            startBtn.Click += StartQuiz_Click;
            QuizButtonContainer.Children.Add(startBtn);
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.ResetQuiz();
            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            var q = _quizManager.GetNextQuestion();
            if (q == null) return;

            QuestionDisplay.Text = q.Text;
            QuizButtonContainer.Children.Clear(); // Clears Start button or previous question

            if (q.Type == QType.Typing)
            {
                TextBox answerInput = new TextBox { Name = "QuizTypingBox", Width = 200, Height = 30 };
                Button submitBtn = new Button { Content = "SUBMIT", Tag = -1, Height = 30, Width = 100 };
                submitBtn.Click += AnswerButton_Click;
                QuizButtonContainer.Children.Add(answerInput);
                QuizButtonContainer.Children.Add(submitBtn);
            }
            else
            {
                foreach (var option in q.Options)
                {
                    Button btn = new Button { Content = option, Width = 150, Height = 30, Margin = new Thickness(5) };
                    btn.Click += AnswerButton_Click;
                    QuizButtonContainer.Children.Add(btn);
                }
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string userInput = "";

                // Handle Typing vs Button input
                if (btn.Tag != null && btn.Tag.ToString() == "-1")
                {
                    var txt = QuizButtonContainer.Children.OfType<TextBox>().FirstOrDefault();
                    userInput = txt?.Text.Trim() ?? ""; // .Trim() removes accidental spaces

                    // --- VALIDATION ADDED HERE ---
                 
                    if (string.IsNullOrEmpty(userInput))
                    {
                        // This creates a native Windows pop-up prompt
                        MessageBox.Show("Please enter an answer before submitting.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // -----------------------------
                }
                else
                {
                    userInput = btn.Content?.ToString() ?? "";
                }

                bool isCorrect = _quizManager.CheckAnswer(userInput);

                // --- UPDATED FEEDBACK LOGIC ---
                string feedbackHeader = isCorrect ? "CORRECT!" : "INCORRECT.";
                string detailedReasoning = _quizManager.GetDetailedFeedback(userInput, isCorrect);

                AddMessage(botDisplayName, $"{feedbackHeader}\n{detailedReasoning}", AegisCyan, HorizontalAlignment.Left);
                // ------------------------------
                if (_quizManager.IsGameOver())
                {
                    AddMessage(botDisplayName, $"QUIZ OVER. SCORE: {_quizManager.GetScore()}/10. {_quizManager.GetFinalFeedback()}", AegisCyan, HorizontalAlignment.Left);
                    QuestionDisplay.Text = "Quiz Finished. Press START to try again.";

                    Button startBtn = new Button { Content = "START QUIZ", Height = 40, Width = 150 };
                    startBtn.Click += StartQuiz_Click;
                    QuizButtonContainer.Children.Add(startBtn);
                }
                else
                {
                    ShowNextQuestion();
                }
            }
        }

        private void LoadTask1Requirements()
        {
            // ASCII Branding: Providing a visual "hacker" aesthetic for the header
            AsciiDisplay.Text = @"
      █████╗ ███████╗ ██████╗ ██╗███████╗      ██╗  ██╗
     ██╔══██╗██╔════╝██╔════╝ ██║██╔════╝      ╚██╗██╔╝
     ███████║█████╗  ██║  ███╗██║███████╗█████╗ ╚███╔╝ 
     ██╔══██║██╔══╝  ██║   ██║██║╚════██║╚════╝ ██╔██╗ 
     ██║  ██║███████╗╚██████╔╝██║███████║      ██╔╝ ██╗
     ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝╚══════╝      ╚═╝  ╚═╝";

            // Audio Feedback: Playing a welcome sound to improve the user experience
            try
            {
                SoundPlayer player = new SoundPlayer("welcome2.wav");
                player.Play();
            }
            catch { /* If the sound file is missing, the app continues without crashing */ }

            // Initial bot handshake to prompt the user for their name
            AddMessage(botDisplayName, "Systems online. I am Aegis-X, your Cybersecurity Sentinel. What is your name?", AegisCyan, HorizontalAlignment.Left);
        }

        // Triggering the input handler when the Send button is clicked
        private void SendButton_Click(object sender, RoutedEventArgs e) => HandleInput();

        // Allowing the user to press 'Enter' instead of clicking the button for better flow
        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) HandleInput();
        }

        // --- SIMPLIFIED MainWindow.xaml.cs ---
        private void HandleInput()
        {
            string input = UserInputBox.Text;
            if (string.IsNullOrWhiteSpace(input)) return;

            AddMessage("You", input, UserPurple, HorizontalAlignment.Right);
            UserInputBox.Clear();

            // Pass the raw input to the bot; the bot now handles the cleaning
            string response = myBot.HandleUserQuery(input);

            if (!string.IsNullOrWhiteSpace(response))
            {
                AddMessage(botDisplayName, response, AegisCyan, HorizontalAlignment.Left);
            }

            if (response.Contains("saved that task"))
            {
                RefreshTaskGrid();
            }
        }

        // =========================================================================
        // --- PART 3: TASK HUB BACKEND INTERFACE METHODS ---
        // =========================================================================

        /// <summary>
        /// Reads all task rows from the database manager and binds them to the UI DataGrid
        /// </summary>
        private void RefreshTaskGrid()
        {
            try
            {
                DataTable dt = db.GetAllTasks();
                TasksDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                // FIXED: Changed MessageBoxIcon to MessageBoxImage
                MessageBox.Show($"Error loading task dashboard records: {ex.Message}", "Database Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Triggered when the user clicks 'REFRESH' manually
        /// </summary>
        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTaskGrid();
        }

        /// <summary>
        /// Marks a selected grid row task item as complete inside the persistent database layer
        /// </summary>
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int taskId = Convert.ToInt32(selectedRow["Id"]);

                // --- REPLACE YOUR EXISTING LOGIC WITH THIS BLOCK ---
                if (db.UpdateTaskStatus(taskId, true))
                {
                    RefreshTaskGrid();
                }
                else
                {
                    MessageBox.Show("Could not update task in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                // ---------------------------------------------------
            }
            else
            {
                MessageBox.Show("Please select a task from the board list first to mark it as complete!", "Action Required", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Completely deletes a chosen task row from the backend database records
        /// </summary>
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int taskId = Convert.ToInt32(selectedRow["Id"]);
                string taskTitle = selectedRow["Title"].ToString();

                MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to completely delete the task: \"{taskTitle}\"?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    // NOTE: If your DatabaseManager uses a different name like RemoveTask, change it here!
                    if (db.DeleteTask(taskId))
                    {
                        RefreshTaskGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task from the board list first to delete it!", "Action Required", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // --- DYNAMIC UI GENERATION ---
        // This method builds the chat bubbles programmatically in C# instead of hard-coding in XAML
        public void AddMessage(string sender, string text, Color color, HorizontalAlignment alignment)
        {
            // BUBBLE DESIGN: Implementing a "Glass-morphism" effect with semi-transparent backgrounds
            Border bubbleBorder = new Border
            {
                // Calculating background color based on the theme (opacity set to 60 for contrast)
                Background = new SolidColorBrush(Color.FromArgb(60, color.R, color.G, color.B)),
                BorderBrush = new SolidColorBrush(color),
                BorderThickness = new Thickness(1.5),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                // Setting margins so bubbles appear on opposite sides (User vs Bot)
                Margin = new Thickness(alignment == HorizontalAlignment.Right ? 60 : 10, 8, alignment == HorizontalAlignment.Right ? 10 : 60, 8),
                HorizontalAlignment = alignment,
                // Adding a Neon Glow effect using DropShadow
                Effect = new DropShadowEffect { Color = color, BlurRadius = 12, ShadowDepth = 0, Opacity = 0.6 }
            };

            StackPanel contentStack = new StackPanel();

            // SENDER LABEL: Shows "YOU" or "AEGIS-X" in small, bold, neon text
            contentStack.Children.Add(new TextBlock
            {
                Text = sender.ToUpper(),
                FontSize = 10,
                FontWeight = FontWeights.ExtraBold,
                Foreground = new SolidColorBrush(color),
                Margin = new Thickness(0, 0, 0, 4),
            });

            // MESSAGE BODY: Displays the actual chat text
            contentStack.Children.Add(new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap, // Ensures long messages don't go off-screen
                FontSize = 14,
                LineHeight = 18
            });

            // Nesting the text stack inside the border and adding it to the UI container
            bubbleBorder.Child = contentStack;
            ChatContainer.Children.Add(bubbleBorder);

            // Auto-scrolling logic: Ensures the latest message is always visible
            ChatContainer.UpdateLayout();
            MainScrollViewer.ScrollToEnd();
        }
    }
}