
DocumentProcessor documentProcessor = new(@"D:\Trainings\Codes");

Console.WriteLine("Started Processing......");
foreach (var filePath in documentProcessor.FetchFiles())
{
    Console.WriteLine($"Processing File {filePath}");
    
    ProcessFile.ProcessCodeFile(
        documentProcessor.GetFileContent(filePath), 
        documentProcessor.GetFileName(filePath));
} ;
Console.WriteLine("Processing Completed");


