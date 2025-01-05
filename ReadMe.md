# C# Code Abstract Syntax Tree Extractor & Analysis Tool

A .NET Core console application for parsing and analyzing `.cs` files. This tool extracts:
- Namespaces, Classes, Methods, Properties, and Constructors
- Access modifiers (e.g., `public`, `private`, `protected`)
- Static and `readonly` fields
- Property accessors (`get`, `set`)
- Regions and other metadata

## Features

- Extract namespaces and their classes
- Analyze fields, properties, and constructors
- Determine if a method or class is `static`
- Detect `readonly` fields
- Check if a property has `get` and/or `set` accessors
- Parse all `.cs` files from a given directory and subdirectories

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) version 9.0 or later
- Git for version control

## Setup

### Contributing
Feel free to fork the repository and create a pull request. For major changes, please open an issue first to discuss what you would like to change.

### Clone the Repository

```bash
git clone https://github.com/<YourUsername>/<RepositoryName>.git
cd <RepositoryName>

### Build and Run the Application

$ dotnet build
$ dotnet run

# Input Directory
Update the directory path in the Program.cs file to point to the folder containing your .cs files.

# Process Output
The application processes each .cs file and outputs detailed metadata (e.g., namespaces, classes, methods).

Example Usage
Input
Directory structure:

RoslynAbstractSyntaxTree/
├── bin/
├── Codes/
│   ├── FactoryMethodPattern.cs
│   ├── TestProgram.cs
├── Data/
│   ├── FactoryMethodPattern.json
│   ├── TestProgram.json
├── dtos/
│   ├── ClassAST_Dto.cs
│   ├── CodeAST_Dto.cs
│   ├── ConstructorAST_Dto.cs
│   ├── MethodAST_Dto.cs
│   ├── NamespaceAST_Dto.cs
│   ├── PropertiesAST_Dto.cs
│   ├── VariablesAST_Dto.cs
├── extraction/
│   ├── MetaExtractionProcess.cs
├── obj/
├── processing/
│   ├── DocumentProcessor.cs
│   ├── ProcessFile.cs
├── Program.cs
├── ReadMe.md
├── RoslynAbstractSyntaxTree.csproj
├── Projects.sln
.gitignore
