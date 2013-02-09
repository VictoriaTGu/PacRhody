using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace WindowsApplication22
{
    public class Ghost
    {
        public Point Position;
        Bitmap GhostImage = null;
        //static Bitmap GhostImage2 = null;
  
        int inc = 10;
        int LastPositionX = 0;
        int LastPositionY = 0;

        public int prevDirection = 1;
        public int direction = 1;
        private Random RandomGen = new Random();

        public Ghost()
        {
            Position.X = 50;
            Position.Y = 55;

            //if (GhostImage == null)
            //{
                //GhostImage = new Bitmap("Ghost_Blinky.gif");
            //}


        }

        public Ghost(int x, int y, String image)
		{
			// 
			// TODO: Add constructor logic here
			//
			Position.X = x;
			Position.Y = y;
			//if (GhostImage  == null)
			//{
                String filename = image;
                GhostImage = new Bitmap(filename);
			//}

		}

        public int Direct()
        {
            direction = RandomDirection(1, 4);
            prevDirection = direction;
            return direction;
        }

        private int RandomDirection(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public Rectangle GetFrame()
        {
            Rectangle myRect = new Rectangle(Position.X, Position.Y, GhostImage.Width, GhostImage.Height);
            return myRect;
        }

        public void Draw(Graphics g)
        {

            Rectangle destR = new Rectangle(Position.X, Position.Y, GhostImage.Width, GhostImage.Height);
            Rectangle srcR = new Rectangle(0, 0, GhostImage.Width, GhostImage.Height);
            /*if (((Position.X % 2 == 1) && ((Position.X - LastPositionX) != 0)) ||
                 ((Position.Y % 2 == 1) && ((Position.Y - LastPositionY) != 0))
               )*/
                g.DrawImage(GhostImage, destR, srcR, GraphicsUnit.Pixel);

            //else
                //g.DrawImage(GhostImage2, destR, srcR, GraphicsUnit.Pixel);

            LastPositionX = Position.X;
            LastPositionY = Position.Y;
        }

        public void MoveLeft(Rectangle r)
        {
            //if (Position.X <= 0)
                //return;  // precondition

            Position.X -= inc;
        }

        public void MoveRight(Rectangle r)
        {
            //if (Position.X >= r.Width - GhostImage.Width)
                //return;  // precondition

            Position.X += inc;
        }

        public void MoveUp(Rectangle r)
        {
            //if (Position.Y <= 0)
                //return;  // precondition

            Position.Y -= inc;
        }

        public void MoveDown(Rectangle r)
        {
            //if (Position.Y >= r.Height - GhostImage.Height)
                //return;  // precondition

            Position.Y += inc;
        }
    }
}
