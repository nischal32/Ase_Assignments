using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ase_Assignments
{
    public partial class Form1 : Form
    {
        int x = 0, y = 0;
        Graphics graphics;
        public Form1()
        {
            InitializeComponent();
            graphics = panel.CreateGraphics();
        }

        private void command_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            panel.BackColor = Color.LightBlue;

        }

        private void run_Click(object sender, EventArgs e)
        {
            String cmd = command.Text;
            ValidateCode val = new ValidateCode();
            String[] result = val.valid(cmd);
            try
            {
                if (result[0] == "moveTo")
                {
                    int fVal = Convert.ToInt32(result[1]);
                    int sVal = Convert.ToInt32(result[2]);
                    x = fVal;
                    y = sVal;
                    MessageBox.Show("points has been moved to \r\nx:" + x + "\r\ny:" + y + "");//shows message box

                }
                else if (result[0] == "drawTo")
                {
                    int a = Convert.ToInt32(result[1]);
                    int b = Convert.ToInt32(result[2]);
                    Pen pen = new Pen(Color.Bisque, 3);
                    graphics.DrawLine(pen, x, y, a, b);
                }

                else if (result[0] == "rectangle")
                {
                    Factory s1 = new Factory();
                    Shape sh = s1.getShape(result[0]);
                    sh.drawShape(result, graphics, x, y);
                }
                else if (result[0] == "triangle")
                {
                    Factory t1 = new Factory();
                    Shape sh = t1.getShape(result[0]);
                    sh.drawShape(result, graphics, x, y);
                }
                else if (result[0] == "circle")
                {
                    Factory c1 = new Factory();
                    Shape sh = c1.getShape(result[0]);
                    sh.drawShape(result, graphics, x, y);
                }
            }
            catch (IndexOutOfRangeException j)//catches array ouut of range exceptions
            {
                if (string.IsNullOrEmpty(command.Text))//checks if textbox is empty
                {
                    MessageBox.Show("No Command to Run");
                }
                else
                {
                    MessageBox.Show("Invalid Command");
                }

            }

        }
    }
}
