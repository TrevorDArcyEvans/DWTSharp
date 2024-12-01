namespace DWTSharp.Tests;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

internal static class ImageDecomposition
{
  public static double[,] GetGrayscaleDate(Image<Rgba32> img, int hashSideSize = 256)
  {
    // resize img to 256x256px (by default) or with configured size 
    img.Mutate(ctx => ctx
      .Resize(new Size(hashSideSize, hashSideSize))
      .Grayscale(GrayscaleMode.Bt601));

    var output = new double[hashSideSize, hashSideSize];

    img.ProcessPixelRows(acc =>
    {
      for (var y = 0; y < acc.Height; y++)
      {
        var pxRow = acc.GetRowSpan(y);

        for (var x = pxRow.Length - 1; x >= 0; x--)
        {
          ref var px = ref pxRow[x];
          if (px.R != px.G ||
              px.G != px.B ||
              px.B != px.R)
          {
            throw new ArgumentException("Invalid pixel: should be grayscale");
          }

          output[x, y] = px.R / 255d;
        }
      }
    });

    return output;
  }
}
