/* S!--CODE ATTRIBUTION
TITLE: Microsoft Learn: Dictionaries, LINQ, and Random Class
AUTHOR: Microsoft .NET Team
DATE: 14 May 2026
VERSION: .NET 8.0
AVAILABLE: https://learn.microsoft.com/en-us/dotnet/
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberBot_POE
{
    public class ChatBot
    {
        public string UserName { get; set; } = "Tahani";
        private bool isFirstMessage = true;
        private readonly Random random = new Random();

        private string lastTopic = "";
        public string FavoriteTopic { get; set; } = "";

        private List<string> usedTips = new List<string>();
        private List<string> discussedTopics = new List<string>();

        // --- PART 3: TASK ASSISTANT STATE VARIABLES ---
        private bool isWaitingForReminder = false;
        private string pendingTaskTitle = "";
        private string pendingTaskDescription = "";
        private readonly DatabaseManager db = new DatabaseManager();

        private readonly Dictionary<string, string> keywordMap = new Dictionary<string, string>
        {
            { "password", "passwords" },
            { "scam", "online scams" },
            { "phish", "phishing" },
            { "email", "phishing" },
            { "privacy", "data privacy" },
            { "safe browsing", "safe browsing" },
            { "social", "social engineering" },
            { "malware", "malware" },
            { "mobile", "mobile security" }
        };

        private readonly Dictionary<string, string> topicDefinitions = new Dictionary<string, string>
        {
            { "passwords", "Passwords are your first line of defense. In South Africa, banking fraud often starts with a stolen password." },
            { "online scams", "Online scams in SA are common, especially on platforms like Facebook Marketplace or via fake courier SMSes." },
            { "phishing", "Phishing is when scammers pretend to be your bank to steal your login details." },
            { "data privacy", "Data privacy in South Africa is protected by the POPI Act (POPIA), which gives you rights over your personal info." },
            { "safe browsing", "Safe browsing means avoiding 'dodgy' websites that might install malware." },
            { "social engineering", "Social engineering is manipulating people into giving up confidential information." },
            { "malware", "Malware is malicious software designed to damage or gain unauthorized access to your system." },
            { "mobile security", "Mobile security protects your smartphone from threats like malicious apps and unsecured Wi-Fi." }
        };

        private readonly List<string> negativeKeywords = new List<string> { "worried", "scared", "stressed", "confused", "frustrated", "worries" };
        private readonly List<string> continuationKeywords = new List<string> { "more", "tip", "example", "yes", "another", "tell me more" };

        public string HandleUserQuery(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "I noticed you didn't type anything! I'm here whenever you're ready.";

            string cleanInput = input.ToLower().Trim();

            // Check for Task Intents FIRST
            if (cleanInput.Contains("add") || cleanInput.Contains("remind") || cleanInput.Contains("task"))
            {
                // This keeps your code clean by delegating the logic
                return HandleTaskIntent(cleanInput, input);
            }


            // Setup User Name
            if (isFirstMessage)
            {
                UserName = input;
                isFirstMessage = false;
                return $"Hello {UserName}! I'm Aegis-X, your South African Cybersecurity Sentinel. What would you like to ask me about today?";
            }


            // =========================================================================
            // --- PART 3: TASK ASSISTANT STATE ENGINE ---
            // =========================================================================

            // State B: The user is replying with their reminder preference timeframe
            // --- REPLACED LOGIC INSIDE HandleUserQuery ---

            // --- REPLACED LOGIC INSIDE HandleUserQuery ---
            if (isWaitingForReminder)
            {
                // 1. Clean the input to get JUST the time (e.g., "2 weeks" or "3 days")
                string rawInput = input.ToLower();

                // Define phrases to remove to isolate the date/time
                string[] phrasesToRemove = { "yes, remind me in", "remind me in", "yes remind me in", "in" };
                string reminderTime = input;

                foreach (string phrase in phrasesToRemove)
                {
                    if (rawInput.Contains(phrase))
                    {
                        reminderTime = rawInput.Replace(phrase, "").Trim();
                        break;
                    }
                }
                // Clean up any stray punctuation
                reminderTime = reminderTime.TrimEnd('.', '!', '?');

                // 2. Save only the clean 'reminderTime' string to the database
                bool success = db.InsertTask(pendingTaskTitle, pendingTaskDescription, reminderTime);

                isWaitingForReminder = false;
                pendingTaskTitle = "";
                pendingTaskDescription = "";

                if (success)
                {
                    // 3. Return a clean, professional response to the UI
                    return $"Got it! I've saved that task. I'll remind you in {reminderTime}.";
                }
                else
                {
                    return "System Alert: There was an issue saving the reminder to the database.";
                }
            }
            // State A: Catching the initial command keyword structure to add a new task
            if (cleanInput.StartsWith("add task -"))
            {
                // Extract everything after the "add task -" prefix
                int dashIndex = input.IndexOf('-');
                string extractedTitle = input.Substring(dashIndex + 1).Trim();

                if (string.IsNullOrWhiteSpace(extractedTitle))
                {
                    return "Please provide a task name! Example: 'Add task - Review privacy settings'";
                }

                // Stage the explicit data attributes dynamically as requested by task parameters
                pendingTaskTitle = extractedTitle;
                pendingTaskDescription = $"Review and configure your settings to ensure your {extractedTitle} data is protected.";

                // Switch the contextual conversation mode so the next user loop hits the reminder logic
                isWaitingForReminder = true;

                return $"Task added with the description \"{pendingTaskDescription}\" Would you like a reminder?";
            }


            // =========================================================================
            // --- EXISTING PART 1 & 2 RESPONSES ---
            // =========================================================================

            // --- 1. EXIT TRIGGER ---
            if (cleanInput.Contains("thank you for all your help"))
            {
                return "You're very welcome, Tahani! Stay safe out there. Goodbye!";
            }

            // --- 2. IDENTITY, PURPOSE & HELP ---
            if (cleanInput.Contains("purpose") || cleanInput.Contains("who are you") || cleanInput.Contains("what are you"))
            {
                return "My purpose is to act as your Cybersecurity Sentinel, providing South African-specific advice on staying safe online. You can ask me about Passwords, Phishing, or Scams!";
            }

            if (cleanInput.Contains("how are you"))
                return $"I am functioning at 100% efficiency, {UserName}! How are you doing today?";

            if (cleanInput.Contains("what can i ask") || cleanInput.Contains("help"))
                return "You can ask me about: Passwords, Phishing, Online Scams, Data Privacy, Safe Browsing, Social Engineering, Malware, or Mobile Security!";

            // --- 3. SENTIMENT DETECTION (Empathy First) ---
            if (negativeKeywords.Any(word => cleanInput.Contains(word)))
            {
                return ProcessSentiment(cleanInput);
            }

            string detectedTopic = DetermineTopic(cleanInput);

            // --- 4. RECALL LOGIC (Returning to Favorite) ---
            if (!string.IsNullOrEmpty(detectedTopic) && detectedTopic == FavoriteTopic && (cleanInput.Contains("again") || cleanInput.Contains("talk about")))
            {
                return $"Certainly, {UserName}. Since you are interested in {FavoriteTopic}, let's look at it again. {topicDefinitions[detectedTopic]}\n\nWould you like another tip?";
            }

            // --- 5. MEMORY TRIGGER (Saving Favorite) ---
            if (!string.IsNullOrEmpty(detectedTopic) && cleanInput.Contains("interested in"))
            {
                FavoriteTopic = detectedTopic;
                lastTopic = detectedTopic;
                if (!discussedTopics.Contains(detectedTopic)) discussedTopics.Add(detectedTopic);
                return $"Certainly, {UserName}. Since you are interested in {FavoriteTopic}, I'll remember that. {topicDefinitions[detectedTopic]}\n\nWould you like a local tip on this?";
            }

            // --- 6. STANDARD TOPIC DEFINITIONS ---
            if (!string.IsNullOrEmpty(detectedTopic))
            {
                if (lastTopic != detectedTopic) usedTips.Clear();
                lastTopic = detectedTopic;
                if (!discussedTopics.Contains(detectedTopic)) discussedTopics.Add(detectedTopic);

                string prefix = detectedTopic == FavoriteTopic
                    ? $"Certainly, {UserName}. Since you are interested in {FavoriteTopic}, here is more info: "
                    : $"Certainly, {UserName}. ";

                return $"{prefix}{topicDefinitions[detectedTopic]}\n\nWould you like a specific tip or example?";
            }

            // --- 7. CONTINUATION (YES / MORE) ---
            if (continuationKeywords.Any(word => cleanInput.Contains(word)) && !string.IsNullOrEmpty(lastTopic))
            {
                return GetUniqueTipResponse();
            }

            // --- 8. CONVERSATION FLOW (NO / TRANSITION) ---
            if (cleanInput == "no" || cleanInput.Contains("no thank you") || cleanInput.Contains("stop"))
            {
                string suggestion = GetProactiveSuggestion();
                return $"No problem! {suggestion}";
            }

            // --- 9. ERROR HANDLING (Gibberish/Unknown) ---
            string unknownSuggestion = GetProactiveSuggestion();
            return $"I'm not sure I understand, {UserName}. {unknownSuggestion}";
        }

        private string DetermineTopic(string input)
        {
            foreach (var entry in keywordMap)
                if (input.Contains(entry.Key)) return entry.Value;
            return "";
        }

        private string GetProactiveSuggestion()
        {
            var remaining = topicDefinitions.Keys.Where(t => !discussedTopics.Contains(t)).ToList();
            if (remaining.Count > 0)
            {
                string topicList = string.Join(", ", remaining.Select(t => char.ToUpper(t[0]) + t.Substring(1)));
                return $"Since we haven't discussed these yet, would you like to talk about: {topicList}?";
            }
            return "We've covered all our main topics! Is there anything you'd like to revisit?";
        }

        private string ProcessSentiment(string cleanInput)
        {
            string empathyMsg = "It's completely understandable to feel that way. Scammers can be very convincing.";
            string detected = DetermineTopic(cleanInput);

            if (!string.IsNullOrEmpty(detected))
            {
                lastTopic = detected;
                if (!discussedTopics.Contains(detected)) discussedTopics.Add(detected);
                return $"{empathyMsg} Let me share a tip to help you stay safe with {lastTopic}. {GetUniqueTipResponse()}";
            }
            return $"{empathyMsg} What specifically is on your mind?";
        }

        private string GetUniqueTipResponse()
        {
            List<string> allTips = GetAllTipsForTopic(lastTopic);
            var availableTips = allTips.Where(t => !usedTips.Contains(t)).ToList();

            if (availableTips.Count > 0)
            {
                string selectedTip = availableTips[random.Next(availableTips.Count)];
                usedTips.Add(selectedTip);

                if (usedTips.Count == allTips.Count)
                {
                    return $"Tip: {selectedTip}\n\nI've shared all my current tips for this! What else would you like to discuss?";
                }

                return $"Tip: {selectedTip}\n\nWould you like another one?";
            }

            return "I've shared all my current tips for this! " + GetProactiveSuggestion();
        }

        private List<string> GetAllTipsForTopic(string topic)
        {
            switch (topic)
            {
                case "phishing":
                    return new List<string> { "Be careful of 'Smishing' via fake courier SMSes.", "Banks will never ask for your OTP over email." };
                case "passwords":
                    return new List<string> { "Avoid local terms like 'Springboks2023' as passwords.", "Use a long passphrase like 'I-Love-Braai-2026!'." };
                case "online scams":
                    return new List<string> { "Never pay a deposit on Marketplace before seeing the item.", "Beware of WhatsApp groups promising to double your money." };
                case "data privacy":
                    return new List<string> { "Use POPIA to request companies to delete your data.", "Don't share your phone number on public Facebook groups." };
                case "safe browsing":
                    return new List<string> { "Check for 'https' and the padlock icon on shopping sites.", "Avoid public Wi-Fi for banking; use your mobile data instead." };
                case "social engineering":
                    return new List<string> { "Scammers create a sense of panic; stay calm and hang up.", "Verify 'emergencies' by calling your family member directly." };
                case "malware":
                    return new List<string> { "Keep your software updated to patch security holes.", "Never click links in pop-up windows claiming your PC is infected." };
                case "mobile security":
                    return new List<string> { "Enable 'Find My Device' to wipe data if your phone is stolen.", "Only download apps from the official Play Store or App Store." };
                default:
                    return new List<string> { "Always stay alert and keep your software updated!" };
            }
        }

        private string HandleTaskIntent(string cleanInput, string originalInput)
        {
            // If the user used the specific format "Add task - Name"
            if (cleanInput.StartsWith("add task -"))
            {
                // This is a direct hand-off to your existing logic below!
                // We return an empty string to signal that the main loop should handle it
                return "";
            }

            if (cleanInput.Contains("add") || cleanInput.Contains("task"))
            {
                return "I see you want to add a task. Please use the format: 'Add task - [Task Name]'.";
            }

            if (cleanInput.Contains("remind"))
            {
                return "I can help you set a reminder. What should I remind you about?";
            }

            return "I'm not sure what to do with that task request. Could you rephrase?";
        }
    }
}