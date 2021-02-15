using System;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace Skyscrapers
{
    public partial class Form1 : Form
    {
        private Board Board { get; set; }
        private System.Timers.Timer GameClock;
        private int m, s;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Board = new Board(Board.BoardSize);
            GameClock = new System.Timers.Timer();
            InitClock();
            InitWindow();
            highScore.Text = Board.GetHighScore().Item1;
            highScoreName.Text = Board.GetHighScore().Item2;
        }

        private void InitWindow()
        {
            InitLabels();
            InitButtons();
            ResetCellsTextAndColor();
            ResetEdegesLabels();
            ResetCellChoice();
        }

        private void InitLabels()
        {
            labelSkyScrapers.ForeColor = Board.TitleColor;
            labelSkyScrapers.Text = Board.TitleLabelText;
            allRightsLabel.Text = Board.AllRightsLabelText;
            highScoreTitle.Text = Board.HighscoreLabelText;
            madeByTitle.Text = Board.MadeByLabelText;
        }

        private void InitButtons()
        {
            buttonMainMenu.Text = Board.MainMenuButtonText;
            buttonStart.Text = Board.StartButtonText;
            buttonNewGame.Text = Board.NewGameButtonText;
            buttonReset.Text = Board.ResetButtonText;
            buttonHint.Text = Board.HintButtonText;
            buttonSolve.Text = Board.SolveButtonText;
            buttonExit.Text = Board.ExitButtonText;
        }

        private void InitClock()
        {
            /* This function resests the current Game clock and creates an interval call
             * for the the UpdateTime function.
             */
            displayClock.BackColor = Board.TimerColor;
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

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Construction area...");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ResetCellChoice();
            // Casting object to Label:
            Label chosenCell = sender as Label;
            // Change the color of the clicked cell:
            chosenCell.BackColor = Board.ChosenCellColor;
        }

        private void ResetCellChoice()
        {
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (cell.Name.StartsWith("cell"))
                {
                    cell.BackColor = Board.ResetCellColor;
                }
            }
        }

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


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {   // if key between 1-4 (in ASCII) pressed (in this case, 4x4 board)
            if (e.KeyChar >= 49 && e.KeyChar <= 48 + Board.GetSolvingdBoard().GetLength(0))
            {
                foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
                {   // Checks where is the chosen cell using its color, and update its text
                    if (cell.Name.StartsWith("cell") && cell.BackColor == Board.ChosenCellColor)
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
                    if (cell.Name.StartsWith("cell") && cell.BackColor == Board.ChosenCellColor)
                    {
                        cell.Text = "";
                        Board.UpdateSolvingBoard(cell.Name, cell.Text);
                    }
                }
            }
        }

        private void GameFinishedActions()
        {
            ConfigEndGameButtons(); // Changing the buttons to disable to press (reset, hint, solve)
            ResetCellChoice(); // Clears cells choice
            GameClock.Stop(); // Stops highscore
            if (Board.CheckHighScore(displayClock.Text) && Board.DidNotUsedHints())
            {
                highScore.Text = displayClock.Text;
                highScoreName.Text = "";
                highScoreName.Enabled = true;
                highScoreName.ReadOnly = false;
                submit.Visible = true;
                MessageBox.Show(Board.NewHighScoreEndGameMessage);
            }
            else
                MessageBox.Show(Board.EndGameMessage);
        }

        private void ConfigEndGameButtons()
        {   // Changing the buttons to disable to press (reset, hint, solve)
            guiPanel.Enabled = false;
            buttonStart.Enabled = false;
            buttonReset.Enabled = false;
            buttonHint.Enabled = false;
            buttonSolve.Enabled = false;

        }

        private void ClickedExit(object sender, EventArgs e)
        {
            GameClock.Stop();
            Close();
        }

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

        private void ClickedReset(object sender, EventArgs e)
        {
            ResetCellChoice();
            Board.BackToResetBoard();
            ResetCellsTextAndColor();
        }

        private void ResetCellsTextAndColor()
        {
            foreach (var cell in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (cell.Name.StartsWith("cell"))
                {   // Find the index of the cell using its name in the GUI:
                    int row = cell.Name[4] - 48;
                    int col = cell.Name[6] - 48;
                    cell.BackColor = Board.ResetCellColor;
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

        private void ResetEdegesLabels()
        {
            foreach (var edge in guiPanel.Controls.OfType<Label>().ToList())
            {
                if (edge.Name.StartsWith("edge"))
                {
                    edge.BackColor = Board.EdgesColor;
                    edge.Text = "";
                }
            }
        }

        private void ClickedNewGame(object sender, EventArgs e)
        {
            Board = new Board(4);
            ClickedReset(sender, e);
            ResetEdegesLabels();
            EnableCells(false);
            ConfigNewGameButtons();
            InitClock();

        }

        private void Submit_Click(object sender, EventArgs e)
        {
            highScoreName.Enabled = false;
            highScoreName.ReadOnly = true;
            if (highScoreName.Text == "")
                highScoreName.Text = Board.DefaultSubmitName;
            (string, string) new_high_score = (displayClock.Text, highScoreName.Text);
            Board.UpdateHighScore(new_high_score);
            submit.Visible = false;
        }

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
                                cell.Text = "1";
                                guiPanel.Refresh();
                                cell.Text = "2";
                                guiPanel.Refresh();
                                cell.Text = "3";
                                guiPanel.Refresh();
                                cell.Text = "4";
                                guiPanel.Refresh();
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
                MessageBox.Show(Board.ComputerCouldNotSolveMessage);
        }

        private void SolvedActions()
        {
            EnableCells(false);
            ConfigEndGameButtons();
            GameClock.Stop();
            MessageBox.Show(Board.SolvedByComputerMessage);

        }
        private void ConfigNewGameButtons()
        {
            buttonHint.Text = Board.HintButtonText;
            buttonHint.Enabled = false;
            buttonReset.Enabled = false;
            buttonSolve.Enabled = false;
            buttonStart.Enabled = true;
            buttonExit.Enabled = true;
            buttonMainMenu.Enabled = true;
        }
    }
}
