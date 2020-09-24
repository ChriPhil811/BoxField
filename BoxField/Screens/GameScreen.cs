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

        //used to draw boxes on screen
        SolidBrush boxBrush = new SolidBrush(Color.White);
        SolidBrush heroBrush = new SolidBrush(Color.Red);

        //list to hold a column of boxes    
        List<Box> left = new List<Box>();
        List<Box> right = new List<Box>();

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
            //set game start values
            Box newBoxL = new Box(50, 0, 20);
            left.Add(newBoxL);
            Box newBoxR = new Box(250, 0, 20);
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
                Box newBoxL = new Box(50, 0, 20);
                left.Add(newBoxL);
                Box newBoxR = new Box(250, 0, 20);
                right.Add(newBoxR);
            }

            if(rightArrowDown == true)
            {
                hero1.Move(5);
            }
            else if(leftArrowDown == true)
            {
                hero1.Move(-5);
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach(Box b in left)
            {
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            foreach (Box b in right)
            {
                e.Graphics.FillRectangle(boxBrush, b.x, b.y, b.size, b.size);
            }

            e.Graphics.FillRectangle(heroBrush, hero1.x, hero1.y, hero1.size, hero1.size);
        }
    }
}
