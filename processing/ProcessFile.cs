using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

class ProcessFile{
    public static void ProcessCodeFile(string sourceCode, string fileName){
        // Parse the source code into a Syntax Tree
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Get the root node of the Syntax Tree
        SyntaxNode rootNode = syntaxTree.GetRoot();

        // Display the structure of the AST
        string json = JsonSerializer.Serialize(MetaExtractionProcess.ExtractClassesAndMethods(rootNode), 
            new JsonSerializerOptions { WriteIndented = true });
        
        DocumentProcessor.SaveJsonFile(json,fileName);
        //Console.WriteLine(json);
    }

}