using ArrowSample;
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

namespace ArrowSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Arrow SelectedArrow;
        Arrow gosh = new Arrow() { Stroke = Brushes.Aquamarine };

        //For testing
        Arrow arrow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectedArrow == null) return;

            Mouse.OverrideCursor = Cursors.Arrow;

            Point p = Mouse.GetPosition(container);
            MainWindow.SelectedArrow.TranslateSelectedPoint(p);

            container.Children.Remove(gosh);
            SelectedArrow = null;
        }
        private void container_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedArrow == null || Mouse.LeftButton != MouseButtonState.Pressed) return;

            gosh.SetFrom(new Point(SelectedArrow.From_X, SelectedArrow.From_Y));
            gosh.SetTo(new Point(SelectedArrow.To_X, SelectedArrow.To_Y));
            gosh.Container = arrow.Container;

            Point p = Mouse.GetPosition(container);
            gosh.SetSelectedPoint(SelectedArrow.SelectedPoint);
            gosh.TranslateSelectedPoint(p);

            if (!container.Children.Contains(gosh)) container.Children.Add(gosh);
        }

        //For testing ===================================================================
        private void connect_bt_Click(object sender, RoutedEventArgs e)
        {
            var f = rect1.TranslatePoint(new Point(rect1.Width, rect1.Height), container);
            var t = rect2.TranslatePoint(new Point(0, 0), container);

            container.Children.Add(arrow = new Arrow(f, t, 20, 25, false) { Container=container});
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (arrow == null) throw new InvalidOperationException("You must create the arrow first");

                arrow.From_X = double.Parse(x1_tb.Text);
                arrow.From_Y = double.Parse(y1_tb.Text);
                arrow.To_X = double.Parse(x2_tb.Text);
                arrow.To_Y = double.Parse(y2_tb.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       
    }
}