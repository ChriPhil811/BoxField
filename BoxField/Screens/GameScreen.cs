using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {

        #region variables

        //player1 button control keys
        Boolean aDown, dDown;

        //random num gen
        Random randNum = new Random();

        //used to draw boxes on screen
        SolidBrush boxBrush = new SolidBrush(Color.White);
        SolidBrush heroBrush = new SolidBrush(Color.Blue);

        //list to hold a column of boxes    
        List<Box> left = new List<Box>();
        List<Box> right = new List<Box>();

        //variables for boxes
        int leftX = 200;
        int gap = 200;

        public static int score = 0; //variable for tracking player score
        int timer = 0; //timer int
        int fallSpeed = 4; //box fall speed
        int heroMoveSpeed = 4; //hero move speed

        Boolean moveRight = true; //boolean for hero movement

        //variables for pattern
        int patternLength = 10;
        int patternSpeed = 10;

        //create the hero
        Hero hero1 = new Hero(150, 475, 25);

        #endregion variables

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        #region game setup

        public void OnStart()
        {
            //color for boxes and pattern
            Color c = RandomColor();
            leftX = pattern();

            //set game start values
            Box newBoxL = new Box(leftX, 0, 20, c);
            left.Add(newBoxL);
            Box newBoxR = new Box(leftX + gap, 0, 20, c);
            right.Add(newBoxR);

            score = 0;
        }

        #endregion game setup

        #region key down and up

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
            }
        }

        #endregion key down and up

        #region game loop

        private void gameLoop_Tick(object sender, EventArgs e)
        {

            #region falling box creation and removal

            //update location of all boxes (drop down screen)
            foreach (Box b in left)
            {
                b.Move(fallSpeed);
            }

            foreach (Box b in right)
            {
                b.Move(fallSpeed);
            }

            //remove box if it has gone of screen
            if (left[0].y > 500)
            {
                left.RemoveAt(0);
            }

            if (right[0].y > 500)
            {
                right.RemoveAt(0);
            }

            //add new box if it is time
            if (left[left.Count - 1].y > 20)
            {
                Color c = RandomColor(); //color for boxes
                leftX = pattern(); //pattern maker

                Box newBoxL = new Box(leftX, 0, 20, c);
                left.Add(newBoxL);
                Box newBoxR = new Box(leftX + gap, 0, 20, c);
                right.Add(newBoxR);
            }

            #endregion falling box creation and removal

            #region hero movement and collision

            //hero movement
            if (aDown == true && hero1.x > 0)
            {
                hero1.Move(-heroMoveSpeed);
            }
            if (dDown == true && hero1.x < this.Width - hero1.size)
            {
                hero1.Move(heroMoveSpeed);
            }

            //hero collision
            Rectangle heroRec = new Rectangle(hero1.x, hero1.y, hero1.size, hero1.size);

            if (left.Count >= 4)
            {
                //check bottom 4 boxes
                for (int i = 0; i < 4; i++)
                {
                    Rectangle boxRecL = new Rectangle(left[i].x, left[i].y, left[i].size, left[i].size);
                    Rectangle boxRecR = new Rectangle(right[i].x, right[i].y, right[i].size, right[i].size);

                    if (heroRec.IntersectsWith(boxRecL) || heroRec.IntersectsWith(boxRecR))
                    {
                        gameLoop.Enabled = false; //stop the game loop

                        //switch to the game over screen
                        Form f = this.FindForm();
                        f.Controls.Remove(this);
                        GameOverScreen gos = new GameOverScreen();
                        f.Controls.Add(gos);

                        gos.Focus();
                    }
                }
            }

            #endregion hero movement and collision

            #region score tracking

            timer++;

            if(timer == 20)
            {
                score++; //increase score by 1
                scoreLabel.Text = "Score: " + score; //display the score
                timer = 0; //set timer back to zero
            }

            if(score % 25 == 0 && timer == 0)
            {
                fallSpeed += 2;
                heroMoveSpeed += 2;
            }
            
            #endregion score tracking

            Refresh();
        }

        #endregion game loop

        #region paint graphics

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach(Box b in left)
            {
                boxBrush.Color = b.color;
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            foreach (Box b in right)
            {
                boxBrush.Color = b.color;
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            e.Graphics.FillRectangle(heroBrush, hero1.x, hero1.y, hero1.size, hero1.size);
        }

        #endregion paint graphics

        #region random color gen

        private Color RandomColor()
        {
            int randColor = randNum.Next(1, 5); //random number for color choice
            Color c = Color.White; //initialize color

            //switch for color picking
            switch (randColor)
            {
                case (1):
                    c = Color.Red;
                    break;

                case (2):
                    c = Color.OrangeRed;
                    break;

                case (3):
                    c = Color.Orange;
                    break;

                case (4):
                    c = Color.Yellow;
                    break;
            }

            return (c);
        }

        #endregion random color gen

        #region pattern stuff

        private int pattern()
        {
            //pattern maker
            patternLength--;
            if (patternLength == 0)
            {
                moveRight = !moveRight;
                patternLength = randNum.Next(2, 10);
                patternSpeed = randNum.Next(10, 25);
            }

            //if pattern reaches a wall, switch direction 
            //(this should prevent the pattern from going out of the screen)
            if(leftX < 0)
            {
                moveRight = true;
                patternLength = 10;
                patternSpeed = 30;
            }
            if(leftX + gap > this.Width - 20)
            {
                moveRight = false;
                patternLength = 10;
                patternSpeed = 30;
            }

            //pattern speed adjustment
            if (moveRight)
            {
                leftX += patternSpeed;
            }
            else
            {
                leftX -= patternSpeed;
            }

            return (leftX);
        }

        #endregion pattern stuff

    }
}
