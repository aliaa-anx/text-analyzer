namespace TextAnalyzer

module UI =

    open System
    open System.Windows.Forms
    open System.Drawing
    open System.IO
    open System.Text.Json
    open System.Text.Json.Serialization


    open TextAnalyzer.InputValidation
    open TextAnalyzer.Tokenizer
    open TextAnalyzer.MetricsCalculator
    open TextAnalyzer.FrequencyAnalyzer

    type MainForm() as this =
        inherit Form()

        // main form properties
        do
            this.Text <- "Text Analyzer System"
            this.Size <- Size(950, 700)
            this.StartPosition <- FormStartPosition.CenterScreen
            this.BackColor <- Color.Lavender

        // the textbox if the user want to enter the text by himself not upload it from a textfile
        let txtInput =
            new TextBox(
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,                  // expand the textbox to fill the panel it’s in
                Font = new Font("Segoe UI", 10.0f)
            )

        // the buttons
        let btnLoad =
            new Button(
                Text = "Load Text",
                BackColor = Color.Plum,
                FlatStyle = FlatStyle.Flat,
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10.0f)
            )

        let btnAnalyze =
            new Button(
                Text = "Analyze",
                BackColor = Color.Thistle,
                FlatStyle = FlatStyle.Flat,
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10.0f)
            )

        let btnExportJSON =
            new Button(
                Text = "Export JSON",
                BackColor = Color.LavenderBlush,
                FlatStyle = FlatStyle.Flat,
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 10.0f)
            )



        // those are the two tables one shows metrics calculations and the other shows frequency analysis
        let metricsCalculationsTable = new DataGridView(Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true)
        let frequencyAnalysisTable = new DataGridView(Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true)

        // thats the top panel that contains the buttons
        let topPanel = new TableLayoutPanel(
            Dock = DockStyle.Top,
            Height = 100
        )

        do
            topPanel.ColumnCount <- 3
            topPanel.RowCount <- 1
            topPanel.ColumnStyles.Add(ColumnStyle(SizeType.Percent, 33.3f))
            topPanel.ColumnStyles.Add(ColumnStyle(SizeType.Percent, 33.3f))
            topPanel.ColumnStyles.Add(ColumnStyle(SizeType.Percent, 33.3f))

            btnLoad.Anchor <- AnchorStyles.None
            btnAnalyze.Anchor <- AnchorStyles.None
            btnExportJSON.Anchor <- AnchorStyles.None

            topPanel.Controls.Add(btnLoad, 0, 0)
            topPanel.Controls.Add(btnAnalyze, 1, 0)
            topPanel.Controls.Add(btnExportJSON, 2, 0)

        
        // thats the panel for input textbox
        let textPanel = new Panel(Dock = DockStyle.Top, Height = 250, Padding = Padding(10))
        do textPanel.Controls.Add(txtInput)

        // thats the tables' panel
        let tablesPanel = new TableLayoutPanel(Dock = DockStyle.Fill, Padding = Padding(10))
        do
            tablesPanel.ColumnCount <- 2        // one column for showing the metrics calculations and the other for showing frequency analysis
            tablesPanel.RowCount <- 2
            tablesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))  // for showing metrics calculations
            tablesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f))  // for showing frequency analysis
            tablesPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30.0f))       // for showing labels, the name of each table
            tablesPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f))       // for showning the content of the tables

            // those are the labels for each table
            let lblResults =
                new Label(
                    Text = "Metrics Calculations",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 11.0f, FontStyle.Bold)
                )

            let lblFrequency =
                new Label(
                    Text = "Frequency Analysis",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 11.0f, FontStyle.Bold)
                )

            // adding the labels and the tables to the tablesPanel
            tablesPanel.Controls.Add(lblResults, 0, 0)
            tablesPanel.Controls.Add(lblFrequency, 1, 0)
            tablesPanel.Controls.Add(metricsCalculationsTable, 0, 1)
            tablesPanel.Controls.Add(frequencyAnalysisTable, 1, 1)

        // adding panels to form to be able to be viewed in the ui, from bottop to top to be viewed from top to bottom ;)
        do
            this.Controls.Add(tablesPanel)
            this.Controls.Add(textPanel)
            this.Controls.Add(topPanel)

        // this function fills metrics calculations table, |> ignore → ignores the return value i used it to avoid the annoying warnings :)
        let fillMetricsGrid (m: Metrics) =
            metricsCalculationsTable.Columns.Clear()
            metricsCalculationsTable.Rows.Clear()

            metricsCalculationsTable.Columns.Add("Metric", "Metric") |> ignore      // first column in the metrics calculations table (name="Metric", value="Metric")
            metricsCalculationsTable.Columns.Add("Value", "Value") |> ignore        // second column in the metrics calculations table (name="Value", value="Value")

            // filling the actual values
            metricsCalculationsTable.Rows.Add("Word Count", m.WordCount) |> ignore
            metricsCalculationsTable.Rows.Add("Sentence Count", m.SentenceCount) |> ignore
            metricsCalculationsTable.Rows.Add("Paragraph Count", m.ParagraphCount) |> ignore
            metricsCalculationsTable.Rows.Add("Avg Sentence Length", m.AverageSentenceLength) |> ignore
            metricsCalculationsTable.Rows.Add("Readability Score", m.ReadabilityScore) |> ignore

        // this function fills frequency analysis table
        let fillFrequencyGrid (freqs: (string * int) list) =
            frequencyAnalysisTable.Columns.Clear()
            frequencyAnalysisTable.Rows.Clear()

            frequencyAnalysisTable.Columns.Add("Word", "Word") |> ignore
            frequencyAnalysisTable.Columns.Add("Count", "Count") |> ignore

            // filling the actual values
            for (word, count) in freqs do
                frequencyAnalysisTable.Rows.Add(word, count) |> ignore


        // this function exports the results of the metrics calculations and the frequency analysis into a json file
        let exportToJson (metrics: Metrics) (freqs: (string * int) list) =
            let wordFreq =
                freqs
                |> List.map (fun (word, count) -> 
                    {| Word = word; Count = count |}
                )

            let data =
                {| Metrics_Calculations = metrics
                   Frequency_Analysis = wordFreq |}

            use sfd = new SaveFileDialog()
            sfd.Filter <- "JSON Files|*.json|All Files|*.*"
            sfd.Title <- "Save Analysis as JSON"

            if sfd.ShowDialog() = DialogResult.OK then
                let json =
                    JsonSerializer.Serialize(
                        data,
                        JsonSerializerOptions(WriteIndented = true)
                    )

                File.WriteAllText(sfd.FileName, json)
                MessageBox.Show("JSON File exported successfully!") |> ignore



        // to keep track of analysis state because we will need it when we wanna export to JSON
        let mutable hasAnalyzed = false

        // to keep the metrics calculations and frequecy analysis results because we will need it when we wanna export to JSON
        let mutable lastMetricsCalculations : Metrics option = None     // we studied it in an earlier lecture, do u remember? if you don't go study!
        let mutable lastFrequencyAnalysis : (string * int) list option = None

        // now what happens when we click any button? hmmmmmmmm.....
        do
            // Load Text From File
            btnLoad.Click.Add(fun _ ->
                use ofd = new OpenFileDialog()
                // make sure that only filtering is not enough at all, humans are smarter than that so we needed to handle it if humans could pass it :)
                ofd.Filter <- "Text Files|*.txt|All Files|*.*"

                if ofd.ShowDialog() = DialogResult.OK then
                    match tryReadFile ofd.FileName with
                    | Ok content -> txtInput.Text <- content        // if everything is oke then fill the textbox with the content of the textfile
                    | Error msg ->                                  // if the user bypassed the filter or the file is empty then an error message is displayed
                        MessageBox.Show(
                            msg,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        ) |> ignore 
            )

            // analyze → validate → tokenize → metrics
            btnAnalyze.Click.Add(fun _ ->
                match validateManualInput txtInput.Text with
                | Error msg ->
                    MessageBox.Show(
                        msg,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    ) |> ignore
                    // keep it false as it is to not let the user export amoty thing
                    hasAnalyzed <- false

                | Ok rawText ->
                    let tokenized = Tokenizer.tokenize rawText
                    let metrics = MetricsCalculator.calculateMetrics tokenized
                    let frequencyAnalysis = FrequencyAnalyzer.analyze tokenized

                    fillMetricsGrid metrics
                    fillFrequencyGrid frequencyAnalysis

                    // as i just said we need to save these analysis results because we will need it when we wanna export to JSON, so FOCUS 0_0
                    lastMetricsCalculations <- Some metrics
                    lastFrequencyAnalysis <- Some frequencyAnalysis
                    hasAnalyzed <- true

                    MessageBox.Show("Analysis Completed Successfully!") |> ignore
            )

            btnExportJSON.Click.Add(fun _ ->
                if not hasAnalyzed then
                    MessageBox.Show(
                        "Please analyze the text first before exporting.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    ) |> ignore
                else
                    match lastMetricsCalculations, lastFrequencyAnalysis with
                    | Some metrics, Some frequency ->       // export only if their status is 'Some' means they contain values, if its 'None' then its empty so error
                        exportToJson metrics frequency
                    | _ ->
                        MessageBox.Show(
                            "Unexpected error: analysis data is missing.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        ) |> ignore
            )
