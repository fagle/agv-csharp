/* Author: Evren DAGLIOGLU
 * E-mail: evrenda@yahoo.com
 * This software is copyrighted to the author himself. It can be used freely for educational purposes.
 * For commercial usage written consent of the author must be taken and a reference to the author should be provided. 
 * No responsibility will be taken for any loss or damage that will occur as a result of the usage of this code. 
 * 
 * Please feel free to inform me about any bugs, problems, ideas etc.
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace AGV
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainGUI : System.Windows.Forms.Form
    {

        public bool keepCtrlFormWithinBorders = true;
        public bool IsPBoxVisible = true;


        private Canvas newCanvas;

        public ArrayList listMasterArray = new ArrayList();
        public string inputFileTxt;


        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private Panel panel1;
        private IContainer components;

        public MainGUI()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7,
            this.menuItem8});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Enabled = false;
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Open";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Exit";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.MdiList = true;
            this.menuItem5.Text = "Window";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "添加一个站点";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "删除一个站点";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 4;
            this.menuItem8.Text = "指定相邻两个站点的路径";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 0);
            this.panel1.TabIndex = 0;
            // 
            // MainGUI
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(736, 566);
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu1;
            this.Name = "MainGUI";
            this.Text = "DXF Reader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainGUI());
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            newCanvas = new Canvas();			//a new canvas is created...

            newCanvas.TopLevel = false;
            //newCanvas.MdiParent = this.panel1;			//...its mdiparent is set...           
            panel1.Controls.Add(newCanvas);
            newCanvas.Show();							//the canvas is displayed...
            newCanvas.Activate();
            newCanvas.Focus();
        }

        private void menuItem2_Click(object sender, System.EventArgs e)		//Opens openfile dialog to select a DXF file
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

        private void menuItem4_Click(object sender, System.EventArgs e)		//exits program...
        {
            this.Close();
        }

    }

    /*private void menuItem5_Click(object sender, EventArgs e)
    {

    }*/

}
