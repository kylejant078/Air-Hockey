using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Air_Hockey_Game
{
    public partial class Form1 : Form
    {
        //global variables
        Rectangle player1 = new Rectangle(30, 210, 20, 20);
        Rectangle player1Top = new Rectangle(30, 209, 20, 1);
        Rectangle player1Bottom = new Rectangle(30, 231, 20, 1);
        Rectangle player1Left = new Rectangle(29, 210, 1, 20);
        Rectangle player1Right = new Rectangle(50, 210, 1, 20);
        Rectangle player2 = new Rectangle(550, 210, 20, 20);
        Rectangle player2Top = new Rectangle(550, 209, 20, 1);
        Rectangle player2Bottom = new Rectangle(550, 230, 20, 1);
        Rectangle player2Left = new Rectangle(549, 210, 1, 20);
        Rectangle player2Right = new Rectangle(570, 210, 1, 20);
        Rectangle ball = new Rectangle(290, 210, 20, 20);
        Rectangle hockeyTop = new Rectangle(0, 70, 600, 290);
        Rectangle leftGoal = new Rectangle(1, 180, 9, 80);
        Rectangle rightGoal = new Rectangle(590, 180, 9, 80);
        int player1Score = 0;
        int player2Score = 0;
        int playerSpeed = 5;
        int ballXSpeed = -8;
        int ballYSpeed = 8;
        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool aDown = false;
        bool dDown = false;
        bool leftDown = false;
        bool rightDown = false;
        //Create graphics tools to be used later for graphic making in the paint event
        Pen redPen = new Pen(Color.Red, 5);
        Pen bluePen = new Pen(Color.Blue, 5);
        Pen whitePen = new Pen(Color.White, 3);
        Pen greenPen = new Pen(Color.LawnGreen, 3);
        SolidBrush darkRedBrush = new SolidBrush(Color.DarkRed);
        SolidBrush darkBlueBrush = new SolidBrush(Color.DarkBlue);
        SolidBrush darkGreenBrush = new SolidBrush(Color.ForestGreen);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        //Create Soundplayers to be used later
        SoundPlayer playerHit = new SoundPlayer();
        SoundPlayer playerScore = new SoundPlayer();
        SoundPlayer playerWin = new SoundPlayer();

        public Form1()
        {
            InitializeComponent();
            //Set up sounds with proper soundplayer
            playerHit = new SoundPlayer(Properties.Resources.airHockeyHit);
            playerScore = new SoundPlayer(Properties.Resources.pongScoreSound);
            playerWin = new SoundPlayer(Properties.Resources.winSound);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //create key down events
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //create key up events
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //create local variables to hold the current values of the globaly created rectangles
            int player1X = player1.X;
            int player1Y = player1.Y;
            int player2X = player2.X;
            int player2Y = player2.Y;
            int ballX = ball.X;
            int ballY = ball.Y;
            //move ball
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;
            //move players
            if (wDown == true && player1.Y > 70)
            {
                player1.Y -= playerSpeed;
            }
            if (sDown == true && player1.Y < 340)
            {
                player1.Y += playerSpeed;
            }
            if (aDown == true && player1.X > 0)
            {
                player1.X -= playerSpeed;
            }
            if (dDown == true && player1.X < 280)
            {
                player1.X += playerSpeed;
            }
            if (upArrowDown == true && player2.Y > 70)
            {
                player2.Y -= playerSpeed;
            }
            if (downArrowDown == true && player2.Y < 340)
            {
                player2.Y += playerSpeed;
            }
            if (leftDown == true && player2.X > 300)
            {
                player2.X -= playerSpeed;
            }
            if (rightDown == true && player2.X < 580)
            {
                player2.X += playerSpeed;
            }
            //Modify and calculate the positions of the globaly created rectangles "HitBoxes" surrounding each 'player' rectangle
            //HitBoxes are defined to be 5 pixels away from each corner in order to approximate the circle graphic of the players
            player1Top = new Rectangle(player1.X + 5, player1.Y, 10, 1);
            player1Bottom = new Rectangle(player1.X + 5, player1.Y + player1.Height, 10, 1);
            player1Left = new Rectangle(player1.X, player1.Y + 5, 1, 10);
            player1Right = new Rectangle(player1.X + player1.Width, player1.Y + 5, 1, 10);
            player2Top = new Rectangle(player2.X + 5, player2.Y, 10, 1);
            player2Bottom = new Rectangle(player2.X + 5, player2.Y + player2.Height, 10, 1);
            player2Left = new Rectangle(player2.X, player2.Y + 5, 1, 10);
            player2Right = new Rectangle(player2.X + player2.Width, player2.Y + 5, 1, 10);
            //ball colliion with top and bottom limits of the arena
            if (ball.Y < 70)
            {
                ballYSpeed *= -1;
                ball.Y = 70;
            }
            else if (ball.Y > hockeyTop.Height + 70 - ball.Height)
            {
                ballYSpeed *= -1;
                ball.Y = hockeyTop.Height + 70 - ball.Height;
            }
            //ball collision with left and right limits of the arena
            if (ball.X < 0)
            {
                ballXSpeed *= -1;
                ball.X = 0;
            }
            else if (ball.X > hockeyTop.Width - ball.Width)
            {
                ballXSpeed *= -1;
                ball.X = hockeyTop.Width - ball.Width;
            }
            //ball collisions with player 1 and checking to see if the ball direction is approaching from the opposite direction
            //To avoid the ball from having a double collison with the player HitBoxes
            if (player1Left.IntersectsWith(ball) && ballXSpeed > 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballXSpeed *= -1;
                ball.X = player1.X - ball.Width - 1;
            }
            else if (player1Right.IntersectsWith(ball) && ballXSpeed < 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballXSpeed *= -1;
                ball.X = player1.X + ball.Width + 1;
            }
            else if (player1Top.IntersectsWith(ball) && ballYSpeed > 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballYSpeed *= -1;
                ball.Y = player1.Y - ball.Height - 1;
            }
            else if (player1Bottom.IntersectsWith(ball) && ballYSpeed < 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballYSpeed *= -1;
                ball.Y = player1.Y + ball.Height + 1;
            }
            //ball collisions with player 2 and checking to see if the ball direction is approaching from the opposite direction
            //To avoid the ball from having a double collison with the player HitBoxes
            if (player2Left.IntersectsWith(ball) && ballXSpeed > 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballXSpeed *= -1;
                ball.X = player2.X - ball.Width -1;
            }
            else if (player2Right.IntersectsWith(ball) && ballXSpeed < 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballXSpeed *= -1;
                ball.X = player2.X + ball.Width +1;
            }
            else if (player2Top.IntersectsWith(ball) && ballYSpeed > 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballYSpeed *= -1;
                ball.Y = player2.Y - ball.Height;
            }
            else if (player2Bottom.IntersectsWith(ball) && ballYSpeed < 0)
            {
                playerHit.Stop();
                playerHit.Play();
                ballYSpeed *= -1;
                ball.Y = player2.Y + ball.Height;
            }
            //check for point scored
            if (ball.IntersectsWith(leftGoal))
            {
                playerScore.Play();
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";
                ball.X = 290;
                ball.Y = 210;
                ballXSpeed = -8;
                player1.X = 30;
                player1.Y = 210;
                player2.X = 550;
                player2.Y = 210;
            }
            else if (ball.IntersectsWith(rightGoal))
            {
                playerScore.Play();
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";
                ball.X = 290;
                ball.Y = 210;
                ballXSpeed = 8;
                player1.X = 30;
                player1.Y = 210;
                player2.X = 550;
                player2.Y = 210;
            }
            //check for game over
            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 1 Wins!!";
                playerWin.Play();
            }
            else if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 2 Wins!!";
                playerWin.Play();
            }
            Refresh();
        } 

        private void Form1_Paint(object sender, PaintEventArgs e)
        {   
            //Create graphics to be used as decoration or as functional items
            e.Graphics.FillRectangle(darkRedBrush, leftGoal);
            e.Graphics.FillRectangle(darkBlueBrush, rightGoal);
            e.Graphics.DrawRectangle(whitePen, hockeyTop);
            e.Graphics.DrawEllipse(whitePen, 260, 180, 80, 80);
            e.Graphics.DrawLine(redPen, 300, 70, 300, 360);
            e.Graphics.DrawArc(whitePen, -40, 180, 80, 80, 270, 180);
            e.Graphics.DrawArc(whitePen, 560, 180, 80, 80, 90, 180);
            e.Graphics.FillEllipse(darkRedBrush, player1);
            e.Graphics.DrawEllipse(redPen, player1);
            e.Graphics.FillEllipse(darkBlueBrush, player2);
            e.Graphics.DrawEllipse(bluePen, player2);
            e.Graphics.FillEllipse(darkGreenBrush, ball);
            e.Graphics.DrawEllipse(greenPen, ball);
        }
    }
}
