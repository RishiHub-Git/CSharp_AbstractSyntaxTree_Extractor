class CodeAST_Dto{
    public CodeAST_Dto()
    {
        Namespaces = new();
    }
    public List<NameSpaceAST_Dto> Namespaces { get; set; }
}