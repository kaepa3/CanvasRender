using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VInspectionLib;
using HalconDotNet;
using System.IO;
using System.Globalization;

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var image = new VImage("Master.png");
            VRegion region;
            Common.ThresholdMultiChannel(image, out region, 210, 255);
            HOperatorSet.WriteRegion((HObject)region.RegionObj, "region");
            var bi = Common.VImageToImageSource(image);

            // The Visual to use as the source of the RenderTargetBitmap.
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(bi, new Rect(0, 0, bi.Width, bi.Height));

            var xlds = region.ToViewXlds();
            foreach (var item in xlds)
            {
                var reg = CreateGeometryFromXld2(item);
                drawingContext.DrawGeometry(Brushes.AliceBlue, new Pen(Brushes.Red, 2), reg);
            }

            drawingContext.Close();

            // The BitmapSource that is rendered with a Visual.
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bi.Width, (int)bi.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);

            // Encoding the RenderBitmapTarget as a PNG file.
            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            using (Stream stm = File.Create("new.png"))
            {
                png.Save(stm);
            }
        }
        public static StreamGeometry CreateGeometryFromXld2(VXLD xld, bool isFilled = true, bool isClosed = true)
        {
            if (xld.Length <= 1)
                return null;

            StreamGeometry stream = new StreamGeometry();

            // LINQによってPoint配列に変換
            var points = (from p in xld.Columns.Zip(xld.Rows, (r, c) => new Point(r, c))
                          select p).ToArray<Point>();

            //var compressedPointes = DrawingUtility.CompressPoints(points);
            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(points[0], isFilled, isClosed);
                ctx.PolyLineTo(points, true, false);
            }

            return stream;
        }

    }
}
