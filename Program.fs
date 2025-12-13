open System
open System.IO
open System.Text.RegularExpressions

// Function to read text from a file with validation
let readTextFromFile (filePath: string) =
    if File.Exists(filePath) then
        if Path.GetExtension(filePath).ToLower() = ".txt" then
            let text = File.ReadAllText(filePath)
            if String.IsNullOrWhiteSpace(text) then
                printfn "The file is empty!"
                ""
            else
                // Remove extra spaces
                let cleanText = Regex.Replace(text, @"\s+", " ").Trim()
                cleanText
        else
            printfn "The file is not a .txt file!"
            ""
    else
        printfn "File does not exist!"
        ""

// Function to read text from user input
let readTextFromUser () =
    printfn "Enter the text you want to analyze (press Enter when done):"
    let input = Console.ReadLine()
    if String.IsNullOrWhiteSpace(input) then
        printfn "The text is empty!"
        ""
    else
        // Remove extra spaces
        Regex.Replace(input, @"\s+", " ").Trim()

// Function to choose input method
let getText () =
    printfn "Do you want to read text from a file or type it manually? (file/user)"
    let choice = Console.ReadLine()
    match choice.ToLower() with
    | "file" ->
        printfn "Enter the file path:"
        let path = Console.ReadLine()
        readTextFromFile path
    | "user" -> readTextFromUser ()
    | _ ->
        printfn "Invalid choice, defaulting to manual input."
        readTextFromUser ()

// Test the input handling
let text = getText ()
if text <> "" then
    printfn "The text ready for analysis:\n%s" text
else
    printfn "No valid text was entered."
