namespace DWTSharp;

using CommandLine;

internal sealed class Options
{
  [Value(index: 0, Required = true, HelpText = "Path to image file 1")]
  public string ImageFile1Path { get; set; }

  [Value(index: 1, Required = true, HelpText = "Path to image file 2")]
  public string ImageFile2Path { get; set; }

  [Value(index: 2, Required = false, Default = 256, HelpText = "Hash window size")]
  public int HashSize { get; set; }
}
