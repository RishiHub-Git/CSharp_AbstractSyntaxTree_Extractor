class PropertiesAST_Dto{
    public string Name { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string Access_Modifier { get; set; } = string.Empty;
    public bool HasGet { get; set; }
    public bool HasSet { get; set; }
}