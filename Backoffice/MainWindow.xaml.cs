using GameCore;
using GameCore.Creator;
using GameCore.Map;
using GameCore.Mechanics;
using GameCore.Mechanics.Seeder;
using GameCore.Services;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace Backoffice;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    [StructLayout(LayoutKind.Sequential)]
    public struct PixelColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }

    private PixelColor[,] GetPixels(BitmapSource source)
    {
        if (source.Format != PixelFormats.Bgra32)
            source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
        
        int width = source.PixelWidth;
        int height = source.PixelHeight;
        PixelColor[,] result = new PixelColor[width, height];

        source.CopyPixels2(result, width * 4, 0);
        return result;
    }

    private TanimsizRenkHandler tanimsizRenkHandler;
    public MainWindow()
    {
        InitializeComponent();
        tanimsizRenkHandler = new TanimsizRenkHandler(OnTanimsizRenk);

        GameServiceStatic.GameService.HaritaCreator.OnTanimsizRenk += tanimsizRenkHandler;

        var m = hedef.Margin;

        m.Left = image.Margin.Left - hedef.Width / 2;
        m.Top = image.Margin.Top - hedef.Height / 2;

        hedef.Margin = m;
        //RefreshCommunities();

        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1);
        timer.Tick += timer_Tick;
        timer.Start();

    }


    void timer_Tick(object? sender, EventArgs e)
    {
        RefreshInfo();
    }


    private void button_Click(object sender, RoutedEventArgs e)
    {
        image.Source = ToBitmapImage(GameServiceStatic.GameService.HaritaCreator.ZeminHarita);
    }

    public static BitmapImage ToBitmapImage(Bitmap bitmap)
    {
        using (var memory = new MemoryStream())
        {
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }

    private void btnDagTepe_Click(object sender, RoutedEventArgs e)
    {
        image.Source = ToBitmapImage(GameServiceStatic.GameService.HaritaCreator.DagHarita);
    }

    private void btnOrman_Click(object sender, RoutedEventArgs e)
    {
        image.Source = ToBitmapImage(GameServiceStatic.GameService.HaritaCreator.OrmanHarita);
    }

    private void btnNehir_Click(object sender, RoutedEventArgs e)
    {
        image.Source = ToBitmapImage(GameServiceStatic.GameService.HaritaCreator.NehirHarita);
    }

    private async void button_Click_1(object sender, RoutedEventArgs e)
    {
        await Task.Run(() => RefreshCommunities());
    }

    private void OnTanimsizRenk(int x, int y, MapCellDecoder hucre)
    {
        textBlock.Text = $"OnTanimsizRenk {x}, {y} {hucre.SeedFromBitmapUndefinedInfo}";
        var renk = hucre.SeedFromBitmapUndefinedColor;
        renkGosterge.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(renk.A, renk.R, renk.G, renk.B));
        Debug.Print($"OnTanimsizRenk {x}, {y} {hucre.SeedFromBitmapUndefinedInfo}");

        var m = hedef.Margin;

        
        m.Left = scrollView.Margin.Left + image.Margin.Left + x - hedef.Width/2;
        m.Top = scrollView.Margin.Top + image.Margin.Top + y - hedef.Height / 2;

        hedef.Margin = m;
    }

    private void image_MouseMove(object sender, MouseEventArgs e)
    {
        RefreshInfoOnMouse(e);
    }

    private string? hucreInfo;

    private void RefreshInfo()
    {
        textBlock.Text = $"{TimeService.ToTimeString()} {hucreInfo} ";
    }
    private void RefreshInfoOnMouse(MouseEventArgs e)
    {
        try
        {
            var pos = e.GetPosition(image);

            if (GameServiceStatic.GameService.Harita.Hucreler != null)
            {
                var hucre = GameServiceStatic.GameService.Harita.Hucreler[(int)pos.X, (int)pos.Y];
                if (hucre != null)
                    hucreInfo = $"Hucre ({hucre.X},{hucre.Y}) {hucre.ToString()}";

            }

            var imgSrc = (BitmapSource)image.Source;
            var pixels = GetPixels(imgSrc);

            var renk = pixels[(int)pos.X, (int)pos.Y];
            lblKoordinat.Content = $"{(int)pos.X}, {(int)pos.Y} \n R: {renk.Red} B:{renk.Blue} G:{renk.Green} A:{renk.Alpha} \n{((int)renk.Alpha).ToHex()}{((int)renk.Red).ToHex()}{((int)renk.Blue).ToHex()}{((int)renk.Green).ToHex()}";

            cursorRenk.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(renk.Alpha, renk.Red, renk.Green, renk.Blue));
            RefreshInfo();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void RefreshInfo(MouseEventArgs e)
    {
        var pos = e.GetPosition(image);

        if (GameServiceStatic.GameService.Harita.Hucreler != null)
        {
            var hucre = GameServiceStatic.GameService.Harita.Hucreler[(int)pos.X, (int)pos.Y];
            if (hucre != null)
                textBlock.Text = $"{TimeService.ToTimeString()} Hucre ({hucre.X},{hucre.Y}) {hucre.ToString()}";
        }
    }

    Bitmap GetBitmap(BitmapSource source)
    {
        Bitmap bmp = new Bitmap(
          source.PixelWidth,
          source.PixelHeight,
          //System.Windows.Media.
          PixelFormat.Format32bppPArgb);
        BitmapData data = bmp.LockBits(
          new System.Drawing.Rectangle(Point.Empty, bmp.Size),
          ImageLockMode.WriteOnly,
          PixelFormat.Format32bppPArgb);
        source.CopyPixels(
          Int32Rect.Empty,
          data.Scan0,
          data.Height * data.Stride,
          data.Stride);
        bmp.UnlockBits(data);
        return bmp;
    }

    private void ButtonClickSeedCommunities(object sender, RoutedEventArgs e)
    {
        GameSeeder.SeedGame();

        RefreshCommunities();
    }

    private void RefreshCommunities()
    {
        const int communityWidth = 6;
        var red = System.Windows.Media.Color.FromRgb(200, 0, 0);
        GameFieldCanvas.Children.Clear();
        GameFieldCanvas.Children.Add(image);
        GameFieldCanvas.Width = GameServiceStatic.GameService.Harita.MaxX;
        GameFieldCanvas.Height= GameServiceStatic.GameService.Harita.MaxY;

        SolidColorBrush newColor = new SolidColorBrush(red);

        if (GameServiceStatic.GameService.GameContext.Units == null)
            return;

        foreach (var u in GameServiceStatic.GameService.GameContext.Units)
        {
            var ellipse = new Ellipse() { Width = communityWidth, Height = communityWidth };

            ellipse.StrokeThickness = 2;
            ellipse.Stroke = newColor;

            GameFieldCanvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, u.X - communityWidth/2);
            Canvas.SetTop(ellipse, u.Y - communityWidth/2);
        }
    }

    private void ButtonClickStartGame(object sender, RoutedEventArgs e)
    {
        if(TimeService.IsGamePaused)
            TimeService.ContinueGame();
        else
            TimeService.PauseGame();

    }



    //public static System.Drawing.Bitmap BitmapSourceToBitmap2(BitmapSource srs)
    //{
    //    int width = srs.PixelWidth;
    //    int height = srs.PixelHeight;
    //    int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
    //    IntPtr ptr = IntPtr.Zero;
    //    try
    //    {
    //        ptr = Marshal.AllocHGlobal(height * stride);
    //        srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
    //        using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
    //        {
    //            // Clone the bitmap so that we can dispose it and
    //            // release the unmanaged memory at ptr
    //            return new System.Drawing.Bitmap(btm);
    //        }
    //    }
    //    finally
    //    {
    //        if (ptr != IntPtr.Zero)
    //            Marshal.FreeHGlobal(ptr);
    //    }
    //}
}
