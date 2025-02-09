class MethodAST_Dto{
    public string XMLComment { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Access_Modifier { get; set; } = "private";
    public bool Is_Method_Static { get; set; } = false;
    public string Parameters { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}