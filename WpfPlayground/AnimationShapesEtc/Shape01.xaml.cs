using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPlayground.AnimationShapesEtc
{
    /// <summary>
    /// Interaction logic for Shape01.xaml
    /// </summary>
    public partial class Shape01 : Window
    {
        ShapeStore s;

        public Shape01()
        {
            InitializeComponent();
            s = new ShapeStore(this);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            double xval = double.Parse(textBoxX.Text);
            double yval = double.Parse(textBoxY.Text);
            s.AddPoint(new Point(xval, yval));
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            s.AddPoint(e.GetPosition(this));
        }
    }

    public class ShapeStore
    {
        Shape01 W;

        public ShapeStore(Shape01 w)
        {
            W = w;
            DrawLine();
        }

        PointCollection pc = new PointCollection();
        private void DrawLine()
        {
            Polyline myline = new Polyline();
            myline.Stroke = new SolidColorBrush(Colors.Black);


            pc.Add(new Point(10, 10));
            pc.Add(new Point(50, 10));
            pc.Add(new Point(50, 50));
            myline.Points = pc;
            W.sp.Children.Add(myline);
        }

        public void AddPoint(Point p)
        {
            pc.Add(p);
        }
    }
}
