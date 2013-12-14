using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using SQLiteQueryBrowser;
using System.Data.Common;
using System.Diagnostics;

namespace CC
{
    public partial class Form3 : Form
    {
        DataSet ds = new DataSet();
        SQLiteDataAdapter da = new SQLiteDataAdapter();
        SQLiteParameter param = new SQLiteParameter();        
        private string[] name = new string[] { "", "", "", "", "", "", "", "" };
        private string[] col = new string[] { "", "", "", "", "", "", "", "" };
        
        public Form3()
        {
            InitializeComponent();
        }

        private SQLiteDBHelper getDataBase()
        {
            return new SQLiteDBHelper("D:\\Demo.db3");
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先输入插入数据的表、表字段和表的列数！");
                return;
            }
            if (tcol2.Text == "")
            {
                MessageBox.Show("请先输入插入数据的表、表字段和表的列数！");
                return;            
            }
            SQLiteConnection sqConnection = new SQLiteConnection(dbconn.connection);
            sqConnection.Open();
            SQLiteCommand sqCommand = new SQLiteCommand();
            SQLiteTransaction myTrans;
            // Start a local transaction
            myTrans = sqConnection.BeginTransaction(System.Data.IsolationLevel.Serializable);
            // Assign transaction object for a pending local transaction
            sqCommand.Connection = sqConnection;
            sqCommand.Transaction = myTrans;
            //int cn = Convert.ToInt16(textBox2.Text);
            String t = textBox1.Text.ToString().Trim();
            //判断表是否存在
            sqCommand.CommandText = "SELECT COUNT(*)  as CNT FROM sqlite_master where type='table' and name=" + "'" + t + "'";
            try
            {
                sqCommand.ExecuteNonQuery();
                if (0 == Convert.ToInt32(sqCommand.ExecuteScalar()))
                {
                    MessageBox.Show("您选择的表不存在!");
                    return;
                }                
            }
            catch (System.Exception ex)
            {                
                MessageBox.Show(ex.Message);
            }
           
            int j = 0;            
            name[0] = tcol1.Text.ToString().Trim();
            name[1] = tcol2.Text.ToString().Trim();
            name[2] = tcol3.Text.ToString().Trim();
            name[3] = tcol4.Text.ToString().Trim();
            name[4] = tcol5.Text.ToString().Trim();
            name[5] = tcol6.Text.ToString().Trim();
            name[6] = tcol7.Text.ToString().Trim();
            name[7] = tcol8.Text.ToString().Trim();
            for (int i = 0; i < 7; i++)
            {
                if (name[i] != "")
                    col[j++] = name[i];
            }

            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {                    
                    for (int nu = 0; nu < j; nu++)
                    {
                        if (name[nu] != "")
                        {
                            switch (nu)
                            {
                                case 0: 
                                   
                                    sqCommand.CommandText = "insert into " + t + "(" + col[nu];
                                    break;
                                case 1:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 2:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 3:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 4:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 5:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 6:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                                case 7:
                                    sqCommand.CommandText += "," + col[nu];
                                    break;
                            }
                        }
                    }                    
                    sqCommand.CommandText += ") values(";
                    for (int cnum = 0; cnum < j;cnum ++ )
                    {
                        if (cnum == 0)
                            sqCommand.CommandText += "@" + name[cnum];
                        else
                            sqCommand.CommandText += ",@" + name[cnum];
                    }
                    sqCommand.CommandText += ")";

                    for (int cnu = 0; cnu < j; cnu++)
                    {
                        sqCommand.Parameters.Add(new SQLiteParameter("@"+name[cnu], dataGridView1.Rows[i].Cells[cnu].Value));
                    }
                    try
                    {
                        sqCommand.ExecuteNonQuery();
                        //MessageBox.Show("添加数据成功！");
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("请确认输入的信息！");
                        return;
                    }
                }
                myTrans.Commit();
                MessageBox.Show("添加数据成功！"); 
            }
            catch //(Exception e)
            {
                myTrans.Rollback();
                MessageBox.Show("添加失败！");
                throw;                 
                 return;
                //string Error = string.Format("Neither record was written to database,Details:{0}", e.ToString());
            }
            finally
            {
                sqConnection.Close();
            }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tcol1.Text = "";
            tcol2.Text = "";
            tcol3.Text = "";
            tcol4.Text = "";
            tcol5.Text = "";
            tcol6.Text = "";
            tcol7.Text = "";
            tcol8.Text = "";
        }
         
    }
}