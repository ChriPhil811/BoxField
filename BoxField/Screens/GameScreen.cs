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
        //player1 button control keys
        Boolean leftArrowDown, rightArrowDown;

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

        Boolean moveRight = true; //boolean for hero movement

        //variables for pattern
        int patternLength = 10;
        int patternSpeed = 10;

        //create the hero
        Hero hero1 = new Hero(150, 475, 25);

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
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
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;           
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        private void gameLoop_Tick(object sender, EventArgs e)
        {
            //update location of all boxes (drop down screen)
            foreach(Box b in left)
            {
                b.Move(10);
            }

            foreach (Box b in right)
            {
                b.Move(10);
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

            //hero movement
            if(rightArrowDown == true && hero1.x < this.Width - hero1.size)
            {
                hero1.Move(10);
            }
            else if(leftArrowDown == true && hero1.x > 0)
            {
                hero1.Move(-10);
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
                        gameLoop.Enabled = false;
                    }
                }
            }

            Refresh();
        }

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

        private int pattern()
        {
            //pattern maker
            patternLength--;
            if (patternLength == 0)
            {
                moveRight = !moveRight;
                patternLength = randNum.Next(2, 10);
                patternSpeed = randNum.Next(10, 30);
            }

            //if pattern reaches a wall, switch direction 
            //(this should prevent the pattern from going out of the screen)
            if(leftX < 0 || leftX + gap > this.Width - 20)
            {
                moveRight = !moveRight;
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
    }
}
