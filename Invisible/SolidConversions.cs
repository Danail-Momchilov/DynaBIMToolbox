using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;

namespace DynaBIMToolbox.Invisible
{
    // with the IsVisibleInDynamoLibrary function, the class remains hidden and can only be used internally
    [IsVisibleInDynamoLibrary(false)]
    public class SolidConversions
    {
        public SolidConversions() { }

        public static List<Autodesk.Revit.DB.Solid> ReturnSolids(List<Revit.Elements.Wall> hostModelWalls)
        {
            try
            {
                Options options = new Options();
                List<Autodesk.Revit.DB.Solid> hostWallSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Revit.Elements.Wall wall in hostModelWalls)
                {
                    Element revitWall = wall.InternalElement;

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
        
        public static Autodesk.Revit.DB.Solid ReturnTransformedSolid(Autodesk.Revit.DB.Solid solid, RevitLinkInstance linkInstance)
        {
            try
            {
                Document linkDocument = linkInstance.GetLinkDocument();

                // create transform to get the link instance 
                Transform coordinateSystemTransform = Transform.Identity;

                // get the coordinate system information from the link instance
                XYZ origin = linkInstance.GetTotalTransform().Origin;
                XYZ xAxis = linkInstance.GetTotalTransform().BasisX;
                XYZ yAxis = linkInstance.GetTotalTransform().BasisY;
                XYZ zAxis = linkInstance.GetTotalTransform().BasisZ;

                // set the translation and rotation in the transform
                coordinateSystemTransform.Origin = origin;
                coordinateSystemTransform.BasisX = xAxis;
                coordinateSystemTransform.BasisY = yAxis;
                coordinateSystemTransform.BasisZ = zAxis;

                Autodesk.Revit.DB.Solid transformedSolid = SolidUtils.CreateTransformed(solid, coordinateSystemTransform);
                return transformedSolid;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid CreateSolidExtrusion(List<Autodesk.DesignScript.Geometry.Point> points, double height)
        {
            XYZ point0 = new XYZ(points[0].X / 30.48, points[0].Y / 30.48, points[0].Z / 30.48);
            XYZ point1 = new XYZ(points[1].X / 30.48, points[1].Y / 30.48, points[1].Z / 30.48);
            XYZ point2 = new XYZ(points[2].X / 30.48, points[2].Y / 30.48, points[2].Z / 30.48);
            XYZ point3 = new XYZ(points[3].X / 30.48, points[3].Y / 30.48, points[3].Z / 30.48);

            Autodesk.Revit.DB.Curve edge0 = Autodesk.Revit.DB.Line.CreateUnbound(point0, point1);
            Autodesk.Revit.DB.Curve edge1 = Autodesk.Revit.DB.Line.CreateUnbound(point1, point2);
            Autodesk.Revit.DB.Curve edge2 = Autodesk.Revit.DB.Line.CreateUnbound(point2, point3);
            Autodesk.Revit.DB.Curve edge3 = Autodesk.Revit.DB.Line.CreateUnbound(point3, point0);

            List<Autodesk.Revit.DB.Curve> edges = new List<Autodesk.Revit.DB.Curve> { edge0, edge1, edge2, edge3 };
            CurveLoop crvLoop = CurveLoop.Create(edges);
            List<CurveLoop> crvLoopList = new List<CurveLoop> { crvLoop };

            return GeometryCreationUtilities.CreateExtrusionGeometry(crvLoopList, XYZ.BasisZ, height);
        }
    }
}
