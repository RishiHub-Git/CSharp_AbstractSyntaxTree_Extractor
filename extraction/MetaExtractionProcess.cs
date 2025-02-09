using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

class MetaExtractionProcess{

    /// <summary>
    /// Extract all the code file metadata like Namespace, Class, Method, Properties
    /// </summary>
    /// <param name="rootNode"></param>
    /// <returns></returns>
    public static CodeAST_Dto ExtractClassesAndMethods(SyntaxNode rootNode)
    {
        List<NameSpaceAST_Dto> nameSpaceAST_Dtos = new();

        // Extract regions
        //ExtractRegions(rootNode);

        var namespaceDeclarations = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>();
        if(namespaceDeclarations.Count()==0)
        {
            nameSpaceAST_Dtos.Add(new NameSpaceAST_Dto{
                Name = "Default NameSpace",
                Classes = ExtractClassMeta(rootNode)
            });
        }   
        else{
            
            NameSpaceAST_Dto nameSpaceAST_Dto;

            foreach (var namespaceDeclaration in namespaceDeclarations)
            {
                nameSpaceAST_Dto = new(){
                    Name = namespaceDeclaration.Name.ToString(),
                    Classes = ExtractClassMeta(rootNode)
                };

                nameSpaceAST_Dtos.Add(nameSpaceAST_Dto);
            }
        }

        return new CodeAST_Dto{Namespaces = nameSpaceAST_Dtos};
    }
    
    static List<ClassAST_Dto> ExtractClassMeta(SyntaxNode rootNode){
        List<ClassAST_Dto> classAST_Dtos = new();
        ClassAST_Dto classAST_Dto;

        var classDeclarations = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

        foreach (var classDeclaration in classDeclarations)
        {
            classAST_Dto = new();

            // Extract class name
            classAST_Dto = ExtractClassMeta(classDeclaration);
            
            // Extract methods within the class
            var methods = classDeclaration.Members.OfType<MethodDeclarationSyntax>();
            classAST_Dto.Methods = ExtractMethodMeta(methods);
            
            classAST_Dtos.Add(classAST_Dto);
        }

        return classAST_Dtos;
    }

    static ClassAST_Dto ExtractClassMeta(ClassDeclarationSyntax classDeclaration){
        ClassAST_Dto classAST_Dto = new(){
            Name = classDeclaration.Identifier.Text,
            Access_Modifier = GetClassAccessModifier(classDeclaration),
            Is_Class_Static = IsStatic(classDeclaration),
            XMLComment = ExtractXmlComments(classDeclaration)?? string.Empty,
            Comment = ExtractGeneralComments(classDeclaration)?? string.Empty,
            Constructors = ExtractConstructorMeta(classDeclaration),
            Fields = ExtractFieldsMeta(classDeclaration),
            Properties = ExtractPropertiesMeta(classDeclaration)
        };
        return classAST_Dto;
    }

    static List<VariablesAST_Dto> ExtractFieldsMeta(ClassDeclarationSyntax classDeclaration){
        List<VariablesAST_Dto> lst_Fields = new();
        VariablesAST_Dto item;

        // Extract class-level variables
        var fields = classDeclaration.Members.OfType<FieldDeclarationSyntax>();
        foreach (var field in fields)
        {
            item = new(){
                Name = string.Join(", ", field.Declaration.Variables.Select(v => v.Identifier.Text)),
                FieldType = field.Declaration.Type.ToString(),
                Access_Modifier = GetFieldAccessModifier(field),
                IsReadonly = field.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
            };
            lst_Fields.Add(item);
        }
        return lst_Fields;
    }

    static List<PropertiesAST_Dto> ExtractPropertiesMeta(ClassDeclarationSyntax classDeclaration){
        List<PropertiesAST_Dto> lst_Properties = new();
        PropertiesAST_Dto item;
        // Extract properties
        var properties = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();
        foreach (var property in properties)
        {
            item = new(){
                Name = property.Identifier.Text,
                PropertyType = property.Type.ToString(),
                Access_Modifier = GetPropertyAccessModifier(property),
                HasGet = HasAccessor(property, SyntaxKind.GetAccessorDeclaration),
                HasSet = HasAccessor(property, SyntaxKind.SetAccessorDeclaration)
            };
            lst_Properties.Add(item);
        }
        return lst_Properties;
    }

    static bool HasAccessor(PropertyDeclarationSyntax property, SyntaxKind accessorKind)
    {
        // Check if the property has the specified accessor
        return property.AccessorList?.Accessors.Any(a => a.Kind() == accessorKind) ?? false;
    }
    static List<ConstructorAST_Dto> ExtractConstructorMeta(ClassDeclarationSyntax classDeclaration){
        List<ConstructorAST_Dto> lst_Constructors = new();
        ConstructorAST_Dto item;
        // Extract constructors
        var constructors = classDeclaration.Members.OfType<ConstructorDeclarationSyntax>();
        foreach (var constructor in constructors)
        {
            item = new(){
                Name = constructor.Identifier.Text,
                Access_Modifier = GetConstructorAccessModifier(constructor),
                Body = constructor.Body?.ToString() ?? "No Body",
                Parameters = string.Join(", ", constructor.ParameterList.Parameters.Select(p => $"{p.Type} {p.Identifier.Text}"))
            };
            lst_Constructors.Add(item);
        }
        return lst_Constructors;
    }

    static List<MethodAST_Dto> ExtractMethodMeta(IEnumerable<MethodDeclarationSyntax> methods){
        List<MethodAST_Dto> methodAST_Dtos = new(); 
        MethodAST_Dto methodAST_Dto;

        foreach (var method in methods)
        {
            // Extract method name and signature
            methodAST_Dto = new(){
                Name = method.Identifier.Text,
                Access_Modifier = GetMethodAccessModifier(method),
                Is_Method_Static = IsStatic(method),
                Parameters = string.Join(", ", method.ParameterList.Parameters),
                Body = method.Body?.ToString() ?? "No Body",
                ReturnType = method.ReturnType.ToString(),
                XMLComment = ExtractXmlComments(method)?? string.Empty,
                Comment = ExtractGeneralComments(method)?? string.Empty
            };
            
            methodAST_Dtos.Add(methodAST_Dto);
        }
        return methodAST_Dtos;
    }

    static bool IsStatic(SyntaxNode node)
    {
        // Check if the node contains the 'static' modifier
        return node is BaseTypeDeclarationSyntax baseType && baseType.Modifiers.Any(SyntaxKind.StaticKeyword) ||
               node is MethodDeclarationSyntax method && method.Modifiers.Any(SyntaxKind.StaticKeyword);
    }

    static string GetMethodAccessModifier(MethodDeclarationSyntax method)
    {
        // Check the method's modifiers
        var modifiers = method.Modifiers.Select(m => m.Text);
        
        // Default to 'private' if no access modifier is specified
        if (modifiers.Contains("public")) return "public";
        if (modifiers.Contains("protected")) return "protected";
        if (modifiers.Contains("internal")) return "internal";
        if (modifiers.Contains("private")) return "private";
        
        // Default to private if no explicit access modifier is specified
        return "private";
    }

    static string GetClassAccessModifier(ClassDeclarationSyntax classSynctax)
    {
        // Check the classSynctax's modifiers
        var modifiers = classSynctax.Modifiers.Select(m => m.Text);
        
        // Default to 'private' if no access modifier is specified
        if (modifiers.Contains("public")) return "public";
        if (modifiers.Contains("protected")) return "protected";
        if (modifiers.Contains("internal")) return "internal";
        if (modifiers.Contains("private")) return "private";
        
        // Default to private if no explicit access modifier is specified
        return "private";
    }

    static string GetConstructorAccessModifier(ConstructorDeclarationSyntax classSynctax)
    {
        // Check the classSynctax's modifiers
        var modifiers = classSynctax.Modifiers.Select(m => m.Text);
        
        // Default to 'private' if no access modifier is specified
        if (modifiers.Contains("public")) return "public";
        if (modifiers.Contains("protected")) return "protected";
        if (modifiers.Contains("internal")) return "internal";
        if (modifiers.Contains("private")) return "private";
        
        // Default to private if no explicit access modifier is specified
        return "private";
    }

    static string GetFieldAccessModifier(FieldDeclarationSyntax fieldSyntax)
    {
        // Check the method's modifiers
        var modifiers = fieldSyntax.Modifiers.Select(m => m.Text);
        
        // Default to 'private' if no access modifier is specified
        if (modifiers.Contains("public")) return "public";
        if (modifiers.Contains("protected")) return "protected";
        if (modifiers.Contains("internal")) return "internal";
        if (modifiers.Contains("private")) return "private";
        
        // Default to private if no explicit access modifier is specified
        return "private";
    }

    static string GetPropertyAccessModifier(PropertyDeclarationSyntax propertySyntax)
    {
        // Check the method's modifiers
        var modifiers = propertySyntax.Modifiers.Select(m => m.Text);
        
        // Default to 'private' if no access modifier is specified
        if (modifiers.Contains("public")) return "public";
        if (modifiers.Contains("protected")) return "protected";
        if (modifiers.Contains("internal")) return "internal";
        if (modifiers.Contains("private")) return "private";
        
        // Default to private if no explicit access modifier is specified
        return "private";
    }


    static void ExtractRegions(SyntaxNode rootNode)
    {
        var regions = rootNode.DescendantTrivia()
            .Where(trivia => trivia.IsKind(SyntaxKind.RegionDirectiveTrivia) || trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia));

        string? currentRegion = null;
        foreach (var trivia in regions)
        {
            if (trivia.IsKind(SyntaxKind.RegionDirectiveTrivia))
            {
                currentRegion = trivia.ToFullString().Trim();
                Console.WriteLine($"  Region Start: {currentRegion}");
            }
            else if (trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia) && currentRegion != null)
            {
                Console.WriteLine($"  Region End: {trivia.ToFullString().Trim()}");
                currentRegion = null;
            }
        }
    }

    static string ExtractXmlComments(SyntaxNode node)
    {
        // Find XML documentation comments associated with the node
        var trivia = node.GetLeadingTrivia()
            .Where(tr => tr.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        return string.Join(Environment.NewLine, trivia.Select(tr => tr.ToFullString().Trim()));
    }

    static string ExtractGeneralComments(SyntaxNode node)
    {
        // Find general comments associated with the node
        var trivia = node.GetLeadingTrivia()
            .Where(tr => tr.IsKind(SyntaxKind.SingleLineCommentTrivia) || tr.IsKind(SyntaxKind.MultiLineCommentTrivia));
        return string.Join(Environment.NewLine, trivia.Select(tr => tr.ToFullString().Trim()));
    }
}