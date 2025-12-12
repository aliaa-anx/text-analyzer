namespace TextAnalyzer.Tests

open Xunit
open TextAnalyzer
open TextAnalyzer.MetricsCalculator
open TextAnalyzer.Tokenizer
//open TextAnalyzer.TokenizedText
module MetricesTests =
    let createTokenizedText words sentences paragraphs =
        { Words = words; Sentences = sentences; Paragraphs = paragraphs }
    [<Fact>]
    let calculateMetrices_emptyText() =
        let input = createTokenizedText [] [] []
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(0, actual.WordCount)
        Assert.Equal(0, actual.SentenceCount)
        Assert.Equal(0, actual.ParagraphCount)
        Assert.Equal(0.0, actual.AverageSentenceLength)
        Assert.Equal(100.0, actual.ReadabilityScore)
    [<Fact>]
    let calculateMetrices_sampleText() =
        let words = ["I"; "love"; "programming"; "in"; "F#"]
        let sentences = ["I love programming in F#"]
        let paragraphs = ["I love programming in F#."]
        let input = createTokenizedText words sentences paragraphs
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(5, actual.WordCount)
        Assert.Equal(1, actual.SentenceCount)
        Assert.Equal(1, actual.ParagraphCount)
        Assert.Equal(5.0, actual.AverageSentenceLength)
        Assert.Equal(83.32, actual.ReadabilityScore, 2)
    [<Fact>]
    let calculateMetrices_multipleSentences() =
        let words = ["This"; "is"; "the"; "first"; "sentence"; "This"; "is"; "the"; "second"]
        let sentences = ["This is the first sentence"; "This is the second"]
        let paragraphs = ["This is the first sentence. This is the second."]
        let input = createTokenizedText words sentences paragraphs
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(9, actual.WordCount)
        Assert.Equal(2, actual.SentenceCount)
        Assert.Equal(1, actual.ParagraphCount)
        Assert.Equal(4.5, actual.AverageSentenceLength)
        Assert.Equal(98.87, actual.ReadabilityScore, 2)
    [<Fact>]
    let calculateMetrices_noSentences() =
        //avoids division by zero
        let words = ["Lonely"; "words"; "without"; "sentences"]
        // unusual input
        let sentences = []
        let paragraphs = ["Lonely words without sentences."]
        let input = createTokenizedText words sentences paragraphs
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(4, actual.WordCount)
        Assert.Equal(0, actual.SentenceCount)
        Assert.Equal(1, actual.ParagraphCount)
        //0.0 not 2/0
        Assert.Equal(0.0, actual.AverageSentenceLength)
        Assert.Equal(16.49, actual.ReadabilityScore, 2)
    [<Fact>]
    let calculateMetrices_noWords() =
       // case of unusual input
        let words = []
        let sentences = ["An empty sentence"]
        let paragraphs = ["An empty sentence."]
        let input = createTokenizedText words sentences paragraphs
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(0, actual.WordCount)
        Assert.Equal(1, actual.SentenceCount)
        Assert.Equal(1, actual.ParagraphCount)
        Assert.Equal(0.0, actual.AverageSentenceLength)
        Assert.Equal(100.0, actual.ReadabilityScore)
    [<Fact>]
    let calculateMetrices_multipleParagraphs() =
        let words = ["First"; "paragraph"; "Second"; "paragraph"; "Third"]
        let sentences = ["First paragraph"; "Second paragraph"; "Third"]
        let paragraphs = ["First paragraph."; "Second paragraph."; "Third."]
        let input = createTokenizedText words sentences paragraphs
        let actual = MetricsCalculator.calculateMetrics input
        Assert.Equal(5, actual.WordCount)
        Assert.Equal(3, actual.SentenceCount)
        Assert.Equal(3, actual.ParagraphCount)
        Assert.Equal(1.6666666666666667, actual.AverageSentenceLength)
        Assert.Equal(35.94, actual.ReadabilityScore, 2)
    