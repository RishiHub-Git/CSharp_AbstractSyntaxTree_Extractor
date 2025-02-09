class ClassAST_Dto{
    public ClassAST_Dto()
    {
        Constructors = new();
        Properties = new();
        Fields = new();
        Methods = new();
    }
    public string Name { get; set; } = string.Empty;
    public string Access_Modifier { get; set; } = "private";
    public bool Is_Class_Static { get; set; } = false;
    public string XMLComment { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public List<MethodAST_Dto> Methods { get; set; }
    public List<ConstructorAST_Dto> Constructors { get; set; } 
    public List<VariablesAST_Dto> Fields { get; set; }
    public List<PropertiesAST_Dto> Properties { get; set; } 
}