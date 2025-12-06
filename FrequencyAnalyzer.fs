namespace TextAnalyzer

open System.Text.RegularExpressions

module FrequencyAnalyzer =

    // Stopwords
    let stopwords =
        set [
            "the"; "a"; "an"; "and"; "or"; "but"; "to"; "of"; "in"; "on"; "for";
            "is"; "are"; "was"; "were"; "be"; "it"; "this"; "that"; "with"; "as";
            "at"; "by"; "from"; "up"; "down"; "out"; "over"; "under"
        ]

    // Extract words from text
    let extractWords (text: string) : string list =
        Regex.Matches(text.ToLowerInvariant(), @"[a-z']+")
        |> Seq.cast<Match>
        |> Seq.map (fun m -> m.Value)
        |> Seq.toList

    // Remove stopwords
    let removeStopwords (words: string list) : string list =
        words |> List.filter (fun w -> not (stopwords.Contains w))

    // Count frequencies
    let countFrequencies (words: string list) : (string * int) list =
        words
        |> Seq.groupBy id
        |> Seq.map (fun (word, group) -> word, Seq.length group)
        |> Seq.toList

    // Sort by frequency
    let sortByFrequency (freqs: (string * int) list) =
        freqs |> List.sortByDescending snd

    // Take top N
    let topN (n: int) (sortedFreqs: (string * int) list) =
        sortedFreqs |> List.truncate n

    // Public function
    let analyze (text: string) : (string * int) list =
        text
        |> extractWords
        |> removeStopwords
        |> countFrequencies
        |> sortByFrequency
        |> topN 10
