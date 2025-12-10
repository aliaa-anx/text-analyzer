namespace TextAnalyzer.Tests

open Xunit
open TextAnalyzer // Opens the namespace where TokenizedText and Tokenizer live

module TokenizerTests =
    // --- Word Tokenizer Tests (getwords) ---

    [<Fact>]
    let getwords_splits_simpletext_into_words () =
        let text = "hello the cat sat on my bed."

        let expected: string list =
            [ "hello"
              "the"
              "cat"
              "sat"
              "on"
              "my"
              "bed" ]

        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX: Explicit list comparison

    [<Fact>]
    let getwords_handles_multiple_punctuations_and_whitespaces () =
        // NOTE: Expected list must match the output of getwords exactly!
        let text = "Hello, world!This     is a test.\nnew line here."

        let expected: string list =
            [ "Hello"
              "world"
              "This"
              "is"
              "a"
              "test"
              "new"
              "line"
              "here" ]

        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getwords_returns_empty_list_for_empty_string () =
        let text = ""
        let expected: string list = []
        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getwords_handles_only_punctuation_string () =
        let text = ",! ... ! \t \n ? ;"
        let expected: string list = []
        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getwords_handles_leading_trailing_whitespaces () =
        let text = "   Leading and trailing spaces   "

        let expected: string list =
            [ "Leading"
              "and"
              "trailing"
              "spaces" ]

        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getwords_handles_mixed_case_words () =
        let text = "This is a TeSt of MiXeD CaSe WoRdS."

        let expected: string list =
            [ "This"
              "is"
              "a"
              "TeSt"
              "of"
              "MiXeD"
              "CaSe"
              "WoRdS" ]

        let actual = Tokenizer.getwords text
        Assert.Equal<string list>(expected, actual) // FIX


    // --- Sentence Tokenizer Tests (getsentences) ---

    [<Fact>]
    let getsentences_splits_simpleMultipleSentencedText_into_sentences () =
        let text = "Hello world! This is a test. How are you?"

        let expected: string list =
            [ "Hello world"
              "This is a test"
              "How are you" ]

        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_handles_leading_trailing_whitespaces () =
        let text = "   Hello world!   This is a test.   "
        let expected: string list = [ "Hello world"; "This is a test" ]
        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_returns_empty_list_for_empty_string () =
        let text = ""
        let expected: string list = []
        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_handles_only_punctuation_string () =
        //let text = "!!!???"
        let text = "!!!...???\n\n\r\r"
        let expected: string list = []
        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_handles_multiple_consecutive_punctuations () =
        let text = "Hello!!! How are you??? This is great..."

        let expected: string list =
            [ "Hello"
              "How are you"
              "This is great" ]

        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_handles_newlines_within_text () =
        let text = "Hello world!\nThis is a test.\nHow are you?"

        let expected: string list =
            [ "Hello world"
              "This is a test"
              "How are you" ]

        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getsentences_handles_noSeparators () =
        let text = "This text has no sentence separators"
        let expected: string list = [ "This text has no sentence separators" ]
        let actual = Tokenizer.getsentences text
        Assert.Equal<string list>(expected, actual) // FIX


    // --- Paragraph Tokenizer Tests (getparagraphs) ---

    [<Fact>]
    let getparagraphs_splits_simpleMultipleParagraphText_into_paragraphs () =
        let text =
            "This is the first paragraph.\n\nThis is the second paragraph.\n\nThis is the third paragraph."

        let expected: string list =
            [ "This is the first paragraph."
              "This is the second paragraph."
              "This is the third paragraph." ]

        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getparagraphs_handles_leading_trailing_whitespaces () =
        let text =
            "   This is the first paragraph.   \n\n   This is the second paragraph.   "

        let expected: string list =
            [ "This is the first paragraph."
              "This is the second paragraph." ]

        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getparagraphs_returns_empty_list_for_empty_string () =
        let text = ""
        let expected: string list = []
        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getparagraphs_handles_only_newlines_string () =
        let text = "\n\n\n\r\r"
        let expected: string list = []
        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getparagraphs_handles_multiple_consecutive_newlines () =
        let text = "This is the first paragraph.\n\n\n\nThis is the second paragraph."

        let expected: string list =
            [ "This is the first paragraph."
              "This is the second paragraph." ]

        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX

    [<Fact>]
    let getparagraphs_handles_noNewlines () =
        let text = "This text has no paragraph separators."
        let expected: string list = [ "This text has no paragraph separators." ]
        let actual = Tokenizer.getparagraphs text
        Assert.Equal<string list>(expected, actual) // FIX


    // --- Combined Tokenizer Tests (tokenize) ---

    [<Fact>]
    let tokenize_processes_text_correctly () =
        let text = "Hello world! This is a test.\n\nNew paragraph here."

        let expectedWords: string list =
            [ "Hello"
              "world"
              "This"
              "is"
              "a"
              "test"
              "New"
              "paragraph"
              "here" ]

        let expectedSentences: string list =
            [ "Hello world"
              "This is a test"
              "New paragraph here" ]

        let expectedParagraphs: string list =
            [ "Hello world! This is a test."
              "New paragraph here." ]

        let actual = Tokenizer.tokenize text

        // FIX: Explicit list comparison on the record fields
        Assert.Equal<string list>(expectedWords, actual.Words)
        Assert.Equal<string list>(expectedSentences, actual.Sentences)
        Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)

    [<Fact>]
    let tokenize_handles_empty_string () =
        let text = ""
        let expectedWords: string list = []
        let expectedSentences: string list = []
        let expectedParagraphs: string list = []
        let actual = Tokenizer.tokenize text

        // FIX: Explicit list comparison on the record fields
        Assert.Equal<string list>(expectedWords, actual.Words)
        Assert.Equal<string list>(expectedSentences, actual.Sentences)
        Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)

    //[<Fact>]
    // let tokenize_handles_only_punctuation_and_newlines () =
    //     let text = "!!!...???\n\n\r\r"
    //     let expectedWords: string list = []
    //     let expectedSentences: string list = []
    //     let expectedParagraphs: string list = []
    //     let actual = Tokenizer.tokenize text

    //     // FIX: Explicit list comparison on the record fields
    //     Assert.Equal<string list>(expectedWords, actual.Words)
    //     Assert.Equal<string list>(expectedSentences, actual.Sentences)
    //     Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)

    [<Fact>]
    let tokenize_handles_mixed_content () =
        let text = "Hello, world!!!\nThis is a test.\n\nNew paragraph here???"

        let expectedWords: string list =
            [ "Hello"
              "world"
              "This"
              "is"
              "a"
              "test"
              "New"
              "paragraph"
              "here" ]

        let expectedSentences: string list =
            [ "Hello, world"
              "This is a test"
              "New paragraph here" ]

        let expectedParagraphs: string list =
            [ "Hello, world!!!"
              "This is a test."
              "New paragraph here???" ]

        let actual = Tokenizer.tokenize text

        // FIX: Explicit list comparison on the record fields
        Assert.Equal<string list>(expectedWords, actual.Words)
        Assert.Equal<string list>(expectedSentences, actual.Sentences)
        Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)

    [<Fact>]
    let tokenize_handles_leading_trailing_whitespaces () =
        let text = "   Hello world!   \n\n   This is a test.   "

        let expectedWords: string list =
            [ "Hello"
              "world"
              "This"
              "is"
              "a"
              "test" ]

        let expectedSentences: string list = [ "Hello world"; "This is a test" ]
        let expectedParagraphs: string list = [ "Hello world!"; "This is a test." ]
        let actual = Tokenizer.tokenize text

        // FIX: Explicit list comparison on the record fields
        Assert.Equal<string list>(expectedWords, actual.Words)
        Assert.Equal<string list>(expectedSentences, actual.Sentences)
        Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)

    [<Fact>]
    let tokenize_handles_noSeparators () =
        let text = "This text has no separators"

        let expectedWords: string list =
            [ "This"
              "text"
              "has"
              "no"
              "separators" ]

        let expectedSentences: string list = [ "This text has no separators" ]
        let expectedParagraphs: string list = [ "This text has no separators" ]
        let actual = Tokenizer.tokenize text

        // FIX: Explicit list comparison on the record fields
        Assert.Equal<string list>(expectedWords, actual.Words)
        Assert.Equal<string list>(expectedSentences, actual.Sentences)
        Assert.Equal<string list>(expectedParagraphs, actual.Paragraphs)
