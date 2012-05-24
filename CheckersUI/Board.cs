using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SbsSW.SwiPlCs;
using System.Threading;

namespace CheckersUI
{
    public partial class Board : Form
    {
        private string mPath; // Prolog file path
        private bool mHumanStart = true; // whether the human or the computer starts
        private int mDifficulty = 2; // difficulty level (2-5)

        // double buffer drawing bitmaps
        private Bitmap mBitmapdrawingArea;
        private Bitmap mBitmapOriginalDrawingArea;
        private List<ShapeBase> mListShapes; // list of all game piece

        // Board location on the screen
        private const int FormXSpace = 10;
        private const int FormYSpace = 34;

        // Piece location inside a board's square
        private const int CellSpace = 5;
        private const int CellSize = 50;

        // Whether the pieces can be moved right now
        private bool mLocked = false;

        // The game engine
        private GameEngine mEngine;

        /// <summary>
        /// Constructor. Starts a new game.
        /// </summary>
        public Board(string path)
        {
            InitializeComponent();
            mPath = path;
            StartGame();
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        private void StartGame()
        {
            /*
            * Create a blank canvas for drawing
            */
            mBitmapOriginalDrawingArea = new Bitmap(
                 this.ClientRectangle.Width,
                 this.ClientRectangle.Height,
                 System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(mBitmapOriginalDrawingArea);
            DrawBoard(g);
            g.Dispose();

            mBitmapdrawingArea = (Bitmap)mBitmapOriginalDrawingArea.Clone();

            // Init the prolog engine
            mEngine = new GameEngine(mPath, mHumanStart, mDifficulty);

            // Draw pawns on the board
            InitPawns(mEngine.Board);

            // First move
            if (!mHumanStart)
            {
                this.Cursor = Cursors.WaitCursor;
                mLocked = true;

                ThreadPool.QueueUserWorkItem(new WaitCallback(PlayCompAsync));
            }

            Refresh();
        }

        /// <summary>
        /// An event that called every time the board should be painted again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_Paint(object sender, PaintEventArgs e)
        {
            // Draw image on the bitmap

            Graphics graphics = Graphics.FromImage(mBitmapdrawingArea);
            DrawBoard(graphics);
            DrawPawns(graphics, mEngine.Board);
            graphics.Dispose();

            // Draw bitmap onto the form
            e.Graphics.DrawImage(mBitmapdrawingArea, new Point(0,0));
        }

        /// <summary>
        /// Draws the game board
        /// </summary>
        /// <param name="g"></param>
        private void DrawBoard(Graphics g)
        {
            g.FillRectangle(Brushes.DarkGray, this.ClientRectangle);
            Pen p = new Pen(Color.White, (float)1.0);
            Brush[] b = new Brush[2] { Brushes.Maroon, Brushes.Black };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    g.FillRectangle(b[(i * 8 + j + (i % 2)) % 2],
                        FormXSpace + CellSize * j, FormYSpace + CellSize * i, CellSize, CellSize);
                }
            }

            for (int i = 0; i <= 8; i++)
            {
                g.DrawLine(p, FormXSpace + CellSize * i, FormYSpace, FormXSpace + CellSize * i, CellSize * 8 + FormYSpace);
                g.DrawLine(p, FormXSpace, FormYSpace + CellSize * i, CellSize * 8 + FormXSpace, FormYSpace + CellSize * i);
            }
        }

        /// <summary>
        /// Draw pawns on the board
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pawns"></param>
        private void DrawPawns(Graphics g, Pawn[] pawns)
        {
            InitPawns(pawns);

            foreach (ShapeBase s in mListShapes)
            {
                s.Draw(g);
            }
        }

        /// <summary>
        /// Initialize the pieces list
        /// </summary>
        /// <param name="pawns"></param>
        private void InitPawns(Pawn[] pawns)
        {
            mListShapes = new List<ShapeBase>();

            for (int i = 0; i < pawns.Length; i++)
            {
                ShapePawn s = new ShapePawn();

                if (pawns[i] != Pawn.Empty && pawns[i] != Pawn.None)
                {
                    int a = i % 8;
                    int b = (int)Math.Floor(i / 8.0);

                    s.IsDragged = true;
                    s.Location = new Point(FormXSpace + CellSpace + CellSize * a, FormYSpace + CellSpace + CellSize * b);
                    s.OriginalLocation = s.Location;
                    s.IsSelected = false;
                    s.IsDragged = false;
                    s.Locked = mLocked;

                    if (pawns[i] == Pawn.KingX || pawns[i] == Pawn.KingO)
                    {
                        s.IsKing = true;
                    }
                    else
                    {
                        s.IsKing = false;
                    }

                    if (pawns[i] == Pawn.X || pawns[i] == Pawn.KingX)
                    {
                        s.Moveable = false;
                        if (mHumanStart) s.Color = Brushes.White;
                        else s.Color = Brushes.Red;
                    }
                    else
                    {
                        s.Moveable = true;
                        if (!mHumanStart) s.Color = Brushes.White;
                        else s.Color = Brushes.Red;
                    }

                    mListShapes.Add(s);
                }
            }
        }

        /// <summary>
        /// On mouse down switch a dragged shape to a static shape
        /// </summary>
        private void Board_MouseDown(object sender, MouseEventArgs e)
        {
            Point mouseLocation = new Point(e.X, e.Y);
            mBitmapdrawingArea =(Bitmap)mBitmapOriginalDrawingArea.Clone();
            Graphics graphics = Graphics.FromImage(mBitmapdrawingArea);

            //Draw out the non dragged shapes and non selected on 
            // the Canvas to be saved;
            foreach (ShapeBase shape in mListShapes)
            {
                /*
                * If we are not a dragged shape but our mouse is contained in our
                * shape then we want to become a dragged shape
                */
                if (shape.Moveable && !shape.Locked && !shape.IsDragged && shape.IsCollision(mouseLocation))
                {
                    Point lpntOffset = new Point(shape.Location.X -
                                            mouseLocation.X,shape.Location.Y - 
                                            mouseLocation.Y);
                    shape.MouseOffset = lpntOffset;
                    shape.IsDragged = true;
                    shape.OriginalLocation = shape.Location;
                    continue;
                }
                else
                {
                    // we are just a static shape that needs to 
                    //be drawn to the canvas
                    shape.IsDragged = false;
                    shape.Draw(graphics);
                }
            }
            graphics.Dispose();
            // Draw out the dragged shapes on the Cavas not to save;
            Bitmap bitMapTemp = (Bitmap)mBitmapdrawingArea.Clone();
            graphics = Graphics.FromImage(bitMapTemp);
            foreach (ShapeBase shape in mListShapes)
            {
                if (shape.IsDragged)
                {
                    shape.Location = new Point(mouseLocation.X, mouseLocation.Y);
                    shape.Draw(graphics);
                }
            }
            graphics.Dispose();
            // draw the canvas to the control's surface
            Graphics graphicsForm = this.CreateGraphics();
            graphicsForm.DrawImage(bitMapTemp, new Point(0, 0));
            graphicsForm.Dispose();
            bitMapTemp.Dispose();
        }
         
        /// <summary>
        /// Draw our dragged images as the user moves the mouse
        /// </summary>
        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap bitmap = (Bitmap)mBitmapdrawingArea.Clone();
            Graphics graphics = Graphics.FromImage(bitmap);
            Point location = new Point(e.X, e.Y);
            foreach (ShapeBase shape in mListShapes)
            {
                // if dragged then change its location 
                if (shape.IsDragged)
                {
                    shape.Location = new Point(location.X, location.Y);
                    shape.Draw(graphics);
                }
            }

            graphics.Dispose();

            // draw the bitmap canvas to the control's surface
            Graphics graphicsForm = this.CreateGraphics();
            graphicsForm.DrawImage(bitmap, new Point(0, 0));
            graphicsForm.Dispose();

            bitmap.Dispose();
        }

        /// <summary>
        /// On mouse up switch back a dragged shape to a static shape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_MouseUp(object sender, MouseEventArgs e)
        {
            ShapeBase validMoveShape = null;
            Point mouseLocation = new Point(e.X, e.Y);
            mBitmapdrawingArea = (Bitmap)mBitmapOriginalDrawingArea.Clone();
            Graphics graphics = Graphics.FromImage(mBitmapdrawingArea);

            //Draw out the non dragged shapes and non selected on 
            // the Canvas to be saved;
            foreach (ShapeBase shape in mListShapes)
            {
                // we are just a static shape that needs to 
                //be drawn to the canvas
                if (shape.IsDragged)
                {
                    shape.IsDragged = false;

                    bool backMove = false;
                    foreach (ShapeBase s in mListShapes)
                    {
                        if (s != shape && s.IsCollision(shape.Location))
                        {
                            shape.Location = shape.OriginalLocation;
                            backMove = true;
                        }
                    }

                    if (!backMove && !shape.Location.Equals(shape.OriginalLocation))
                    {
                        validMoveShape = shape;
                    }
                }

                shape.Draw(graphics);
            }
            graphics.Dispose();
            // Draw out the dragged shapes on the Cavas not to save;
            Bitmap bitMapTemp = (Bitmap)mBitmapdrawingArea.Clone();
            graphics = Graphics.FromImage(bitMapTemp);
            foreach (ShapeBase shape in mListShapes)
            {
                if (shape.IsDragged)
                {
                    shape.Location = mouseLocation;
                    shape.Draw(graphics);
                }
            }
            graphics.Dispose();
            // draw the canvas to the control's surface
            Graphics graphicsForm = this.CreateGraphics();
            graphicsForm.DrawImage(bitMapTemp, new Point(0, 0));
            graphicsForm.Dispose();
            bitMapTemp.Dispose();

            if (validMoveShape != null)
            {
                PlayComputer((ShapePawn)validMoveShape);
            }
        }

        /// <summary>
        /// If the human move is valid, plays one computer turn
        /// </summary>
        /// <param name="shape"></param>
        private void PlayComputer(ShapePawn shape)
        {
            bool res = mEngine.PlayHuman(shape.FromL + 1, shape.FromC + 1, shape.ToL + 1, shape.ToC + 1);

            if (res)
            {
                shape.OriginalLocation = shape.Location;

                if (mEngine.CheckHumanWon())
                {
                    MessageBox.Show("You Won!", "Game Over", MessageBoxButtons.OK);
                    EndGame(false);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    mLocked = true;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(PlayCompAsync));
                }
            }
            else
            {
                shape.Location = shape.OriginalLocation;
            }

            RedrawBoard();
        }

        /// <summary>
        /// Redraw the board
        /// </summary>
        private void RedrawBoard()
        {
            mBitmapdrawingArea = (Bitmap)mBitmapOriginalDrawingArea.Clone();
            Graphics graphics = Graphics.FromImage(mBitmapdrawingArea);
            DrawPawns(graphics, mEngine.Board);
            Graphics graphicsForm = this.CreateGraphics();
            graphicsForm.DrawImage(mBitmapdrawingArea, new Point(0, 0));
            graphicsForm.Dispose();
            graphics.Dispose();
        }

        /// <summary>
        /// Play the comuter move asynchronusly on another thread
        /// </summary>
        delegate void VoidDelegate();
        private void PlayCompAsync(object temp)
        {
            mEngine.PlayComputer();

            mLocked = false;
            this.Invoke(new VoidDelegate(PlayComp));
        }

        private void PlayComp()
        {
            this.Cursor = Cursors.Default;
            
            if (mEngine.CheckComputerWon())
            {
                MessageBox.Show("You Lost!", "Game Over", MessageBoxButtons.OK);
                EndGame(false);
            }

            RedrawBoard();
        }

        /// <summary>
        /// Show end game dialog
        /// </summary>
        /// <param name="ask"></param>
        /// <returns></returns>
        private bool EndGame(bool ask)
        {
            if (ask)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to end this game?", "Checkers", MessageBoxButtons.YesNo);
                if (DialogResult.Yes != res)
                {
                    return false;
                }
            }

            mEngine.Dispose();
            return true;
        }

        /// <summary>
        /// Menu item: Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Click on the form X button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!EndGame(true))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Menu item: New game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EndGame(true))
            {
                StartGame();
            }
        }

        /// <summary>
        /// Menu item: About
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }

        /// <summary>
        /// Menu item: Options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options opts = new Options(mHumanStart, mDifficulty);
            DialogResult res = opts.ShowDialog();

            if (DialogResult.OK == res)
            {
                mHumanStart = opts.HumanStart;
                mDifficulty = opts.Difficulty;

                if (EndGame(true))
                {
                    StartGame();
                }
            }
        }
    }
}
