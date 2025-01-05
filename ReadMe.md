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

### 1. Clone the Repository

```bash
git clone https://github.com/<YourUsername>/<RepositoryName>.git
cd <RepositoryName>
