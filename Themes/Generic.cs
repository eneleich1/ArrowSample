using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static ArrowSample.Arrow;

namespace ArrowSample.Themes
{
    public partial class Generic:ResourceDictionary
    {

        private void MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Cross;
        }

        private void MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        public void from_Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;

            var arrow = (Arrow)((Rectangle)sender).Tag;
            MainWindow.SelectedArrow = arrow;
            arrow.SetSelectedPoint(ArrowPointsSelected.From);

        }

        public void to_Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;

            var arrow = (Arrow)((Rectangle)sender).Tag;
            MainWindow.SelectedArrow = arrow;
            arrow.SetSelectedPoint(ArrowPointsSelected.To);

        }
    }
}
