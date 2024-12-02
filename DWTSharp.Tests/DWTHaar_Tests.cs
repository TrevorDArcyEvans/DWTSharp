namespace DWTSharp.Tests;

using FluentAssertions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public sealed class DWTHaar_Tests
{
  [Test]
  public void FWT_IWT_succeeds()
  {
    var img = Image.Load<Rgba32>("images/AlysonHannigan_0.jpg");
    var data = ImageDecomposition.GetGrayscaleDate(img);
    var copy = (double[,])data.Clone();

    DWTHaar.FWT(data, 1);
    DWTHaar.IWT(data, 1);

    data.Should().BeEquivalentTo(copy, options => options
      .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.000001d))
      .WhenTypeIs<double>());
  }
}
