using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake{
    public partial class MainScreen : Form{

        List<Point> FreeSpace = new List<Point>();
        Point Head, Food;
        List<Point> Body = new List<Point>();
        Random random = new Random();
        int maxwidth = 300, maxheight = 300;
        enum Direction: byte {Up,Right,Down,Left};
        Direction SnakeDir;
        public MainScreen(){
            InitializeComponent();
        }

        private void MainScreen_KeyUp(object sender, KeyEventArgs e){
            switch (e.KeyData){
                case Keys.Up:
                    if (SnakeDir != Direction.Down && Head.Y != Body[0].Y + 10) {
                        SnakeDir = Direction.Up;
                    }
                        break;
                case Keys.Left:
                    if (SnakeDir != Direction.Right && Head.X != Body[0].X + 10) {
                        SnakeDir = Direction.Left;
                    }
                    break;
                case Keys.Right:
                    if (SnakeDir != Direction.Left && Head.X != Body[0].X - 10){
                        SnakeDir = Direction.Right;
                    }   
                    break;
                case Keys.Down:
                    if (SnakeDir != Direction.Up && Head.Y != Body[0].Y - 10) {
                        SnakeDir = Direction.Down;
                    }
                    break;
            }
            
        }

        private void MainScreen_Paint(object sender, PaintEventArgs e){
            /*//BEGIN GRID
                    Pen gridline = new Pen(Color.DarkGreen, 1);
            for (int i = 0; i<maxwidth; i += 10){
                e.Graphics.DrawLine(gridline, i, 0, i, maxwidth);
            }
            for (int i = 0; i<maxheight; i += 10) {
                e.Graphics.DrawLine(gridline, 0, i, maxheight, i);
            }
            e.Graphics.DrawLine(Pens.Red, maxwidth, 0, maxwidth, maxwidth);
            e.Graphics.DrawLine(Pens.Red, 0, maxheight, maxheight, maxheight);
            //END GRID */
            e.Graphics.FillRectangle(Brushes.Tomato, Head.X, Head.Y, 10, 10);
            foreach (Point part in Body){
                e.Graphics.FillRectangle(Brushes.Olive, part.X, part.Y, 10, 10);
            }
            e.Graphics.FillRectangle(Brushes.Goldenrod, Food.X, Food.Y, 10, 10);
        //Console.WriteLine(SnakeDir.ToString());
        }

        private void MainScreen_Load(object sender, EventArgs e) {
            for (int i = 0; i<maxwidth; i += 10){
                for (int j=0; j<maxheight; j += 10) {
                    FreeSpace.Add(new Point(i, j));
                }
            }
            Head = new Point(random.Next(3, 27) * 10, random.Next(0, 30) * 10);
            FreeSpace.Remove(Head);

            for(int i = 1; i<4; i++){
                if(Head.X > maxwidth / 2){
                    SnakeDir = Direction.Left;
                    Body.Add(new Point(Head.X + i * 10 , Head.Y));
                }
                else{
                    SnakeDir = Direction.Right;
                    Body.Add(new Point(Head.X - i * 10, Head.Y));
                }
                FreeSpace.Remove(Body[Body.Count - 1]);
            }
            Food = FreeSpace[random.Next(0, FreeSpace.Count)];
            FreeSpace.Remove(Food);
            this.Text = "Score: 0";
        }

        private void timer1_Tick(object sender, EventArgs e){
            Body.Insert(0, Head);
            switch (SnakeDir){
                case Direction.Down:
                    Head.Y += 10;
                    break;
                case Direction.Left:
                    Head.X -= 10;
                    break;
                case Direction.Up:
                    Head.Y -= 10;
                    break;
                case Direction.Right:
                    Head.X += 10;
                    break;
            }
            FreeSpace.Remove(Head);
            if (Head == Food){
                Food = FreeSpace[random.Next(0, FreeSpace.Count)];
                FreeSpace.Remove(Food);
                this.Text = string.Format("Score: {0}", Body.Count() - 3);
            }
            else{
                FreeSpace.Add(Body[Body.Count() - 1]);
                Body.RemoveAt(Body.Count() - 1);
            }
            if (Head.X>290||Head.X<0||Head.Y>290||Head.Y<0||Body.Contains(Head)){
                timer1.Stop();
                this.Text = string.Format("Game Over. Score: {0}", Body.Count() - 3) ;
            }
            else { Refresh(); }

        }
    }
}
