using System;
using System.Drawing;
using System.Windows.Forms;
namespace CheckersUI
{
    /// <summary>
    /// Abstract class to represent all movable shapes on the form
    /// </summary>
    public abstract class ShapeBase
    {
        public abstract void Draw(System.Drawing.Graphics g);
        public virtual bool IsDragged { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual System.Drawing.Point Location { get; set; }
        public virtual System.Drawing.Point MouseOffset { get; set; }
        public Point OriginalLocation { get; set; }
        public virtual bool Moveable { get; set; }
        public virtual bool Locked { get; set; }

        public Cursor GetCursor()
        {
            return Cursors.Arrow;
        }

        /// <summary>
        /// returns true if the mouse pointer is inside the shape
        /// </summary>
        /// <param name="pPoint"></param>
        /// <returns></returns>
        public bool IsCollision(Point pPoint)
        {
            // determine type of collision
            /*
            * For now just see if there is any kind of collision
            * with the mouse cursor
            */
            Rectangle lrectCursorRect = new Rectangle(pPoint, new Size(2, 2));
            Rectangle region = new Rectangle(Location, new Size(40, 40));
            if (region.IntersectsWith(lrectCursorRect))
            {
                //this.Cursor = Cursors.SizeAll;
                return true;
            }
            return false;
        }
    }
}
