//module for tokenizing text into words, sentences, and paragraphs

open System
//whole module gets called by no.4 text statistics
//task no.5 frequency analysis calls function getwords only

type TokenizedText =
    { Words: string list
      Sentences: string list
      Paragraphs: string list } 

      
module Tokenizer=

  //1.word tokenizer
    let getwords(text:string)=
        // space, !, , . ? ; \t \n
        let separators=[|' ' ; '!'; ','; '.' ; '?'; ';' ;';' ; '\t' ;'\n'|]
        text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
        |>Array.toList
   //2.Sentence tokenizer
    let getsentences(text:string)=
          // . ! ?
        let separators=[|'.' ; '!'; '?'|]
        text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
        |>Array.map(fun s->s.Trim())
        |>Array.toList

   //3.Paragraph tokenizer
    let getparagraphs(text:string)=
          // \n\n
        let separators=[|'\n';'\r'|]
        text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
        |>Array.map(fun s->s.Trim())
        |>Array.toList

    let tokenize (text: string) : TokenizedText =
        { Words = getwords text
          Sentences = getsentences text
          Paragraphs = getparagraphs text }

