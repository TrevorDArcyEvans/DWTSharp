namespace DWTSharp;

/// <summary>
/// https://www.codeproject.com/Articles/683663/Discrete-Haar-Wavelet-Transformation
/// </summary>
public static class DWTHaar
{
  private const double w0 = 0.5;
  private const double w1 = -0.5;
  private const double s0 = 0.5;
  private const double s1 = 0.5;

  /// <summary>
  ///   Discrete Haar Wavelet Transform
  /// </summary>
  /// 
  public static void FWT(double[] data)
  {
    var temp = new double[data.Length];

    var h = data.Length >> 1;
    for (var i = 0; i < h; i++)
    {
      var k = (i << 1);
      temp[i] = data[k] * s0 + data[k + 1] * s1;
      temp[i + h] = data[k] * w0 + data[k + 1] * w1;
    }

    for (var i = 0; i < data.Length; i++)
    {
      data[i] = temp[i];
    }
  }

  /// <summary>
  ///   Discrete Haar Wavelet 2D Transform
  /// </summary>
  public static void FWT(double[,] data, int iterations)
  {
    var rows = data.GetLength(0);
    var cols = data.GetLength(1);

    for (var k = 0; k < iterations; k++)
    {
      var lev = 1 << k;

      var levCols = cols / lev;
      var levRows = rows / lev;

      var row = new double[levCols];
      for (var i = 0; i < levRows; i++)
      {
        for (var j = 0; j < row.Length; j++)
        {
          row[j] = data[i, j];
        }

        FWT(row);

        for (var j = 0; j < row.Length; j++)
        {
          data[i, j] = row[j];
        }
      }


      var col = new double[levRows];
      for (var j = 0; j < levCols; j++)
      {
        for (var i = 0; i < col.Length; i++)
        {
          col[i] = data[i, j];
        }

        FWT(col);

        for (var i = 0; i < col.Length; i++)
        {
          data[i, j] = col[i];
        }
      }
    }
  }

  /// <summary>
  ///   Inverse Haar Wavelet Transform
  /// </summary>
  public static void IWT(double[] data)
  {
    var temp = new double[data.Length];

    var h = data.Length >> 1;
    for (var i = 0; i < h; i++)
    {
      var k = (i << 1);
      temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
      temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
    }

    for (var i = 0; i < data.Length; i++)
    {
      data[i] = temp[i];
    }
  }

  /// <summary>
  ///   Inverse Haar Wavelet 2D Transform
  /// </summary>
  public static void IWT(double[,] data, int iterations)
  {
    var rows = data.GetLength(0);
    var cols = data.GetLength(1);

    for (var k = iterations - 1; k >= 0; k--)
    {
      var lev = 1 << k;

      var levCols = cols / lev;
      var levRows = rows / lev;

      var col = new double[levRows];
      for (var j = 0; j < levCols; j++)
      {
        for (var i = 0; i < col.Length; i++)
        {
          col[i] = data[i, j];
        }

        IWT(col);

        for (var i = 0; i < col.Length; i++)
        {
          data[i, j] = col[i];
        }
      }

      var row = new double[levCols];
      for (var i = 0; i < levRows; i++)
      {
        for (var j = 0; j < row.Length; j++)
        {
          row[j] = data[i, j];
        }

        IWT(row);

        for (var j = 0; j < row.Length; j++)
        {
          data[i, j] = row[j];
        }
      }
    }
  }
}
