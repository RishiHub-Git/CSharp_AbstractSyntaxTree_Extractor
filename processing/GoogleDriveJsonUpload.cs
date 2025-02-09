using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

class GoogleDriveJsonUpload
    {
        // Define the scopes required. 'DriveFile' allows read/write access to the files created or opened by this app.
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string ApplicationName = "RAST-AI";

        public async ValueTask UploadGoogleDriveJsonFile(string filePath)
        {
            // Authenticate the user using OAuth2.
            UserCredential credential;
            using (var stream = new FileStream("creds/credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The token will be stored in 'token.json' after the first run.
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            // Create the Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Specify the path to the JSON file you want to upload.
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File '{filePath}' not found.");
                return;
            }

            // Specify the folder ID where you want to upload the file.
            // Replace "YOUR_FOLDER_ID" with the actual folder ID from your Google Drive.
            string folderId = "1earP85lKiG-vXfqvDlYLbGZSgW6hY935";

            // Create file metadata.
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                MimeType = "application/json",
                Parents = new List<string> { folderId }
            };

            // Upload the file.
            FilesResource.CreateMediaUpload request;
            try{
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    request = service.Files.Create(fileMetadata, fileStream, "application/json");
                    request.Fields = "id, parents";
                    var progress = await request.UploadAsync();

                    if (progress.Status == Google.Apis.Upload.UploadStatus.Completed)
                    {
                        var file = request.ResponseBody;
                        Console.WriteLine("File uploaded successfully!");
                        Console.WriteLine("File ID: " + file.Id);
                        Console.WriteLine("Parent Folder(s): " + string.Join(", ", file.Parents));
                    }
                    else
                    {
                        Console.WriteLine($"File upload failed with status: {progress.Status}");
                        if (progress.Exception != null)
                        {
                            Console.WriteLine("Error details: " + progress.Exception.Message);
                        }
                    }
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            
        }
    }