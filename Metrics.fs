namespace TextAnalyzer
open Tokenizer

module MetricsCalculator =

    // Metrics datatype
    type Metrics =
        { WordCount: int
          SentenceCount: int
          ParagraphCount: int
          AverageSentenceLength: float
          ReadabilityScore: float }

//------------------------- syllable counter------------------------

// This function estimates how many syllables are in a word

// How it works:
// first: it counts groups of vowels (a, e, i, o, u, y)
//    Example: "beau-ti-ful" -> vowel groups: "eau", "i", "u" → 3 syllables
//
// Second: If a word ends with a silent 'e', remove one syllable
//    Example: "love" → normally 2 vowel groups: "o", "e"
//    But 'e' is silent -> final count becomes 1
//
// Third: Always return at least 1 syllable:
//    Example: "my" -> 1 vowel group -> 1 syllable
//
// This is not a perfect English syllable counter,
// but it is accurate enough for readability formulas
//---------------------------------------------------------------------

    let countSyllables (word: string) =
        let vowels = ['a'; 'e'; 'i'; 'o'; 'u'; 'y']
        let chars = word.ToLower() |> Seq.toList // Convert the word to lower and then turns it into list of characters

        // Count vowel groups (ea, oo, ai = 1 group each)
        let mutable count = 0
        let mutable previousWasVowel = false

        for c in chars do
            if List.contains c vowels then
                if not previousWasVowel then
                    count <- count + 1
                previousWasVowel <- true
            else
                previousWasVowel <- false // if the letter not a vowel

        // Remove silent 'e' at the end of word bec it is not a syllable
        if chars |> List.last = 'e' && count > 1 then
            count <- count - 1

        if count = 0 then 1 else count  //Ensures that every word has at least 1 syllable


    //---------------------------------------------------------------------
    let calculateMetrics (input: TokenizedText) =
        let wordCount = input.Words.Length
        let sentenceCount = input.Sentences.Length
        let paragraphCount = input.Paragraphs.Length

        // Average Sentence Length
        let avgSentenceLength =
            if sentenceCount = 0 then 0.0
            else float wordCount / float sentenceCount

        // Total syllables
        let totalSyllables =
            input.Words
            |> List.sumBy countSyllables  // applies  countSyllables to each element of a list and sums the results

        // Average Syllables Per Word
        let avgSyllablesPerWord =
            if wordCount = 0 then 0.0
            else float totalSyllables / float wordCount

       
        //  Flesch Reading Formula (the readability formula i use)
        let readabilityScore =
            206.835 - (1.015 * avgSentenceLength) - (84.6 * avgSyllablesPerWord)
            |> max 0.0
            |> min 100.0

        // Return metrics
        { WordCount = wordCount
          SentenceCount = sentenceCount
          ParagraphCount = paragraphCount
          AverageSentenceLength = avgSentenceLength
          ReadabilityScore = readabilityScore }

