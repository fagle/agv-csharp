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

namespace CC
{
    public partial class Form2 : Form
    {
        private int pageSize = 15;
        private int nmax = 0;
        private int pagecount = 0;
        private int pageCurrent = 0;
        private int ncurrent = 0;
        private int number = 0;
        private SQLiteDataAdapter da;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private string[] name = new string[] { "", "", "", "", "", "", "", "" };
        private string[] col = new string[] { "", "", "", "", "", "", "", "" };
        private string[] d = new string[] { "", "", "", "", "", "", "", "" };

        public Form2()
        {
            InitializeComponent();
        }

        private SQLiteDBHelper getDataBase()
        {
            return new SQLiteDBHelper("D:\\Demo.db3");
        }

        private void dataview()
        {            
            dataGridView1.DataSource = null;
            dt.Rows.Clear();
            SQLiteConnection dbConnection = new SQLiteConnection("Data Source=D:\\Demo.db3");
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请先选择表");
                return;
            }
            string sql = comboBox1.Text.ToString().Trim();
            //只绑定指定的列
            //string strsql = string.Format("select id,name,passwd from {0}", sql);
            string strsql = string.Format("select * from {0}", sql);
            SQLiteCommand dbCommand = new SQLiteCommand(strsql, dbConnection);
            dbConnection.Open();
            dbCommand.CommandType = CommandType.Text;
            da = new SQLiteDataAdapter(dbCommand);
            da.Fill(ds,sql);
            dbConnection.Close();
            dt = ds.Tables[sql];            
            dataGridView1.DataSource = ds.Tables[sql].DefaultView;
            InitDataSet();
        } 


        private void InitDataSet()
        {
            //pageSize = 15;            
            nmax = dt.Rows.Count;
            pagecount = (nmax/pageSize);
            if (nmax%pageSize > 0)
            {
                pagecount++;
            }
            pageCurrent = 1;    //当前页数从1开始
            ncurrent = 0;       //当前记录数从0开始

            LoadData();
        }

        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行

            DataTable dtTemp = dt.Clone();   //克隆DataTable结构框架

            if (pageCurrent == pagecount)
                nEndPos = nmax;
            else
                nEndPos = pageSize * pageCurrent;

            nStartPos = ncurrent;
            toolStripLabel2.Text = "/" + pagecount.ToString();
            if (dt.Rows.Count == 0)
            {
                toolStripTextBox1.Text = "0";
            }
            else
            {
                toolStripTextBox1.Text = Convert.ToString(pageCurrent);
            }

            //从元数据源复制记录行
            if (dt.Rows.Count != 0)
            {
                for (int i = nStartPos; i < nEndPos; i++)
                {
                    dtTemp.ImportRow(dt.Rows[i]);
                    ncurrent++;
                }
            }
            
            bdsInfo.DataSource = dtTemp;
            bdnInfo.BindingSource = bdsInfo;
            dataGridView1.DataSource = bdsInfo;
        }

        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "上一页")
            {      
                if (pageCurrent >= 0)
                {
                    pageCurrent--;
                }
                if (pageCurrent <= 0)
                {
                    pageCurrent++;
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    ncurrent = pageSize * (pageCurrent - 1);                    
                }

                LoadData();
            }
            if (e.ClickedItem.Text == "下一页")
            {                
                if (pageCurrent <= pagecount)
                {
                    pageCurrent++;
                }
                if (pageCurrent > pagecount)
                {
                    pagecount--;
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    ncurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataview();            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;
            int rowIndx = dataGridView1.CurrentCell.RowIndex;
            for (int i = 0; i < dataGridView1.ColumnCount ;i++ )
            {
                switch (i)
                {
                    case 0:
                        col1.Text = this.dataGridView1[0, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 1:
                        col2.Text = this.dataGridView1[1, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 2:
                        col3.Text = this.dataGridView1[2, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 3:
                        col4.Text = this.dataGridView1[3, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 4:
                        col5.Text = this.dataGridView1[4, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 5:
                        col6.Text = this.dataGridView1[5, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 6:
                        col7.Text = this.dataGridView1[6, e.RowIndex].Value.ToString().Trim();
                        break;
                    case 7:
                        col8.Text = this.dataGridView1[7, e.RowIndex].Value.ToString().Trim();
                        break;
                }                
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.dataGridView1.Columns[0].Name.ToString();
            if (col1.Text == "")
            {
                MessageBox.Show("请选择查询条件！");
            }
            else
            {
                string str = null;
                string t = comboBox1.Text;
                str = "select * from " + t +" where "+ name  + "=" + "'" + col1.Text + "'";  //第一列的字段不同              
                Databind(str);
            }
        }

        private void Databind(string sql)
        {
            string t = comboBox1.Text;
            SQLiteConnection connection = new SQLiteConnection(dbconn.connection);
            connection.Open();
            da = new SQLiteDataAdapter(sql, connection);
            ds = new DataSet();
            da.Fill(ds, t);
            dataGridView1.DataSource = ds.Tables[t].DefaultView;

        }        

        private void button5_Click(object sender, EventArgs e)
        {
            col1.Text = "";
            col2.Text = "";
            col3.Text = "";
            col4.Text = "";
            col5.Text = "";
            col6.Text = "";
            col7.Text = "";
            col8.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {                
            SQLiteConnection sqConnection = new SQLiteConnection(dbconn.connection);
            sqConnection.Open();
            SQLiteCommand sqCommand = new SQLiteCommand();
            sqCommand.Connection = sqConnection;
            String t = comboBox1.Text.ToString().Trim();
            //string[] col = new string[] { "", "", "", "", "", "", "", "" };
            //string[] d = new string[] { "", "", "", "", "", "", "", "" };
            d[0] = col1.Text.ToString().Trim();
            d[1] = col2.Text.ToString().Trim();
            d[2] = col3.Text.ToString().Trim();
            d[3] = col4.Text.ToString().Trim();
            d[4] = col5.Text.ToString().Trim();
            d[5] = col6.Text.ToString().Trim();
            d[6] = col7.Text.ToString().Trim();
            d[7] = col8.Text.ToString().Trim();
            int j = 0;
            //string[] name = new string[] { "", "", "", "", "", "", "", "" };
            name[0] = tcol1.Text.ToString().Trim();
            name[1] = tcol2.Text.ToString().Trim();
            name[2] = tcol3.Text.ToString().Trim();
            name[3] = tcol4.Text.ToString().Trim();
            name[4] = tcol5.Text.ToString().Trim();
            name[5] = tcol6.Text.ToString().Trim();
            name[6] = tcol7.Text.ToString().Trim();
            name[7] = tcol8.Text.ToString().Trim();
            for (; j < number; j++)
            {
                if (name[j] == "")
                {
                    MessageBox.Show("请先查看表字段定义，并写入表列字段对应列中！");
                    return;
                }
            }
            j = 0;
            for (int i = 0; i < 7; i++)
            {
                if (name[i] != "")
                    col[j++] = name[i];
            }  
               
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
                sqCommand.Parameters.Add(new SQLiteParameter("@" + name[cnu],d[cnu]));
            }
            try
            {
                sqCommand.ExecuteNonQuery();
                MessageBox.Show("插入成功！");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("修改数据失败！请检查列字段信息等！");
            }
           
//             string sql = "insert into t(id,name,passwd,num)values(@id,@name,@passwd,@num)";
//             SQLiteDBHelper db = getDataBase();
//             SQLiteParameter[] parameters = new SQLiteParameter[]
//                                             { 
//                                                  new SQLiteParameter("@id",DbType.Int32), 
//                                                  new SQLiteParameter("@name",DbType.String), 
//                                                  new SQLiteParameter("@passwd",DbType.String), 
//                                                  new SQLiteParameter("@num",DbType.String),                                                 
//                                             };
// 
//             parameters[0].Value = col1.Text;
//             parameters[1].Value = Convert.ToString(col2.Text);
//             parameters[2].Value = Convert.ToString(col3.Text);
//             parameters[3].Value = Convert.ToString(col4.Text);
//             try
//             {
//                 db.ExecuteNonQuery(sql, parameters);
//                 MessageBox.Show("添加数据成功！");
//             }
//             catch (Exception x)
//             {
//                 MessageBox.Show("添加数据失败！\n\r"+ x.Message);
//             }
//             this.col3.Text = "";
//             this.col4.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择表！");
                return;
            }
            string sql = comboBox1.Text;
            if ((MessageBox.Show("确定删除？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) == DialogResult.Yes)
            {                
                int i = (pageCurrent - 1) * pageSize + dataGridView1.CurrentCell.RowIndex;//dataGridView1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    ds.Tables[sql].Rows[i].Delete();                    
                    SQLiteCommandBuilder cb = new SQLiteCommandBuilder(da);
                    if (da.Update(ds.Tables[sql]) > 0)
                    {
                        foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                        {
                            dataGridView1.Rows.Remove(r);
                        }                      
                        MessageBox.Show("删除成功！");
                    }
                    else
                        MessageBox.Show("删除失败！");
                }
                else
                    return;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //string[] d = new string[] { "", "", "", "", "", "", "", "" };
            d[0] = col1.Text.ToString().Trim();
            d[1] = col2.Text.ToString().Trim();
            d[2] = col3.Text.ToString().Trim();
            d[3] = col4.Text.ToString().Trim();
            d[4] = col5.Text.ToString().Trim();
            d[5] = col6.Text.ToString().Trim();
            d[6] = col7.Text.ToString().Trim();
            d[7] = col8.Text.ToString().Trim();
            while (d[number] != "")
            {
                number++;
            }            
            
            //存储表列
            //string[] col = new string[] { "", "", "", "", "", "", "", ""};
            int j = 0;
            //string[] name = new string[] { "", "", "", "", "", "", "", "" };
            name[0] = tcol1.Text.ToString();
            name[1] = tcol2.Text.ToString();
            name[2] = tcol3.Text.ToString();
            name[3] = tcol4.Text.ToString();
            name[4] = tcol5.Text.ToString();
            name[5] = tcol6.Text.ToString();
            name[6] = tcol7.Text.ToString();
            name[7] = tcol8.Text.ToString();
            for (; j < number;j ++ )
            {
                if (name[j] == "")
                {
                    MessageBox.Show("请先查看表字段定义，并写入表列字段对应列中！");
                    return;
                }
            }
            j = 0;
            for (int i = 0; i < 7; i++)
            {
                if (name[i] != "")
                    col[j++] = name[i];
            }
            if (col1.Text == "")
            {
                MessageBox.Show("请先选择要修改的项！");
            }
            String str = null;
            String t = comboBox1.Text.ToString();
            for (int i = 0; i < j; i++)
            {
                if (name[i] != "")
                {
                    switch (i)
                    {
                        case 0:                            
                            col[i] = col1.Text.ToString().Trim();
                            break;
                        case 1:
                            if (col2.Text != "")
                            {
                                col[i] = col2.Text.ToString().Trim();
                                str = "update " + t + " set " + name[i] + "= '" + col[i] + "'";
                            }
                            break;
                        case 2:
                            if (col3.Text != "")
                            {
                                col[i] = col3.Text.ToString().Trim();
                                str += ", " + name[i] + "='" + col[i] + "'";
                            }
                            break;
                        case 3:
                            if (col4.Text != "")
                            {
                                col[i] = col4.Text.ToString().Trim();
                                str += ", " + name[i] + "='" + col[i] + "'";
                            }
                            break;
                        case 4:
                            if (col5.Text != "")
                            {
                                col[i] = col5.Text.ToString().Trim();
                                str += ", " + name[i] + "'" + col[i] + "'";
                            }
                            break;
                        case 5:
                            if (col6.Text != "")
                            {
                                col[i] = col6.Text;
                                str += ", " + name[i] + "='" + col[i] + "'";
                            }
                            break;
                        case 6:
                            if (col7.Text != "")
                            {
                                col[i] = col7.Text.ToString().Trim();
                                str += ", " + name[i] + "'" + col[i] + "',";
                            }
                            break;
                        case 7:
                            if (col8.Text != "")
                            {
                                col[i] = col8.Text.ToString().Trim();
                                str += ", " + name[i] + "='" + col[i] + "'";
                            }
                            break;
                    }                    
                }                
            }
            str = str + "where " + name[0] + "= '" + col[0] + "'";
            SQLiteConnection conn = new SQLiteConnection(dbconn.connection);
            conn.Open();           
            SQLiteCommand cm = conn.CreateCommand();
            cm.CommandText = str;
            try
            {
                cm.ExecuteNonQuery();
                MessageBox.Show("修改成功");
                //return;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("修改数据失败！请检查列字段信息等！");
            }          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }

        private void button11_Click(object sender, EventArgs e)
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
