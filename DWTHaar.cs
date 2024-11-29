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
    double[] temp = new double[data.Length];

    int h = data.Length >> 1;
    for (int i = 0; i < h; i++)
    {
      int k = (i << 1);
      temp[i] = data[k] * s0 + data[k + 1] * s1;
      temp[i + h] = data[k] * w0 + data[k + 1] * w1;
    }

    for (int i = 0; i < data.Length; i++)
      data[i] = temp[i];
  }

  /// <summary>
  ///   Discrete Haar Wavelet 2D Transform
  /// </summary>
  public static void FWT(double[,] data, int iterations)
  {
    int rows = data.GetLength(0);
    int cols = data.GetLength(1);

    double[] row;
    double[] col;

    for (int k = 0; k < iterations; k++)
    {
      int lev = 1 << k;

      int levCols = cols / lev;
      int levRows = rows / lev;

      row = new double[levCols];
      for (int i = 0; i < levRows; i++)
      {
        for (int j = 0; j < row.Length; j++)
          row[j] = data[i, j];

        FWT(row);

        for (int j = 0; j < row.Length; j++)
          data[i, j] = row[j];
      }


      col = new double[levRows];
      for (int j = 0; j < levCols; j++)
      {
        for (int i = 0; i < col.Length; i++)
          col[i] = data[i, j];

        FWT(col);

        for (int i = 0; i < col.Length; i++)
          data[i, j] = col[i];
      }
    }
  }

  /// <summary>
  ///   Inverse Haar Wavelet Transform
  /// </summary>
  public static void IWT(double[] data)
  {
    double[] temp = new double[data.Length];

    int h = data.Length >> 1;
    for (int i = 0; i < h; i++)
    {
      int k = (i << 1);
      temp[k] = (data[i] * s0 + data[i + h] * w0) / w0;
      temp[k + 1] = (data[i] * s1 + data[i + h] * w1) / s0;
    }

    for (int i = 0; i < data.Length; i++)
      data[i] = temp[i];
  }

  /// <summary>
  ///   Inverse Haar Wavelet 2D Transform
  /// </summary>
  public static void IWT(double[,] data, int iterations)
  {
    int rows = data.GetLength(0);
    int cols = data.GetLength(1);

    double[] col;
    double[] row;

    for (int k = iterations - 1; k >= 0; k--)
    {
      int lev = 1 << k;

      int levCols = cols / lev;
      int levRows = rows / lev;

      col = new double[levRows];
      for (int j = 0; j < levCols; j++)
      {
        for (int i = 0; i < col.Length; i++)
          col[i] = data[i, j];

        IWT(col);

        for (int i = 0; i < col.Length; i++)
          data[i, j] = col[i];
      }

      row = new double[levCols];
      for (int i = 0; i < levRows; i++)
      {
        for (int j = 0; j < row.Length; j++)
          row[j] = data[i, j];

        IWT(row);

        for (int j = 0; j < row.Length; j++)
          data[i, j] = row[j];
      }
    }
  }
}
