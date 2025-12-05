namespace TextAnalyzer

open System
open System.IO


module InputValidation =

    //check if file extension is .txt
    let isValidExtension (filePath: string) =
        Path.GetExtension(filePath).ToLower() = ".txt"

    // reads the file content 
    let tryReadFile (filePath: string) =
        try
            if not (File.Exists(filePath)) then
                Error "File not found!"
            
            elif not (isValidExtension filePath) then
                Error "Invalid file type. Only .txt files are allowed."
            
            else
                let content = File.ReadAllText(filePath)
                
                if String.IsNullOrWhiteSpace(content) then
                    Error "The file is empty."
                else
                    // Success! Return the RAW content.
                    Ok content
        with
        | ex -> Error ("System Error: " + ex.Message)

    //validate manual text input
    let validateManualInput (text: string) =
        if String.IsNullOrWhiteSpace(text) then
            Error "You didn't type anything! Please enter some text."
        else
            // Success! Return the raw text so Role 3 can process it.
            Ok text