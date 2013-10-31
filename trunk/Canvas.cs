/* Author: Evren DAGLIOGLU
 * E-mail: evrenda@yahoo.com
 * This software is copyrighted to the author himself. It can be used freely for educational purposes.
 * For commercial usage written consent of the author must be taken and a reference to the author should be provided. 
 * No responsibility will be taken for any loss or damage that will occur as a result of the usage of this code. 
 * 
 * Please feel free to inform me about any bugs, problems, ideas etc.
*/

using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AGV
{
	/// <summary>
	/// Summary description for Canvas.
	/// </summary>
	public class Canvas : System.Windows.Forms.Form
	{
		private bool multipleSelect = false;
        private bool clicked = false;
        private MapDataBase mapDB;
        private Point dragStartPoint;
        private Point dragEndPoint;
		private double XMax, XMin;
		private double YMax, YMin;

		private double scaleX = 1;
		private double scaleY = 1;
		private double mainScale = 1;
        private double importScale = 0.04;

		private Point aPoint;
		//private bool sizeChanged = false;
		
		/*private Point startPoint;
		private Point endPoint;*/

		private static Point exPoint;

        private AdjacencyList adjList = new AdjacencyList(100);
        private List<Station> stationList= new List<Station>(100);
		private ArrayList drawingList;
		private ArrayList objectIdentifier;
        private CarScheduler scheduler = new CarScheduler();

		public bool onCanvas = false;
		private polyline thePolyLine = null;
		
		//private bool polyLineStarting = true;
		//private bool CanIDraw = false;
        private int objNumSelect = -1;

		private FileInfo theSourceFile;

        private Rectangle highlightedRegion = new Rectangle(0, 0, 0, 0);
        //private Button button2;
        //private Button button1;
        private Label label1;
        //private Button button3;
        //private Button button4;
        //private Button button5;
        private Car car1;
        private int canvasHeight = 800;
        private int canvasWidth = 1280;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Canvas()
		{

			//startPoint = new Point (0, 0);
			//endPoint = new Point (0, 0);
			exPoint = new Point (0, 0);

			InitializeComponent();
            mapDB = new MapDataBase("geli_agv");
            //mapDB.addMap("geli_agv");

			XMax = canvasWidth;
			YMax = canvasHeight /2;


			
			drawingList = new ArrayList ();
			objectIdentifier = new ArrayList ();
            mapDB.loadMapFromDataBase(drawingList,objectIdentifier);
            car1 = new Car("Car1",label1);
            car1.carPosEvent += carPositionChange;
            scheduler.demo(car1);
            addDrawingListToTrack(drawingList,objectIdentifier,scheduler.TrackToGo);
            scheduler.run();
            //focusCar(car1);
			//.Net Style Double Buffering/////////////////
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			//////////////////////////////////////////////
			//////////////////////////////////////////////

            loadStations();

		}

        private void btnClicked(object sender, EventArgs e) 
        {
            Button fromButton = (Button)sender;
            //Console.WriteLine(e.ToString());
            //Console.WriteLine(sender.ToString());
        }

        private void initButton(Button btn, string  name, System.EventHandler eh)
        {
            btn.BackColor = System.Drawing.Color.Blue;
            btn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btn.Location = new System.Drawing.Point(354, 92);
            btn.Name = name;
            btn.Size = new System.Drawing.Size(32, 32);            
            btn.TabIndex = 1;
            btn.TabStop = false;
            btn.Text = name;
            btn.UseVisualStyleBackColor = false;
            btn.Click += new System.EventHandler(eh);
            this.Controls.Add(btn);
        }

        private void creatStation(string name, int shapeIndex, int xOffset, int yOffset) 
        {            
            Point p = ((Line)drawingList[shapeIndex]).GetEndPoint;
            Station s = new Station(name, p.X, p.Y);
            stationList.Add(s);
            adjList.AddVertex(s);
            p.X = p.X + xOffset;
            p.Y = canvasHeight + p.Y + yOffset;
            Button btn = new Button();
            initButton(btn, name, btnClicked);
            btn.Location = p;
        }

        private void creatFork(string name, int shapeIndex) 
        {
            Point p = ((Line)drawingList[shapeIndex]).GetEndPoint;
            Station s = new Station(name, p.X, p.Y);
            stationList.Add(s);
            adjList.AddVertex(s);
        }

        private void loadStations() 
        {
            creatStation("S0",0,-50,0);
            creatStation("S1",3,0,-40);
            creatStation("S2",14,0,-40);
            creatStation("S3", 22, 0, -40);
            creatStation("S4", 27, 0, -40);
            creatStation("S5", 32, 0, -40);
            creatStation("S6", 37, 0, -40);
            creatStation("S7", 78, 0, 20);
            creatStation("S8", 86, 0, 20);
            creatStation("S9", 94, 0, 20);
            creatStation("S10", 99, 0, 20);
            creatStation("S11", 104, 0, 20);
            creatStation("S12", 109, 0, 20);
            creatStation("S13", 73, 0, -40);
            creatStation("S14", 68, 0, -40);
            creatStation("S15", 63, 0, -40);
            creatStation("S16", 58, 0, -40);
            creatStation("S17", 50, 0, -40);
            creatStation("S18", 43, 0, -40);
            creatStation("S19", 144, 0, 20);
            creatStation("S20", 139, 0, 20);
            creatStation("S21", 134, 0, 20);
            creatStation("S22", 129, 0, 20);
            creatStation("S23", 121, 0, 20);
            creatStation("S24", 114, 0, 20);
            creatFork("F0", 6);
            creatFork("F1", 17);
            creatFork("F2", 18);
            creatFork("F3", 181);
            creatFork("F4", 200);
            creatFork("F5", 202);
            creatFork("F6", 204);
            creatFork("F7", 206);
            creatFork("F8", 208);
            creatFork("F9", 210);
            creatFork("F10", 213);
            creatFork("F11", 54);
            creatFork("F12", 53);
            creatFork("F13", 117);
            creatFork("F14", 124);
            creatFork("F15", 125);
            creatFork("F16", 183);
            creatFork("F17", 184);
            creatFork("F18", 186);
            creatFork("F19", 189);
            creatFork("F20", 188);
            creatFork("F21", 191);
            creatFork("F22", 192);
            creatFork("F23", 195);
            creatFork("F24", 90);
            creatFork("F25", 89);
        }
         
        private void showForks(Graphics g, Brush b)
        {
            Pen redPen = new Pen(Color.Red,6);
            foreach( Station s in stationList)
            {
                if (s.name[0] == 'F')
                {
                    g.DrawString(s.name, new Font("宋体", 10), b, s.X, s.Y);
                    g.DrawLine(redPen, s.X-3, s.Y, s.X+3, s.Y);
                }
            }
        }

        private void addDrawingListToTrack(ArrayList drawingList,ArrayList objIdentifier,Track t) 
        {
            foreach (DrawingObject obj in objectIdentifier)						//iterates through the objects
            {
                switch (obj.shapeType)
                {
                    case 2:				//line
                        {
                            Line temp = new Line( (Line)drawingList[obj.indexNo]);
                            t.AddLine(temp);
                            break;
                        }                   
                    case 6:				//arc
                        {
                            Arc temp = new Arc( (Arc)drawingList[obj.indexNo]);                            
                            int startAngle = -(int)temp.StartAngle;
                            
                            int sweepAngle = (int) temp.SweepAngle;
                            //if (sweepAngle > 0)
                            sweepAngle = -(int)sweepAngle;
                            //else {
                            //    sweepAngle = -360 - (int)sweepAngle;
                            //}
                            t.AddArc((int)(temp.CenterPoint.X - temp.Radius),  (int)(temp.CenterPoint.Y - temp.Radius), 2 * (int)(temp.Radius), 2 * (int)(temp.Radius),startAngle,sweepAngle);
                            break;
                        }
                }
            }
        }

        private delegate void SetPosCallBack(Label label,int x, int y);
        private int lastPosX;
        private void setCarPosition(Label label,int x, int y)
        {
            if (label.InvokeRequired)
            {
                SetPosCallBack d = new SetPosCallBack(setCarPosition);
                if(!this.IsDisposed)
                    Invoke(d, new object[] { label,x, y });
            }
            else
            {
               lastPosX = label.Location.X;
                label.Location = new Point(x, y);
                if (carFocus != null)
                {
                    AutoScrollPosition = new Point( -(AutoScrollPosition.X - label.Location.X + lastPosX),0);
                }
            }
        }

        private void carPositionChange(object sender, CarEventArgs e)
        {
            int x = AutoScrollPosition.X;
            setCarPosition(e.BingdingLabel,e.Position.X + x,canvasHeight - e.Position.Y);
        }

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}

		            
            drawingList = null;
			objectIdentifier = null;
			base.Dispose(disposing);
            scheduler.stop();
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Lime;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(252, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "1";
            // 
            // Canvas
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(5000, 768);
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(1278, 779);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(1260, 800);
            this.Name = "Canvas";
            this.ShowInTaskbar = false;
            this.Text = "Canvas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Canvas_Scroll);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CanvasRenewed_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoveCanvas);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            this.ResumeLayout(false);

		}
		#endregion

		#region Drawing and Highlighting Methods

		public void Draw (Graphics g)
		{
           //int xOffSet = 0;
			Pen lePen = new Pen(Color.White, 3);
            Size scrollOffset = new Size(this.AutoScrollPosition);
       
			g.TranslateTransform( 1 + scrollOffset.Width, canvasHeight - 1 + scrollOffset.Height);

			if (YMin < 0)
				g.TranslateTransform(0, - (int)Math.Abs(YMin) );			//transforms point-of-origin to the lower left corner of the canvas.

			if (XMin < 0)
				g.TranslateTransform((int) Math.Abs(XMin), 0);
			
			//	g.SmoothingMode = SmoothingMode.AntiAlias; 
            
            showForks(g,lePen.Brush);

			foreach (DrawingObject obj in objectIdentifier)						//iterates through the objects
			{
				switch (obj.shapeType)
				{
					case 2:				//line
					{
						Line temp = (Line) drawingList[obj.indexNo];

						lePen.Color = temp.ContourColor;
						lePen.Width = temp.LineWidth;

						
						highlightedRegion.Location = temp.GetStartPoint;

						highlightedRegion.Width = temp.GetStartPoint.X - temp.GetEndPoint.X;
						highlightedRegion.Height = temp.GetStartPoint.Y - temp.GetEndPoint.Y;
                        int centerX = (temp.GetStartPoint.X + temp.GetEndPoint.X)/2;
                        int centerY = (temp.GetStartPoint.Y + temp.GetEndPoint.Y)/2;
						if (mainScale == 0)
							mainScale = 1;

						temp.Draw(lePen, g, mainScale);                        
                        g.DrawString("L"+obj.indexNo,new Font("宋体",10),lePen.Brush,centerX,centerY);

						break;
					}
					case 3:				//rectangle 
					{
						
						rectangle temp = (rectangle) drawingList[obj.indexNo];

						lePen.Color = temp.ContourColor;
						lePen.Width = temp.LineWidth;


						temp.Draw(lePen, g);

						break;
					}
					case 4:				//circle
					{

						circle temp = (circle) drawingList[obj.indexNo];
					
						lePen.Color = temp.ContourColor;
						lePen.Width = temp.LineWidth;

						if (mainScale == 0)
							mainScale = 1;

						temp.Draw(lePen, g, mainScale);

						break;
					}
					case 5:				//polyline
					{
						polyline temp = (polyline) drawingList[obj.indexNo];
					
						lePen.Color = temp.ContourColor;
						lePen.Width = temp.LineWidth;

						if (mainScale == 0)
							mainScale = 1;

						temp.Draw(lePen, g, mainScale);

						break;
					}
					case 6:				//arc
					{
						Arc temp = (Arc) drawingList[obj.indexNo];

						lePen.Color = temp.ContourColor;
						lePen.Width = temp.LineWidth;

						if (mainScale == 0)
							mainScale = 1;

						temp.Draw(lePen, g, mainScale);
                        g.DrawString("A"+obj.indexNo,new Font("宋体",10),lePen.Brush,temp.CenterPoint);
						break;
					}
				}				
			}

						
			//	g.Dispose();		//not disposed because "g" is get from the paintbackground event..
			lePen.Dispose();
		}


		public bool HiglightObject ()			//Highlighting the object with mouse
		{
			Graphics daGe = this.CreateGraphics();

            objNumSelect = -1;
			foreach (DrawingObject obj in objectIdentifier)			//iterates through the objects and send the relevant info to the checkLineProximity(...) module
			{
				if (checkLineProximity(obj.indexNo, obj.shapeType, daGe) == true)		
				{
					this.Cursor = Cursors.Cross;                    
					//CanIDraw = true;
                    objNumSelect = obj.indexNo;
					if (multipleSelect == false)
						return true;
				}
							
			}
			return false;

		}



		private bool checkLineProximity(int indexno, int identifier, Graphics daGe)	//checks whether if the mouse pointer is on an object (i.e. shape)
		{
			Graphics g = daGe;
			Pen lePen = new Pen(Color.Yellow, 1);

			//g = this.CreateGraphics();
			
			g.TranslateTransform(8 , canvasHeight+8 );	//transforms point-of-origin to the lower left corner of the canvas.

			g.SmoothingMode = SmoothingMode.HighQuality; 

			switch (identifier)											//depending on the "identifier" value, the relevant object will be highlighted
			{
				case 2:		//Line
				{
					Line line = (Line) drawingList[indexno];
					
					if (mainScale == 0)
						mainScale = 1;

					if (line.Highlight(lePen, g, aPoint, mainScale))
					{
						this.Cursor = Cursors.Hand;
						line.highlighted = true;
						return true;
					}
				
					break;			

				}
				case 3:		//rectangle
				{
					rectangle rect = (rectangle) drawingList[indexno];

					if (rect.Highlight(lePen, g, aPoint))
					{
						this.Cursor = Cursors.Hand;
						rect.highlighted = true;
						return true;
					}

					break;
				}
				case 4:		//circle
				{
					circle tempCircle= (circle) drawingList[indexno];

					if (mainScale == 0)
						mainScale = 1;

					if (tempCircle.Highlight(lePen, g, aPoint, mainScale))
					{
						this.Cursor = Cursors.Hand;
						tempCircle.highlighted = true;
						return true;
					}
				
					break;
				}
				case 5:		//polyline
				{
					polyline tempPoly = (polyline) drawingList[indexno];

					if (mainScale == 0)
						mainScale = 1;

					if (tempPoly.Highlight(lePen, g, aPoint, mainScale))
					{
						this.Cursor = Cursors.Hand;
						tempPoly.highlighted = true;
						return true;
					}
					break;
				}
				case 6:		//arc
				{
					Arc tempArc = (Arc) drawingList[indexno];

					if (mainScale == 0)
						mainScale = 1;

					if (tempArc.Highlight(lePen, g, aPoint, mainScale))
					{
						this.Cursor = Cursors.Hand;
						tempArc.highlighted = true;
						return true;
					}
					break;
				}
			}

			return false;
		}




		#endregion

		#region Helper Methods


		/*private double CalculateRadius()		//this helper function is used to calculate the radius for the circle-drawing mode.
		{
			double circleRadius = Math.Sqrt( (endPoint.X - startPoint.X)*(endPoint.X - startPoint.X) + (endPoint.Y - startPoint.Y)*(endPoint.Y - startPoint.Y) );
			return circleRadius;
		}*/


		public void RecalculateScale()
		{
			/*if (XMax > this.pictureBox1.Size.Width)
				scaleX = (double) (this.pictureBox1.Size.Width) / (double) XMax;
			
			if (YMax > this.pictureBox1.Size.Height)
				scaleY = (double) (this.pictureBox1.Size.Height) / (double) YMax;
			
			mainScale = Math.Min(scaleX, scaleY);*/
            adjViewScale();
		}

		protected override void DefWndProc(ref Message m)		//DefWndProc is overriden to capture left mouse click on the title bar of the canvas...
		{

			
			const int WM_NCLBUTTONDOWN = 0x0A1;
            const int WM_NCMOUSEMOVE = 0x0A0;
			//const int WM_STYLECHANGED = 0x07D;

			
			switch (m.Msg)
			{
					
				case WM_NCLBUTTONDOWN:
				{

					//clicked = true;
					break;
				}
                case WM_NCMOUSEMOVE:
				{

					break;
				}

			}
            
			base.DefWndProc(ref m);
			
		}


		#endregion
        
        #region DXF Data Extraction and Interpretation

        //Reads a text file (in fact a DXF file) for importing an Autocad drawing.
        //In the DXF File structure, data is stored in two-line groupings ( or bi-line, coupling line ...whatever you call it)
        //in this grouping the first line defines the data, the second line contains the data value.
        //..as a result there is always even number of lines in the DXF file..
        public void ReadFromFile (string textFile)																		
		{
			string line1, line2;							//these line1 and line2 is used for getting the a/m data groups...

			line1 = "0";									//line1 and line2 are are initialized here...
			line2 = "0";

			//long position = 0;

			theSourceFile = new FileInfo (textFile);		//the sourceFile is set.

			StreamReader reader = null;						//a reader is prepared...

			try
			{
				reader = theSourceFile.OpenText();			//the reader is set ...
			}
			catch (FileNotFoundException e)
			{
				MessageBox.Show(e.FileName.ToString() + " cannot be found");
			}
			catch
			{
				MessageBox.Show("An error occured while opening the DXF file");
				return;
			}


			

			do
			{
				////////////////////////////////////////////////////////////////////
				//This part interpretes the drawing objects found in the DXF file...
				////////////////////////////////////////////////////////////////////

				if (line1 == "0" && line2 == "LINE")
					LineModule(reader);

				else if (line1 == "0" && line2 == "LWPOLYLINE")
					PolylineModule(reader);

				else if (line1 == "0" && line2 == "CIRCLE")
					CircleModule(reader);

				else if (line1 == "0" && line2 == "ARC")
					ArcModule(reader);

				////////////////////////////////////////////////////////////////////
				////////////////////////////////////////////////////////////////////


				GetLineCouple (reader, out line1, out line2);		//the related method is called for iterating through the text file and assigning values to line1 and line2...
				
			}
			while (line2 != "EOF");



			reader.DiscardBufferedData();							//reader is cleared...
			theSourceFile = null;
			

			reader.Close();											//...and closed.

		}


		private void GetLineCouple (StreamReader theReader, out string line1, out string line2)		//this method is used to iterate through the text file and assign values to line1 and line2
		{
			string t1 = "1.500";
			string t2 = "1,500";

			decimal dotcheck = Convert.ToDecimal(t1);
			decimal commacheck = Convert.ToDecimal(t2);

			line1 = line2 = "";

			if (theReader == null)
				return;
            
			line1 = theReader.ReadLine();
			if (line1 != null)
			{
				line1 = line1.Trim();

				if (dotcheck > commacheck)
					line1 = line1.Replace('.', ',');

			}
			line2 = theReader.ReadLine();
			if (line2 != null)
			{
				line2 = line2.Trim();
                
				if (dotcheck > commacheck)
					line2 = line2.Replace('.', ',');
			}

		}

        private void adjViewScale() 
        { 
            if ((Math.Abs(XMax-XMin)) > canvasWidth)
			{
				scaleX = (double) (canvasWidth) / (double) (Math.Abs(XMax-XMin));
			}
			else
				scaleX = 1;


			if ((Math.Abs(YMax-YMin)) > canvasHeight)
			{
				scaleY = (double) (canvasHeight) / (double) (Math.Abs(YMax-YMin));
			}
			else
				scaleY = 1;

			//mainScale = Math.Min(scaleX, scaleY);
            mainScale = 1;
        }

		private void LineModule (StreamReader reader)		//Interpretes line objects in the DXF file
		{
			string line1, line2;
			line1 = "0";
			line2 = "0";

			double x1= 0;
			double y1 = 0;
			double x2= 0;
			double y2 = 0;

			do
			{
				GetLineCouple (reader, out line1, out line2);

				if (line1 == "10")//start point  x value
				{
					x1 = Convert.ToDouble(line2); 
					
					if (x1>XMax)
						XMax = x1;

					if (x1 < XMin)
						XMin = x1;
				}
                
				if (line1 == "20")//start point x value
				{
					y1 = Convert.ToDouble(line2); 
					if (y1 > YMax)
						YMax = y1;

					if (y1 < YMin)
						YMin = y1;
				}

				if (line1 == "11")//end point x value
				{
					x2 = Convert.ToDouble(line2); 

					if (x2 > XMax)
						XMax = x2;

					if (x2 < XMin)
						XMin = x2;
				}
				
				if (line1 == "21")//end point y value
				{
					y2 = Convert.ToDouble(line2); 
					
					if (y2 > YMax)
						YMax = y2;

					if (y2 < YMin)
						YMin = y2;
				}

				
			}
			while (line1 != "21");

			
		
			//****************************************************************************************************//
			//***************This Part is related with the drawing editor...the data taken from the dxf file******//
			//***************is interpreted hereinafter***********************************************************//



            adjViewScale();

            Point startPoint1 = new Point((int)(x1*importScale), (int) -(y1*importScale));
            Point endPoint1 = new Point((int)(x2*importScale), (int)-(y2*importScale));
			int ix = drawingList.Add(new Line (startPoint1, endPoint1 , Color.White, 1));
			objectIdentifier.Add (new DrawingObject (2, ix));
            //mapDB.addLine(startPoint1, endPoint1, ix);
            //mapDB.addShapeInfo(ix, "line");
            //mapDB.modifyLine(startPoint1,endPoint1,ix);
			///////////////////////////////////////////////////////////////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////////////////////////////////////////
			
		}


		private void PolylineModule (StreamReader reader)	//Interpretes polyline objects in the DXF file
		{
			string line1, line2;
			line1 = "0";
			line2 = "0";

			double x1= 0;
			double y1 = 0;
			//double x2= 0;
			//double y2 = 0;
            			

			thePolyLine = new polyline (Color.White, 1);
			
			int ix = drawingList.Add(thePolyLine);
			objectIdentifier.Add (new DrawingObject (5, ix));

			int counter = 0;
			int numberOfVertices = 1;
			int openOrClosed = 0;
			ArrayList pointList = new ArrayList();
            
			
			do
			{
				GetLineCouple (reader, out line1, out line2);

				if (line1 == "90")
					numberOfVertices = Convert.ToInt32(line2);

				if (line1 == "70")
					openOrClosed = Convert.ToInt32(line2);
				

				if (line1 == "10")
				{
					x1 = Convert.ToDouble(line2); 
					if (x1 > XMax)
						XMax = x1;

					if	(x1 < XMin)
						XMin = x1;
				}
                
				if (line1 == "20")
				{
					y1 = Convert.ToDouble(line2); 
				
					if (y1 > YMax)
						YMax = y1;

					if (y1 < YMin)
						YMin = y1;

					pointList.Add(new Point((int)(x1*importScale), (int)-(y1*importScale)));
					counter++;
				}

			}
			while(counter < numberOfVertices);
				
			//****************************************************************************************************//
			//***************This Part is related with the drawing editor...the data taken from the dxf file******//
			//***************is interpreted hereinafter***********************************************************//


			for (int i = 1; i<numberOfVertices; i++)
			{
				thePolyLine.AppendLine (new Line ( (Point)pointList[i-1], (Point)pointList[i],Color.White, 1));
			}

			if (openOrClosed == 1)
				thePolyLine.AppendLine (new Line ( (Point)pointList[numberOfVertices-1], (Point)pointList[0],Color.White, 1));

			if ((Math.Abs(XMax-XMin)) > canvasWidth)
			{
				scaleX = (double) (canvasWidth) / (double) (Math.Abs(XMax-XMin));
			}
			else
				scaleX = 1;


			if ((Math.Abs(YMax-YMin)) > canvasHeight)
			{
				scaleY = (double) (canvasHeight) / (double) (Math.Abs(YMax-YMin));
			}
			else
				scaleY = 1;

			mainScale = Math.Min(scaleX, scaleY);

			//////////////////////////////////////////////////////////////////////////////////////////////////////
			//////////////////////////////////////////////////////////////////////////////////////////////////////


		}


		private void CircleModule (StreamReader reader)		//Interpretes circle objects in the DXF file
		{
			string line1, line2;
			line1 = "0";
			line2 = "0";

			double x1= 0;
			double y1 = 0;

			double radius = 0;

			do
			{
				GetLineCouple (reader, out line1, out line2);

				if (line1 == "10")//center point x value
				{
					x1 = Convert.ToDouble(line2);
					
				}


				if (line1 == "20")//center point y value
				{
					y1 = Convert.ToDouble(line2);
					
				}


				if (line1 == "40")//radius
				{
					radius = Convert.ToDouble(line2);

					if ( (x1 + radius) > XMax)
						XMax = x1 + radius;

					if ( (x1 - radius) < XMin)
						XMin = x1 - radius;

					if (y1 + radius > YMax)
						YMax = y1 + radius;

					if ( (y1 - radius) < YMin)
						YMin = y1 - radius;

				}



			}
			while(line1 != "40");

			//****************************************************************************************************//
			//***************This Part is related with the drawing editor...the data taken from the dxf file******//
			//***************is interpreted hereinafter***********************************************************//


			if ((Math.Abs(XMax-XMin)) > canvasWidth)
			{
				scaleX = (double) (canvasWidth) / (double) (Math.Abs(XMax-XMin));
			}
			else
				scaleX = 1;


			if ((Math.Abs(YMax-YMin)) > canvasHeight)
			{
				scaleY = (double) (canvasHeight) / (double) (Math.Abs(YMax-YMin));
			}
			else
				scaleY = 1;

			mainScale = Math.Min(scaleX, scaleY);


			int ix = drawingList.Add(new circle (new Point ((int)(x1*importScale), (int)-(y1*importScale)), (int)(radius*importScale), Color.White, Color.Red, 1));
			objectIdentifier.Add (new DrawingObject (4, ix));

			//////////////////////////////////////////////////////////////////////////////////////////////////////
			//////////////////////////////////////////////////////////////////////////////////////////////////////
			
		}


		private void ArcModule (StreamReader reader)		//Interpretes arc objects in the DXF file
		{
			string line1, line2;
			line1 = "0";
			line2 = "0";

			double x1= 0;
			double y1 = 0;

			double radius = 0;
			double angle1 = 0;
			double angle2 = 0;

			do
			{
				GetLineCouple (reader, out line1, out line2);

				if (line1 == "10")//center point x value
				{
					x1 = Convert.ToDouble(line2);
					if (x1 > XMax)
						XMax = x1;
					if (x1 < XMin)
						XMin = x1;

				}


				if (line1 == "20")//center point y value
				{
					y1 = Convert.ToDouble(line2);
					if (y1 > YMax)
						YMax = y1;
					if (y1 < YMin)
						YMin = y1;
				}


				if (line1 == "40")// radius 
				{
					radius = Convert.ToDouble(line2);

					if ( (x1 + radius) > XMax)
						XMax = x1 + radius;

					if ( (x1 - radius) < XMin)
						XMin = x1 - radius;

					if (y1 + radius > YMax)
						YMax = y1 + radius;

					if ( (y1 - radius) < YMin)
						YMin = y1 - radius;
				}

				if (line1 == "50")//start angle
					angle1 = Convert.ToDouble(line2);

				if (line1 == "51")//end angle
					angle2 = Convert.ToDouble(line2);


			}
			while(line1 != "51");


			//****************************************************************************************************//
			//***************This Part is related with the drawing editor...the data taken from the dxf file******//
			//***************is interpreted hereinafter***********************************************************//


            adjViewScale();

            Point center = new Point((int)(x1*importScale), (int)-(y1*importScale));
            int sweepAngle = (int)(angle2 - angle1);
            if (sweepAngle < 0)
                sweepAngle += 360;
			int ix = drawingList.Add(new Arc (center, (int) (radius*importScale), angle1, sweepAngle, Color.White, Color.Red, 1));
			objectIdentifier.Add (new DrawingObject (6, ix));
            //mapDB.addArc(center, (int) (radius*importScale),(int)angle1,(int)(angle2 - angle1),ix);
            //mapDB.addShapeInfo(ix, "arc");
            //mapDB.modifyArc(center,(int)(radius*importScale),(int)angle1,(int)(angle2 - angle1),ix);
			//////////////////////////////////////////////////////////////////////////////////////////////////////
			//////////////////////////////////////////////////////////////////////////////////////////////////////

		}


		#endregion

		#region Events
		
		private void OnSizeChanged(object sender, System.EventArgs e)
		{
			
			RecalculateScale();
			
			Refresh();
			
			//sizeChanged = true;

			
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)		//all drawing is made here in OnPaintBackground...
		{
			
			base.OnPaintBackground(e);

			if (this.WindowState == FormWindowState.Minimized)
				return;
            
            //this.pictureBox1.Location = new Point(0,0);
			Graphics g = e.Graphics;

            
			//Rectangle rect = new Rectangle(new Point(0,0), new Size(canvasWidth,canvasHeight));            

			/*System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
																											rect, 
																											Color.SteelBlue, 
																											Color.Black, 
																											System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);		
             */
            if (this.WindowState != FormWindowState.Minimized)
			{
				//e.Graphics.FillRectangle(brush, rect);
				
				Draw(g);				//All drawing is made here...
			}
		
			g = null;
			//brush.Dispose();
		}


		protected override void OnResize(EventArgs e)
		{
			
			if (this.Width < 500) 
			{
				this.Width = 500;
				return;
			}
			if (this.Height < 400)
			{
				this.Height = 400;
				return;
			}
			
            
			base.OnResize(e);            

		}

        //这个函数好流弊啊,用了以后腰不酸腿不痛
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }  

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    // 处理代码
                    break;
                case Keys.Right:
                    // 处理代码
                    break;
                case Keys.Up:
                    car1.Speed++;
                    break;
                case Keys.Down:
                    car1.Speed--;
                    break;
                case Keys.Shift | Keys.ShiftKey:
                    multipleSelect = true;
                    break;
            }
            return base.ProcessDialogKey(keyData);
        }

		public void CanvasRenewed_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			multipleSelect = false;		
		}


		#endregion

        private Car carFocus = null;
        private void focusCar(Car car)
        {
            carFocus = car;
        }

		#region Mouse Events

       
		private void MouseMoveCanvas(object sender, System.Windows.Forms.MouseEventArgs e)		//mousemove event...while the "shift" button is pressed down, the shapes can be highlighted...
		{
            Size scrollOffset = new Size(this.AutoScrollPosition);
            aPoint.X = e.X  - (int) Math.Abs(XMin) - 1 - scrollOffset.Width;
			aPoint.Y = e.Y  - canvasHeight + (int) Math.Abs(YMin) + 1 - scrollOffset.Height;
           
            
			Rectangle rect = this.ClientRectangle;
        
			if (rect.Contains(new Point(e.X , e.Y)))
			{
                this.Cursor = Cursors.Cross;              
				onCanvas = true;
			}
			else
			{
				this.Cursor = Cursors.Arrow;
				onCanvas = false;
			}

            if (clicked)
            {
                dragEndPoint = e.Location;
                int dragOffset = dragEndPoint.X - dragStartPoint.X;
                dragStartPoint = dragEndPoint;
                int x = -(AutoScrollPosition.X + dragOffset);
                this.AutoScrollPosition = new Point(x, 0);                
            }

			if (onCanvas == true)
			{
				if (multipleSelect)
					HiglightObject();
				
				Refresh();			                	
			}            
		}


		#endregion

        private void Canvas_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }



        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            clicked = true;
            dragStartPoint = e.Location;
            //dragStartPoint.X = dragStartPoint.X - this.AutoScrollPosition.X;
        }
        private LineEdit dlgLineEdit;
        private ArcEdit dlgArcEdit;
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            clicked = false;
            if (objNumSelect >= 0) { 
                DrawingObject obj = (DrawingObject)objectIdentifier[objNumSelect];
                if (obj.shapeType == 2) { 
                    Line line = (Line)(drawingList[obj.indexNo]);             
                    Point startPoint = new Point((int)(line.GetStartPoint.X * mainScale),(int)(line.GetStartPoint.Y * (-mainScale)));
                    Point endPoint = new Point((int)(line.GetEndPoint.X * mainScale), (int)(line.GetEndPoint.Y * (-mainScale)));
                    dlgLineEdit = new LineEdit(startPoint,endPoint,obj.indexNo);
                    dlgLineEdit.Owner = this;
                    DialogResult res = dlgLineEdit.ShowDialog();
                    if (res ==  DialogResult.OK) {                        
                        mapDB.modifyLine(dlgLineEdit.StartPoint,dlgLineEdit.EndPoint,obj.indexNo);
                    }
                    else if (res == DialogResult.Yes)
                    {
                        int ix = drawingList.Add(new Line(dlgLineEdit.StartPoint, dlgLineEdit.EndPoint, Color.White, 1));
                        objectIdentifier.Add(new DrawingObject(2, ix));
                        mapDB.addLine(dlgLineEdit.StartPoint, dlgLineEdit.EndPoint, ix);
                        mapDB.addShapeInfo(ix, "line");
                    }
                    else if (res == DialogResult.No)
                    {
                        int centerX = (dlgLineEdit.StartPoint.X + dlgLineEdit.EndPoint.X)/2;
                        int centerY = (dlgLineEdit.StartPoint.Y + dlgLineEdit.EndPoint.Y)/2;                        
                        mapDB.modifyLine(dlgLineEdit.StartPoint, new Point( centerX, centerY), obj.indexNo);
                        int ix = drawingList.Add(new Line(new Point(centerX,centerY), dlgLineEdit.EndPoint, Color.White, 1));
                        objectIdentifier.Add(new DrawingObject(2, ix));
                        mapDB.addLine(new Point(centerX, centerY), dlgLineEdit.EndPoint, ix);
                        mapDB.addShapeInfo(ix, "line");
                    }
                    dlgLineEdit.Dispose();
                    //dlgLineEdit.Activate();
                    //dlgLineEdit.Focus();
                }
                else if (obj.shapeType == 6) {
                    Arc arc1 = (Arc)(drawingList[obj.indexNo]);
                    Point o = arc1.CenterPoint;
                    dlgArcEdit = new ArcEdit((int)(o.X * mainScale),-(int)(o.Y * mainScale),(int)(arc1.Radius * mainScale),(int)arc1.StartAngle,(int)(arc1.SweepAngle),obj.indexNo);
                    dlgArcEdit.Owner = this;
                    if (dlgArcEdit.ShowDialog() == DialogResult.OK) { 
                        mapDB.modifyArc(dlgArcEdit.Center,dlgArcEdit.Radius,dlgArcEdit.StartAngle,dlgArcEdit.SweepAngle,obj.indexNo);
                    }                    
                    dlgArcEdit.Dispose();
                }
                multipleSelect = false;
                drawingList.Clear();
                objectIdentifier.Clear();
                scheduler.TrackToGo.clear();                
                mapDB.loadMapFromDataBase(drawingList,objectIdentifier);
                addDrawingListToTrack(drawingList, objectIdentifier, scheduler.TrackToGo);
            }
            objNumSelect = -1;
               
        }

        //private bool focus = false;
        /*private void button2_Click(object sender, EventArgs e)
        {
            focus = !focus;
            if (focus)
                focusCar(car1);
            else
                focusCar(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            car1.Speed = 90;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            car1.Speed = 60;
        }
        */
	}
}
