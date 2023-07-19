using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;

// namespace : interpreted as a main category in the package
namespace Solids
{
    // class : interpreted as a subcategory 
    public class Math
    {
        // constructors : labelled as a create node in the subcategory (if private and empty it will not be displayed)
        private Math() { }

        // methods : interpreted as a modify node
        public static double Multiply(int xCount, int yCount)
        {
            double x = xCount;
            double y = yCount;

            return x*y;
        }

        public static double Plus(int xCount, int yCount)
        {
            double x = xCount;
            double y = yCount;

            return x + y;
        }

        // properties : interpreted as Query nodes
        public double Number3 = 3;
    }
}