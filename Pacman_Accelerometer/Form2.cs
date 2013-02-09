using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;
using DFSAlgorithmMaze;
using System.IO;
using System.IO.Ports;


namespace WindowsApplication22
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{


        // Needed in order to play sound
        [DllImport("winmm.dll")]
        public static extern long PlaySound(String lpszName, long hModule, long dwFlags);

        private Random RandomGen = new Random();                        // Needed to generate random numbers

        private ArrayList Stones = new ArrayList(30);                   // Generates an array of stones for pacman to eat

        private int NumberOfStones;                                     // Keeps track of the total number of stones not eaten

        private Score TheScore;                                         // Keeps track of the number of dots eaten


        private GameMessage TheStoneMessage;                            // Writes "dot" or "dots" on the form

        private int TheSeconds = 90;                                    // Keeps track of the number of seconds left in the game
        private int startTime = 90;                                     // Records the number of seconds at the start
        private int overallScore = 0;                                   // Overall score = number of dots eaten + time left
        private String ScoreMessage;                                    // "dots"

        private System.Windows.Forms.Timer timer1;                      // Create a timer
        private TimerDisplay TheTime = new TimerDisplay(20, 290);       // Display the time ticking down

        private System.ComponentModel.IContainer components;
        private Thread oThread = null;

        private Maze TheMaze = new Maze();                              // Initialize the maze
        private bool m_bGameDone = false;                               // Game is not over yet
        private GameMessage TheStatusMessage = new GameMessage(150, 10);// 
        private GameMessage TheNameMessage = new GameMessage(150, 20);
        private bool clearwindow = false;

        public enum Side { top = 0, left = 1, bottom = 2, right = 3 };

        // Initialize Eater
        private Eater TheEater = new Eater(100, 100);

        // Initialize ghosts
        private Ghost blinky = new Ghost(150, 150, "Ghost_Blinky.gif");
        private Ghost pinky = new Ghost(190, 190, "pinky.gif");
        private Ghost clyde = new Ghost(220, 220, "clyde.gif");
        private Ghost inky = new Ghost(240, 240, "inky.gif");

        private RichTextBox richTextBox1;

        private TextBox textBox1;                                     // Initialize textbox where user enters name
        private Button button1;                                       // Initialize restart button

        private string name = "";                                     // Hold user name as user enters characters

        public static string finalName = "";                          // Hold final user name

        public static string direct;
        private Button button2;
        public static int dec;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            // Sets the size of the maze
            Maze.kDimension = 10;

            // Sets the size of the cells
            Cell.kCellSize = 30;

            // The number of Stones, the location of the score and stone message all depend on the size of the maze
            NumberOfStones = (Maze.kDimension - 5) * (Maze.kDimension - 5);
            TheScore = new Score(((Maze.kDimension * 2/3) * (Cell.kCellSize + Cell.kPadding)), 10);
            TheStoneMessage = new GameMessage(((Maze.kDimension * 3/4) * (Cell.kCellSize + Cell.kPadding)), 10);
            TheNameMessage = new GameMessage(8, (Maze.kDimension) * Cell.kCellSize + Cell.kPadding + 22);
            AskName();

            // Determines how much time you are given, according to the size of the maze
            if (Maze.kDimension >= 10 && Maze.kDimension <= 20)
                TheSeconds = 90;
            if (Maze.kDimension >= 20)
                TheSeconds = 120;

            // Records the start time
            startTime = TheSeconds;

			TheMaze.Initialize();
			TheMaze.Generate();

			InitializeStones();
			InitializeEater();
            InitializeGhost();

			InitializeMessage();
            InitializeTimer();
			InitializeScore();

            // Sets window size
            SetBounds(10, 10, 700, 800);

            //(Maze.kDimension + 2) * Cell.kCellSize + Cell.kPadding, (Maze.kDimension + 3) * Cell.kCellSize + Cell.kPadding + 8)
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}



        public static SerialPort sp = new SerialPort();

        private string m_strCurrentSoundFile = "";

        public void PlayASound()
        {
            if (m_strCurrentSoundFile.Length > 0)
            {
                PlaySound(Application.StartupPath + "\\" + m_strCurrentSoundFile, 0, 0);
            }
            m_strCurrentSoundFile = "";
            oThread.Abort();
        }

        public void PlaySoundInThread(string wavefile)
        {
            m_strCurrentSoundFile = wavefile;
            oThread = new Thread(new ThreadStart(PlayASound));
            oThread.Start();
        }
        public void LogScore(int score)
        {
            String myscore = score + "";                                    // Take in the score and convert it to a string

            using (StreamWriter w = File.AppendText("log.txt"))             // Append the string to the specified file
            {
                Log(myscore, w);
                w.Close();                                                  // Close the writer and underlying file.
            }

            using (StreamReader r = File.OpenText("log.txt"))               // Open and read the file.
            {
                DumpLog(r);
            }

        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nTime : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
            w.WriteLine("\n Name: " + finalName + "." + " Score : {0} ", logMessage);
            w.WriteLine("-------------------------------");
            w.Flush();                                                      // Update the underlying file.
        }

        public static void DumpLog(StreamReader r)
        {

            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);                                    // While not at the end of the file, read and write lines.
            }
            r.Close();
        }

        public void DisplayScores()
        {
            String msg = "";
            string[] readText = File.ReadAllLines("log.txt");
            foreach (string s in readText)
                msg = msg + "\n" + s;
            MessageBox.Show(msg, "Record of Scores");

        }

		public void InitializeTimer()
		{
			//timer1.Start();
			TheTime.Direction = TimeDirection.Down;
			TheTime.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
		}

		public void InitializeMessage()
		{
			TheStatusMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
			TheStoneMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
			TheStoneMessage.Message = "Dot";
		}


		public void InitializeScore()
		{
			TheScore.Reset();
			TheScore.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
		}

        public void AskName()
        {
            TheNameMessage.Message = "Enter Name: ";
        }


		public void InitializeStones()
		{
			for (int i = 0; i < NumberOfStones; i++)
			{
				Point cellCenter = GetRandomCellPosition();
				Stones.Add(new Stone(cellCenter.X - 6, cellCenter.Y - 6)); // 12 is stone image width and height, 6 is half of this
			}
		} 

		public Point GetRandomCellPosition()
		{
			int xCell = RandomGen.Next(0, Maze.kDimension);
			int yCell = RandomGen.Next(0, Maze.kDimension);
			Point cellCenter = TheMaze.GetCellCenter(xCell, yCell);
			return cellCenter;
		}

		public void InitializeEater()
		{
			Point cellCenter = GetRandomCellPosition();
			TheEater.Position.X = cellCenter.X - 10;
			TheEater.Position.Y = cellCenter.Y - 10;
		}

        public void InitializeGhost()
        {
            Point cellCenter = GetRandomCellPosition();
            blinky.Position.X = cellCenter.X - 10;
            blinky.Position.Y = cellCenter.Y - 10;

            // Relocate ghost if it hits the eater
            while (blinky.GetFrame().IntersectsWith(TheEater.GetFrame()))
            {
                cellCenter = GetRandomCellPosition();
                blinky.Position.X = cellCenter.X - 10;
                blinky.Position.Y = cellCenter.Y - 10;
            }

            cellCenter = GetRandomCellPosition();
            pinky.Position.X = cellCenter.X - 10;
            pinky.Position.Y = cellCenter.Y - 10;

            // Relocate ghost if it hits the eater or other ghosts
            while (pinky.GetFrame().IntersectsWith(TheEater.GetFrame())
                || pinky.GetFrame().IntersectsWith(blinky.GetFrame()))
            {
                cellCenter = GetRandomCellPosition();
                pinky.Position.X = cellCenter.X - 10;
                pinky.Position.Y = cellCenter.Y - 10;
            }

            cellCenter = GetRandomCellPosition();
            clyde.Position.X = cellCenter.X - 10;
            clyde.Position.Y = cellCenter.Y - 10;

            // Relocate ghost if it hits the eater or other ghosts
            while (clyde.GetFrame().IntersectsWith(TheEater.GetFrame())
                || clyde.GetFrame().IntersectsWith(blinky.GetFrame())
                || clyde.GetFrame().IntersectsWith(pinky.GetFrame()))
            {
                cellCenter = GetRandomCellPosition();
                clyde.Position.X = cellCenter.X - 10;
                clyde.Position.Y = cellCenter.Y - 10;
            }

            cellCenter = GetRandomCellPosition();
            inky.Position.X = cellCenter.X - 15;
            inky.Position.Y = cellCenter.Y - 15;

            // Relocate ghost if it hits the eater or other ghosts
            while (inky.GetFrame().IntersectsWith(TheEater.GetFrame())
                || inky.GetFrame().IntersectsWith(blinky.GetFrame())
                || inky.GetFrame().IntersectsWith(pinky.GetFrame())
                || inky.GetFrame().IntersectsWith(clyde.GetFrame()))
            {
                cellCenter = GetRandomCellPosition();
                inky.Position.X = cellCenter.X - 15;
                inky.Position.Y = cellCenter.Y - 15;
            }
        }

        /* Checks if the ghost bumped into pacman
           If it did, then the game ends. */
        public void CheckGhost(Ghost ghost)
        {
            if (m_bGameDone == false)
            {
                if (TheEater.GetFrame().IntersectsWith(ghost.GetFrame()))
                {
                    timer1.Stop();
                    PlaySoundInThread("pacman_dies.wav");
                    m_bGameDone = true;
                    int dotsEaten = NumberOfStones - Stones.Count;
                    MessageBox.Show("Game Over\nDots Eaten: " + dotsEaten);
                    LogScore(dotsEaten);
                    DisplayScores();
                    Invalidate(TheStatusMessage.GetFrame());
                    Application.Exit();
                }
            }
        }
       

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(131, 331);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(84, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(391, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(198, 549);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(233, 328);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Restart";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(929, 573);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Pacman";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}


		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.FillRectangle(Brushes.Black, 0, 0, this.ClientRectangle.Width, ClientRectangle.Height);

			TheMaze.Draw(g);

			// draw the score

			TheScore.Draw(g);

            TheNameMessage.Draw(g);
	
			// draw the time

			TheTime.Draw(g, TheSeconds);

			// Draw a message indicating the status of the game
			TheStatusMessage.Draw(g);

			// Draw a message indicating a Stone
			//if (TheScore.Value == 1)
            if (NumberOfStones - Stones.Count == 1)
				TheStoneMessage.Message = "Dot";
            else
                TheStoneMessage.Message = "Dots";

			TheStoneMessage.Draw(g);


			// draw the stones

			for (int i = 0; i < Stones.Count; i++)
			{
				((Stone)Stones[i]).Draw(g);
			}

			// also draw the eater
			TheEater.Draw(g);

            // and the ghosts
            blinky.Draw(g);
            pinky.Draw(g);
            clyde.Draw(g);
            inky.Draw(g);
		}

		private int CheckIntersection()
		{
			for (int i = 0; i < Stones.Count; i++)
			{
				Rectangle stoneRect = ((Stone)Stones[i]).GetFrame();
				if (TheEater.GetFrame().IntersectsWith(stoneRect))
				{
					return i;
				}
			}

			return -1;
		}

        private bool CanEaterMove(Side aSide)
        {
            int theSide = (int)aSide;
            Cell EaterCell = TheMaze.GetCellFromPoint(TheEater.Position.X + 10, TheEater.Position.Y + 10);
            if (EaterCell.Walls[theSide] == 1)
            {
                if (EaterCell.GetWallRect((int)aSide).IntersectsWith(TheEater.GetFrame()))
                {
                    return false;  // blocked
                }

             }
            return true;
        }

        private bool CanGhostMove(Side aSide, Ghost ghost)
        {
            int theSide = (int)aSide;
            Cell GhostCell = TheMaze.GetCellFromPoint(ghost.Position.X + 10, ghost.Position.Y + 10);
            if (GhostCell.Walls[theSide] == 1)
            {
                if (GhostCell.GetWallRect((int)aSide).IntersectsWith(ghost.GetFrame()))
                {
                    return false;  // blocked
                }

            }
            return true;
		}

		string LatestKey = "none";

		private void HandleLatestKey()
		{
			if (m_bGameDone)
				return;  // precondition

			//string result = LatestKey;
			Invalidate(TheEater.GetFrame());
			switch (direct)
			{
				case "Left":
					if (CanEaterMove(Side.left))
					{
						TheEater.MoveLeft(ClientRectangle);
					}
					Invalidate(TheEater.GetFrame());
                    
					break;
				case "Right":
					if (CanEaterMove(Side.right))
					{
						TheEater.MoveRight(ClientRectangle);
					}
					Invalidate(TheEater.GetFrame());
					break;
				case "Up":
					if (CanEaterMove(Side.top))
					{
						TheEater.MoveUp(ClientRectangle);
					}
					Invalidate(TheEater.GetFrame());
					break;
				case "Down":
					if (CanEaterMove(Side.bottom))
					{
						TheEater.MoveDown(ClientRectangle);
					}
					Invalidate(TheEater.GetFrame());
					break;
				default:
					break;

			}

			int hit = CheckIntersection();
			if (hit != -1)
			{
				TheScore.Increment();
                PlaySoundInThread("hit.wav");
				Invalidate(TheScore.GetFrame());
				Invalidate(((Stone)Stones[hit]).GetFrame()); 
				Stones.RemoveAt(hit);
				if (Stones.Count == 0)
				{
                    timer1.Stop();
                    int dotsEaten = NumberOfStones - Stones.Count;
					MessageBox.Show("You Win!\nYour time is " + TheTime.TheString + " seconds. "
                       + "\nDots Eaten: " + dotsEaten + "\n" + OverallScore());
                    LogScore(overallScore);
                    DisplayScores();
                    Application.Exit();
				}
			}

		}

		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			string result = e.KeyData.ToString();
			LatestKey = result;

            if (e.KeyCode == Keys.Enter)
            {
                ReadName();
                timer1.Start();
                richTextBox1.Text = "BaudRate = " + sp.BaudRate.ToString();
                richTextBox1.Text += "\nStop Bit = " + sp.StopBits.ToString();
                richTextBox1.Text += "\nDataBits = " + sp.DataBits.ToString();
                richTextBox1.Text += "\nParity = " + sp.Parity.ToString();
                richTextBox1.Text += "\nReadTimeout = " + sp.ReadTimeout.ToString();
                richTextBox1.Text += "\nCOM Port\n = " + sp.PortName.ToString();


                richTextBox1.Text += "Status...Connected\n";

                
            }

		}

		static long TimerTickCount = 0;

        private void GhostMove(Ghost ghost)
        {
            int direction = Direction(ghost);

            Invalidate(ghost.GetFrame());

            // Moves the ghost based on the direction given by the Direction method
            switch (direction)
            {
                case 1:
                    {
                        if (CanGhostMove(Side.left, ghost))
                        {
                            ghost.MoveLeft(ClientRectangle);
                        }
                        Invalidate(ghost.GetFrame());
                        break;

                    }

                case 2:
                    {

                        if (CanGhostMove(Side.right, ghost))
                        {
                            ghost.MoveRight(ClientRectangle);
                        }

                        Invalidate(ghost.GetFrame());
                        break;

                    }

                case 3:
                    {

                        if (CanGhostMove(Side.bottom, ghost))
                        {
                            ghost.MoveDown(ClientRectangle);
                        }
                        Invalidate(ghost.GetFrame());
                        break;

                    }

                case 4:
                    {
                        if (CanGhostMove(Side.top, ghost))
                        {
                            ghost.MoveUp(ClientRectangle);
                        }
                        Invalidate(ghost.GetFrame());
                        break;

                    }
                default:
                    break;

            }


        }

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			TimerTickCount++;

			if (TimerTickCount % 2 == 0) 
			{
				//HandleLatestKey();
                
                // Check whether pacman has run into one of the ghosts
                CheckGhost(blinky);
                CheckGhost(pinky);
                CheckGhost(clyde);
                CheckGhost(inky);
			}

            if (TimerTickCount % 10 == 0)
            {
                GhostMove(blinky);

                GhostMove(pinky);

                GhostMove(clyde);

                GhostMove(inky);
            }

            if (TimerTickCount % 2 == 0)
            {
                
                if (dec == 72)
                {
                    direct = "Right";
                    richTextBox1.Text += "R";
                }
                else if (dec == 73)
                {
                    direct = "Left";
                    richTextBox1.Text += "L";
                }
                else if (dec == 74)
                {
                    direct = "Down";
                    richTextBox1.Text += "D";
                }
                else if (dec == 75)
                {
                    direct = "Up";
                    richTextBox1.Text += "U";
                }
               
                HandleLatestKey();

            }

            if (TimerTickCount % 25 == 0)
            {
                sp.Open();
                sp.ReadTimeout = 10000;
                dec = sp.ReadByte();
                sp.Close();

            }


			if (TimerTickCount % 50 == 0) // every 50 is one second
			{

				if (TheTime.Direction == TimeDirection.Up)
					TheSeconds++;
				else
					TheSeconds--;

				Invalidate(TheTime.GetFrame());

				if (TheSeconds == 0)
				{
                    timer1.Stop();
                    Invalidate(TheStatusMessage.GetFrame());
                    int dotsEaten = NumberOfStones - Stones.Count;
                    MessageBox.Show("Game Over\nFinal Score: " + dotsEaten);
                    LogScore(dotsEaten);
                    DisplayScores();
					m_bGameDone = true;
                    Application.Exit();
				}
            }

        }


        private String OverallScore()
        {
            int dotsEaten = NumberOfStones - Stones.Count;
            overallScore = TheSeconds + dotsEaten;
            ScoreMessage = "Final Score: " + overallScore;
            return ScoreMessage;
        }
		

        private int RandomDirection(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max); 
        }

        /* The Direction method gives each ghost a direction every time the GhostMove method is called.*/

        private int Direction(Ghost ghost)
        {
            int direction = RandomDirection(1, 4);

            if (CanGhostMove(Side.left, ghost) && ghost.prevDirection == 1)
                direction = 1;
            else
            {
                if (!CanGhostMove(Side.left, ghost) && ghost.prevDirection == 1)
                {
                    if (!CanGhostMove(Side.top, ghost))
                    {
                        if (CanGhostMove(Side.bottom, ghost))
                            direction = 3;
                        else
                            direction = 2;
                    }
                    else
                        direction = 4;
                }
            }
                
            if (CanGhostMove(Side.right, ghost) && ghost.prevDirection == 2)
                direction = 2;
            else
            {
                if (!CanGhostMove(Side.right, ghost) && ghost.prevDirection == 2)
                {
                    if (!CanGhostMove(Side.top, ghost))
                    {
                        if (CanGhostMove(Side.bottom, ghost))
                            direction = 3;
                        else
                            direction = 1;
                    }
                    else
                        direction = 4;
                }
            }

            if (CanGhostMove(Side.bottom, ghost) && ghost.prevDirection == 3)
                direction = 3;
            else
            {
                if (!CanGhostMove(Side.bottom, ghost) && ghost.prevDirection == 3)
                {
                    if (!CanGhostMove(Side.right, ghost))
                    {
                        if (CanGhostMove(Side.left, ghost))
                            direction = 1;
                        else
                            direction = 4;
                    }
                    else
                        direction = 2;
                }
            }

            if (CanGhostMove(Side.top, ghost) && ghost.prevDirection == 4)
                direction = 4;
            else
            {
                if (!CanGhostMove(Side.top, ghost) && ghost.prevDirection == 4)
                {
                    if (!CanGhostMove(Side.right, ghost))
                    {
                        if (CanGhostMove(Side.left, ghost))
                            direction = 1;
                        else
                            direction = 3;
                    }
                    else
                        direction = 2;
                }
            }

        ghost.prevDirection = direction;                                        // Keep track of the previous direction of each ghost
        return direction;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;

        }

        private string ReadName()
        {
            finalName = name;
            return finalName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearwindow = true;
            Application.Restart();
        }

	}
}
