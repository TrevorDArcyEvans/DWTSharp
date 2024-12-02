namespace DWTSharp.UI.CLI;

using CommandLine;

internal sealed class Options
{
  [Value(index: 0, Required = true, HelpText = "Path to image file")]
  public string ImageFilePath { get; set; }
}
