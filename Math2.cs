using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;

// load Runtime module in order to use the MultiReturn attribute : more info on multiple return values in the Dynamo Primer
using Autodesk.DesignScript.Runtime;

// load Revit api
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

// load Dynamo API
using Revit.Elements;
using RevitServices.Persistence;


// TO DO! Research NodeModel nodes with custom UI


// namespace : interpreted as a main category in the package
namespace Solids
{
    // class : interpreted as a subcategory 
    public class Math2
    {
        // constructors : labelled as a create node in the subcategory (if private and empty it will not be displayed) : it is preferable to use the "By" keyword, when possible
        private Math2() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// This method multiplies the two input variables
        /// </summary>
        /// <param name="xCount"> The x value to be multiplied </param>
        /// <param name="yCount"> The x value to be multiplied </param>
        /// <returns> x * y </returns>
        /// <search> grid, rectangle </search>
        public static double Multiply(int xCount, int yCount)
        {
            double x = xCount;
            double y = yCount;

            return x * y;
        }

        public static double Plus(int xCount, int yCount)
        {
            double x = xCount;
            double y = yCount;

            return x + y;
        }

        public static Revit.Elements.Element UnwrapWrapElement(Revit.Elements.Element element)
        {
            try
            {
                Autodesk.Revit.DB.Element UnwrappedElement = element.InternalElement;
                Revit.Elements.Element WrappedElement = UnwrappedElement.ToDSType(true);
                return WrappedElement;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // properties : interpreted as Query nodes
        private double Number3 { get; set; }
        private double _Number3 { get; set; }
    }
}