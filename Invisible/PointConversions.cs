using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaBIMToolbox.Invisible
{
    [IsVisibleInDynamoLibrary(false)]
    internal class PointConversions
    {
        public PointConversions() { }

        public static XYZ GetMinimumPoint(XYZ point1, XYZ point2)
        {
            double x = Math.Min(point1.X, point2.X);
            double y = Math.Min(point1.Y, point2.Y);
            double z = Math.Min(point1.Z, point2.Z);
            return new XYZ(x, y, z);
        }

        public static XYZ GetMaximumPoint(XYZ point1, XYZ point2)
        {
            double x = Math.Max(point1.X, point2.X);
            double y = Math.Max(point1.Y, point2.Y);
            double z = Math.Max(point1.Z, point2.Z);
            return new XYZ(x, y, z);
        }
    }
}
