using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_2
{
    public partial class frmSnake : Form
    {
        public frmSnake()
        {
            InitializeComponent();
        }

        Graphics g;
        //This is to draw and represent the snake
        List<Point> snake = new List<Point>();
        string direction = "up";

        //This is to display the food (apples) 
        Point apples;
        
        //Initial snake position
        Point CurrentHead; 

        //Initial score
        int i_score=0;

        Brush FoodColour = Brushes.Red; //Food colour
        Brush SnakeColour = Brushes.Blue; //Snake colour

        //When user clicks on 'New Game'
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            Graphics();
            timeMoveSnake.Interval = 100;
            timeMoveSnake.Start();
            Food();
            btnNewGame.Enabled = false;      
        }

        private void Graphics()
        {
            //This is to represent the graphic of the game
            g = picGame.CreateGraphics();
            //g.DrawRectangle(Pens.Blue, 0, 0, picGame.Width-1, picGame.Height-1);

            snake.Add(new Point(251, 251));
        }

        private void MoveSnake()
        {
            CurrentHead = snake[snake.Count - 1];

            //Removes the tail of the snake when the direction is up 
            if (direction == "up")
            {
                snake.Add(new Point(CurrentHead.X, CurrentHead.Y - 10));
                g.FillRectangle(SnakeColour, CurrentHead.X, CurrentHead.Y - 10, 10, 10);
            }

            //Removes the tail of the snake when the direction is right
            if (direction == "right")
            {
                snake.Add(new Point(CurrentHead.X + 10, CurrentHead.Y));
                g.FillRectangle(SnakeColour, CurrentHead.X + 10, CurrentHead.Y, 10, 10);
            }

            //Removes the tail of the snake when the direction is left
            if (direction == "left")
            {
                snake.Add(new Point(CurrentHead.X - 10, CurrentHead.Y));
                g.FillRectangle(SnakeColour, CurrentHead.X - 10, CurrentHead.Y, 10, 10);
            }

            //Removes the tail of the snake when the direction is down
            if (direction == "down")
            {
                snake.Add(new Point(CurrentHead.X, CurrentHead.Y + 10));
                g.FillRectangle(SnakeColour, CurrentHead.X, CurrentHead.Y + 10, 10, 10);
            }

            CurrentHead = snake[snake.Count - 1];

            //If snake hits apple
            if (HitApple(CurrentHead.X, CurrentHead.Y))
            {
                i_score += 1;
                timeMoveSnake.Interval -= 5;
                Food();
                lblScore.Text = "Score =" + " " + i_score.ToString();
                ColourChange();
            }
            else
            {
                Point snakeEnd = snake[0];
                snake.RemoveAt(0);
                g.FillRectangle(Brushes.White, snakeEnd.X, snakeEnd.Y, 10, 10);
            }
        }

        private void timeMoveSnake_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            CollisionDetection(CurrentHead.X, CurrentHead.Y);
        }

        private void ProcessKeyCmd(object sender, KeyEventArgs e)
        {
            //Controls the movement of the snake
            switch (e.KeyData)
            {
                case Keys.Right: { if (direction != "left") { direction = "right"; } break; }
                case Keys.Down: { if (direction != "up") { direction = "down"; } break; }
                case Keys.Up: { if (direction != "down") { direction = "up"; } break; }
                case Keys.Left: { if (direction != "right") { direction = "left"; } break; }
            }
        }

        private void CollisionDetection(int hitX, int hitY)
        {
            //Collision with the walls
            if(hitX < 0 || hitX > picGame.Width || hitY < 0 || hitY > picGame.Height)
            {
                EndGame();
            }
            
            //Collision with itself
            for (int i = 0; i < snake.Count-1; i++)
            {
                if (hitX == snake[i].X && hitY == snake[i].Y)
                {
                    EndGame();
                }
            }
        }

        //This is to end the game
        private void EndGame()
        {
            btnNewGame.Enabled = true;
            timeMoveSnake.Stop();
            MessageBox.Show("Your score is : " + i_score.ToString(),"Game over", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            snake = new List<Point>();
            g.Clear(Color.White);
        }

        //Food
        private void Food()
        {
            Random rdm = new Random();

            apples.X = 1 + rdm.Next(0, picGame.Width / 10) * 10;
            apples.Y = 1 + rdm.Next(0, picGame.Height / 10) * 10;

            g.FillRectangle(FoodColour, apples.X, apples.Y, 10, 10);
        }

        //When snake eats an apple
        public Boolean HitApple(int hitX, int hitY)
        {
            if (apples.X == hitX && apples.Y == hitY)
            {
                return true;
            }
            return false;
        }

        //Change colour for snake and food 
        private void ColourChange()
        {
            if (SnakeColour == Brushes.Blue)
            {
                SnakeColour = Brushes.DeepPink;
                FoodColour = Brushes.Black;
            }

            else {
                SnakeColour = Brushes.Blue;
                FoodColour = Brushes.Red;
            }
        }
    }
}