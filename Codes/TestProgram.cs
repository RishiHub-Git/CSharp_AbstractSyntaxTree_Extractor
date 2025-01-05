using System;
using System.IO;

class ProgramFile
{
    static void MainMethod(string[] args)
    {
        // Path to the folder containing .cs files
        string folderPath = @"Path\To\Your\Folder";

        // Check if the folder exists
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return;
        }

        // Get all .cs files in the folder (and subfolders)
        var csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        // Process each .cs file
        foreach (var file in csFiles)
        {
            Console.WriteLine($"Reading file: {file}");

            // Read the content of the .cs file
            string content = File.ReadAllText(file);

            // Output or process the content
            Console.WriteLine($"Content of {Path.GetFileName(file)}:");
            Console.WriteLine(content);
            Console.WriteLine(new string('-', 80)); // Separator for readability
        }
    }
}
