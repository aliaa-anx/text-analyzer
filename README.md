##Text Analyzer

##F# Text Analysis Tool

A collaborative text analysis tool built in F#. This application accepts raw text input, breaks it down into tokens, calculates readability metrics, and analyzes word frequency.

Pipeline: How It Works

The application follows a clear data pipeline where text is processed in stages:

User Input → Tokenization → Metrics Calculation → Frequency Analysis → Database Storage → UI Display

## Features & Modules

#1. Input & Validation (Role 1)

Function: Collecting text from the user.

Features:

-Support for manual text entry.

-Support for .txt file uploads.

-Validation to ensure content is readable and non-empty.

#2. Tokenization & GitHub Manager (Role 3 - Implemented)

File: Tokenizer.fs

Function: Breaking text into manageable pieces.

Features:

Splits text into Words (removing punctuation).

Splits text into Sentences (based on ., !, ?).

Splits text into Paragraphs (based on new lines).

#3. Metrics & Readability (Role 4 - Implemented)

File: Metrics.fs

Function: Calculating statistical data.

Features:

Counts total words, sentences, and paragraphs.

Calculates Average Sentence Length.

Computes a basic Readability Score.

#4. Frequency Analysis (Role 5)

Function: Identifying key themes.

Features:

Detailed word frequency count.

Filters out common "stop words" (the, a, and, etc.).

Generates a "Top 10" repeated words table.

#5. Database & Storage (Role 2)

Function: Persistent memory.

Features:

Saves analysis results to a local database.

Tables: TextRecord, Statistics, WordFrequency.

#6. User Interface (Role 6)

Function: Visual interaction.

Features:

Windows-based GUI.

Controls to Analyze, Save, and Export to JSON.

📂 Project Structure

/text-analyzer
│
├── Program.fs            # Main Entry Point & Input Logic
├── Tokenizer.fs          # Text Splitting Logic (Role 3)
├── Metrics.fs            # Statistics Logic (Role 4)
├── TextAnalyzer.fsproj   # Main Project Configuration
├── .gitignore            # Git Settings (Ignores bin/obj)
└── README.md             # Documentation


💻 How to Run

Clone the repository:

git clone [https://github.com/aliaa-anx/text-analyzer.git](https://github.com/aliaa-anx/text-analyzer.git)


Navigate to the folder:

-cd text-analyzer


-Run the application:

dotnet run


🤝 Contributing

This is a team project.

-Please create a new branch for your feature:

-git checkout -b feature-name


-Submit a Pull Request to merge your changes into main.