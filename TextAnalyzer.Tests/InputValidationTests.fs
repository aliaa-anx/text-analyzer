namespace TextAnalyzer
open Xunit
open TextAnalyzer
open TextAnalyzer.InputValidation
module InputValidationTests =
    
    let validInput="this is a valid input"
    let InvalidInput=""
    let invalidExtension=".pdf"
    let validExtension=".txt"

    let createTempFile (extension: string) (content: string) =
        let fileName = System.IO.Path.GetRandomFileName()
        let filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{fileName}{extension}")
        System.IO.File.WriteAllText(filePath, content)
        let cleanup = fun () -> 
          if System.IO.File.Exists(filePath) then 
            System.IO.File.Delete(filePath)
        (filePath, cleanup)

    [<Fact>]
    let test_isValidExtension_ValidTxtFile() =
        let filePath = "document.txt"
        let result = InputValidation.isValidExtension filePath
        Assert.True(result)

    [<Fact>]
    let test_isValidExtension_InvalidFile() =
        let filePath = "image.png"
        let result = InputValidation.isValidExtension filePath
        Assert.False(result)

    [<Fact>]
    let test_validateManualInput_NonEmptyText() =
        let input = "This is a sample text."
        let result = InputValidation.validateManualInput input
        match result with
        | Ok text -> Assert.Equal(input, text)
        | Error msg -> Assert.True(false, "Expected Ok but got Error: " + msg)

    [<Fact>]
    let test_validateManualInput_EmptyText() =
        let input = "   "
        let result = InputValidation.validateManualInput input
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("You didn't type anything! Please enter some text.", msg)
    [<Fact>]
    let test_tryReadFile_NonExistentFile() =
        let filePath = "nonexistent.txt"
        let result = InputValidation.tryReadFile filePath
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("File not found!", msg)
    
    [<Fact>]
    let test_tryReadFile_InvalidExtension() =
        let filePath, cleanup = createTempFile invalidExtension validInput
        try
        let result = InputValidation.tryReadFile filePath
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("Invalid file type. Only .txt files are allowed.", msg)
        finally
            cleanup()
    [<Fact>]
    let test_tryReadFile_EmptyFile() =
        let filePath, cleanup = createTempFile validExtension ""
        try
        let result = InputValidation.tryReadFile filePath
        match result with
        | Ok _ -> Assert.True(false, "Expected Error but got Ok")
        | Error msg -> Assert.Equal("The file is empty, Upload a .txt file that contains ACTUAL TEXT", msg)
        finally
            cleanup()
    [<Fact>]
    let test_tryReadFile_ValidFile() =
        let filePath, cleanup = createTempFile validExtension validInput
        try
        let result = InputValidation.tryReadFile filePath
        match result with
        | Ok text -> Assert.Equal(validInput, text)
        | Error msg -> Assert.True(false, "Expected Ok but got Error: " + msg)
        finally
            cleanup()