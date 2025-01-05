#region Utilities

#region Utilities2

#endregion
namespace MyApp.UtilitiesFolder;
/// <summary>
/// This is a utility class.
/// </summary>
public class UtilityClass
{
    private readonly string HappyCoding;
    private string HappyCoding2;
    public int MyProperty { get; set; }
    public int MyProperty2 { get; }
    public UtilityClass()
    {
        HappyCoding = string.Empty;
        HappyCoding2 = string.Empty;
    }

    #region MathOperations
    /// <summary>
    /// Adds two numbers.
    /// </summary>
    public int Add(int x, int y)
    {
        return x + y;
    }
    #endregion

    #region StringOperations
    //This is a general comment.
    /// <summary>
    /// Formats a string.
    /// </summary>
    public string Format(string input)
    {
        //This will return all text to Uppercase
        return input.ToUpper();
    }

    private static void Add(){
        
    }
    #endregion
}

#endregion
