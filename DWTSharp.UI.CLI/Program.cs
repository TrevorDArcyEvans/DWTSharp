namespace DWTSharp.UI.CLI;

using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using static ImageDecomposition;

internal static class Program
{
  public static async Task Main(string[] args)
  {
    var result = await Parser.Default.ParseArguments<Options>(args)
      .WithParsedAsync(Run);
    await result.WithNotParsedAsync(HandleParseError);
  }

  private static async Task Run(Options opt)
  {
    var input = await Image.LoadAsync<Rgba32>(opt.ImageFilePath);
    var data = GetGrayscaleDate(input);
    DWTHaar.FWT(data, 1);
    var output = new Image<Rgba32>(data.GetLength(0), data.GetLength(1));
    output.ProcessPixelRows(accessor =>
    {
      for (var y = 0; y < accessor.Height; y++)
      {
        var pixelRow = accessor.GetRowSpan(y);
        for (var x = 0; x < pixelRow.Length; x++)
        {
          ref var pixel = ref pixelRow[x];
          pixel.R = pixel.G = pixel.B = (byte)(255 * data[x, y]);
        }
      }
    });
    
    var outputImageFilePath = Path.GetFileNameWithoutExtension(opt.ImageFilePath) + "_out" + Path.GetExtension(opt.ImageFilePath);
    await output.SaveAsync(outputImageFilePath);
  }

  private static Task HandleParseError(IEnumerable<Error> errs)
  {
    if (errs.IsVersion())
    {
      Console.WriteLine("Version Request");
      return Task.CompletedTask;
    }

    if (errs.IsHelp())
    {
      Console.WriteLine("Help Request");
      return Task.CompletedTask;
      ;
    }

    Console.WriteLine("Parser Fail");
    return Task.CompletedTask;
  }
}
