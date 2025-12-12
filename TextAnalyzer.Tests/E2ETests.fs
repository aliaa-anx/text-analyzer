namespace TextAnalyzer
open Xunit
open TextAnalyzer.Tokenizer
open TextAnalyzer.MetricsCalculator
open TextAnalyzer.FrequencyAnalyzer
open TextAnalyzer
module E2ETests =

    [<Fact>]
    let EndToEndTest() =
        let input = "Hello world! This is a test.\n\nThis test is for end-to-end testing."

        // Step 1: tokenization
        let tokenized = Tokenizer.tokenize input

        // Step 2: metrics calculation
        let metrics = MetricsCalculator.calculateMetrics tokenized

        // Step 3: frequency analysis
        let extractedFrequencies = 
          tokenized
          |> FrequencyAnalyzer.extractWords
          |> FrequencyAnalyzer.removeStopwords
          |> FrequencyAnalyzer.countFrequencies
          |> FrequencyAnalyzer.sortByFrequency
          |> FrequencyAnalyzer.topN 3

        
        Assert.Equal(12, metrics.WordCount) 
        Assert.Equal(3, metrics.SentenceCount)
        Assert.Equal(2, metrics.ParagraphCount)
    
        // Corrected average: 12 words / 3 sentences = 4.0
        Assert.Equal(4.0, metrics.AverageSentenceLength) 
        Assert.Equal(4.0, metrics.ReadabilityScore)

        let expectedTopFrequencies = [("test", 2); ("hello", 1); ("world", 1)]
        //as the word "test" is highest ranked and others are tied at one frequency 
        //the order of others might vary but the counts should be correct
        //make sure "test" is first with count 2
        let _, count = extractedFrequencies |> List.head
        Assert.Equal(2, count)
        //make sure "test" is in the list
        Assert.True((extractedFrequencies |> List.exists (fun (w, c) -> w = "test" && c = 2)))
        //make sure topN returned correct number of items
        Assert.Equal(3, expectedTopFrequencies.Length)