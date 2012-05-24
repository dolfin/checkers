using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CheckersUI
{
    /// <summary>
    /// A pawn graphic on the board
    /// </summary>
    public class ShapePawn : CheckersUI.ShapeBase
    {
        // The last move of the pawn
        public int FromL { get; private set; }
        public int FromC { get; private set; }
        public int ToL { get; private set; }
        public int ToC { get; private set; }

        // Properties of the pawn
        public Brush Color { get; set; }
        public bool IsKing { get; set; }

        /// <summary>
        /// If we dropped the pawn on the board, locate it
        /// in the center of a square.
        /// </summary>
        public override bool IsDragged
        {
            get
            {
                return base.IsDragged;
            }
            set
            {
                if (base.IsDragged && !value)
                {
                    Point a = new Point();
                    int i = (int)Math.Floor((Location.X - 10) / 50.0);
                    int j = (int)Math.Floor((Location.Y - 10 - 24) / 50.0);
                    a.X = i * 50 + 15;
                    a.Y = j * 50 + 15 + 24;

                    if (a.X >= 15 && a.Y >= (15 + 24) && a.X <= 410 && a.Y <= 434 &&
                        (((j * 8 + i + (j % 2)) % 2) == 1))
                    {
                        ToC = i;
                        ToL = j;
                        FromC = (int)Math.Floor((OriginalLocation.X - 10) / 50.0);
                        FromL = (int)Math.Floor((OriginalLocation.Y - 10 - 24) / 50.0);
                        Location = a;
                    }
                    else
                    {
                        Location = OriginalLocation;
                    }
                }

                base.IsDragged = value;
            }
        }

        /// <summary>
        /// Draw the pawn
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            int x, y;

            if (IsDragged)
            {
                x = Location.X + MouseOffset.X;
                y = Location.Y + MouseOffset.Y;
            } else {
                x = Location.X;
                y = Location.Y;
            }

            g.FillEllipse(Color, x, y, 40, 40);

            if (IsKing)
            {
                Point[] points = DrawCrown(x, y);
                g.FillPolygon(Brushes.Gold, points);
            }
        }

        /// <summary>
        /// Draw the crown for the king piece
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Point[] DrawCrown(int x, int y)
        {
            Point[] point = new Point[7]
            {
                new Point(x + 10, y + 30),
                new Point(x + 10, y + 10),
                new Point(x + 15, y + 15),
                new Point(x + 20, y + 10),
                new Point(x + 25, y + 15),
                new Point(x + 30, y + 10),
                new Point(x + 30, y + 30)
            };
            return point;
        }
    }

}
