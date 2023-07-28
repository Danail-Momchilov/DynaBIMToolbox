using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using System;
using System.Security.Cryptography;

namespace DynaBIMToolbox.Invisible
{
    // with the IsVisibleInDynamoLibrary function, the class remains hidden and can only be used internally
    [IsVisibleInDynamoLibrary(false)]
    public class SolidConversions
    {
        public SolidConversions() { }

        public static List<Autodesk.Revit.DB.Solid> ReturnWallsSolids(List<Revit.Elements.Wall> hostModelWalls)
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

        public static List<Autodesk.Revit.DB.Solid> ReturnElementsSolids(List<Revit.Elements.Element> elements)
        {
            try
            {
                Options options = new Options();
                List<Autodesk.Revit.DB.Solid> elementsSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Revit.Elements.Element element in elements)
                {
                    Element revitElement = element.InternalElement;

                    foreach (GeometryObject geoEle in revitElement.get_Geometry(options))
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
                                        elementsSolids.Add(geoSolid);
                                    }
                                }
                            }
                        }
                        else if (geoEle is Autodesk.Revit.DB.Solid)
                        {
                            Autodesk.Revit.DB.Solid geoSolid = geoEle as Autodesk.Revit.DB.Solid;

                            if (geoSolid.Volume > 0)
                            {
                                elementsSolids.Add(geoSolid);
                            }
                        }
                    }
                }

                return elementsSolids;
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

        // potential issues - Revise!
        public static Autodesk.Revit.DB.Solid CreateSolidExtrusion(List<Autodesk.DesignScript.Geometry.Point> points, double height)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }
        // potential issues - Revise!

        public static Autodesk.Revit.DB.Solid CreateSolidExtrusionFromCurve(Autodesk.DesignScript.Geometry.Curve line, double width, double height)
        {
            try
            {
                Autodesk.Revit.DB.Curve revitCurve = line.ToRevitType();

                XYZ verticalNormal = new XYZ(0, 0, 1);

                List<CurveLoop> crvLoop = new List<CurveLoop> { CurveLoop.CreateViaThicken(revitCurve, width/30.48, verticalNormal) };
                return GeometryCreationUtilities.CreateExtrusionGeometry(crvLoop, XYZ.BasisZ, height/30.48);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid UniteSolids(List<Autodesk.Revit.DB.Solid> solids)
        {
            try
            {
                if (solids == null)
                {
                    throw new ArgumentException("At least two solids are required for the boolean operation");
                }
                else
                {
                    Autodesk.Revit.DB.Solid baseSolid = solids[0];
                    List<Autodesk.Revit.DB.Solid> remainingSolids = new List<Autodesk.Revit.DB.Solid>();
                    List<Autodesk.Revit.DB.Solid> leftSolids = new List<Autodesk.Revit.DB.Solid>();

                    foreach (Autodesk.Revit.DB.Solid solid in solids)
                        try
                        {
                            baseSolid = BooleanOperationsUtils.ExecuteBooleanOperation(baseSolid, solid, BooleanOperationsType.Union);
                        }
                        catch
                        {
                            remainingSolids.Add(solid);
                        }
                    
                    if (remainingSolids.Count > 0)
                    {
                        foreach (Autodesk.Revit.DB.Solid solid in remainingSolids)
                            try
                            {
                                baseSolid = BooleanOperationsUtils.ExecuteBooleanOperation(baseSolid, solid, BooleanOperationsType.Union);
                            }
                            catch { leftSolids.Add(solid); }
                    }

                    return baseSolid;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Autodesk.Revit.DB.Solid> RotateSolidsAroundPoint(List<Autodesk.Revit.DB.Solid> solids, Autodesk.DesignScript.Geometry.Point rotationCenter, double angleDegree)
        {
            try
            {
                if (solids == null || solids.Count == 0)
                {
                    throw new ArgumentException("The list of solids is empty.");
                }

                XYZ centerPoint = new XYZ(rotationCenter.X, rotationCenter.Y, rotationCenter.Z);

                double angleRadians = angleDegree * (Math.PI / 180);

                Transform rotationTransform = Transform.CreateRotationAtPoint(XYZ.BasisZ, angleRadians, centerPoint);

                List<Autodesk.Revit.DB.Solid> rotatedSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Autodesk.Revit.DB.Solid solid in solids)
                {
                    Autodesk.Revit.DB.Solid rotatedSolid = SolidUtils.CreateTransformed(solid, rotationTransform);
                    rotatedSolids.Add(rotatedSolid);
                }

                return rotatedSolids;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static BoundingBoxXYZ BoundingBoxFromMultipleSolids(List<Autodesk.Revit.DB.Solid> solids)
        {
            try
            {
                if (solids == null || solids.Count == 0)
                {
                    throw new ArgumentException("The list of solids is empty.");
                }

                XYZ minPt = solids[0].GetBoundingBox().Min;
                XYZ maxPt = solids[0].GetBoundingBox().Max;

                if (solids.Count != 1)
                {
                    foreach (Autodesk.Revit.DB.Solid solid in solids)
                    {
                        BoundingBoxXYZ bBox = solid.GetBoundingBox();

                        minPt = PointConversions.GetMinimumPoint(minPt, bBox.Min);
                        maxPt = PointConversions.GetMaximumPoint(maxPt, bBox.Max);
                    }
                }

                BoundingBoxXYZ finalBbox = new BoundingBoxXYZ();
                finalBbox.Min = minPt;
                finalBbox.Max = maxPt;

                return finalBbox;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid BoundingBoxToSolid(BoundingBoxXYZ bbox)
        {
            XYZ minPt = bbox.Min;
            XYZ maxPt = bbox.Max;

            XYZ pt0 = minPt;
            XYZ pt1 = new XYZ(maxPt.X/30.48, minPt.Y/30.48, minPt.Z/30.48);
            XYZ pt2 = new XYZ(maxPt.X/30.48, maxPt.Y/30.48, minPt.Z/30.48);
            XYZ pt3 = new XYZ(minPt.X/30.48, maxPt.Y/30.48, minPt.Z/30.48);

            double height = maxPt.Z / 30.48 - minPt.Z / 30.48;

            List<Autodesk.Revit.DB.Curve> edges = new List<Autodesk.Revit.DB.Curve>();

            edges.Add(Autodesk.Revit.DB.Line.CreateUnbound(pt0, pt1));
            edges.Add(Autodesk.Revit.DB.Line.CreateUnbound(pt1, pt2));
            edges.Add(Autodesk.Revit.DB.Line.CreateUnbound(pt2, pt3));
            edges.Add(Autodesk.Revit.DB.Line.CreateUnbound(pt3, pt0));

            CurveLoop crvLoop = CurveLoop.Create(edges);

            List<CurveLoop> loopList = new List<CurveLoop> { crvLoop };

            return GeometryCreationUtilities.CreateExtrusionGeometry(loopList, XYZ.BasisZ, height);
        }
    }
}
