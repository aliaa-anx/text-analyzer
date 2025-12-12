//module for tokenizing text into words, sentences, and paragraphs
namespace TextAnalyzer

open System
//whole module gets called by no.4 text statistics
//task no.5 frequency analysis calls function getwords only

type TokenizedText =
    { Words: string list
      Sentences: string list
      Paragraphs: string list }


module Tokenizer =

    //1.word tokenizer
    let getwords (text: string) =
        // space, !, , . ? ; \t \n
        let separators =
            [| ' '
               '!'
               ','
               '.'
               '?'
               ';'
               ';'
               '\t'
               '\n'
               '\r' |]

        text.Split(separators, StringSplitOptions.None) // Use None first, then manually filter everything
        |> Array.map (fun s -> s.Trim()) // 1. Trim whitespace from start/end of every token
        |> Array.filter (fun s -> s <> "") // 2. Filter out any tokens that become empty after trimming
        |> Array.toList
    //2.Sentence tokenizer
    let getsentences (text: string) =
        // . ! ?
        let separators = [| '.'; '!'; '?'; '\n'; '\r' |]

        text.Split(separators, System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun s -> s.Trim()) // 1. Trim whitespace from start/end of every token
        |> Array.filter (fun s -> s <> "") // 2. Filter out any tokens that become empty after trimming
        |> Array.toList

    //3.Paragraph tokenizer
    let getparagraphs (text: string) =
        // \n\n
        let separators = [| '\n'; '\r' |]

        text.Split(separators, StringSplitOptions.RemoveEmptyEntries) // Use None first, then manually filter everything
        |> Array.map (fun s -> s.Trim()) // 1. Trim whitespace from start/end of every token
        // 2. Filter out any tokens that become empty after trimming
        |> Array.toList

    let tokenize (text: string) : TokenizedText =
        { Words = getwords text
          Sentences = getsentences text
          Paragraphs = getparagraphs text }
