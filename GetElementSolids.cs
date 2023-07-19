using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;

namespace Solids
{
    // with the IsVisibleInDynamoLibrary function, the class remains hidden and can only be used internally
    [IsVisibleInDynamoLibrary(false)]
    public class GetElementSolids
    {
        public GetElementSolids() { }

        public static List<Autodesk.Revit.DB.Solid> ReturnSolids(List<Revit.Elements.Wall> hostModelWalls)
        {
            try
            {
                Options options = new Options();
                List<Autodesk.Revit.DB.Solid> hostWallSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Revit.Elements.Wall wall in hostModelWalls)
                {
                    Autodesk.Revit.DB.Element revitWall = wall.InternalElement;

                    foreach (GeometryObject geoEle in revitWall.get_Geometry(options))
                    {
                        if (geoEle is GeometryInstance)
                        {
                            GeometryInstance geoInst = geoEle as GeometryInstance;
                            GeometryElement geoSet = geoInst.GetInstanceGeometry();

                            foreach (GeometryObject geoObj in geoSet)
                            {
                                if (geoObj is Autodesk.Revit.DB.Solid)
                                {
                                    Autodesk.Revit.DB.Solid geoSolid = geoObj as Autodesk.Revit.DB.Solid;

                                    if (geoSolid.Volume > 0)
                                    {
                                        hostWallSolids.Add(geoSolid);
                                    }
                                }
                            }
                        }
                        else if (geoEle is Autodesk.Revit.DB.Solid)
                        {
                            Autodesk.Revit.DB.Solid geoSolid = geoEle as Autodesk.Revit.DB.Solid;

                            if (geoSolid.Volume > 0)
                            {
                                hostWallSolids.Add(geoSolid);
                            }
                        }
                    }
                }

                return hostWallSolids;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
