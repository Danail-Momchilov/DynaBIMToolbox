﻿using Autodesk.DesignScript.Geometry;
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
using DynaBIMToolbox.Invisible;
using System.Linq.Expressions;
using System;
using System.Security.Cryptography;
using System.Linq;


// TO DO! Research NodeModel nodes with custom UI

// namespace : interpreted as a main category in the package
namespace GeometryAPI
{
    // class : interpreted as a subcategory 
    public class SolidsAPI
    {
        // constructors : labelled as a create node in the subcategory (if private and empty it will not be displayed) : it is preferable to use the "By" keyword, when possible
        private SolidsAPI() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// Get Dynamo wall and return its solid geometry as Revit API solid
        /// </summary>
        /// <param name="hostModelWall"> Wall || A wall instance from the host model </param>
        /// <returns> Autodesk.DB.Solid || The solid, representing the wall Geometry in the API </returns>
        /// <search> wall, solid, API </search>
        public static Autodesk.Revit.DB.Solid GetWallSolid(Revit.Elements.Wall hostModelWall)
        {
            try
            {
                List<Revit.Elements.Wall> wallList = new List<Revit.Elements.Wall> { hostModelWall };
                // get solids of host model walls
                List<Autodesk.Revit.DB.Solid> hostedWallsSolids = SolidConversions.ReturnWallsSolids(wallList);

                return hostedWallsSolids[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get the Autodesk.DB.Solid for the specified wall from linked model
        /// </summary>
        /// <param name="linkModelWall"> Wall || A wall instance from linked model </param>
        /// <param name="linkInstance"> LinkInstance || Revit Link Instance </param>
        /// <returns> Autodesk.DB.Solid || The solid, representing the wall Geometry in the API </returns>
        /// <search> wall, solid, API </search>
        public static Autodesk.Revit.DB.Solid GetWallSolidTransformed(Revit.Elements.Wall linkModelWall, RevitLinkInstance linkInstance)
        {
            try
            {
                List<Revit.Elements.Wall> wallList = new List<Revit.Elements.Wall> { linkModelWall };
                // get solids of host model walls
                List<Autodesk.Revit.DB.Solid> hostedWallsSolids = SolidConversions.ReturnWallsSolids(wallList);

                return SolidConversions.ReturnTransformedSolid(hostedWallsSolids[0], linkInstance);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get all the solids of an Element, unite them and return the united solid with transformed coordinate system, based on the link instance. Works for all objects, classified as Dynamo Elements, such as Windows, Doors, etc.
        /// </summary>
        /// <param name="element"> Element || Revit.Elements.Element </param>
        /// <returns> Autodesk.DB.Solid || A single solid, representing all element solids in the API </returns>
        /// <search> element, solid, API </search>
        public static Autodesk.Revit.DB.Solid GetAndUniteElementSolids(Revit.Elements.Element element)
        {
            try
            {
                List<Revit.Elements.Element> elemList = new List<Revit.Elements.Element> { element };
                List<Autodesk.Revit.DB.Solid> elemSolids = SolidConversions.ReturnElementsSolids(elemList);

                return SolidConversions.UniteSolids(elemSolids);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get all the solids of an Element, unite them and return the united solid with transformed coordinate system, based on the link instance. Works for all objects, classified as Dynamo Elements, such as Windows, Doors, etc.
        /// </summary>
        /// <param name="element"> Element || Revit.Elements.Element </param>
        /// <param name="linkInstance"> LinkInstance || Revit Link Instance </param>
        /// <returns> Autodesk.DB.Solid || A single solid, representing all element solids in the API </returns>
        /// <search> element, solid, API </search>
        public static Autodesk.Revit.DB.Solid GetAndUniteElementSolidsTransformed(Revit.Elements.Element element, RevitLinkInstance linkInstance)
        {
            try
            {
                List<Revit.Elements.Element> elemList = new List<Revit.Elements.Element> { element };
                List<Autodesk.Revit.DB.Solid> elemSolids = SolidConversions.ReturnElementsSolids(elemList);

                return SolidConversions.ReturnTransformedSolid(SolidConversions.UniteSolids(elemSolids), linkInstance);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Wrap Revit Autodesk.Revit.DB.Solid to get Dynamo solid
        /// </summary>
        /// <param name="solid"> Solid || Autodesk.Revit.DB.Solid </param>
        /// <returns> Autodesk.DesignScript.Geometry || Dynamo Solid </returns>
        /// <search> solid, API, Dynamo </search>
        public static Autodesk.DesignScript.Geometry.Solid ReturnDynamoSolid(Autodesk.Revit.DB.Solid solid)
        {
            try
            {
                return solid.ToProtoType();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create Autodesk.Revit.DB.Solid from a single curve, by offseting the curve in both direction, creating closed curveloop and extruding it
        /// </summary>
        /// <param name="line"> [points] || A straight Dynamo line </param>
        /// <param name="width"> height || A specified width for the offset </param>
        /// <param name="height"> Height for the extrusion </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit API solid </returns>
        /// <search> solid, API, Dynamo </search>
        public static Autodesk.Revit.DB.Solid RevitAPIExtrusionFromCurve(Autodesk.DesignScript.Geometry.Curve line, double width, double height)
        {
            try
            {
                return SolidConversions.CreateSolidExtrusionFromCurve(line, width, height);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create Autodesk.Revit.DB.Solid from a single curve, by offseting the curve in both direction, creating closed curveloop, extruding and transforming its coordinate system
        /// </summary>
        /// <param name="line"> Line || A straight Dynamo line </param>
        /// <param name="width"> Double || A specified width for the offset </param>
        /// <param name="height"> Double || Height for the extrusion </param>
        /// <param name="linkInstance"> Revit Link Instance </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit API solid </returns>
        /// <search> solid, API, Dynamo </search>
        public static Autodesk.Revit.DB.Solid RevitAPIExtrusionFromCurveTransformed(Autodesk.DesignScript.Geometry.Curve line, double width, double height, RevitLinkInstance linkInstance)
        {
            try
            {
                return SolidConversions.ReturnTransformedSolid(SolidConversions.CreateSolidExtrusionFromCurve(line, width, height), linkInstance);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Translate Autodesk.revit.DB.Solid along the Z axis at the specified distance
        /// </summary>
        /// <param name="solid"> Solid || Autodesk.Revit.DB.Solid </param>
        /// <param name="translation"> Number || A specified distance for vertical translation </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit API solid </returns>
        /// <search> solid, API, Dynamo, translate, vertical </search>
        public static Autodesk.Revit.DB.Solid TranslateSolidVertically(Autodesk.Revit.DB.Solid solid, double translation)
        {
            try
            {
                XYZ translationVector = new XYZ(0, 0, translation / 30.48);
                Transform translationTransform = Transform.CreateTranslation(translationVector);

                return SolidUtils.CreateTransformed(solid, translationTransform);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns element oriented bounding box for the specified element, represented as Revit API solid
        /// </summary>
        /// <param name="element"> Revit Family Instance || Tested with Windows and Doors </param>
        /// <param name="point"> Point || Dynamo Point </param>
        /// <param name="degAngle"> Decimal Degrees || Rotation Angle </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit API solid </returns>
        /// <search> solid, API, boundingbox, element oriented </search>
        public static Autodesk.Revit.DB.Solid ElementOrientedBboxSolid(Revit.Elements.Element element, Autodesk.DesignScript.Geometry.Point point, double degAngle)
        {
            try
            {
                // get all solids of the element
                List<Revit.Elements.Element> elements = new List<Revit.Elements.Element> { element };
                List<Autodesk.Revit.DB.Solid> elemSolids = SolidConversions.ReturnElementsSolids(elements);

                // define transform to align all the solids with Project North
                XYZ centerPoint = new XYZ(point.X / 30.48, point.Y / 30.48, point.Z / 30.48);
                double angleRadians = -degAngle * (Math.PI / 180);
                Transform rotationTransform = Transform.CreateRotationAtPoint(XYZ.BasisZ, angleRadians, centerPoint);

                // unite and transform solids
                Autodesk.Revit.DB.Solid unitedSolid = SolidConversions.UniteSolids(elemSolids);
                Autodesk.Revit.DB.Solid rotatedSolid = SolidUtils.CreateTransformed(unitedSolid, rotationTransform);

                // get and transform element bounding box
                BoundingBoxXYZ bbox = rotatedSolid.GetBoundingBox();
                Transform bboxTransform = bbox.Transform;

                // convert the bounding box to solid, rotate it back and return
                Autodesk.Revit.DB.Solid bboxSolid = SolidUtils.CreateTransformed(SolidConversions.BoundingBoxToSolid(bbox), bboxTransform);
                rotationTransform = Transform.CreateRotationAtPoint(XYZ.BasisZ, -angleRadians, centerPoint);
                return SolidUtils.CreateTransformed(bboxSolid, rotationTransform);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns element oriented bounding box for the specified element, represented as Revit API solid, transformed, based on the link instance coordinate system
        /// </summary>
        /// <param name="element"> Revit Family Instance || Tested with Windows and Doors </param>
        /// <param name="point"> Point || Dynamo Point </param>
        /// <param name="degAngle"> Decimal Degrees || Rotation Angle </param>
        /// <param name="linkInstance"> Revit Link Instance </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit API solid </returns>
        /// <search> solid, API, boundingbox, element oriented </search>
        public static Autodesk.Revit.DB.Solid ElementOrientedBboxSolidTransformed(Revit.Elements.Element element, Autodesk.DesignScript.Geometry.Point point, double degAngle, RevitLinkInstance linkInstance)
        {
            try
            {
                // get all solids of the element
                List<Revit.Elements.Element> elements = new List<Revit.Elements.Element> { element };
                List<Autodesk.Revit.DB.Solid> elemSolids = SolidConversions.ReturnElementsSolids(elements);

                // define transform to align all the solids with Project North
                XYZ centerPoint = new XYZ(point.X / 30.48, point.Y / 30.48, point.Z / 30.48);
                double angleRadians = -degAngle * (Math.PI / 180);
                Transform rotationTransform = Transform.CreateRotationAtPoint(XYZ.BasisZ, angleRadians, centerPoint);

                // unite and transform solids
                Autodesk.Revit.DB.Solid unitedSolid = SolidConversions.UniteSolids(elemSolids);
                Autodesk.Revit.DB.Solid rotatedSolid = SolidUtils.CreateTransformed(unitedSolid, rotationTransform);

                // get and transform element bounding box
                BoundingBoxXYZ bbox = rotatedSolid.GetBoundingBox();
                Transform bboxTransform = bbox.Transform;

                // convert the bounding box to solid, rotate it back and transform, based on the linkinstance coordinates
                Autodesk.Revit.DB.Solid bboxSolid = SolidUtils.CreateTransformed(SolidConversions.BoundingBoxToSolid(bbox), bboxTransform);
                rotationTransform = Transform.CreateRotationAtPoint(XYZ.BasisZ, -angleRadians, centerPoint);
                Autodesk.Revit.DB.Solid negativeRotationSolid = SolidUtils.CreateTransformed(bboxSolid, rotationTransform);

                return SolidConversions.ReturnTransformedSolid(negativeRotationSolid, linkInstance);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    // class : interpreted as a subcategory
    public class SurfacesAPI
    {
        // constructors : labelled as a create node in the subcategory (if private and empty it will not be displayed) : it is preferable to use the "By" keyword, when possible
        private SurfacesAPI() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// Get a list of Autodesk.Revit.DB.PlanarFace, representing all the wall faces of a room, based on specified height and base offset
        /// </summary>
        /// <param name="room"> Room </param>
        /// <param name="height"> double height </param>
        /// <param name="baseOffset"> double base offset </param>
        /// <returns> List[PlanarFace] || List of planar faces, representing wall finish surfaces </returns>
        /// <search> room, faces, API </search>
        public static List<PlanarFace> WallSurfacesFromRooms(Revit.Elements.Room room, double height, double baseOffset)
        {
            try
            {
                XYZ translationVector = new XYZ(0, 0, baseOffset / 30.48);
                Transform translationTransform = Transform.CreateTranslation(translationVector);

                double heightFeet = height / 30.48;

                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                List<Autodesk.Revit.DB.Curve> roomCurves = new List<Autodesk.Revit.DB.Curve>();

                foreach (BoundarySegment segment in revitRoom.GetBoundarySegments(options)[0])
                    roomCurves.Add(segment.GetCurve());

                CurveLoop crvLoop = CurveLoop.Create(roomCurves);
                List<CurveLoop> crvLoopList = new List<CurveLoop> { crvLoop };

                Autodesk.Revit.DB.Solid roomSolid = GeometryCreationUtilities.CreateExtrusionGeometry(crvLoopList, XYZ.BasisZ, heightFeet);
                roomSolid = SolidUtils.CreateTransformed(roomSolid, translationTransform);

                List<PlanarFace> roomFaces = new List<PlanarFace>();

                foreach (PlanarFace face in roomSolid.Faces)
                    if (SurfaceConversions.isFaceVertical(face))
                        roomFaces.Add(face);

                return roomFaces;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // methods : interpreted as a modify node
        /// <summary>
        /// Surface wrapper - gets an Autodesk.Revit.DB.Surface and returns Dynamo surface
        /// </summary>
        /// <param name="face"> Autodesk.Revit.DB.PlanarFace || Revit API PlanarFace </param>
        /// <returns> Dynamo Surface </returns>
        /// <search> PlanarFace, surface, API </search>
        public static List<Autodesk.DesignScript.Geometry.Surface> ReturnDynamoFaces(PlanarFace face)
        {
            try
            {
                List<Autodesk.DesignScript.Geometry.Surface> surfacesList = new List<Autodesk.DesignScript.Geometry.Surface>();

                foreach (Autodesk.DesignScript.Geometry.Surface surface in face.ToProtoType())
                    surfacesList.Add(surface);

                return surfacesList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class BooleanAPI
    {
        private BooleanAPI() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// Gets a list of Autodesk.Revit.DB.PlanarFace elements, as well as Revit API Solids. Generates the sum of all the faces areas, computes intersection of these faces with the solids and subtracts the summed area of all intersections
        /// </summary>
        /// <param name="face"> Autodesk.Revit.DB.PlanarFace || Revit API PlanarFace </param>
        /// <returns> Dynamo Surface </returns>
        /// <search> PlanarFace, surface, API </search>
        public static double SurfaceSolidIntersectionRemainingArea(List<PlanarFace> faces, List<Autodesk.Revit.DB.Solid> solids)
        {
            try
            {
                List<Autodesk.Revit.DB.Solid> test = new List<Autodesk.Revit.DB.Solid>();
                double intersectionAreas = 0;
                double roomWallAreas = 0;

                foreach (PlanarFace face in faces)
                {
                    roomWallAreas += face.Area * 0.092;

                    List<CurveLoop> faceCurveLoops = (List<CurveLoop>)face.GetEdgesAsCurveLoops();

                    Autodesk.Revit.DB.Solid faceSolid = GeometryCreationUtilities.CreateExtrusionGeometry(faceCurveLoops, face.FaceNormal, 0.0328083989501312);
                    Autodesk.Revit.DB.Solid faceSolidNegative = GeometryCreationUtilities.CreateExtrusionGeometry(faceCurveLoops, -face.FaceNormal, 0.0328083989501312);
                    faceSolid = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, faceSolidNegative, BooleanOperationsType.Union);

                    foreach (Autodesk.Revit.DB.Solid solid in solids)
                    {
                        Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, solid, BooleanOperationsType.Intersect);

                        if (intersection != null && intersection.Volume != 0)
                            intersectionAreas += SurfaceConversions.largestVerticalFaceArea(intersection.Faces);
                    }
                }

                return roomWallAreas - intersectionAreas;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}