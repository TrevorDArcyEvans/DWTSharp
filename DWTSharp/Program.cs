namespace DWTSharp;

using System.Numerics;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

internal static class Program
{
  private static double Average<T>(T[,] data) where T : INumber<T>
  {
    var sum = 0d;
    for (var i = 0; i < data.GetLength(0); i++)
    {
      for (var j = 0; j < data.GetLength(1); j++)
      {
        sum += double.CreateChecked(data[i, j]);
      }
    }

    return sum / (data.GetLength(0) * data.GetLength(1));
  }

  private static double Distance<T>(T[,] data1, T[,] data2) where T : INumber<T>
  {
    var avg1 = Average(data1);
    var avg2 = Average(data2);

    var sumDistSq = 0d;
    for (var i = 0; i < data1.GetLength(0); i++)
    {
      for (var j = 0; j < data1.GetLength(1); j++)
      {
        sumDistSq += Math.Pow(double.CreateChecked(data1[i, j]) - avg1 / avg2 * double.CreateChecked(data2[i, j]), 2);
      }
    }

    return Math.Sqrt(sumDistSq / (data1.GetLength(0) * data1.GetLength(1)));
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
    }

    Console.WriteLine("Parser Fail");
    return Task.CompletedTask;
  }
}
