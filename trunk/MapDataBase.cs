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
        private ArrayList drawingList;
        private ArrayList objectIdentifier;

        #region  database Methods
        public MapDataBase(string mapName)
        {
            this.mapName = mapName;
        }
        public MapDataBase(string mapName, ArrayList drawingList, ArrayList objectIdentifier)
        {
            this.mapName = mapName;
            this.drawingList = drawingList;
            this.objectIdentifier = objectIdentifier;
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
            }
        }
        public void addArc(Point center, int radius, int startAngle, int sweepAngle, int index)
        {
            
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
            }
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

        public List<ShapeInfoPack> shapeList;

        public void loadMapFromDataBase()
        {
            shapeList = new List<ShapeInfoPack>(100);            
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
                        shapeList.Add(new ShapeInfoPack(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
            for (int i = 0; i < shapeList.Count; i++)
            {
                parameters = new SQLiteParameter[]
                                           { 
                                                new SQLiteParameter("@ownerMap",mapName),      
                                                new SQLiteParameter("@indexNo",i),
                                           };
                if (shapeList[i].shape == "line")
                {
                    sql = "SELECT startX,startY,endX,endY FROM lineTable  WHERE (ownerMap = @ownerMap AND indexNo=@indexNo)";
                }
                else if (shapeList[i].shape == "arc")
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
                            if (shapeList[i].shape == "line")
                            {
                                Point startPoint = new Point(reader.GetInt32(0),reader.GetInt32(1));
                                Point endPoint = new Point(reader.GetInt32(2),reader.GetInt32(3));
                                int ix = drawingList.Add(new Line(startPoint,endPoint,Color.White, 1));
                                objectIdentifier.Add(new DrawingObject(2, ix));
                                int centerX = (startPoint.X + endPoint.X) / 2;
                                int centerY = (startPoint.Y + endPoint.Y) / 2;
                                shapeList[i].center = new Point(centerX,-centerY);
                            }
                            else if (shapeList[i].shape == "arc")
                            {
                                Point center = new Point(reader.GetInt32(1), reader.GetInt32(2));
                                int ix = drawingList.Add(new Arc(center, reader.GetInt32(0), reader.GetInt32(3), reader.GetInt32(4), Color.White, Color.Red, 1));
                                objectIdentifier.Add(new DrawingObject(6, ix));
                                shapeList[i].center = center;
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

        public void loadStationsFromDB(Dictionary<string, Station> stationDic) 
        {
//            string sql = @"SELECT name,positionX,positionY,btnX,btnY,next 
//                           FROM station  
//                           WHERE (ownerMap = @ownerMap)";
            string sql = @"SELECT name,positionX,positionY,btnX,btnY,next,cardID,stationID 
                             FROM station  
                             WHERE (ownerMap = @ownerMap)";
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
                        Station s =  new Station( reader.GetString(0), reader.GetInt32(1),
                            reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(6), reader.GetInt32(7));//select语句的顺序
                        stationDic.Add(reader.GetString(0),s);
                        if (!reader.IsDBNull(5))
                            s.Next = reader.GetString(5);

                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }            
        }

        public void loadPathsFromDB(Dictionary<string, Track> trackDic)
        {
            string sql = @"SELECT *
                           FROM pathTable  
                           WHERE (ownerMap = @ownerMap)";            
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
                        Track track = new Track(drawingList);
                        track.Name = reader.GetString(0);
                        track.StartStation = reader.GetString(1);
                        track.EndStation = reader.GetString(2);
                        track.PathString = reader.GetString(3);
                        if (reader.IsDBNull(6) == false)
                            track.CarAction = reader.GetString(6);
                        trackDic.Add(track.Name,track);                        
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }    
        }

        #endregion
    }
}
