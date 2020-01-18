using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ase_Assignments
{
    //Initializing variables
    public partial class Form1 : Form
    {
        int x = 0, y = 0, radius = 0, width = 0, height = 0, d = 0;
        Graphics graphics;
        int fVal = 0, sVal = 0;
        int a = 0, b = 0, xTranslate = 0, yTranslate = 0;
        public Form1()
        {
            InitializeComponent();
            graphics = panel.CreateGraphics();
        }

        private void command_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// sets panel background color to light blue
        /// </summary>

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            panel.BackColor = Color.LightBlue;

        }

        private void textarea_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary> 
        ///this code check commannd line by line and executes them 
        /// </summary>
        private void Execute_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textarea.Text))
            {
                string textOrder = textarea.Text.Trim();

                string[] arrayOrder = Regex.Split(textOrder, "\r\n");//splits the data at next line and stores themin array

                for (int i = 0; i < arrayOrder.Length; i++)
                {
                    ValidateCode v = new ValidateCode();
                    String[] result = v.valid(arrayOrder[i]);
                    try
                    {
                        if (result[0] == "moveTo")
                        {
                            int a = Convert.ToInt32(result[1]);
                            int b = Convert.ToInt32(result[2]);
                            x = a;
                            y = b;
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
                    catch (IndexOutOfRangeException n)          //catches exception whendex is out of array
                    {
                        MessageBox.Show("Invalid Command");
                    }
                }
            }
            else
            {
                MessageBox.Show("No Command to Run");
            }
        }

        private void textClear_Click(object sender, EventArgs e)
        {
            command.ResetText();
            MessageBox.Show("Text has been cleared");
        }
        /// <summary>
        /// clears the panel by refreshing the panel
        /// </summary>
        private void clear_Click(object sender, EventArgs e)
        {
            panel.Refresh();
            MessageBox.Show("Drawing has been cleared");
        }
        /// <summary>
        /// resets the position of the cursor
        /// displays message 'points has been reseted' in the message box
        /// </summary>
        private void reset_Click(object sender, EventArgs e)
        {
            x = 0;
            y = 0;
            MessageBox.Show("Points has been reseted to \r\nX:0\r\nY:0");
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            textarea.ResetText();
            MessageBox.Show("Text has been cleared");
        }



        /// <summary>
        /// Open,read and display the text file selected by user on clicking load menu item
        /// </summary>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();
            load.Title = "Load.";
            load.Filter = "Text Files(*.txt)|*.txt| All Files(*.*)|*.*";

            if (load.ShowDialog() == DialogResult.OK)
            {
                StreamReader openStream = new StreamReader(File.OpenRead(load.FileName));
                textarea.Text = openStream.ReadToEnd();
                openStream.Dispose();
            }
        }
        /// <summary>
        /// .Saves or overWrites  the text file loaded by the user on cliking save menu item. 
        /// </summary>
        private void laveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save";
            save.Filter = "Text Files(*.txt)|*.txt| All Files(*.*)|*.*";

            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter saveStream = new StreamWriter(File.Create(save.FileName));
                saveStream.Write(textarea.Text);

                saveStream.Dispose();
            }
        }
        /// <summary>
        /// Closes the application
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        /// <summary>
        /// this code is used reads text from textbox and validates them by sending the data through valid method in ValidateCode class
        /// once validated return value is checked for command if found code is executed 
        /// if command is not found catch block is executed
        /// </summary>

        private void run_Click(object sender, EventArgs e)
        {
            String cmd = command.Text;
            ValidateCode val = new ValidateCode();
            String[] result = val.valid(cmd);
            getShape(result);
        }

        //method to get shape
        public void getShape(String[] result)
        {
            try
            {
                if (result[0] == "radius" && result[1] == "=")
                {
                    radiusE(result);
                }
                else if (result[0] == "radius" && result[1] == "+")
                {
                    radiusP(result);

                }
                else if (result[0] == "width" && result[1] == "=")
                {
                    d = 0;
                    width = Convert.ToInt32(result[2]);
                }
                else if (result[0] == "height" && result[1] == "=")
                {
                    d = 0;
                    height = Convert.ToInt32(result[2]);
                }
                else if (result[0] == "width" && result[1] == "+")
                {
                    width = Convert.ToInt32(result[2]);
                }
                else if (result[0] == "height" && result[1] == "+")
                {
                    height = Convert.ToInt32(result[2]);
                }
                
            }
            catch (IndexOutOfRangeException j)//catches array ouut of range exceptions
            {

                MessageBox.Show("Invalid Command");

            }
        }
        /// <summary>
        /// Method to store radius
        /// </summary>
        /// <param name="result"></param>
        private void radiusE(string[] result)
        {
            d = 0;
            radius = Convert.ToInt32(result[2]);
        }
        /// <summary>
        /// Method to add radius and store it
        /// </summary>
        /// <param name="result"></param>
        private void radiusP(string[] result)
        {
            d = 0;
            radius = radius + Convert.ToInt32(result[2]);
        }


    }
}