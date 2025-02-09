
GoogleDriveJsonUpload googleDriveJsonUpload = new();
DocumentProcessor documentProcessor = new(@"D:\Trainings\Codes");

Console.WriteLine("Started Processing......");
foreach (var filePath in documentProcessor.FetchFiles())
{
    Console.WriteLine($"Processing File {filePath}");
    var fileName = documentProcessor.GetFileName(filePath);
    ProcessFile.ProcessCodeFile(
        documentProcessor.GetFileContent(filePath), 
        fileName);
    
    await googleDriveJsonUpload.UploadGoogleDriveJsonFile(String.Format("data/{0}.json", fileName));
} ;
Console.WriteLine("Processing Completed");
