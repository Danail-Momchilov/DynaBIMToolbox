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
using Autodesk.Revit.DB.Architecture;


// TO DO! Research NodeModel nodes with custom UI

// namespace : interpreted as a main category in the package
namespace GeometryAPI
{
    // class : interpreted as a subcategory 
    /// <summary>
    /// Solids generation and handling, directly in the Revit API
    /// </summary>
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
                if (line == null)
                    return null;
                else
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
    /// <summary>
    /// Surfaces generation and handling, directly in the Revit API
    /// </summary>
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

    /// <summary>
    /// Boolean operations, directly in the Revit API
    /// </summary>
    public class BooleanAPI
    {
        private BooleanAPI() { }

        // methods : interpreted as a modify node
        /// <summary>
        /// Gets a list of Rooms, as well as Revit API Solids. Constructs room finish faces, based on the input base offset and height and returns all the face intersections
        /// </summary>
        /// <param name="room"> Revit.Elements.Rooms || Room element, wrapped through Dynamo </param>
        /// <param name="solids"> [Autodesk.Revit.DB.Solid] || List of Revit API Solids </param>
        /// <param name="baseOffset"> Double || Base offset constraint </param>
        /// <param name="height"> Double || Height </param>
        /// <returns> [Autodesk.Revit.DB.PlanarFace] || Revit Surfaces </returns>
        /// <search> room, surface, solid, API </search>
        [MultiReturn(new[] { "surfaceIntersections", "roomExceptions" })]
        public static Dictionary<string, object> RoomFacesSolidIntersection(Revit.Elements.Room room, List<Autodesk.Revit.DB.Solid> solids, double baseOffset, double height)
        {
            try
            {
                // unwrap Dynamo rooms to get Revit rooms
                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                // get room's curves from the specified room
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                List<Autodesk.Revit.DB.Curve> roomCurves = new List<Autodesk.Revit.DB.Curve>();

                foreach (BoundarySegment segment in revitRoom.GetBoundarySegments(options)[0])
                    roomCurves.Add(segment.GetCurve());

                // generate thin solids for each room wall surface
                List<Autodesk.Revit.DB.Solid> roomWallFaceSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Autodesk.Revit.DB.Curve roomCurve in roomCurves)
                    roomWallFaceSolids.Add(SolidConversions.CreateSolidExtrusionFromRevitCurve(roomCurve, 5, height));

                // define the output lists
                List<PlanarFace> faceIntersections = new List<PlanarFace>();
                List<Revit.Elements.Room> returnRoom = new List<Revit.Elements.Room>();

                // intersect rooms wall face solids with all the DB.Solids provided as an input
                foreach (Autodesk.Revit.DB.Solid faceSolid in roomWallFaceSolids)
                {
                    foreach (Autodesk.Revit.DB.Solid solid in solids)
                    {
                        try
                        {
                            // create intersectoins
                            Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, solid, BooleanOperationsType.Intersect);

                            // extracnt the singel largest vertical face of each intersection
                            if (intersection != null && intersection.Volume != 0)
                                faceIntersections.Add(SurfaceConversions.largestVerticalFace(intersection.Faces));
                        }
                        catch
                        {
                            // catch exceptions if any, indication there might be a slight miscalculation with the given room
                            returnRoom.Add(room);
                        }
                    }
                }

                // define outputs
                Dictionary<string, object> returnDict = new Dictionary<string, object>();
                returnDict.Add("surfaceIntersections", faceIntersections);

                if (returnRoom.Count != 0)
                    returnDict.Add("roomExceptions", returnRoom[0]);
                else
                    returnDict.Add("roomExceptions", null);

                return returnDict;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // methods : interpreted as a modify node
        /// <summary>
        /// Gets a list of Rooms, as well as Revit API Solids. Constructs room finish faces, based on the input base offset and height and returns all the face intersections
        /// </summary>
        /// <param name="room"> Revit.Elements.Rooms || Room element, wrapped through Dynamo </param>
        /// <param name="solids"> [Autodesk.Revit.DB.Solid] || A list of Revit API solids </param>
        /// <param name="baseOffset"> Double || Base offset constraint </param>
        /// <param name="height"> Double || Height parameter </param>
        /// <returns>
        /// <list type = "bullet">
        /// <item>
        /// <description> test1 </description>
        /// </item>
        /// <item>
        /// <description> test2 </description>
        /// </item>
        /// <item>
        /// <description> test3 </description>
        /// </item>
        /// <item>
        /// <description> test4 </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <search> room, surface, solid, API </search>
        [MultiReturn(new[] { "wallSurfacesArea", "wallSurfaceIntersectionsArea", "remainingWallSurfaceArea", "roomExceptions" })]
        public static Dictionary<string, object> RoomSurfaceIntersectionAreas(Revit.Elements.Room room, List<Autodesk.Revit.DB.Solid> solids, double baseOffset, double height)
        {
            try
            {
                // unwrap Dynamo rooms to get Revit rooms
                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                // get room's curves from the specified room
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                List<Autodesk.Revit.DB.Curve> roomCurves = new List<Autodesk.Revit.DB.Curve>();

                foreach (BoundarySegment segment in revitRoom.GetBoundarySegments(options)[0])
                    roomCurves.Add(segment.GetCurve());

                // generate thin solids for each room wall surface
                List<Autodesk.Revit.DB.Solid> roomWallFaceSolids = new List<Autodesk.Revit.DB.Solid>();

                foreach (Autodesk.Revit.DB.Curve roomCurve in roomCurves)
                    roomWallFaceSolids.Add(SolidConversions.CreateSolidExtrusionFromRevitCurve(roomCurve, 5, height));

                // define the output lists
                List<PlanarFace> faceIntersections = new List<PlanarFace>();
                List<Revit.Elements.Room> returnRoom = new List<Revit.Elements.Room>();

                // intersect rooms wall face solids with all the DB.Solids provided as an input
                double roomWallsArea = 0;
                double roomIntersectionAreas = 0;

                foreach (Autodesk.Revit.DB.Solid faceSolid in roomWallFaceSolids)
                {
                    // extract the single largest vertical face of each wall solid and add its area to the total sum
                    roomWallsArea += (SurfaceConversions.largestVerticalFace(faceSolid.Faces).Area) / 10.7639;

                    foreach (Autodesk.Revit.DB.Solid solid in solids)
                    {
                        try
                        {
                            // create intersectoin
                            Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, solid, BooleanOperationsType.Intersect);

                            // extract the single largest vertical face of each intersection and add its area to the total sum
                            if (intersection != null && intersection.Volume != 0)
                                roomIntersectionAreas += (SurfaceConversions.largestVerticalFace(intersection.Faces).Area) / 10.7639;
                        }
                        catch
                        {
                            // catch exceptions if any, indication there might be a slight miscalculation with the given room
                            returnRoom.Add(room);
                        }
                    }
                }

                // define outputs
                Dictionary<string, object> returnDict = new Dictionary<string, object>();
                returnDict.Add("wallSurfacesArea", roomWallsArea);
                returnDict.Add("wallSurfaceIntersectionsArea", roomIntersectionAreas);
                returnDict.Add("remainingWallSurfaceArea", Math.Max(roomWallsArea - roomIntersectionAreas, 0));

                if (returnRoom.Count != 0)
                    returnDict.Add("roomExceptions", returnRoom[0]);
                else
                    returnDict.Add("roomExceptions", null);

                return returnDict;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

namespace Inspect
{
    /// <summary>
    /// Retrieving and working with different elements' data
    /// </summary>
    public class ElementsData
    {
        private ElementsData() { }

        /// <summary>
        /// Gets an element, belonging to a system family, e.g. Waals, Roofs, Floors, Ceilings. Works for both linked elements and well as elements in the host model
        /// </summary>
        /// <param name="element"> Revit.Elements.Element || Revit element, wrapped through Dynamo </param>
        /// <returns> string || The type name of the element, eg. in the case of Walls, the name of the wall type </returns>
        /// <search> system families, type, name </search>
        public static string ReturnSystemTypeName(Revit.Elements.Element element)
        {
            try
            {
                Autodesk.Revit.DB.Element apiElement = element.InternalElement;

                return apiElement.Name;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Works for Walls, obtained from linked files. The node gets the end point of the wall location curve, gets its Z component and check if the given height corresponds to a level in the host model. 
        /// It applies a tolerance of a 1 cm. If such level is found, its name will be returned as an output
        /// </summary>
        /// <param name="wall"> Revit.Elements.Wall || Revit wall, wrapped through Dynamo </param>
        /// <returns> string || Level name </returns>
        /// <search> linked, walls, level </search>
        public static string GetLinkedWallHostLevelName(Revit.Elements.Wall wall)
        {
            try
            {
                // get the current document, the API element and all levels in the model
                Document hostDocument = DocumentManager.Instance.CurrentDBDocument;
                Autodesk.Revit.DB.Wall apiWall = (Autodesk.Revit.DB.Wall)wall.InternalElement;
                FilteredElementCollector levels = new FilteredElementCollector(hostDocument).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();

                // check if element is null
                if (wall == null || apiWall == null)
                    return "Either the wall or its API representation is null";

                // check if the element is a Wall
                if (apiWall.Category.Id.IntegerValue != (int)BuiltInCategory.OST_Walls)
                    return "This node inly works for Walls. The element provided is not a Wall";

                // Get the wall's location curve
                LocationCurve wallLocation = apiWall.Location as LocationCurve;

                Autodesk.Revit.DB.Curve curve = wallLocation.Curve;
                double endPtHeight = Math.Round(curve.GetEndPoint(0).Z * 30.48);

                // Use the point to determine the associated level
                foreach (Autodesk.Revit.DB.Level level in levels)
                    if (Math.Round(level.Elevation * 30.48) - endPtHeight <= 1 && Math.Round(level.Elevation * 30.48) - endPtHeight >= -1)
                        return level.Name;

                return "Could not find corresponding level for the specified wall";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the host model elevation for the specified linked wall. The elevation is measured as the distance between the wall base curve's endpoint and the Project Base Point
        /// </summary>
        /// <param name="wall"> Revit.Elements.Wall || Revit wall, wrapped through Dynamo </param>
        /// <returns> double || Height elevation </returns>
        /// <search> linked, wall, elevation </search>
        public static double GetLinkedWallHostElevation(Revit.Elements.Wall wall)
        {
            try
            {
                LocationCurve wallLocation = wall.InternalElement.Location as LocationCurve;

                return Math.Round(wallLocation.Curve.GetEndPoint(0).Z * 30.48);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This node would check if any of the specified parameters is available in the element, either as type or instance.
        /// It returns their value in the order they were specified - if a value is found for the first parameter, it will be returned. If not, it will proceed to the second one.
        /// If no value is found for both, it will return null
        /// </summary>
        /// <param name="element"> Revit.Elements.Element || Revit element, wrapped through Dynamo </param>
        /// <param name="parameterName1"> String || Parameter name </param>
        /// <param name="parameterName2"> String || Parameter name </param>
        /// <returns> Object || Parameter value </returns>
        /// <search> type, instance, parameter, either one </search>
        public static object EitherOneOfTwoParametersTypeInstance(Revit.Elements.Element element, string parameterName1, string parameterName2)
        {
            try
            {
                Autodesk.Revit.DB.Element apiElement = element.InternalElement;
                Document doc = apiElement.Document;

                ElementId typeId = apiElement.GetTypeId();
                Autodesk.Revit.DB.Element elemType = doc.GetElement(typeId);

                if (apiElement.LookupParameter(parameterName1) != null)
                    return element.GetParameterValueByName(parameterName1);
                else if (ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName1) != null)
                    return ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName1);

                else if (apiElement.LookupParameter(parameterName2) != null)
                    return element.GetParameterValueByName(parameterName2);
                else if (ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName2) != null)
                    return ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName2);

                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the elevation for each specified level. Typically the distance to the Internal Origin 
        /// </summary>
        /// <param name="level"> Revit.Elements.Level || Revit level, wrapped through Dynamo </param>
        /// <returns> double || Level elevation </returns>
        /// <search> level, elevation </search>
        public static double LevelElevation(Revit.Elements.Level level)
        {
            try
            {
                Autodesk.Revit.DB.Level apiLevel = level.InternalElement as Autodesk.Revit.DB.Level;
                return Math.Round(apiLevel.Elevation*30.48);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the elevation for each family instance, typically the distance to the Project Base Point. 
        /// Works for families with origin, represented by a location point, such as Wall, Doors and other family instances.
        /// Note: The end result is rounded
        /// </summary>
        /// <param name="element"> Revit.Elements.Element || Revit element, wrapped through Dynamo </param>
        /// <returns> double || Z elevation </returns>
        /// <search> linked, familyinstance, family instance, elevation </search>
        public static double LinkedFamilyInstanceElevation(Revit.Elements.Element element)
        {
            try
            {
                return Math.Round((element.InternalElement.Location as LocationPoint).Point.Z*30.48);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the name of the host model level for a Door or Window in a linked file. The level is calculated, based on the Family Instance location.
        /// Sill Height is subtracted from the elevation. The node uses a tolerance of 1 cm. 
        /// </summary>
        /// <param name="element"> Revit.Elements.Element || Revit element, wrapped through Dynamo </param>
        /// <returns> double || Z elevation </returns>
        /// <search> linked, familyinstance, family instance, elevation </search>
        public static string LinkedDoorWindowHostLevelName(Revit.Elements.Element element)
        {
            try
            {
                // get the current document
                Document hostDocument = DocumentManager.Instance.CurrentDBDocument;
                FilteredElementCollector levels = new FilteredElementCollector(hostDocument).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType();

                // get element's elevation in the host model
                double elementElevation = Math.Round((element.InternalElement.Location as LocationPoint).Point.Z * 30.48) - Math.Round(element.InternalElement.LookupParameter("Sill Height").AsDouble()*30.48);

                foreach (Autodesk.Revit.DB.Level level in levels)
                    if ((level.Elevation*30.48 - elementElevation <= 1) || (level.Elevation * 30.48 - elementElevation >= -1))
                        return level.Name;

                return "Could not find corresponding level for the specified Family Instance";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the elevation of a family instance from the host model. Works for elements with Location Point, such as Doors, Windows, etc. 
        /// Returns the elevation as the distance to Project Base Point.
        /// </summary>
        /// <param name="element"> Revit.Elements.Element || Revit element, wrapped through Dynamo </param>
        /// <returns> double || Z elevation </returns>
        /// <search> linked, familyinstance, family instance, elevation </search>
        public static double HostFamilyInstanceElevation(Revit.Elements.Element element)
        {
            try
            {
                // calculate element's Z coordinate as a vertical distance to the internal origin
                double instanceZ = Math.Round((element.InternalElement.Location as LocationPoint).Point.Z * 30.48);

                // calculate Base Point Elevation
                Document doc = DocumentManager.Instance.CurrentDBDocument;
                double BasePtZ = BasePoint.GetProjectBasePoint(doc).Position.Z*30.48;

                return instanceZ - BasePtZ;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    /// <summary>
    /// Collection of nodes for list operations
    /// </summary>
    public class Lists
    {
        private Lists() { }

        /// <summary>
        /// Checks if any of the bools in a given list is True
        /// </summary>
        /// <param name="list">  </param>
        /// <returns> bool || True or False </returns>
        /// <search> list, any, true </search>
        public static bool AnyTrue(List<bool> list)
        {
            try
            {
                return list.Contains(true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Revit.Elements.Element> returnListIfNot(object input)
        {
            try
            {
                if (input is List<Revit.Elements.Element>)
                    return input as List<Revit.Elements.Element>;
                else
                    return new List<Revit.Elements.Element> { (Revit.Elements.Element)input };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}