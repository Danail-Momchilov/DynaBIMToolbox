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
using System.Xml.Linq;
using Revit.GeometryConversion;


// TO DO! Research NodeModel nodes with custom UI


// namespace : interpreted as a main category in the package
namespace Solids
{
    // class : interpreted as a subcategory 
    public class BooleanAPI
    {
        // constructors : labelled as a create node in the subcategory (if private and empty it will not be displayed) : it is preferable to use the "By" keyword, when possible
        private BooleanAPI() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// Extremely fast RevitAPI intersection between solids and surfaces
        /// </summary>
        /// <param name=""> </param>
        /// <param name=""> </param>
        /// <returns> List of numbers, representing the area to be deducted from each surface after the intersection </returns>
        /// <search> surface, solid, intersection, API </search>
        public static List<Autodesk.Revit.DB.Solid> SurfaceSolidAPIIntersectionAreas(List<Revit.Elements.Wall> hostModelWalls, List<Revit.Elements.Wall> linkedModelWalls)
        {
            try
            {
                // get solids of host model walls
                List<Autodesk.Revit.DB.Solid> hostedWallsSolids = GetElementSolids.ReturnSolids(hostModelWalls);

                // get solids of linked model walls
                List<Autodesk.Revit.DB.Solid> linkedWallsSolids = GetElementSolids.ReturnSolids(linkedModelWalls);

                return linkedWallsSolids;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}