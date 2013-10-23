using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DXFImporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inputFileTxt = "";

            openFileDialog1.InitialDirectory = "c:\\";		//sets the initial directory of the openfile dialog

            openFileDialog1.Filter = "dxf files (*.dxf)|*.dxf|All files (*.*)|*.*";	//filters the visible files...

            openFileDialog1.FilterIndex = 1;


            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)		//open file dialog is shown here...if "cancel" button is clicked then nothing will be done...
            {
                inputFileTxt = openFileDialog1.FileName;	//filename is taken (file path is also included to this name example: c:\windows\system\blabla.dxf

                int ino = inputFileTxt.LastIndexOf("\\");	//index no of the last "\" (that is before the filename) is found here


                newCanvas = new Canvas();			//a new canvas is created...

                newCanvas.MdiParent = this;			//...its mdiparent is set...

                newCanvas.Text = inputFileTxt.Substring(ino + 1, inputFileTxt.Length - ino - 1);  //...filename is extracted from the text...(blabla.dxf)...
                newCanvas.MinimumSize = new Size(500, 400);		//...canvas minimum size is set...


                if (inputFileTxt.Length > 0)
                {
                    newCanvas.ReadFromFile(inputFileTxt);		//the filename is sent to the method for data extraction and interpretation...
                }



                newCanvas.Show();							//the canvas is displayed...
                newCanvas.Activate();
                newCanvas.Focus();

            }

            openFileDialog1.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}