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
using System.Text;


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

        public System.Windows.Forms.Timer timer1;                      // Create a timer
		private TimerDisplay TheTime = new TimerDisplay(20, 290);       // Display the time ticking down
		
		private System.ComponentModel.IContainer components;
		private Thread oThread = null;

		private Maze TheMaze  = new Maze();                             // Initialize the maze
		private bool m_bGameDone = false;                               // Game is not over yet
		private GameMessage TheStatusMessage = new GameMessage(150, 10);// 
        public bool clearwindow = false;
        
		public enum Side  {top = 0, left = 1, bottom = 2, right = 3};


        // Initialize Eater
        private Eater TheEater = new Eater(100, 100);

        // Initialize ghosts
        private Ghost blinky = new Ghost(150, 150, "Ghost_Blinky.gif"); // Blinky is the red ghost
        private Ghost pinky = new Ghost(190, 190, "pinky.gif");         // Pinky is the pink ghost
        private Ghost clyde = new Ghost(220, 220, "clyde.gif");         // Clyde is the orange ghost
        private Ghost inky = new Ghost(240, 240, "pacmaninky.gif");           // Inky is the blue ghost                                     // Initialize textbox where user enters name                                       // Initialize restart button

        public string name = "";
        public ListBox listBox1;
        private RichTextBox richTextBox1;                                     // Hold user name as user enters characters
        
        private static string finalName = "";                          // Hold final user name

        // Paths needed to organize the top scores
        private string path = "top_scores.txt";
        private string path2 = "top_scores_temp.txt";

        // Holds the user score as a string
        private string myscore = "";

        // An array holding the top scores
        private string[] topblock;

        // Variables holding the values of the top scores
        private int top1val = -1;
        private int top2val = -1;
        private int top3val = -1;

        // Holds the top scores in string form
        private string blockString;
        private string blockString2;
        private string blockString3;

        // Keeps track of the end of a block for string processing
        private int dashes1 = 0;
        private int dashes2 = 0;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            InitializeForm();

            // Sets the size of the maze
            Maze.kDimension = 10;

            // Sets the size of the cells
            Cell.kCellSize = 28;

            // The number of Stones, the location of the score and stone message all depend on the size of the maze
            NumberOfStones = (Maze.kDimension - 5) * (Maze.kDimension - 5);
            TheScore = new Score(((Maze.kDimension * 2/3) * (Cell.kCellSize + Cell.kPadding)), 10);
            TheStoneMessage = new GameMessage(((Maze.kDimension * 3/4) * (Cell.kCellSize + Cell.kPadding)), 10);

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
			//SetBounds(10, 10, (Maze.kDimension + 4) * Cell.kCellSize + Cell.kPadding, (Maze.kDimension + 4) * Cell.kCellSize + Cell.kPadding + 5);
            Refresh();

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
             
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        public void InitializeForm()
        {
            BeginForm subform = new BeginForm(this);
            subform.Show();
        }


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
            if (!File.Exists(path))
            {
                // Create the file.
                using (FileStream fs = File.Create(path))
                {
                }
            }

            if (score >= 10)
                myscore = score + "";                                // Take in the score and convert it to a string

            if (score < 10)                                         // Convert a one-digit score to a two-digit score
                myscore = "0" + score;

            ReadTopScores();

            if (top1val == -1 && top2val == -1 && top3val == -1)                      // If we start with no top scores
            {
                using (StreamWriter w = File.AppendText("top_scores.txt"))         // Automatically add to top scores
                {
                    Log(myscore, w);
                    w.Close();
                }
            }

            if (top1val != -1 && top2val == -1 && top3val == -1)                      // If there's only one top score
            {
                    using (StreamWriter w = File.AppendText("top_scores.txt"))
                    {
                        if (score <= top1val)
                        {
                            Log(myscore, w);                                            // Just add the user score
                        }
                        else
                        {
                            File.Delete("top_scores.txt");

                            Log(myscore, w);                                            // Write the user score

                            for (int i = 0; i < (dashes1 + 1); i++)
                            {
                                w.WriteLine(topblock[i]);                               // Write down the highest score
                            }
                        }
                    }
            }

            if (top1val != -1 && top2val != -1 && top3val == -1)                        // If there is no third top score
            {
                if (score <= top2val)
                {
                    using (StreamWriter w = File.AppendText("top_scores.txt"))
                    {
                        Log(myscore, w);
                        w.Close();
                    }
                }
                else
                    if (score > top2val && score < top1val)
                    {
                        File.Delete("top_scores.txt");
                        using (StreamWriter w = File.AppendText("top_scores.txt"))
                        {
                            for (int i = 0; i < (dashes1 + 1); i++)
                            {
                                w.WriteLine(topblock[i]);                               // Write down the highest score
                            }

                            Log(myscore, w);                                            // Write the user score

                            for (int i = dashes1 + 2; i < dashes2 + 1; i++)             // Write the second score last
                            {
                                w.WriteLine(topblock[i]);
                            }

                            w.Close();
                        }
                    }
                    else
                        if (score >= top1val)
                        {
                            File.Delete("top_scores.txt");

                            using (StreamWriter w = File.AppendText("top_scores.txt"))
                            {
                                Log(myscore, w);
                                for (int i = 0; i < dashes2 + 1; i++)
                                    w.WriteLine(topblock[i]);
                                w.Close();
                            }

                        }
            }

            // The following will sort scores, assuming there are already three top scores
            
            if (top1val != -1 && top2val != -1 && top3val != -1)
            {
                if (score >= top1val)
                {
                    File.Delete("top_scores.txt");

                    using (StreamWriter w = File.AppendText("top_scores.txt"))
                    {
                        Log(myscore, w);

                        for (int i = 0; i < dashes2; i++)
                        {
                            w.WriteLine(topblock[i]);                                   // Write the next top two scores
                        }
                        w.Close();
                    }


                }
                // Else add it to the end of the top scores file
                else
                {
                    if (score < top1val && score >= top2val)                             // If your score is between the 1st and 2nd
                    {
                        File.Delete("top_scores.txt");

                        using (StreamWriter w = File.AppendText("top_scores.txt"))
                        {
                            for (int i = 0; i < (dashes1 + 1); i++)
                            {
                                w.WriteLine(topblock[i]);                               // Write down the highest score
                            }

                            Log(myscore, w);                                            // Then write the second highest (what the user just got)

                            for (int i = (dashes1 + 1); i < dashes2 + 1; i++)          // Then write the third highest
                            {
                                w.WriteLine(topblock[i]);
                            }

                            w.Close();
                        }
                    }
                    else
                        if (score < top2val && score > top3val)                         // If your score is between the 2nd and 3rd
                        {
                            File.Delete("top_scores.txt");

                            using (StreamWriter w = File.AppendText("top_scores.txt"))
                            {
                                for (int i = 0; i < dashes2 + 1; i++)
                                {
                                    w.WriteLine(topblock[i]);                               // Write down the two highest scores
                                }

                                Log(myscore, w);                                            // Followed by the score just logged
                                w.Close();
                            }
                        }
                        else
                            if (score == top3val)                                               // If the score equals the 3rd highest score
                            {
                                File.Delete("top_scores.txt");

                                using (StreamWriter w = File.AppendText("top_scores.txt"))
                                {
                                    foreach (string i in topblock)
                                    {
                                        w.WriteLine(i);                               // Write down the three highest scores
                                    }

                                    Log(myscore, w);                                            // Followed by the score just logged
                                    w.Close();
                                }

                            }
                    }
            }
            
            // Always add to the log file of all scores
            using (StreamWriter w = File.AppendText("log.txt"))             // Append the string to the specified file
            {
                Log(myscore, w);
                w.Close();                                                  // Close the writer and underlying file.
            }

            using (StreamReader r = File.OpenText("top_scores.txt"))               // Open and read the file.
            {
                DumpLog(r);
            }

            using (StreamReader r = File.OpenText("log.txt"))               // Open and read the file.
            {
                DumpLog(r);
            }

        }

        public static void Log(string logMessage, TextWriter w)
        {
            if (!File.Exists("log.txt"))
            {
                // Create the file.
                using (FileStream fs = File.Create("log.txt"))
                {
                }
            }

            w.Write("\r\nTime : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
            w.WriteLine("\n Name: " + finalName + "." + " Score : {0} ", logMessage);
            w.WriteLine ("-------------------------------");
            w.Flush();                                                      // Update the underlying file.
        }

        // Processes the top scores to get the previous top three scores
        public void ReadTopScores()
        {
            topblock = File.ReadAllLines("top_scores.txt");                 // First read all lines into an array
            foreach (string i in topblock)
                blockString += i;
            dashes1 = Array.IndexOf(topblock, "-------------------------------");

            if (dashes1 == -1)                                              // If no entries are found
            {
                return;
            }

            // Read the highest score
            int index1 = blockString.IndexOf("Score : ") + 8;
            string stringOne = blockString.Substring(index1, 2);
            top1val = Convert.ToInt16(stringOne);

            // Read the second highest score
            blockString2 = blockString.Substring(index1 + 1);               // Cut the first top score out of the string
            int index2 = blockString2.IndexOf("Score : ") + 8;

            if (index2 == 7)                                                // If there is no second top score
            {
                MessageBox.Show("No second score");
                return;
            }

            string stringTwo = blockString2.Substring(index2, 2);
            top2val = Convert.ToInt16(stringTwo);

            string[] bottomTwo = new string[topblock.Length - dashes1 - 1];
            Array.Copy(topblock, dashes1 + 1, bottomTwo, 0, topblock.Length - dashes1 - 1);  // Copy the whole array into another 
                                                                                             //  array with just the bottom two scores
            dashes2 = 1 + dashes1 + Array.IndexOf(bottomTwo, "-------------------------------");          // Find the index of the end of the second highest score

            // Read the third highest score
            blockString3 = blockString2.Substring(index2 + 2);
            int index3 = blockString3.IndexOf("Score : ") + 8;

            if (index3 == 7)                                                // If there is no third top score
            {
                MessageBox.Show("No third score");
                return;
            }

            string stringThree = blockString3.Substring(index3, 2);
            top3val = Convert.ToInt16(stringThree);

            MessageBox.Show("Dashes: " + dashes1 + " " + dashes2);
            MessageBox.Show(top1val + " " + top2val + " " + top3val, "Top Scores");
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

		public void InitializeTimer()
		{
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
            inky.Position.X = cellCenter.X - 10;
            inky.Position.Y = cellCenter.Y - 10;

            // Relocate ghost if it hits the eater or other ghosts
            while (inky.GetFrame().IntersectsWith(TheEater.GetFrame())
                || inky.GetFrame().IntersectsWith(blinky.GetFrame())
                || inky.GetFrame().IntersectsWith(pinky.GetFrame())
                || inky.GetFrame().IntersectsWith(clyde.GetFrame()))
            {
                cellCenter = GetRandomCellPosition();
                inky.Position.X = cellCenter.X - 10;
                inky.Position.Y = cellCenter.Y - 10;
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
                    //PlaySoundInThread("pacman_dies.wav");
                    m_bGameDone = true;
                    int dotsEaten = NumberOfStones - Stones.Count;
                    
                    MessageBox.Show("Game Over\nDots Eaten: " + dotsEaten);
                    LogScore(dotsEaten);
                    ScoreForm();
                    Invalidate(TheStatusMessage.GetFrame());
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Data from Training"});
            this.listBox1.Location = new System.Drawing.Point(12, 346);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(299, 95);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(325, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(247, 429);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(593, 460);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.listBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacRhody";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);

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

            //TheNameMessage.Draw(g);
	
			// draw the time

			TheTime.Draw(g, TheSeconds);

			// Draw a message indicating the status of the game
			TheStatusMessage.Draw(g);

			// Draw a message indicating dots
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

			string result = LatestKey;
			Invalidate(TheEater.GetFrame());
			switch (result)
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
                //PlaySoundInThread("hit.wav");
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
                    ScoreForm();
				}
			}

		}

		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			string result = e.KeyData.ToString();
			LatestKey = result;

            /*if (e.KeyCode == Keys.Enter)
            {
                ReadName();
                timer1.Start();
            }*/

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
                HandleLatestKey();                                          // do the key handling here
                
                // Check whether the pacman has run into any of the ghosts
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

			if (TimerTickCount % 50 == 0) // every 50 is one second
			{

				if (TheTime.Direction == TimeDirection.Up)
					TheSeconds++;
				else
					TheSeconds--;

				Invalidate(TheTime.GetFrame());

				if (TheSeconds == 0)                                        // Game is over if the time goes to 0.
				{
                    timer1.Stop();                                          // Stop the timer
                    Invalidate(TheStatusMessage.GetFrame());
                    int dotsEaten = NumberOfStones - Stones.Count;          // Count the dots eaten
                    MessageBox.Show("Game Over\nFinal Score: " + dotsEaten);// Show the final score
                    LogScore(dotsEaten);                                    // Log the user's score in the log.txt file
                    ScoreForm();
					m_bGameDone = true;                                     // Game is over.
                    
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

        public string ReadName()
        {
            finalName = name;
            return finalName;
        }

        // Form used for training
        public void TrainingForm()
        {
            Form3 subForm = new Form3(this);
            subForm.Show();
        }

        // The form showing the record of scores
        public void ScoreForm()
        {
            Scores subForm = new Scores(this);
            subForm.Show();
            subForm.Display();
        }

        // Keep this. Doesn't do anything when the selected items in the listbox changes
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /* Will diplay a message box asking the user if he or she wants to play again
           Not used in this program. Function moved to Scores form.*/
        public void EndProgram()
        {
            DialogResult result;
            result = MessageBox.Show("Play Again?", "PacRhody", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
                Application.Exit();
            if (result == DialogResult.Yes)
            {
                clearwindow = true;
                Application.Restart();
            }
        }

	}
}
