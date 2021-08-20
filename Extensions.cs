using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArrowSample
{
    public static class Extensions
    {
        public static Point Translate(this Point p, double dx, double dy)
        {
            return new Point(p.X + dx, p.Y + dy);
        }
        public static Point Translate(this Point p, Point d)
        {
            return new Point(p.X + d.X, p.Y + d.Y);
        }

        /// <summary>
        /// Rotate a point around the specific center with the specific sita angle.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="center"></param>
        /// <param name="sita">Angle to rotate respect Y Axe in radians</param>
        /// <returns></returns>
        public static Point Rotate(this Point p,Point center, double sita)
        {
            var rook = Cal.Distance(center, p);
            var Pc = new Point(rook * Math.Sin(sita), rook * Math.Cos(sita));
            return Pc.Translate(center);
        }
        public static Point Rotate(this Point p, double cx,double cy, double sita)
        {
            return p.Rotate(new Point(cx, cy), sita);
        }
    }

    public static class Cal
    {
        public static double Distance(double x1,double y1,double x2,double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + (Math.Pow(y2 - y1, 2)));
        }
        public static double Distance(Point p1,Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + (Math.Pow(p2.Y - p1.Y, 2)));
        }

        public static double ToRadians(double a)
        {
            return (Math.PI * a) / 180;
        }
        public static double ToDegree(double a)
        {
            return (180 * a) / Math.PI;
        }

        public static double Pending(Point p1,Point p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

    }

    public struct Intersection
    {
        public Point p1, p2;
        public byte Count;

        public override string ToString()
        {
            return $"{Count} Solutions: P1{p1}, P2{p2})";
        }

        public void SetSolution(Point P1,Point P2)
        {
            P1 = p1;P2 = p2;
        }
    }

    public class QuadraticEcuation
    {
        /// <summary>
        /// Coeficient
        /// </summary>
        public double a, b, c;

        /// <summary>
        /// Solution 1
        /// </summary>
        public double x1;
        /// <summary>
        /// Solution 2
        /// </summary>
        public double x2;

        public byte SolutionCount;

        public QuadraticEcuation(double a,double b, double c)
        {
            this.a = a;this.b = b;this.c = c;
        }

        public double Eval(double x)
        {
            return a * Math.Pow(x, 2) + b * x + c;
        }
        public void Solve()
        {
            //Discriminant
            var d = Math.Pow(b, 2) - 4 * a * c;

            x1 = (-b + Math.Sqrt(d)) / (2 * a);
            if (d == 0) SolutionCount = 1;

            x2 = (-b - Math.Sqrt(d)) / (2 * a);
            SolutionCount = 2;
        }
    }

    public class Circunference
    {
        double h;
        double k;

        /// <summary>
        /// Center
        /// </summary>
        public Point c;
        /// <summary>
        /// Radius
        /// </summary>
        public double r;

        public Circunference(Point center,double radius)
        {
            c = center;
            r = radius;
            h = center.X;
            k = center.Y;
        }
        public Circunference(double centerH,double centerK, double radius)
        {
            h = centerH;
            k = centerK;
            r = radius;
            c = new Point(h, k);
        }

        /// <summary>
        /// Return solved quadratic equation of sustitute x in the circunference equation.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public QuadraticEcuation EvalX(double x)
        {
            var e = new QuadraticEcuation(1, -2 * k, Math.Pow(x - h, 2) + Math.Pow(k, 2) - Math.Pow(r, 2));
            e.Solve();

            return e;
        }

        public QuadraticEcuation EvalY(double y)
        {
            var e = new QuadraticEcuation(1, -2 * h, Math.Pow(h, 2) + Math.Pow(y - k, 2) - Math.Pow(r, 2));
            e.Solve();

            return e;
        }

        /// <summary>
        /// Return the intersections between Circunference(center,r) and rect y = mx + n
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public Intersection IntersectWithRect(double m, double n)
        {
            //Coeficients of square ecuation between rect and circunference
            var a = Math.Pow(m, 2) + 1;
            var b = -2 * h + 2 * m * (n - k);
            var c = Math.Pow(h, 2) + Math.Pow(n - k, 2) - Math.Pow(r, 2);

            var e = new QuadraticEcuation(a, b, c);
            e.Solve();

            return new Intersection { Count = e.SolutionCount, p1 = new Point(e.x1, m * e.x1 + n), p2 = new Point(e.x2, m * e.x2 + n) };
        }

        public Intersection IntersectWithX(double x)
        {
            var e = EvalX(x);//values of y
            e.Solve();

            return new Intersection
            {
                Count = e.SolutionCount,
                p1 = new Point(x, e.x1),
                p2 = new Point(x, e.x2)
            };
        }

        public Intersection IntersectWithY(double y)
        {
            var e = EvalY(y);//values of x
            e.Solve();

            return new Intersection
            {
                Count = e.SolutionCount,
                p1 = new Point(e.x1,y),
                p2 = new Point(e.x2,y)
            };
        }
    }

}
