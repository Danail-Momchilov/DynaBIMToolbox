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

        private static double Min(double a, double b)
        { return a < b ? a : b; }

        private static double Max(double a, double b)
        { return a > b ? a : b; }

        public static XYZ GetMinimumPoint(XYZ point1, XYZ point2)
        {
            double x = Min(point1.X, point2.X);
            double y = Min(point1.Y, point2.Y);
            double z = Min(point1.Z, point2.Z);
            return new XYZ(x, y, z);
        }

        public static XYZ GetMaximumPoint(XYZ point1, XYZ point2)
        {
            double x = Max(point1.X, point2.X);
            double y = Max(point1.Y, point2.Y);
            double z = Max(point1.Z, point2.Z);
            return new XYZ(x, y, z);
        }
    }
}
