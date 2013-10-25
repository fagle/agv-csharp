using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using SQLiteQueryBrowser;
namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateTable(); 
            //InsertData(); 
            ShowData();
            Console.ReadLine();
        }
        public static void CreateTable()
        {
            string dbPath = "D:\\Demo.db3";
            //如果不存在改数据库文件，则创建该数据库文件 
            if (!System.IO.File.Exists(dbPath))
            {
                SQLiteDBHelper.CreateDB("D:\\Demo.db3");
            }
            SQLiteDBHelper db = new SQLiteDBHelper("D:\\Demo.db3");
            string sql = "CREATE TABLE Test3(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,Name char(3),TypeName varchar(50),addDate datetime,UpdateTime Date,Time time,Comments blob)";
            db.ExecuteNonQuery(sql, null);
        }
        public static void InsertData()
        {
            string sql = "INSERT INTO Test3(Name,TypeName,addDate,UpdateTime,Time,Comments)values(@Name,@TypeName,@addDate,@UpdateTime,@Time,@Comments)";
            SQLiteDBHelper db = new SQLiteDBHelper("D:\\Demo.db3");
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int i = 0; i < 100; i++)
                {
                    SQLiteParameter[] parameters = new SQLiteParameter[]{ 
                                                 new SQLiteParameter("@Name",c+i.ToString()), 
                                         new SQLiteParameter("@TypeName",c.ToString()), 
                                         new SQLiteParameter("@addDate",DateTime.Now), 
                                         new SQLiteParameter("@UpdateTime",DateTime.Now.Date), 
                                         new SQLiteParameter("@Time",DateTime.Now.ToShortTimeString()), 
                                         new SQLiteParameter("@Comments","Just a Test"+i) 
                                         };
                    db.ExecuteNonQuery(sql, parameters);
                }
            }
        }
        public static void ShowData()
        {
            //查询从50条起的20条记录 
            string sql = "select * from test3 order by id desc limit 50 offset 20";
            SQLiteDBHelper db = new SQLiteDBHelper("D:\\Demo.db3");
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    Console.WriteLine("ID:{0},TypeName{1}", reader.GetInt64(0), reader.GetString(1));
                }
            }
        }

    }
}
