module task4
//  the output i will take from Aliaa (task3)
type TokenizedText =
    { Words: string list
      Sentences: string list
      Paragraphs: string list }

module MetricsCalculator =

    // define the metrics datatype
    type Metrics =
        { WordCount: int
          SentenceCount: int
          ParagraphCount: int
          AverageSentenceLength: float
          ReadabilityScore: float }

    // calculate metrics
    let calculateMetrics (input: TokenizedText) =
        let wordCount = input.Words.Length
        let sentenceCount = input.Sentences.Length
        let paragraphCount = input.Paragraphs.Length

        // Average sentence length
        let avgSentenceLength =
            if sentenceCount = 0 then 
                0.0
            else 
                float wordCount / float sentenceCount

        // Simplify readability
        let readabilityScore = avgSentenceLength

        { WordCount = wordCount
          SentenceCount = sentenceCount
          ParagraphCount = paragraphCount
          AverageSentenceLength = avgSentenceLength
          ReadabilityScore = readabilityScore }


// testing

let exampleTokenized : TokenizedText =
    { Words = ["I"; "love"; "AI"; "very"; "much"]
      Sentences = ["I love AI"; "very much"]
      Paragraphs = ["I love AI. very much"] }

let result = MetricsCalculator.calculateMetrics exampleTokenized

printfn "Word Count: %d" result.WordCount
printfn "Sentence Count: %d" result.SentenceCount
printfn "Paragraph Count: %d" result.ParagraphCount
printfn "Average Sentence Length: %f" result.AverageSentenceLength
printfn "Readability Score: %f" result.ReadabilityScore
