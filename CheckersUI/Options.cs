using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckersUI
{
    public partial class Options : Form
    {
        public Options(bool humanStart, int difficulty)
        {
            InitializeComponent();

            HumanStart = humanStart;
            Difficulty = difficulty;
        }

        public bool HumanStart
        {
            get
            {
                return rdHuman.Checked;
            }
            set
            {
                rdHuman.Checked = value;
                rdComputer.Checked = !value;
            }
        }

        public int Difficulty
        {
            get
            {
                if (rdEasy.Checked)
                    return 2;
                else if (rdMedium.Checked)
                    return 3;
                else if (rdHard.Checked)
                    return 4;
                else
                    return 5;
            }
            set
            {
                switch (value)
                {
                    case 2:
                        rdEasy.Checked = true;
                        break;
                    case 3:
                        rdMedium.Checked = true;
                        break;
                    case 4:
                        rdHard.Checked = true;
                        break;
                    case 5:
                        rdExtreme.Checked = true;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
