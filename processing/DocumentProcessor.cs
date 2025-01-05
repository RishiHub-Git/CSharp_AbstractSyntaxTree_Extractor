class DocumentProcessor{
    private readonly string folderPath = "Codes";

    public DocumentProcessor(string _folderPath)
    {
        folderPath = _folderPath;
    }
    public string[] FetchFiles(){
        // Check if the folder exists
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("The specified folder does not exist.");
            return Array.Empty<string>();
        }

        // Get all .cs files in the folder (and subfolders)
        return Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
    }

    public string GetFileContent(string filePath){
        return File.ReadAllText(filePath);
    }

    public string GetFileName(string filePath){
        return Path.GetFileName(filePath).Split(".")[0];
    }

    public static bool SaveJsonFile(string content, string fileName){
        try
        {
            File.WriteAllText($"Data/{fileName}.json", content);
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }
}