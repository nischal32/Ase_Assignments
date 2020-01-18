﻿using assignment;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Ase_Assignments
{
    //Initializing variables
    public partial class Form1 : Form
    {
        int x = 0, y = 0, radius = 0, width = 0, height = 0, d = 0;
        Graphics graphics;
        int fVal = 0, sVal = 0;
        string textOrder;
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
                    getShape(result);
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
                else if (result[0] == "square")
                {
                    square(result);
                }
                else if(result[0] == "transform")
                {
                    transform(result);
                }
                else if (result[0] == "if")
                {
                    ifcase(result);
                }

            }
            catch (IndexOutOfRangeException j)//catches array ouut of range exceptions
            {

                MessageBox.Show("Invalid Command");

            }
        }


        /// <summary>
        /// if case method
        /// </summary>
        /// <param name="result"></param>
        private void ifcase(string[] result)
        {
            d = 1;
            int b = Ending();
            if (b == 7)
            {
                string[] line;
                line = textarea.Lines;
                ValidateCode val = new ValidateCode();
                var textArr = textOrder.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int length = textArr.Length;
                int counter = 0;
                for (int s = 1; s < length - 1; s++)
                {
                    string[] arrayOrder = Regex.Split(line[counter], "\r\n");//splits the data at next line and stores themin array
                    String[] results = val.valid(arrayOrder[0]);

                    if (results[1] == "radius")
                    {
                        symbolcheck(results, line, counter);
                    }
                    counter = counter + 1;
                }
            }
            else
            {
                d = 1;
                MessageBox.Show("endif missing");
            }

        }


        // method for loop case
        private int Ending()
        {
            string[] line;
            line = textarea.Lines;
            ValidateCode val = new ValidateCode();
            var textArr = textOrder.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int length = textArr.Length;
            int counter = 0;
            for (int t = 1; t <= length; t++)
            {
                string[] arrayOrder = Regex.Split(line[counter], "\r\n");//splits the data at next line and stores themin array
                string[] result1 = val.valid(arrayOrder[0]);
                if (result1[0] == "end")
                {
                    return 5;
                    break;
                }
                if (result1[0] == "endmethod")
                {
                    return 6;
                    break;
                }
                if (result1[0] == "endif")
                {
                    return 7;
                    break;
                }

                counter = counter + 1;
            }

            return 0;

        }
        /// <summary>
        /// used to check symbols and check is condition is true or false for the if case method
        /// </summary>
        /// <param name="results"></param>
        /// <param name="line"></param>
        /// <param name="counter"></param>
        private void symbolcheck(string[] results, string[] line, int counter)
        {
            ValidateCode val = new ValidateCode();
            //condition to check if radius is equal
            if (results[2] == "=")
            {
                if (results[1] == "radius")
                {
                    if (Convert.ToInt32(results[3]) == radius)
                    {
                        MessageBox.Show("Radius is equal");
                        string[] arrayOrders = Regex.Split(line[counter + 1], "\r\n");//splits the data at next line and stores themin array
                        String[] result = val.valid(arrayOrders[0]);
                        if (result[0] == "circle")
                        {
                            string[] value = { "circle", result[1] };
                            circle(value);
                            MessageBox.Show("Circle is drawn");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Radius is not equal");
                        d = 1;
                    }
                }
            }
            // condition to check if radius is greater or not
            else if (results[2] == ">")
            {
                if (results[1] == "radius")
                {
                    if (radius > Convert.ToInt32(results[3]))
                    {
                        MessageBox.Show("Radius is greater");
                        string[] arrayOrders = Regex.Split(line[counter + 1], "\r\n");//splits the data at next line and stores themin array
                        String[] result = val.valid(arrayOrders[0]);
                        if (result[0] == "circle")
                        {
                            string[] value = { "circle", result[1] };
                            circle(value);
                            MessageBox.Show("Circle is drawn");
                        }
                    }
                    else
                    {
                        MessageBox.Show("radius is lesser");
                        d = 1;
                    }
                }

            }
            //condition to check if radius is lesser
            else if (results[2] == "<")
            {
                if (results[1] == "radius")
                {
                    if (radius < Convert.ToInt32(results[3]))
                    {
                        MessageBox.Show("Radiius is lesser");
                        string[] arrayOrders = Regex.Split(line[counter + 1], "\r\n");//splits the data at next line and stores themin array
                        String[] r = val.valid(arrayOrders[0]);
                        r = val.valid(arrayOrders[0]);
                        if (r[0] == "circle")
                        {
                            string[] value = { "circle", r[1] };
                            circle(value);
                            MessageBox.Show("Circle is drawn");
                        }
                    }
                    else
                    {
                        MessageBox.Show("radius is greater");
                        d = 1;
                    }
                }
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
        /// <summary>
        /// Method for triangle
        /// </summary>
        /// <param name="result"></param>

        /// <summary>
        /// Method for circle
        /// </summary>
        /// <param name="result"></param>
        private void circle(string[] result)
        {
            d = 0;

            Factory s1 = new Factory();
            Shape sh = s1.getShape(result[0]);
            sh.drawShape(result, graphics, x, y, radius, width, height);
        }
        private void triangle(string[] result)
        {
            d = 0;
            Factory t1 = new Factory();
            Shape sh = t1.getShape(result[0]);
            sh.drawShape(result, graphics, x, y, radius, width, height);
        }
        /// <summary>
        /// Method  for square
        /// </summary>
        /// <param name="result"></param>
        private void square(string[] result)
        {
            d = 0;
            Factory c1 = new Factory();
            Shape sh = c1.getShape(result[0]);
            sh.drawShape(result, graphics, x, y, radius, width, height);
        }
        /// <summary>
        /// Method for drawto upto certain points
        /// </summary>
        /// <param name="result"></param>
        private void drawto(string[] result)
        {
            if (result[1].Equals("a") && result[2].Equals("b"))
            {
                a = 0;
                b = 50;
            }
            else
            {
                a = Convert.ToInt32(result[1]);
                b = Convert.ToInt32(result[2]);
            }
            Pen pen = new Pen(Color.Bisque, 3);
            graphics.DrawLine(pen, x, y, a, b);
        }
        /// <summary>
        /// Method to transform points upto certain  points
        /// </summary>
        /// <param name="result"></param>
        private void transform(string[] result)
        {
            d = 0;
            if (result[1].Equals("xpoint") && result[2].Equals("ypoint"))
            {
                xTranslate = 30;
                yTranslate = 40;
            }
            else
            {
                xTranslate = Convert.ToInt32(result[1]);
                yTranslate = Convert.ToInt32(result[2]);
            }
            graphics.TranslateTransform(x, y);
            MessageBox.Show("points has been transformed to \r\nx:" + xTranslate + "\r\ny:" + yTranslate + "");//shows message box

        }
        private void rectangle(string[] result)
        {
            d = 0;
            Factory s1 = new Factory();
            Shape sh = s1.getShape(result[0]);
            sh.drawShape(result, graphics, x, y, radius, width, height);
        }

    }
}