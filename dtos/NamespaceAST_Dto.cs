class NameSpaceAST_Dto{
    public NameSpaceAST_Dto()
    {
        Classes = new();
    }
    public string Name { get; set; } = string.Empty;
    public List<ClassAST_Dto> Classes { get; set; }
}