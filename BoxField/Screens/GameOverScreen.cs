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
    public partial class GameOverScreen : UserControl
    {
        public GameOverScreen()
        {
            InitializeComponent();
            scoreLabel.Text = "Final Score: " + GameScreen.score;
        }

        private void GameOverScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    GameScreen gs = new GameScreen();
                    f.Controls.Add(gs);
                    gs.Focus();
                    break;
                case Keys.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
