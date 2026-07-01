using System.Collections.Generic;

namespace CyberBot_POE
{
    public enum QType { TrueFalse, MultipleChoice, Typing }

    public class Question
    {
        public string Text { get; set; }
        public QType Type { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public string WrongAnswerReasoning { get; set; }
    }

    public class QuizManager
    {
        private List<Question> _questions = new List<Question>();
        private int _currentIndex = 0;
        private int _score = 0;

        public QuizManager()
        {
            _questions.Add(new Question { Text = "Is it safe to use public Wi-Fi for online banking?", Type = QType.TrueFalse, Options = new List<string> { "True", "False" }, CorrectAnswer = "False", Explanation = "Public Wi-Fi is often unencrypted; use mobile data instead.", WrongAnswerReasoning = "public networks lack the security protocols necessary to protect sensitive financial credentials." });

            _questions.Add(new Question { Text = "What is the best way to handle an email asking for your password?", Type = QType.MultipleChoice, Options = new List<string> { "Reply", "Delete", "Report as Phishing", "Ignore" }, CorrectAnswer = "Report as Phishing", Explanation = "Legitimate companies will never ask for your password via email.", WrongAnswerReasoning = "replying or ignoring does not neutralize the threat; reporting is the standard procedure to alert security teams." });

            _questions.Add(new Question { Text = "What is the act of manipulating people to divulge confidential information called? (One word)", Type = QType.Typing, CorrectAnswer = "social engineering", Explanation = "Social engineering is the manipulation of people into performing actions or divulging confidential information.", WrongAnswerReasoning = "that is a technical term for a different type of attack; social engineering specifically targets the human element." });

            _questions.Add(new Question { Text = "Which of these is a strong password?", Type = QType.MultipleChoice, Options = new List<string> { "123456", "password123", "I-Love-Braai-2026!", "Admin" }, CorrectAnswer = "I-Love-Braai-2026!", Explanation = "Strong passwords use a mix of symbols, numbers, and casing.", WrongAnswerReasoning = "that password lacks the complexity and length required to defend against brute-force attacks." });

            _questions.Add(new Question { Text = "POPIA stands for Protection of Personal Information Act.", Type = QType.TrueFalse, Options = new List<string> { "True", "False" }, CorrectAnswer = "True", Explanation = "POPIA is South Africa's legislation protecting your personal data rights.", WrongAnswerReasoning = "this is a fundamental piece of South African data privacy legislation." });

            _questions.Add(new Question { Text = "What type of malicious software is designed to gain unauthorized access to your system? (One word)", Type = QType.Typing, CorrectAnswer = "malware", Explanation = "Malware is an umbrella term for malicious software.", WrongAnswerReasoning = "that is a specific sub-type, but the general term for all malicious software is malware." });

            _questions.Add(new Question { Text = "What should you do if your phone is stolen?", Type = QType.MultipleChoice, Options = new List<string> { "Buy a new one", "Enable 'Find My Device' to wipe data", "Wait for it to be returned", "Do nothing" }, CorrectAnswer = "Enable 'Find My Device' to wipe data", Explanation = "Remotely wiping data prevents identity theft.", WrongAnswerReasoning = "doing nothing or waiting allows attackers easy access to your personal accounts and data." });

            _questions.Add(new Question { Text = "Should you share your OTP (One-Time PIN) with a friend who asks?", Type = QType.TrueFalse, Options = new List<string> { "True", "False" }, CorrectAnswer = "False", Explanation = "Never share your OTP; it is the final gatekeeper for your bank account.", WrongAnswerReasoning = "sharing an OTP grants the recipient full authorization to access your secure accounts." });

            _questions.Add(new Question { Text = "What is the term for a deceptive email pretending to be from a bank? (One word)", Type = QType.Typing, CorrectAnswer = "phishing", Explanation = "Phishing is the fraudulent practice of sending emails purporting to be from reputable companies.", WrongAnswerReasoning = "that is a different form of attack; email-based deception is specifically defined as phishing." });

            _questions.Add(new Question { Text = "What is the best way to secure your accounts?", Type = QType.MultipleChoice, Options = new List<string> { "Use one password", "Enable 2FA (Two-Factor Authentication)", "Write password on a paper", "Use your birthday" }, CorrectAnswer = "Enable 2FA (Two-Factor Authentication)", Explanation = "2FA adds an extra layer of security beyond just a password.", WrongAnswerReasoning = "using simple or reused passwords significantly increases the risk of a successful account takeover." });
        }

        public void ResetQuiz() { _currentIndex = 0; _score = 0; }
        public Question GetNextQuestion() => (_currentIndex < _questions.Count) ? _questions[_currentIndex] : null;
        public bool CheckAnswer(string userInput)
        {
            bool isCorrect = userInput.Trim().ToLower() == _questions[_currentIndex].CorrectAnswer.ToLower();
            if (isCorrect) _score++;
            _currentIndex++;
            return isCorrect;
        }
        public string GetExplanation() => _questions[_currentIndex - 1].Explanation;
        public bool IsGameOver() => _currentIndex >= _questions.Count;
        public int GetScore() => _score;
        public string GetFinalFeedback()
        {
            if (_score >= 8) return "Great job! You're a cybersecurity pro!";
            if (_score >= 5) return "Good effort! Keep learning to stay safe online.";
            return "You need more practice to stay safe in the cyber world.";
        }
        public string GetDetailedFeedback(string userInput, bool isCorrect)
        {
            var currentQuestion = _questions[_currentIndex - 1];
            if (isCorrect) return currentQuestion.Explanation;

            if (currentQuestion.Type == QType.TrueFalse)
            {
                return $"Incorrect. The correct answer is: {currentQuestion.CorrectAnswer}. {currentQuestion.Explanation}";
            }
            else
            {
                string reasoning = string.IsNullOrEmpty(currentQuestion.WrongAnswerReasoning) ? "That is not the correct approach." : currentQuestion.WrongAnswerReasoning;
                return $"You chose '{userInput}'. This is incorrect because {reasoning}. The correct answer is: {currentQuestion.CorrectAnswer}.";
            }
        }
    }
}