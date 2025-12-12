##  Text Analyzer System

# F# Text Analysis Tool

A robust, collaborative desktop application built in F# and WinForms, designed to process raw text, calculate statistical metrics, and perform word frequency analysis.

---

##  Pipeline: How It Works

The application follows the principles of functional programming with a clear, staged data pipeline. 

**User Input $\rightarrow$ Validation $\rightarrow$ Tokenization $\rightarrow$ Metrics Calculation $\rightarrow$ Frequency Analysis $\rightarrow$ UI Display/JSON Export**

---

##  Features & Modules

| Module Name | File(s) | Function | Status |
| :--- | :--- | :--- | :--- |
| **Input & Validation** (Role 1) | `InputValidation.fs` | Handles all text collection and ensures content is valid before processing. | **Implemented & Tested** |
| **Tokenization** (Role 3) | `Tokenizer.fs` | Breaks raw text into manageable pieces for analysis (Words, Sentences, Paragraphs). | **Implemented & Tested** |
| **Metrics Calculation** (Role 4) | `Metrics.fs` | Computes statistical data and readability scores (Flesch Reading Ease). | **Implemented & Tested** |
| **Frequency Analysis** (Role 5) | `FrequencyAnalyzer.fs` | Identifies and ranks key themes by counting non-stop words. | **Implemented & Tested** |
| **User Interface (GUI)** (Role 6) | `UI.fs` | Windows-based interface to orchestrate the pipeline and display results. | **Implemented** |
| 

### Key Features:

* **Validation:** Supports manual entry and `.txt` file uploads with checks for valid extension, file existence, and non-empty content.
* **Metrics:** Calculates **Word Count, Sentence Count, Paragraph Count, Average Sentence Length,** and the **Flesch Reading Ease Score**.
* **Frequency:** Filters out common "stop words" (`the`, `a`, `is`) and generates a **Top 10** table of most repeated words.
* **Output:** Controls to **Analyze**, save results, and **Export to JSON**.

---

##  Project Structure

```bash
/text-analyzer
│
├── Program.fs            # Main Application Entry Point
├── UI.fs                 # WinForms User Interface (Role 6)
├── InputValidation.fs    # File and Text Input Checks (Role 1)
├── Tokenizer.fs          # Text Splitting Logic (Role 3)
├── FrequencyAnalyzer.fs  # Stopword Filtering & Counting (Role 5)
├── Metrics.fs            # Statistical Logic (Role 4)
├── TextAnalyzer.fsproj   # Main Project Configuration
├── TextAnalyzer.Tests    # Test Project Folder
│   └── *.fs              # Unit and E2E Tests (e.g., InputValidationTests, E2ETests)
└── README.md             # Documentation