🛡️ Aegis-X: South African Cybersecurity Sentinel
Aegis-X is a specialized WPF-based chatbot designed to educate South African users on digital safety. Developed as part of a multi-stage programming assignment, the application provides localized advice on phishing, passwords, and safe browsing.

Table of Contents
1.Project Evolution
2.Key Features
3.Technical Implementation
4.Challenges & Problem Solving
5.Local Context & South African Integration
6.How to Run

1.Project Evolution
Part 1 Refinement (Feedback Implementation)
Based on the rubric feedback from Part 1, significant improvements were made to the core logic:

Logic Optimization: Cleaned up redundant loops and improved the keyword matching efficiency.

Validation: Fixed issues where empty inputs would cause the bot to skip cycles.

Attribution: Corrected missing or poorly formatted code attributions to meet academic standards.

Part 2 Implementation
The second phase focused on moving the application into a Graphical User Interface (GUI) and implementing advanced conversational logic.

2.Key Features
1. Advanced Conversation Engine
Keyword Recognition: Dynamically identifies topics (Passwords, Phishing, Scams, Privacy, Browsing) from natural language.

Randomized Responses: Uses a shuffling algorithm to ensure the bot never gives the same tip twice in a row.

Memory & Recall: A "Favorite Topic" feature allows the bot to remember the user's initial interest and reference it later.

The "No" Factor: A unique navigation system where the bot suggests untapped topics if a user declines further tips.

2. Emotional Intelligence
Sentiment Detection: The bot identifies words associated with anxiety or worry (e.g., "scared," "stressed") and prioritizes empathetic, immediate assistance.

3.Enhanced GUI (WPF)
Cyber-Pop Theme: A high-contrast neon aesthetic using Cyan and Purple to distinguish between Bot and User.

Glass-morphism: Modern UI design with semi-transparent, glowing chat bubbles.

2.Technical Implementation
The application is built using C# and the WPF (.NET 8.0) framework. Key technical concepts include:

LINQ: Used for filtering and selecting unique tips from data collections.

Dictionaries: Mapping keywords to standardized topics for efficient lookup.

Dynamic UI Generation: Chat bubbles are created programmatically in the code-behind to allow for flexible conversation lengths.

SoundPlayer: Integrated audio feedback on application startup.

4.Challenges & Problem Solving
UI Auto-Scrolling: One challenge was ensuring the ScrollViewer automatically stayed at the bottom when new bubbles were added. This was solved by calling UpdateLayout() before ScrollToEnd().

Non-Repetitive Logic: To prevent the bot from repeating tips, a tracking List<string> was implemented to record "used tips" and filter them out of future suggestions.

Local Localization: It was a fun but complex task to translate generic cybersecurity concepts into South African examples (like referencing the POPI Act and local courier scams).

5.Local Context & South African Integration
Aegis-X is specifically tailored for the South African landscape:

POPIA: Advice on data privacy is aligned with the Protection of Personal Information Act.

Regional Threats: Tips include warnings about common "The Courier Guy" smishing and Facebook Marketplace "Advance Fee" scams.

Banking: Direct references to FNB, Standard Bank, and Capitec security protocols.

6.How to Run
Clone the repository: git clone 
Open the solution file (CyberBot_POE.sln) in Visual Studio 2022.
Ensure the welcome2.wav file is present in the output directory.
Build and Run the project.

Ensure the welcome2.wav file is present in the output directory.

Build and Run the project.
