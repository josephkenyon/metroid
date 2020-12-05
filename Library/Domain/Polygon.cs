using Library.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class Polygon
    {
        public readonly List<Point> Points;

        public List<Point> AddPoints(List<Point> points)
        {
            foreach ( Point point in points )
            {
                Points.Add(point);
            }

            return Points;
        }

        public Polygon(Point origin, Point size, float angle)
        {
            Points = new List<Point> {
                new Point{
                    X = origin.X + Convert.ToInt32((size.X / 2f * Math.Cos(angle)) - (size.Y / 2f * Math.Sin(angle))),
                    Y = origin.Y + Convert.ToInt32((size.X / 2f * Math.Sin(angle)) + (size.Y / 2f * Math.Cos(angle)))
                },
               new Point{
                    X = origin.X - Convert.ToInt32((size.X / 2f * Math.Cos(angle)) - (size.Y / 2f * Math.Sin(angle))),
                    Y = origin.Y - Convert.ToInt32((size.X / 2f * Math.Sin(angle)) + (size.Y / 2f * Math.Cos(angle)))
                },
               new Point{
                    X = origin.X - Convert.ToInt32((size.X / 2f * Math.Cos(angle)) + (size.Y / 2f * Math.Sin(angle))),
                    Y = origin.Y - Convert.ToInt32((size.X / 2f * Math.Sin(angle)) - (size.Y / 2f * Math.Cos(angle)))
                },
               new Point{
                    X = origin.X + Convert.ToInt32((size.X / 2f * Math.Cos(angle)) + (size.Y / 2f * Math.Sin(angle))),
                    Y = origin.Y + Convert.ToInt32((size.X / 2f * Math.Sin(angle)) - (size.Y / 2f * Math.Cos(angle)))
                },
            };
        }

        public Polygon(Rectangle rectangle)
        {
            Points = new List<Point> {
                new Point{
                    X = rectangle.X,
                    Y = rectangle.Y
                },
               new Point{
                    X = rectangle.X + Convert.ToInt32(rectangle.Width),
                    Y = rectangle.Y
                },
               new Point{
                    X = rectangle.X,
                    Y = rectangle.Y + Convert.ToInt32(rectangle.Height)
                },
               new Point{
                    X = rectangle.X + Convert.ToInt32(rectangle.Width),
                    Y = rectangle.Y + Convert.ToInt32(rectangle.Height)
                },
            };
        }

        public Polygon(List<Point> points)
        {
            Points = points;
        }

        public bool IsIntersectingWith(Polygon b)
        {
            foreach ( Polygon polygon in new[] { this, b } )
            {
                for ( int i1 = 0; i1 < polygon.Points.Count; i1++ )
                {
                    int i2 = (i1 + 1) % polygon.Points.Count;
                    Point p1 = polygon.Points[i1];
                    Point p2 = polygon.Points[i2];

                    Point normal = new Point(p2.Y - p1.Y, p1.X - p2.X);

                    double? minA = null, maxA = null;
                    foreach ( Point p in Points )
                    {
                        int projected = normal.X * p.X + normal.Y * p.Y;
                        if ( minA == null || projected < minA )
                            minA = projected;
                        if ( maxA == null || projected > maxA )
                            maxA = projected;
                    }

                    double? minB = null, maxB = null;
                    foreach ( Point p in b.Points )
                    {
                        int projected = normal.X * p.X + normal.Y * p.Y;
                        if ( minB == null || projected < minB )
                            minB = projected;
                        if ( maxB == null || projected > maxB )
                            maxB = projected;
                    }

                    if ( maxA < minB || maxB < minA )
                        return false;
                }
            }
            return true;
        }
    }
}
