using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ArrowSample"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ArrowSample;assembly=ArrowSample"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Arrow/>
    ///
    /// </summary>
    public class Arrow : Control
    {
       public enum ArrowPointsSelected { From,To,None};

        public event PropertyChangedEventHandler PropertyChanged;
        private Point f, t;
        public ArrowPointsSelected SelectedPoint { get; protected set; }
        public Canvas Container;

        static Arrow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Arrow), new FrameworkPropertyMetadata(typeof(Arrow)));
        }
        public Arrow()
        {
            Thickness = 2;
            Stroke = Brushes.Black;
            Angle = 20;
            EdgeSize = 25;

            Loaded += Arrow_Loaded;
        }
        public Arrow(Point from, Point to, float angle, float edgeSize, bool closedArrow) : this()
        {
            From_X = from.X;
            From_Y = from.Y;
            To_X = to.X;
            To_Y = to.Y;

            Angle = angle;
            EdgeSize = edgeSize;
            IsClosed = closedArrow;
        }
        public Arrow(Point from, Point to) : this(from, to, 20, 25, false)
        {
        }

        private void Arrow_Loaded(object sender, RoutedEventArgs e)
        {
            var fromRect = (Rectangle)Template.FindName("from_Rect", this);
            fromRect.Tag = this;

            var toRect = (Rectangle)Template.FindName("to_Rect", this);
            toRect.Tag = this;

            ComputeFields();
        }

        protected void OnPropertyChanged()
        {
            ComputeFields();
        }

        #region Properties

        public double From_X
        {
            get { return (double)GetValue(From_XProperty); }
            set{ SetValue(From_XProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty From_XProperty = DependencyProperty.Register(
        "From_X", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double From_Y
        {
            get { return (double)GetValue(From_YProperty); }
            set { SetValue(From_YProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty From_YProperty = DependencyProperty.Register(
        "From_Y", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double To_X
        {
            get { return (double)GetValue(To_XProperty); }
            set { SetValue(To_XProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty To_XProperty = DependencyProperty.Register(
        "To_X", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double To_Y
        {
            get { return (double)GetValue(To_YProperty); }
            set { SetValue(To_YProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty To_YProperty = DependencyProperty.Register(
        "To_Y", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        /// <summary>
        /// Angle between sides and main lines in degrees
        /// </summary>
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
        "Angle", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        /// <summary>
        /// Arrow lateral edge size
        /// </summary>
        public double EdgeSize
        {
            get { return (double)GetValue(EdgeSizeProperty); }
            set { SetValue(EdgeSizeProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty EdgeSizeProperty = DependencyProperty.Register(
        "EdgeSize", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        /// <summary>
        /// Retrieve if the arrow will closed or not
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
        "IsClosed", typeof(bool), typeof(Arrow), new PropertyMetadata(false));

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
       "Thickness", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
        "Stroke", typeof(Brush), typeof(Arrow), new PropertyMetadata(Brushes.Black));

        #endregion

        #region Private Properties

        public double P1_X
        {
            get { return (double)GetValue(P1_XProperty); }
            protected set { SetValue(P1_XProperty, value); }
        }
        public static readonly DependencyProperty P1_XProperty = DependencyProperty.Register(
        "P1_X", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double P1_Y
        {
            get { return (double)GetValue(P1_YProperty); }
            protected set { SetValue(P1_YProperty, value); }
        }
        public static readonly DependencyProperty P1_YProperty = DependencyProperty.Register(
        "P1_Y", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double P2_X
        {
            get { return (double)GetValue(P2_XProperty); }
            protected set { SetValue(P2_XProperty, value); }
        }
        public static readonly DependencyProperty P2_XProperty = DependencyProperty.Register(
        "P2_X", typeof(double), typeof(Arrow), new PropertyMetadata(0d));

        public double P2_Y
        {
            get { return (double)GetValue(P2_YProperty); }
            protected set { SetValue(P2_YProperty, value); }
        }
        public static readonly DependencyProperty P2_YProperty = DependencyProperty.Register(
        "P2_Y", typeof(double), typeof(Arrow), new PropertyMetadata(0d));


        #endregion

        /// <summary>
        /// Set Intersections point of Esphere with rect.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void SetSolution(Point p1,Point p2)
        {
            P1_X = p1.X;
            P1_Y = p1.Y;

            P2_X = p2.X;
            P2_Y = p2.Y;
        }

        public void ComputeFields()
        {
            if (!IsLoaded) return;

            //From
            f = new Point(From_X, From_Y);
            //To
            t = new Point(To_X, To_Y);

            //Edges spec
            var sita = Cal.ToRadians(Angle);
            var rook = EdgeSize;

            //D(From,To)
            var d_ft = Cal.Distance(f, t);

            //D(I,T)
            var d_it = rook * Math.Cos(sita);

            //D(F,I)
            var d_fi = d_ft - d_it;

            //D(I,P1) = D(I,P2)
            var r = rook * Math.Sin(sita);

            //Angle between Axis Y and the rect
            var alpha = Math.Atan((To_X - From_X)/(To_Y-From_Y));

            Point Ic=new Point(), I=new Point();
            Circunference cir = null;

            if(From_Y == To_Y)
            {
                if(From_X==To_X) throw new InvalidOperationException("The arrow must go from point P1 to point P2, where P1 not equals P2");

                if (From_X < To_X)
                {
                    //Canonic Intersection point
                    Ic = new Point(d_fi, 0);
                    I = Ic.Translate(f);
                }
                else//(From_X > To_X)
                {
                    //Canonic Intersection point
                    Ic = new Point(d_it, 0);
                    I = Ic.Translate(t);
                }

                cir = new Circunference(I, r);
                var inters = cir.IntersectWithX(I.X);
                SetSolution(inters.p1, inters.p2);
                return;
            }
            else if (From_X == To_X)
            {
                if (From_Y < To_Y)
                {
                    //Canonic Intersection point
                    Ic = new Point(0, d_fi);

                    //Intersection point
                    I = Ic.Translate(f);
                }
                else
                {
                    //Canonic Intersection point
                    Ic = new Point(0, d_it);

                    //Intersection point
                    I = Ic.Translate(t);
                }

                cir = new Circunference(I, r);
                var inters = cir.IntersectWithY(I.Y);
                SetSolution(inters.p1, inters.p2);
                return;
            }
            else if(From_Y < To_Y)
            {
                //Canonic Intersection point
                Ic = new Point(0, d_fi);

                //Intersection point
                I = Ic.Translate(f).Rotate(f, alpha);
                cir = new Circunference(I, r);
            }
            else//(From_Y>To_Y)
            {
                //Canonic Intersection point
                Ic = new Point(0, d_it);

                //Intersection point
                I = Ic.Translate(t).Rotate(t, alpha);
                cir = new Circunference(I, r);
            }

            //Pendings
            var m_ft = Cal.Pending(f, t);
            var m = -1 / (m_ft);//M_p1p2

            //Ecuation P1, P2. Using I(h;k): y = mx + n
            var n = -m * cir.c.X + cir.c.Y;

            var int2 = cir.IntersectWithRect(m,n);
            SetSolution(int2.p1, int2.p2);
        }
        public void SetSelectedPoint(ArrowPointsSelected selection)
        {
            SelectedPoint = selection;
        }
        public void TranslateSelectedPoint(Point p)
        {
            if (SelectedPoint == ArrowPointsSelected.From)
                SetFrom(p);
            else if (SelectedPoint == ArrowPointsSelected.To)
                SetTo(p);
        }
        public void SetFrom(Point p)
        {
            From_X = p.X;
            From_Y = p.Y;
        }
        public void SetTo(Point p)
        {
            To_X = p.X;
            To_Y = p.Y;
        }
    }
}
