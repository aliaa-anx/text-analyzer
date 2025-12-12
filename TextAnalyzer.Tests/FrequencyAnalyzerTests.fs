namespace TextAnalyzer.Tests

open Xunit
open TextAnalyzer
open TextAnalyzer.FrequencyAnalyzer
open TextAnalyzer.Tokenizer
module FrequencyAnalyzerTests =
     
    let createTokenizedText words sentences paragraphs =
        { Words = words; Sentences = []; Paragraphs =  [] }
    // this one for comparing string lists
    let assertEqualStringList (expected: string list) (actual: string list) =
        
        Assert.Equal<string seq>(expected, actual)
    // this one for comparing string lists of tuples
    let assertEqualFreqList (expected: (string * int) list) (actual: (string * int) list) =
        Assert.Equal<(string * int) seq>(expected, actual)

    [<Fact>]
    let test_extractwords() =
        let input = createTokenizedText ["Hello"; "world"; "Test"] [] []
        let actual = FrequencyAnalyzer.extractWords input
        let expected = ["hello"; "world"; "test"]
        //Assert.Equal(expected, actual)
        assertEqualStringList expected actual
    [<Fact>]
    let test_convertToLowercase() =
        let input = createTokenizedText ["Hello"; "WORLD"; "TeSt"] [] []
        let actual = FrequencyAnalyzer.extractWords input
        let expected = ["hello"; "world"; "test"]
        assertEqualStringList expected actual
    [<Fact>]
    let test_removeStopwords() =
        let input = ["this"; "is"; "a"; "test"; "of"; "the"; "function"]
        let actual = FrequencyAnalyzer.removeStopwords input
        let expected = ["test"; "function"]
        //Assert.Equal(expected, actual)
        assertEqualStringList expected actual
    [<Fact>]
    let test_countFrequencies() =
        let input = createTokenizedText ["test"; "function"; "test"; "example"; "function"; "test"] [] []
        let actual = FrequencyAnalyzer.countFrequencies input.Words
        // i used set as order is not guranteed in frequency count
        let expected = set [("test", 3); ("function", 2); ("example", 1)]
        //Assert.Equal(expected |> Seq.toArray, set actual |> Seq.toArray)
        assertEqualFreqList (expected |> Seq.toList) (set actual |> Seq.toList)
        
    [<Fact>]
    let test_sortByFrequency_DescendingOrder() =
        let input = [("example", 1); ("test", 3); ("function", 2)]
        let actual = FrequencyAnalyzer.sortByFrequency input
        let expected = [("test", 3); ("function", 2); ("example", 1)]
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual
    [<Fact>]
    let test_topN_ReturnsTopNItems() =
        let input = [("test", 5); ("function", 3); ("example", 2); ("sample", 1)]
        let actual = FrequencyAnalyzer.topN 2 input
        let expected = [("test", 5); ("function", 3)]
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual
    [<Fact>]
    let test_analyze_FullPipeline() =
        let input = createTokenizedText ["This"; "is"; "a"; "test"; "This"; "test"; "is"; "only"; "a"; "test"] [] []
        let actual = FrequencyAnalyzer.analyze input
        let expected = [("test", 3); ("only", 1)]
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual

    [<Fact>]
    let test_analyze_EmptyInput() =
        let input = createTokenizedText [] [] []
        let actual = FrequencyAnalyzer.analyze input
        //used as list in f# has no type so it it can be a list of integers 
        //so to solve ambiguity during comparison i specified the type to avoid compiler errors
        let expected : (string * int) list = []
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual
    [<Fact>]
    let test_analyze_AllStopwords() =
        let input = createTokenizedText ["the"; "and"; "is"; "a"; "to"; "of"] [] []
        let actual = FrequencyAnalyzer.analyze input
        let expected : (string * int) list = []
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual
    [<Fact>]
    let test_analyze_TieFrequencies() =
        let input = createTokenizedText ["apple"; "banana"; "apple"; "banana"; "cherry"] [] []
        let actual = FrequencyAnalyzer.analyze input
        let expected = [("apple", 2); ("banana", 2); ("cherry", 1)]
        //Assert.Equal(expected, actual)
        assertEqualFreqList expected actual
    [<Fact>]
    let test_analyze_and_Count_and_sorts_pipeline() =
        let words = [
            "The"; "F#"; "code"; "is"; "functional"; 
            "and"; "the"; "F#"; "code"; "is"; "excellent"; 
            "and"; "the"; "F#"; "code"; "is"; "concise"; 
            "a"; "b"; "c"; "d"; "e"; "f"; "g"; "h"; "i"; "j"; "k"; "l"; "m"; "n" // Adding extra words to exceed top 10
        ]
        let input = createTokenizedText words [] []
        let actual = FrequencyAnalyzer.analyze input //should return top 10
        //let expected = [
            //("f#", 3); ("code", 3); 
            //("functional", 1); ("excellent", 1); ("concise", 1); 
            //("b", 1); ("c", 1); ("d", 1),("e", 1) //for example order isn't guranteed
       // ]
        
        Assert.Equal(10, actual.Length) //MAKE SURE IT RETURNED LIST OF 10 TUPPLES
        let ExpectedTop5= [
            ("f#", 3); ("code", 3); 
            ("functional", 1); ("excellent", 1); ("concise", 1)
        ]
        assertEqualFreqList ExpectedTop5 (actual |> List.truncate 5)
 