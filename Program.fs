(* open System
open System.IO
open System.Text.RegularExpressions
open System.Windows.Forms

let form = new Form(Text = "Text Input & Validation", Width = 600, Height = 400)

let label = new Label(Text = "Enter text manually or upload a .txt file:", Top = 10, Left = 10, Width = 400)
let textBox = new TextBox(Multiline = true, Width = 550, Height = 200, Top = 30, Left = 10)
let uploadButton = new Button(Text = "Upload File", Top = 240, Left = 10, Width = 100)
let prepareButton = new Button(Text = "Preprocessing", Top = 240, Left = 120, Width = 100)
let outputLabel = new Label(Text = "", Top = 280, Left = 10, Width = 550, Height = 80)

let cleanText (text: string) =
    Regex.Replace(text, @"\s+", " ").Trim()

// Upload file button 
uploadButton.Click.Add(fun _ ->
    use ofd = new OpenFileDialog(Filter = "All files|*.*")

    if ofd.ShowDialog() = DialogResult.OK then
        let path = ofd.FileName

        
        if Path.GetExtension(path).ToLower() <> ".txt" then
            MessageBox.Show("Only .txt files are allowed!") |> ignore
        else
           
            try
                let text = File.ReadAllText(path)
                if String.IsNullOrWhiteSpace(text) then
                    MessageBox.Show("The file is empty.") |> ignore
                else
                    textBox.Text <- cleanText text
            with
            | ex -> MessageBox.Show("Error reading file: " + ex.Message) |> ignore
)

// prepare Button  
prepareButton.Click.Add(fun _ ->
    let text = textBox.Text
    if String.IsNullOrWhiteSpace(text) then
        MessageBox.Show("You didn't type anything!") |> ignore
    else
        let cleaned = cleanText text
        outputLabel.Text <- "Here’s your text ready to analyze:\n" + cleaned
)

form.Controls.Add(label)
form.Controls.Add(textBox)
form.Controls.Add(uploadButton)
form.Controls.Add(prepareButton)
form.Controls.Add(outputLabel)

[<STAThread>]
Application.Run(form)
 *)


namespace TextAnalyzer

open System
open Tokenizer
open MetricsCalculator

module Program = 
    [<EntryPoint>]
    let main argv = 
        printfn "--- INTEGRATION TEST ---"

        // 1. Create Dummy Data
        let exampleTokenized : TokenizedText =
            { Words = ["I"; "love"; "AI"; "very"; "much"]
              Sentences = ["I love AI"; "very much"]
              Paragraphs = ["I love AI. very much"] }

        // 2. Call Metrics
        let result = MetricsCalculator.calculateMetrics exampleTokenized

        // 3. Print Results
        printfn "Word Count: %d" result.WordCount
        printfn "Sentence Count: %d" result.SentenceCount
        printfn "Paragraph Count: %d" result.ParagraphCount
        printfn "Avg Sentence Length: %.2f" result.AverageSentenceLength
        printfn "Readability Score: %.2f" result.ReadabilityScore

        0 // Exit code