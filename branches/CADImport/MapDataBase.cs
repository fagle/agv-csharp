using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data.SQLite;
using SQLiteQueryBrowser;

namespace DXFImporter
{
    class MapDataBase
    {
        private string mapName;
        #region  database Methods
        public MapDataBase(string mapName) 
        {
            this.mapName = mapName;
        }
        private SQLiteDBHelper getDataBase (){
            return new SQLiteDBHelper("D:\\Demo.db3");
        }
        public void addMapToDataBase()
        {/*
            string sql = "INSERT INTO mapTable(mapName,user,passwd,modifiedTime)values(@mapName,@user,@passwd,@modifiedTime)";

            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@mapName",mapName), 
                                                new SQLiteParameter("@user","ecs"), 
                                                new SQLiteParameter("@passwd","hel610"), 
                                                new SQLiteParameter("@modifiedTime",DateTime.Now.Date),                                                 
                                           };
            try
            {
                db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }     */    
        }
        public void addLine(Point startPoint, Point endPoint,int index) 
        {
            
            /*
            string sql = "INSERT INTO lineTable(indexNo,startX,startY,endX,endY,ownerMap)values(@index,@startX,@startY,@endX,@endY,@ownerMap)";
            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@index",index), 
                                                new SQLiteParameter("@startX",startPoint.X), 
                                                new SQLiteParameter("@startY",startPoint.Y), 
                                                new SQLiteParameter("@endX",endPoint.X),   
                                                new SQLiteParameter("@endY",endPoint.Y),
                                                new SQLiteParameter("@ownerMap",mapName),
                                           };
            try { 
                db.ExecuteNonQuery(sql, parameters); 
            }
            catch(Exception x)
            {
                Console.WriteLine(x.Message);
            }*/
        }
        public void addArc(Point center, int radius, int startAngle, int sweepAngle , int index) 
        {
            /*
            string sql = @"INSERT INTO arcTable(indexNo,Ox,Oy,startAngle,sweepAngle,endAngle,radius,ownerMap)
                            values(@index,@Ox,@Oy,@startAngle,@sweepAngle,@endAngle,@radius,@ownerMap)";
            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@index",index), 
                                                new SQLiteParameter("@Ox",center.X), 
                                                new SQLiteParameter("@Oy",center.Y), 
                                                new SQLiteParameter("@startAngle",startAngle),   
                                                new SQLiteParameter("@sweepAngle",sweepAngle),
                                                new SQLiteParameter("@endAngle",startAngle + sweepAngle),
                                                new SQLiteParameter("@radius",radius),
                                                new SQLiteParameter("@ownerMap",mapName),
                                           };
            try
            {
                db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }*/
        }
        public void modifyLine(Point startPoint, Point endPoint, int index) 
        {
            string sql = "UPDATE lineTable SET startX = @startX,startY=@startY,endX = @endX, endY = @endY WHERE (indexNo = @index) and (ownerMap=@ownerMap)";
            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@index",index), 
                                                new SQLiteParameter("@startX",startPoint.X), 
                                                new SQLiteParameter("@startY",startPoint.Y), 
                                                new SQLiteParameter("@endX",endPoint.X),   
                                                new SQLiteParameter("@endY",endPoint.Y),
                                                new SQLiteParameter("@ownerMap",mapName),
                                           };
            try
            {
                db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }
        public void modifyArc(Point center, int radius, int startAngle, int sweepAngle, int index)
        {
            string sql = @"UPDATE arcTable SET radius = @radius,Ox=@Ox,Oy = @Oy, startAngle = @startAngle,sweepAngle = @sweepAngle, endAngle = @endAngle 
                            WHERE (indexNo = @index) and (ownerMap=@ownerMap)";
            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@index",index), 
                                                new SQLiteParameter("@Ox",center.X), 
                                                new SQLiteParameter("@Oy",center.Y), 
                                                new SQLiteParameter("@startAngle",startAngle),   
                                                new SQLiteParameter("@sweepAngle",sweepAngle),
                                                new SQLiteParameter("@endAngle",startAngle + sweepAngle),
                                                new SQLiteParameter("@radius",radius),
                                                new SQLiteParameter("@ownerMap",mapName),
                                           };
            try
            {
                db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }
        #endregion
    }
}
