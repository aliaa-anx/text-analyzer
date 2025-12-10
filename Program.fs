namespace TextAnalyzer

open System
open System.Windows.Forms

module Program =

    [<STAThread>]
    [<EntryPoint>]
    let main argv =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)

        // Fully qualify the type from the UI module
        let form = new UI.MainForm()
        Application.Run(form)

        0



//(* open System
//open System.IO
//open System.Text.RegularExpressions
//open System.Windows.Forms

//let form = new Form(Text = "Text Input & Validation", Width = 600, Height = 400)

//let label = new Label(Text = "Enter text manually or upload a .txt file:", Top = 10, Left = 10, Width = 400)
//let textBox = new TextBox(Multiline = true, Width = 550, Height = 200, Top = 30, Left = 10)
//let uploadButton = new Button(Text = "Upload File", Top = 240, Left = 10, Width = 100)
//let prepareButton = new Button(Text = "Preprocessing", Top = 240, Left = 120, Width = 100)
//let outputLabel = new Label(Text = "", Top = 280, Left = 10, Width = 550, Height = 80)

//let cleanText (text: string) =
//    Regex.Replace(text, @"\s+", " ").Trim()

//// Upload file button 
//uploadButton.Click.Add(fun _ ->
//    use ofd = new OpenFileDialog(Filter = "All files|*.*")

//    if ofd.ShowDialog() = DialogResult.OK then
//        let path = ofd.FileName

        
//        if Path.GetExtension(path).ToLower() <> ".txt" then
//            MessageBox.Show("Only .txt files are allowed!") |> ignore
//        else
           
//            try
//                let text = File.ReadAllText(path)
//                if String.IsNullOrWhiteSpace(text) then
//                    MessageBox.Show("The file is empty.") |> ignore
//                else
//                    textBox.Text <- cleanText text
//            with
//            | ex -> MessageBox.Show("Error reading file: " + ex.Message) |> ignore
//)

//// prepare Button  
//prepareButton.Click.Add(fun _ ->
//    let text = textBox.Text
//    if String.IsNullOrWhiteSpace(text) then
//        MessageBox.Show("You didn't type anything!") |> ignore
//    else
//        let cleaned = cleanText text
//        outputLabel.Text <- "Here’s your text ready to analyze:\n" + cleaned
//)

//form.Controls.Add(label)
//form.Controls.Add(textBox)
//form.Controls.Add(uploadButton)
//form.Controls.Add(prepareButton)
//form.Controls.Add(outputLabel)

//[<STAThread>]
//Application.Run(form)
// *)


//namespace TextAnalyzer

//open System
//open System.IO
//open Tokenizer
//open MetricsCalculator

//module Program = 
//   // Simple assertion helper for testing without external libraries
//    let private assertEqual expected actual message =
//        if expected = actual then
//            printfn "  PASS: %s" message
//            true
//        else
//            printfn "  FAIL: %s (Expected: %A, Actual: %A)" message expected actual
//            false

//    // Full end-to-end test case: File Path -> InputValidation -> Tokenizer -> MetricsCalculator
//    let runIntegrationTest () =
//        printfn "\n--- Starting Full File-Based Integration Test ---"

//        let testFilePath = "test_data.txt"

//        // --- 1. Define Test Input & Create File ---
//        let rawText = 
//            "Hello, world! This is the first sentence. It has eight words.\r\n" +
//            "\r\n" +
//            "This is paragraph two. It has five words."
        
//        // This 'try...with' block is now only for SETUP. We must ensure it returns 'unit' in both branches.
//        try
//            File.WriteAllText(testFilePath, rawText)
//            printfn "  SETUP: Created temporary test file: %s" testFilePath
//        with ex ->
//            printfn "  ERROR: Could not create test file: %s. Test execution aborted." ex.Message
//            // FIX: Return a failure code immediately if setup fails.
//            // We use '1' here but need to wrap the whole test in a try/finally for cleanup if possible, 
//            // but for simplicity, we return 1 here and use the main program entry point for the final exit.
//            // For now, let's make sure this block returns unit.
//            () // Return unit on success or failure for the side-effect (write file/print error)

//        let mutable success = true
//        let mutable metrics: MetricsCalculator.Metrics option = None
//        let mutable rawContent: string option = None

//        // --- 2. Execute Pipeline: InputValidation -> Tokenizer -> MetricsCalculator ---
        
//        printfn "\n--- Stage 1: InputValidation.tryReadFile ---"
//        match InputValidation.tryReadFile testFilePath with
//        | Ok content -> 
//            printfn "  PASS: File read successfully."
//            rawContent <- Some content

//            // --- Stage 2 & 3: Tokenizer -> MetricsCalculator ---
//            printfn "\n--- Stage 2 & 3: Tokenizer & MetricsCalculator ---"
//            let tokenized = Tokenizer.tokenize content
//            let calculatedMetrics = MetricsCalculator.calculateMetrics tokenized
//            metrics <- Some calculatedMetrics

//            // --- 3. Define Expected Outputs (Manual Analysis) ---
//            let expectedWordCount = 19
//            let expectedSentenceCount = 5
//            let expectedParagraphCount = 2
//            let expectedAvgSentenceLength = 19.0 / 5.0 // 3.8
//            let expectedReadabilityScore = expectedAvgSentenceLength

//            // --- 4. Assert Tokenizer Results (Implicitly tested by counts) ---
            
//            // --- 5. Assert Metrics Results ---
//            success <- success && assertEqual expectedWordCount calculatedMetrics.WordCount "Word Count"
//            success <- success && assertEqual expectedSentenceCount calculatedMetrics.SentenceCount "Sentence Count"
//            success <- success && assertEqual expectedParagraphCount calculatedMetrics.ParagraphCount "Paragraph Count"
            
//            // Use a tolerance for float comparisons (0.001)
//            let avgPass = abs (expectedAvgSentenceLength - calculatedMetrics.AverageSentenceLength) < 0.001
//            success <- success && avgPass
//            if avgPass then
//                 printfn "  PASS: Average Sentence Length (%.2f)" calculatedMetrics.AverageSentenceLength
//            else
//                 printfn "  FAIL: Average Sentence Length (Expected: %.2f, Actual: %.2f)" expectedAvgSentenceLength calculatedMetrics.AverageSentenceLength
                 
//            let readPass = abs (expectedReadabilityScore - calculatedMetrics.ReadabilityScore) < 0.001
//            success <- success && readPass
//            if readPass then
//                 printfn "  PASS: Readability Score (%.2f)" calculatedMetrics.ReadabilityScore
//            else
//                 printfn "  FAIL: Readability Score (Expected: %.2f, Actual: %.2f)" expectedReadabilityScore calculatedMetrics.ReadabilityScore

//        | Error msg -> 
//            printfn "  FAIL: InputValidation failed: %s" msg
//            success <- false
        
//        // --- 6. Cleanup ---
//        try
//            File.Delete(testFilePath)
//            printfn "\n  CLEANUP: Deleted temporary test file."
//        with ex ->
//            printfn "  ERROR: Could not delete test file: %s" ex.Message

//        // --- 7. Final Report ---
//        printfn "\n--- Test Summary ---"
//        if success then
//            printfn "✅ ALL INTEGRATION TESTS PASSED!"
//            0 // Exit Code 0 for success
//        else
//            printfn "❌ INTEGRATION TEST FAILED. Check the details above."
//            1 // Exit Code 1 for failure

//    // FIX: The EntryPoint must be defined once, and call the test function.
//    [<EntryPoint>]
//    let main argv = 
//        // FIX: The runIntegrationTest function must be CALLED, not just referenced.
//        runIntegrationTest ()