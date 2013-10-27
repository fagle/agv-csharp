using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data.SQLite;
using SQLiteQueryBrowser;
using System.Collections;

namespace AGV
{
    class MapDataBase
    {
        private string mapName;
        #region  database Methods
        public MapDataBase(string mapName)
        {
            this.mapName = mapName;
        }
        private SQLiteDBHelper getDataBase()
        {
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
        public void addLine(Point startPoint, Point endPoint, int index)
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
        public void addArc(Point center, int radius, int startAngle, int sweepAngle, int index)
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

        public void addShapeInfo(int index, string shapeType)
        {
            string sql = @"INSERT INTO shapeTable(indexNo,shape,ownerMap)
                            values(@index,@shapeType,@ownerMap)";
            SQLiteDBHelper db = getDataBase();
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@index",index),      
                                                new SQLiteParameter("@shapeType",shapeType),
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
        public void loadMapFromDataBase(ArrayList drawingList, ArrayList objectIdentifier)
        {
            List<String> strList = new List<string>(100);
            string sql = "SELECT indexNo,shape FROM shapeTable  WHERE (ownerMap = @ownerMap) ORDER BY indexNo";
            SQLiteParameter[] parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@ownerMap",mapName),                                                 
                                           };
            SQLiteDBHelper db = getDataBase();
            try
            {
                using (SQLiteDataReader reader = db.ExecuteReader(sql, parameters))
                {
                    while (reader.Read())
                    {
                        //Console.WriteLine("indexNo:{0},shape:{1}", /*reader.GetInt64(0)*/1, reader.GetString(1));
                        strList.Add(reader.GetString(1));
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            for (int i = 0; i < strList.Count; i++)
            {
                parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@ownerMap",mapName),      
                                                new SQLiteParameter("@indexNo",i),
                                           };
                if (strList[i] == "line")
                {
                    sql = "SELECT startX,startY,endX,endY FROM lineTable  WHERE (ownerMap = @ownerMap AND indexNo=@indexNo)";
                }
                else if (strList[i] == "arc")
                {
                    sql = "SELECT radius,Ox,Oy,startAngle,sweepAngle FROM arcTable  WHERE (ownerMap = @ownerMap AND indexNo=@indexNo)";
                }
                else
                    break;
                try
                {
                    using (SQLiteDataReader reader = db.ExecuteReader(sql, parameters))
                    {
                        while (reader.Read())
                        {
                            if (strList[i] == "line")
                            {
                                Point startPoint = new Point(reader.GetInt32(0),reader.GetInt32(1));
                                Point endPoint = new Point(reader.GetInt32(2),reader.GetInt32(3));
                                int ix = drawingList.Add(new Line(startPoint,endPoint,Color.White, 1));
                                objectIdentifier.Add(new DrawingObject(2, ix));
                            }
                            else if (strList[i] == "arc") {
                                Point center = new Point(reader.GetInt32(1), reader.GetInt32(2));
                                int ix = drawingList.Add(new Arc(center, reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), Color.White, Color.Red, 1));
                                objectIdentifier.Add(new DrawingObject(6, ix));
                            }

                        }
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                }
            }
        }
        #endregion
    }
}
