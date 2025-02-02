using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

class ProcessFile{
    public static void ProcessCodeFile(string sourceCode, string fileName){
        // Parse the source code into a Syntax Tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Get the root node of the Syntax Tree
        SyntaxNode rootNode = syntaxTree.GetRoot();

        string json = JsonConvert.SerializeObject(
            MetaExtractionProcess.ExtractClassesAndMethods(rootNode), 
            Formatting.Indented);
        
        DocumentProcessor.SaveJsonFile(json,fileName);
        //Console.WriteLine("Formatted Text " + FormatRecordToText(json));
    }

    /// <summary>
    /// Converts a JSON-serialized string representing a record into a unified text string.
    /// It concatenates key-value pairs using a template and skips specific keys if needed.
    /// </summary>
    /// <param name="jsonString">The JSON string representing the record.</param>
    /// <returns>A formatted text string.</returns>
    public static string FormatRecordToText(string jsonString)
    {
        // Deserialize the JSON string into a dictionary.
        // Using object as the value type to handle various data types.
        var record = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
        if (record == null)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();
        
        // Loop through each key-value pair.
        foreach (var kv in record)
        {
            // Optionally, skip keys that should not be included in the output.
            // For example, if you want to skip a "last_modified" field:
            if (kv.Key.Equals("last_modified", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Create a formatted sentence for each key-value pair.
            // This example capitalizes the key and appends the value.
            sb.Append($"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kv.Key)}: {kv.Value}; ");
        }

        // Remove the trailing semicolon and space, then add a period at the end.
        string unifiedText = sb.ToString().TrimEnd(' ', ';') + ".";
        return unifiedText;
    }

}