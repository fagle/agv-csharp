using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteQueryBrowser;
using System.Data.SQLite;

namespace CC
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }

//         private void Form1_Load(object sender, EventArgs e)
//         {
//             SQLiteConnection dbConnection = new SQLiteConnection("Data Source=D:\\Demo.db3");
//             SQLiteCommand dbCommand = new SQLiteCommand("SELECT * FROM t", dbConnection);
//             dbCommand.CommandType = CommandType.Text;
//             SQLiteDataAdapter dbDataAdapter = new SQLiteDataAdapter(dbCommand);
//             DataSet dataSet = new DataSet();
//             dbDataAdapter.Fill(dataSet, "t");
//             dataGridView1.DataSource = dataSet.Tables["t"];
//         } 

        private SQLiteDBHelper getDataBase()
        {
            return new SQLiteDBHelper("D:\\Demo.db3");
        }    

        private void 上位机配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();            
            for (int x = 0; x < MdiChildren.Length; x++)
            {
                Form tempChild = (Form)MdiChildren[x];
                tempChild.Close();
            }
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();
        }

        private void 下位机配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serial s = new serial();
            for (int x = 0; x < MdiChildren.Length; x++)
            {
                Form tempChild = (Form)MdiChildren[x];
                tempChild.Close();
            }
            s.MdiParent = this;
            s.WindowState = FormWindowState.Maximized;
            s.Show();
        }
        
    }
}
