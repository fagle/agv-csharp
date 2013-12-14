using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CC
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbconn.connection))
            {
                conn.Open();
                DataTable schemaTable = conn.GetSchema("TABLES");
                this.dataGridView1.DataSource = schemaTable;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;
            int rowIndx = dataGridView1.CurrentCell.RowIndex;
            textBox1.Text = dataGridView1[2, e.RowIndex].Value.ToString() + "___" + dataGridView1[6, e.RowIndex].Value.ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {            
            using (SQLiteConnection conn = new SQLiteConnection(dbconn.connection))
            {
                conn.Open();
                string sql = textBox2.Text.ToString().Trim();
                if (sql == "")
                {
                    MessageBox.Show("命令行为空！");
                    return;
                }
                SQLiteCommand cm = conn.CreateCommand();
                cm.CommandText = sql;
                try
                {
                    cm.ExecuteNonQuery();
                    MessageBox.Show("修改成功！");
                }
                catch (System.Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    MessageBox.Show("修改失败！"+ex.Message);
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("只能添加列，无法删除列！命令为：\n\r alter table (要修改的表的名称) add column (添加列的列名) (列的数据类型)");
        }        
    }
}
