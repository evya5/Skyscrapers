using System;
using System.Linq;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;

namespace Skyscrapers
{
    /// <summary>
    /// The form1 class.
    /// </summary>
    public partial class Form1 : Form
    {
        // Constants Labels text
        private const string TitleLabelText = "SKYSCRAPERS";
        private const string AllRightsLabelText = "© 2021 All Rights Reserved.";
        // Constants Buttons text
        private const string RulesButtonText = "Rules";
        private const string StartButtonText = "Start";
        private const string NewGameButtonText = "New Game";
        private const string ResetButtonText = "Reset";
        private const string HintButtonText = "Hint";
        private const string SolveButtonText = "Solve";
        private const string ExitButtonText = "Exit";
        // Constants messages
        private const string RulesMessage =
            "The rules are simple.\n" +
            "The objective is to place skyscrapers in all cells on the grid according to the rules:\n" +
            "The height of the skyscrapers is from 1 to the size of the grid i.e. 1 to 4 for a 4x4 puzzle.\n" +
            "You cannot have two skyscrapers with the same height on the same row or column.\n" +
            "The numbers on the sides of the grid indicate how many skyscrapers would you see if you look at the row placed in front of the number.\n" +
            "Place numbers in each cell to indicate the height of the skyscrapers.\n" +
            "Have Fun! :)";
        private const string EndGameMessage =
            "Congratulations! You finished the puzzle!";
        private const string SolvedByComputerMessage =
            "Puzzle was solved by the Computer!";
        private const string ComputerCouldNotSolveMessage =
            "There is not an available solution for this specific puzzle!";
        // Constants colors
        private Color TitleColor = Color.Fuchsia;
        private Color TimerColor = Color.DarkMagenta;
        private Color EdgesColor = Color.Orchid;
        private Color ChosenCellColor = Color.Gray;
        private Color ResetCellColor = Color.Pink;

        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        private Board Board { get; set; }
        private System.Timers.Timer GameClock;
        private int m, s;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form1_S the load.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Board = new Board(Board.BoardSize);
            GameClock = new System.Timers.Timer();
            InitClock();
            InitWindow();
        }

        /// <summary>
        /// Inits the window.
        /// </summary>
        private void InitWindow()
        {
            InitLabels();
            InitButtons();
            ResetCellsTextAndColor();
            ResetEdegesLabels();
            ResetCellChoice();
        }

        /// <summary>
        /// Inits the labels.
        /// </summary>
        private void InitLabels()
        {
            labelSkyScrapers.ForeColor = TitleColor;
            labelSkyScrapers.Text = TitleLabelText;
            allRightsLabel.Text = AllRightsLabelText;
        }

        /// <summary>
        /// Inits the buttons.
        /// </summary>
        private void InitButtons()
        {
            buttonRules.Text = RulesButtonText;
            buttonStart.Text = StartButtonText;
            buttonNewGame.Text = NewGameButtonText;
            buttonReset.Text = ResetButtonText;
            buttonHint.Text = HintButtonText;
            buttonSolve.Text = SolveButtonText;
            buttonExit.Text = ExitButtonText;
        }

        /// <summary>
        /// Inits the clock.
        /// </summary>
        private void InitClock()
        {
            /* This function resests the current Game clock and creates an interval call
             * for the the UpdateTime function.
             */
            displayClock.BackColor = TimerColor;
            displayClock.Text = "00:00";
            // Which function to remove from timer:
            GameClock.Elapsed -= UpdateTime;
            // Time between calls:
            GameClock.Interval = 1000;
            // Which function to add to timer:
            GameClock.Elapsed += UpdateTime;
            // Turn clock to 0 minutes and 0 seconds:
            m = 0; s = 0;
            // Stops Gameclock:
            GameClock.Stop();

        }

        /// <summary>
        /// button2_S the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(RulesMessage);
        }

        /// <summary>
        /// label1_S the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void label1_Click(object sender, EventArgs e)
        {
            ResetCellChoice();
            // Casting object to Label:
            Label chosenCell = sender as Label;
            // Change the color of the clicked cell:
            chosenCell.BackColor = ChosenCellColor;
        }

        /// <summary>
        /// Resets the cell choice.
        /// </summary>
        private void ResetCellChoice()
        {
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (cell.Name.StartsWith("cell"))
                {
                    cell.BackColor = ResetCellColor;
                }
            }
        }

        /// <summary>
        /// Clicks the start button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickStartButton(object sender, EventArgs e)
        {
            EnableCells(true);

            ResetCellChoice();
            // Enables all control buttons:
            buttonStart.Enabled = false;
            buttonNewGame.Enabled = true;
            buttonReset.Enabled = true;
            buttonHint.Enabled = true;
            buttonSolve.Enabled = true;
            // Enables clicks on board cells:
            guiPanel.Enabled = true;
            // Starts timer:
            GameClock.Start();
            // Updates the text in the hint button with the amount of hints left:
            buttonHint.Text = Board.UpdateHintButtonText();
            // Reveals the edges:
            Setedges(sender, e);
        }

        /// <summary>
        /// Updates the time.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void UpdateTime(object sender, ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s += 1;
                if (s == 60)
                {
                    s = 0;
                    m += 1;
                }
                if (m == 100)
                {
                    m = 0;
                }
                // Updates the clock on the screen, using format 00:00 :
                displayClock.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
            }));
        }

        /// <summary>
        /// Set the edges.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Setedges(object sender, EventArgs e)
        {
            foreach (var edge in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (edge.Name.StartsWith("edge"))
                {
                    // Use edge's name to find which edge to relate to:
                    int row = edge.Name[4] - 48;
                    int col = edge.Name[6] - 48;
                    edge.Text = Board.GetEdges()[row, col].ToString();
                }
            }
        }


        /// <summary>
        /// Form1_S the key press.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {   // if key between 1-4 (in ASCII) pressed (in this case, 4x4 board)
            if (e.KeyChar >= 49 && e.KeyChar <= 48 + Board.GetSolvingdBoard().GetLength(0))
            {
                foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
                {   // Checks where is the chosen cell using its color, and update its text
                    if (cell.Name.StartsWith("cell") && cell.BackColor == ChosenCellColor)
                    {
                        cell.Text = e.KeyChar.ToString();
                        Board.UpdateSolvingBoard(cell.Name, cell.Text);
                        if (Board.CheckSolution())
                        {
                            GameFinishedActions();
                        }
                    }
                }
            }
            else // Different keyboard button pressed - clear chosen cell.
            {
                foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
                {
                    if (cell.Name.StartsWith("cell") && cell.BackColor == ChosenCellColor)
                    {
                        cell.Text = "";
                        Board.UpdateSolvingBoard(cell.Name, cell.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Games the finished actions.
        /// </summary>
        private void GameFinishedActions()
        {
            ConfigEndGameButtons(); // Changing the buttons to disable to press (reset, hint, solve)
            ResetCellChoice(); // Clears cells choice
            GameClock.Stop(); // Stops highscore
            MessageBox.Show(EndGameMessage);
        }

        /// <summary>
        /// Configs the end game buttons.
        /// </summary>
        private void ConfigEndGameButtons()
        {   // Changing the buttons to disable to press (reset, hint, solve)
            guiPanel.Enabled = false;
            buttonStart.Enabled = false;
            buttonReset.Enabled = false;
            buttonHint.Enabled = false;
            buttonSolve.Enabled = false;

        }

        /// <summary>
        /// Clickeds the exit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickedExit(object sender, EventArgs e)
        {
            GameClock.Stop();
            Close();
        }

        /// <summary>
        /// Clickeds the hint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickedHint(object sender, EventArgs e)
        {
            ResetCellChoice();
            // get row,col of the hinted cell using Board.GetHint:
            (int, int) cell_index = Board.GetHint();
            // Creates string that represents the ending of the cell name e.g: "2x3":
            string ending = cell_index.Item1.ToString() + "x" + cell_index.Item2.ToString();
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (cell.Name.StartsWith("cell") && cell.Name.EndsWith(ending))
                {   // Using the hinted cell index (row,col) to update the text in the cell (
                    // (using the updated currently solving board):
                    cell.Text = Board.GetSolvingdBoard()[cell_index.Item1, cell_index.Item2].ToString();
                    cell.Enabled = false;
                    buttonHint.Text = Board.UpdateHintButtonText();
                }
            }
            if (Board.GetCountHints() == 0)
            {   // NO MORE HINTS AVAILABLE
                buttonHint.Enabled = false;
            }
            if (Board.CheckSolution())
            {   // HINT SOLVED THE PUZZLE!
                GameFinishedActions();
            }



        }

        /// <summary>
        /// Clickeds the reset.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickedReset(object sender, EventArgs e)
        {
            ResetCellChoice();
            Board.BackToResetBoard();
            ResetCellsTextAndColor();
        }

        /// <summary>
        /// Resets the cells text and color.
        /// </summary>
        private void ResetCellsTextAndColor()
        {
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (cell.Name.StartsWith("cell"))
                {   // Find the index of the cell using its name in the GUI:
                    int row = cell.Name[4] - 48;
                    int col = cell.Name[6] - 48;
                    cell.BackColor = ResetCellColor;
                    // Cell is not empty:
                    if (Board.GetResetBoard()[row, col] == 0)
                    {
                        cell.Text = "";
                    }
                    else
                    {
                        // Cell is taken by a hint:
                        cell.Text = Board.GetResetBoard()[row, col].ToString();
                    }

                }
            }
        }

        /// <summary>
        /// Enables the cells.
        /// </summary>
        /// <param name="enable">If true, enable.</param>
        private void EnableCells(bool enable)
        {
            // Enables clicks of the board cells
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {

                if (cell.Name.StartsWith("cell"))
                {
                    cell.Enabled = enable;
                }
            }
        }

        /// <summary>
        /// Resets the edeges labels.
        /// </summary>
        private void ResetEdegesLabels()
        {
            foreach (var edge in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (edge.Name.StartsWith("edge"))
                {
                    edge.BackColor = EdgesColor;
                    edge.Text = "";
                }
            }
        }

        /// <summary>
        /// Clickeds the new game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickedNewGame(object sender, EventArgs e)
        {
            Board = new Board(4);
            ClickedReset(sender, e);
            ResetEdegesLabels();
            EnableCells(false);
            ConfigNewGameButtons();
            InitClock();

        }


        /// <summary>
        /// Clickeds the solve.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ClickedSolve(object sender, EventArgs e)
        {
            ClickedReset(sender,e);
            if (Board.SolveSkyScrapers(0))
            {
                for (int i = 0; i < Board.GetSolvingdBoard().GetLength(0); i++)
                {
                    for (int j = 0; j < Board.GetSolvingdBoard().GetLength(1); j++)
                    {
                        string ending = i + "x" + j;
                        foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
                        {
                            if (cell.Name.StartsWith("cell") && cell.Name.EndsWith(ending))
                            {
                                cell.Text = Board.GetSolveddBoard()[i, j].ToString();
                                System.Threading.Thread.Sleep(100);
                                guiPanel.Refresh();
                            }
                            
                        }
                    }
                }
                SolvedActions();
            }
            else
                MessageBox.Show(ComputerCouldNotSolveMessage);
        }

        /// <summary>
        /// Solveds the actions.
        /// </summary>
        private void SolvedActions()
        {
            EnableCells(false);
            ConfigEndGameButtons();
            GameClock.Stop();
            MessageBox.Show(SolvedByComputerMessage);

        }
        /// <summary>
        /// Configs the new game buttons.
        /// </summary>
        private void ConfigNewGameButtons()
        {
            buttonHint.Text = HintButtonText;
            buttonHint.Enabled = false;
            buttonReset.Enabled = false;
            buttonSolve.Enabled = false;
            buttonStart.Enabled = true;
            buttonExit.Enabled = true;
            buttonRules.Enabled = true;
        }
    }
}
