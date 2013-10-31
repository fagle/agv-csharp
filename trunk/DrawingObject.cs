using System;
using System.Drawing;

namespace AGV
{
	/// <summary>
	/// Summary description for DrawingObject.
	/// </summary>
	public class DrawingObject
	{
		public int shapeType;
		public int indexNo;

		public DrawingObject (int shapeID, int ix)
		{
			shapeType = shapeID;
			indexNo = ix;
			
		}
	}

    class ShapeInfoPack
    {
        public int indexNo;
        public string shape;
        public Point center;
        public ShapeInfoPack(int ix, string shape)
        {
            indexNo = ix;
            this.shape = shape;
        }
    }
}
