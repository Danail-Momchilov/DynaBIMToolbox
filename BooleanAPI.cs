using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;

// load Runtime module in order to use the MultiReturn attribute : more info on multiple return values in the Dynamo Primer
using Autodesk.DesignScript.Runtime;

// load Revit API
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

// load Dynamo API
using Revit.Elements;
using RevitServices.Persistence;
using System.Xml.Linq;
using Revit.GeometryConversion;
using DynaBIMToolbox.Invisible;
using DynaBIMToolbox.Invisible;
using System.Linq.Expressions;
using System;
using System.Security.Cryptography;
using System.Linq;
using Autodesk.Revit.DB.Architecture;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Generate;


// namespace : interpreted as a main category in the package
namespace GeometryAPI
{
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

        /*
        public static Autodesk.Revit.DB.Solid GetRoomSolid(Revit.Elements.Room room, double height)
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

                List<CurveLoop> crvLoopList = new List<CurveLoop> { CurveLoop.Create(roomCurves) };

                return GeometryCreationUtilities.CreateExtrusionGeometry(crvLoopList, XYZ.BasisZ, height / 30.48);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid GetRoomSolidTest(Revit.Elements.Room room, double height)
        {
            try
            {
                // unwrap Dynamo rooms to get Revit rooms
                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                // get room's curves from the specified room
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;

                Document roomDoc = revitRoom.Document;

                SpatialElementGeometryCalculator calculator = new SpatialElementGeometryCalculator(roomDoc);

                return calculator.CalculateSpatialElementGeometry(revitRoom).GetGeometry();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid GetRoomSolidTestCenter(Revit.Elements.Room room, double height)
        {
            try
            {
                // unwrap Dynamo rooms to get Revit rooms
                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                // get room's curves from the specified room
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center;

                Document roomDoc = revitRoom.Document;

                SpatialElementGeometryCalculator calculator = new SpatialElementGeometryCalculator(roomDoc);

                return calculator.CalculateSpatialElementGeometry(revitRoom).GetGeometry();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Autodesk.Revit.DB.Solid GetRoomSolidTransformed(Revit.Elements.Room room, double height, RevitLinkInstance revitLinkInstance)
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

                List<CurveLoop> crvLoopList = new List<CurveLoop> { CurveLoop.Create(roomCurves) };

                return SolidConversions.ReturnTransformedSolid(GeometryCreationUtilities.CreateExtrusionGeometry(crvLoopList, XYZ.BasisZ, height / 30.48), revitLinkInstance);
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

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
        /// Gets a PlanarFace and creates RevitAPI Solid extrusion from it
        /// </summary>
        /// <param name="face"> Autodesk.Revit.DB.PlanarFace || Revit API face </param>
        /// <param name="height"> Double || Height in cm </param>
        /// <returns> Autodesk.Revit.DB.Solid || RevitAPI Solid </returns>
        /// <search> RevitAPI, api, API, solid, Solid, RevitAPI Solid, RevitAPISolid, fromplanarface, from, planar, face, from planar face </search>
        public static Autodesk.Revit.DB.Solid RevitAPIExtrusionFromPlanarFace(PlanarFace face, double height)
        {
            try
            {
                return SolidConversions.CreateSolidExtrusionFromSurface(face, height);
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

        /// <summary>
        /// Gets the centroid of a Revit API solid and returns it as a Dynamo point
        /// </summary>
        /// <param name="solid"> Autodesk.Revit.DB.Solid || Revit API solid </param>
        /// <returns> Autodesk.Designscript.Geometry.Point || Dynamo point</returns>
        /// <search> solid, centroid, RevitAPI, solidcentroid, SolidCentroid, Centroid, return centroid, returncentroid </search>>
        public static Autodesk.DesignScript.Geometry.Point ReturnSolidCentroid(Autodesk.Revit.DB.Solid solid)
        {
            return solid.ComputeCentroid().ToPoint();
        }
        /*
        public static IList<Autodesk.Revit.DB.GeometryObject> ReturnRevitSolid(Autodesk.DesignScript.Geometry.Solid inputSolid)
        {
            try
            {
                // convert the Dynamo solid to API mesh
                IList<GeometryObject> solidGeomObject = inputSolid.ToRevitType();

                Autodesk.Revit.DB.Mesh revitMesh;

                if (solidGeomObject.Count == 1)
                    revitMesh = (Autodesk.Revit.DB.Mesh)solidGeomObject[0];
                else
                    return null;

                // define the shape builder object
                TessellatedShapeBuilder builder = new TessellatedShapeBuilder();
                builder.Target = TessellatedShapeBuilderTarget.Solid;
                builder.OpenConnectedFaceSet(true);

                // get all the mesh data
                ElementId materialId = revitMesh.MaterialElementId;
                int trianglesNumb = revitMesh.NumTriangles;

                // create a face from each triangle and add it to the solid
                for (int i = 0; i < trianglesNumb; i++)
                {
                    MeshTriangle meshTriangle = revitMesh.get_Triangle(i);
                    List<XYZ> vertecesList = new List<XYZ> { meshTriangle.get_Vertex(0), meshTriangle.get_Vertex(1), meshTriangle.get_Vertex(2) };
                    TessellatedFace face = new TessellatedFace(vertecesList, materialId);

                    builder.AddFace(face);
                }

                builder.CloseConnectedFaceSet();
                builder.Build();

                TessellatedShapeBuilderResult result = builder.GetBuildResult();

                return result.GetGeometricalObjects();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public static IList<GeometryObject> ReturnRevitSolidTest(Autodesk.DesignScript.Geometry.Solid inputSolid)
        {
            try
            {
                // convert the Dynamo solid to API mesh
                IList<GeometryObject> solidGeomObject = inputSolid.ToRevitType();

                Autodesk.Revit.DB.Mesh revitMesh;

                if (solidGeomObject.Count == 1)
                    revitMesh = (Autodesk.Revit.DB.Mesh)solidGeomObject[0];
                else
                    return null;

                // match triangles, based on identical normals, if any
                IList<XYZ> normals = revitMesh.GetNormals();
                int trianglesNumb = revitMesh.NumTriangles;

                Dictionary<XYZ, List<XYZ>> pointsByNormals = new Dictionary<XYZ, List<XYZ>>();

                for (int i = 0; i < trianglesNumb; i++)
                {
                    MeshTriangle triangle = revitMesh.get_Triangle(i);
                    XYZ normal = normals[i];

                    if (!pointsByNormals.ContainsKey(normal))
                        pointsByNormals[normal] = new List<XYZ>();

                    for  (int j = 0; j < 3; j++)
                    {
                        XYZ vertex = triangle.get_Vertex(j);
                        pointsByNormals[normal].Add(vertex);
                    }
                }

                // remove duplicating points from the list
                foreach (var kvp in pointsByNormals)
                {
                    List<XYZ> uniquePoints = kvp.Value.Distinct().ToList();
                    pointsByNormals[kvp.Key] = uniquePoints;
                }

                // define the shape builder object
                TessellatedShapeBuilder builder = new TessellatedShapeBuilder();
                builder.Target = TessellatedShapeBuilderTarget.Solid;
                builder.OpenConnectedFaceSet(true);

                ElementId materialId = revitMesh.MaterialElementId;

                // create face from each points set and add it to the shape builder
                foreach (XYZ normal in normals)
                {
                    TessellatedFace face = new TessellatedFace(pointsByNormals[normal], materialId);
                    builder.AddFace(face);
                }

                builder.CloseConnectedFaceSet();
                builder.Build();

                TessellatedShapeBuilderResult result = builder.GetBuildResult();

                return result.GetGeometricalObjects();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static ElementId ReturnSolidMeshMaterialId(Autodesk.DesignScript.Geometry.Solid inputSolid)
        {
            try
            {
                // convert the Dynamo solid to API mesh
                IList<GeometryObject> solidGeomObject = inputSolid.ToRevitType();

                Autodesk.Revit.DB.Mesh revitMesh;

                if (solidGeomObject.Count == 1)
                    revitMesh = (Autodesk.Revit.DB.Mesh)solidGeomObject[0];
                else
                    return null;

                return revitMesh.MaterialElementId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static IList<XYZ> ReturnSolidNormals(Autodesk.DesignScript.Geometry.Solid inputSolid)
        {
            try
            {
                // convert the Dynamo solid to API mesh
                IList<GeometryObject> solidGeomObject = inputSolid.ToRevitType();

                Autodesk.Revit.DB.Mesh revitMesh;

                if (solidGeomObject.Count == 1)
                    revitMesh = (Autodesk.Revit.DB.Mesh)solidGeomObject[0];
                else
                    return null;

                return revitMesh.GetNormals();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int ReturnSolidTrianglesCount(Autodesk.DesignScript.Geometry.Solid inputSolid)
        {
            try
            {
                IList<GeometryObject> solidGeomObject = inputSolid.ToRevitType();

                Autodesk.Revit.DB.Mesh revitMesh;
                
                revitMesh = (Autodesk.Revit.DB.Mesh)solidGeomObject[0];

                return revitMesh.NumTriangles;
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/
    }

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

        /// <summary>
        /// Retrieves room's Geometry and extracts only the lowermost surface from it
        /// </summary>
        /// <param name="room"> Revit.Elements.Room | Dynamo Room </param>
        /// <returns> PlanarFace | RevitAPI Face </returns>
        /// <search> room, surface </search>>
        public static Autodesk.Revit.DB.PlanarFace RoomSurface(Revit.Elements.Room room)
        {
            Autodesk.Revit.DB.Architecture.Room revitRoom = room.InternalElement as Autodesk.Revit.DB.Architecture.Room;

            GeometryElement geometryElement = revitRoom.ClosedShell;

            List<Autodesk.Revit.DB.PlanarFace> horizontalFaces = new List<Autodesk.Revit.DB.PlanarFace>();

            foreach (GeometryObject geometry in geometryElement)
            {
                if (geometry is Autodesk.Revit.DB.Solid solid)
                {
                    foreach (Autodesk.Revit.DB.PlanarFace face in solid.Faces)
                    {
                        if (!SurfaceConversions.isFaceVertical(face))
                            horizontalFaces.Add(face);
                    }
                }
            }

            return SurfaceConversions.lowerMostFace(horizontalFaces);
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

                foreach (List<BoundarySegment> segmentLoops in revitRoom.GetBoundarySegments(options))
                    foreach (BoundarySegment segment in segmentLoops)
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
                            // check if the room's bounding box collides with the one of the solid before intersecting the solids themselves
                            // should make the process less time consuming...
                            if (SolidConversions.DoBoundingBoxesIntersect(revitRoom.get_BoundingBox(null), revitRoom.get_BoundingBox(null).Transform, solid.GetBoundingBox(), solid.GetBoundingBox().Transform))
                            {
                                // create intersectoin
                                Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, solid, BooleanOperationsType.Intersect);

                                // extract the single largest vertical face of each intersection and add its area to the total sum
                                if (intersection != null && intersection.Volume != 0)
                                    roomIntersectionAreas += (SurfaceConversions.largestVerticalFace(intersection.Faces).Area) / 10.7639;
                            }
                            /*
                            // create intersectoin
                            Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(faceSolid, solid, BooleanOperationsType.Intersect);

                            // extract the single largest vertical face of each intersection and add its area to the total sum
                            if (intersection != null && intersection.Volume != 0)
                                roomIntersectionAreas += (SurfaceConversions.largestVerticalFace(intersection.Faces).Area) / 10.7639;
                            */
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

        /// <summary>
        /// Checks if two Autodesk.Revit.DB.Solid objects are intersecting. If an intersection between them is found, the volume of which is larger than zero, the node returns 'True'. Otherwise it returns 'False'
        /// </summary>
        /// <param name="solidA"> Autodesk.Revit.DB.Solid || Revit API Solid </param>
        /// <param name="solidB"> Autodesk.Revit.DB.Solid || Revit API Solid </param>
        /// <returns> Boolean </returns>
        /// <search> solid, intersect, doesintersect, revitapi </search>
        public static bool DoSolidsIntersect(Autodesk.Revit.DB.Solid solidA, Autodesk.Revit.DB.Solid solidB)
        {
            try
            {
                Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(solidA, solidB, BooleanOperationsType.Intersect);

                if (SolidConversions.DoBoundingBoxesIntersect(solidA.GetBoundingBox(), solidA.GetBoundingBox().Transform, solidB.GetBoundingBox(), solidB.GetBoundingBox().Transform))
                    if (intersection != null && intersection.Volume != 0)
                        return true;
                    else
                        return false;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets two Revit API solids and returns their intersection solid, if found
        /// </summary>
        /// <param name="solidA"> Autodesk.Revit.DB.Solid || RevitAPI Solid </param>
        /// <param name="solidB"> Autodesk.Revit.DB.Solid || RevitAPI Solid </param>
        /// <returns>
        /// <item>
        /// <description> Autodesk.Revit.DB.Solid || Revit API Solid </description>
        /// </item>
        /// <item>
        /// <description> Double || Numeric Volume </description>
        /// </item>
        /// </returns>
        /// <search> solids, intersection, intersect solids </search>>
        [MultiReturn(new[] { "solidIntersection", "intersectionVolume" })]
        public static Dictionary<string, object> SolidsIntersection(Autodesk.Revit.DB.Solid solidA, Autodesk.Revit.DB.Solid solidB)
        {
            try
            {
                Dictionary<string, object> returnDict = new Dictionary<string, object>();

                returnDict.Add("solidIntersection", null);
                returnDict.Add("intersectionVolume", null);

                if (SolidConversions.DoBoundingBoxesIntersect(solidA.GetBoundingBox(), solidA.GetBoundingBox().Transform, solidB.GetBoundingBox(), solidB.GetBoundingBox().Transform))
                {
                    Autodesk.Revit.DB.Solid intersection = BooleanOperationsUtils.ExecuteBooleanOperation(solidA, solidB, BooleanOperationsType.Intersect);

                    if (intersection != null && intersection.Volume != 0)
                    {
                        returnDict.Clear();
                        returnDict.Add("solidIntersection", intersection);
                        returnDict.Add("intersectionVolume", intersection.Volume * 28316.846592);
                    }
                }

                return returnDict;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Gets a list of Autodesk.Revit.DB.Solid elements and returns a single, united Solid
        /// </summary>
        /// <param name="solidsList"> List<Autodesk.Revit.DB.Solid> || List of Revit Solids </param>
        /// <returns> Autodesk.Revit.DB.Solid || Revit Solid </returns>
        public static Autodesk.Revit.DB.Solid SolidsUnion(List<Autodesk.Revit.DB.Solid> solidsList)
        {
            try
            {
                return SolidConversions.UniteSolids(solidsList);
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
        /// Gets an element, belonging to a system family, e.g. Waals, Roofs, Floors, Ceilings. Works for both linked elements as well as elements in the host model
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

                if (element.GetParameterValueByName(parameterName1) != "")
                    return element.GetParameterValueByName(parameterName1);
                else if (ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName1) != "")
                    return ElementWrapper.ToDSType(elemType, true).GetParameterValueByName(parameterName1);

                else if (element.GetParameterValueByName(parameterName2) != "")
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
                double elementElevation;

                if (element.InternalElement.Location != null)
                    elementElevation = Math.Round((element.InternalElement.Location as LocationPoint).Point.Z * 30.48) - Math.Round(element.InternalElement.LookupParameter("Sill Height").AsDouble() * 30.48);
                else
                    return "There was an issue obtaining the element's Location";

                foreach (Autodesk.Revit.DB.Level level in levels)
                    if ((level.Elevation*30.48 - elementElevation <= 1) && (level.Elevation * 30.48 - elementElevation >= -1))
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
        /// <search> familyinstance, family instance, elevation </search>
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

        /// <summary>
        /// Determines whether or not an element could be applied to the control phase. If an element was created in an earlier phase, 
        /// than the one specified and was either not demolished or demolished in a later phase, the node returns True. Otherwise, it returns False
        /// </summary>
        /// <param name="phasesOrdered"> [string] || List with phase names, sorted in a chronological order </param>
        /// <param name="controlPhase"> string || Name of the control phase </param>
        /// <param name="element"> [Revit.Elements.Element] || list with Revit elements, wrapped through Dynamo </param>
        /// <returns>
        /// <list type = "bullet">
        /// <item>
        /// <description> True or False </description>
        /// </item>
        /// <item>
        /// <description> If any exceptions were found, they will be displayed here </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <search> phase, control phase, element, does belong to phase </search>
        [MultiReturn(new[] { "doesBelongToPhase", "exceptions" })]
        public static Dictionary<string, object> IsElementInControlPhase(List<string> phasesOrdered, string controlPhase, Revit.Elements.Element element)
        {
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                // get the element's phase created and phase demolished names
                Document elementDocument = element.InternalElement.Document;

                ElementId elementPhaseCreatedId = element.InternalElement.CreatedPhaseId;
                string phaseCreatedName = elementDocument.GetElement(elementPhaseCreatedId).Name;

                ElementId elementPhaseDemolishedId = element.InternalElement.DemolishedPhaseId;
                string phaseDemolishedName = "";
                if (elementDocument.GetElement(elementPhaseDemolishedId) != null)
                    phaseDemolishedName = elementDocument.GetElement(elementPhaseDemolishedId).Name;

                // get the index of PhaseCreated and PhaseDemolished from the given list of phases
                int phaseCreatedIndex = phasesOrdered.IndexOf(phaseCreatedName);

                int phaseDemolishedIndex;

                if (phaseDemolishedName == "")
                    phaseDemolishedIndex = -1;
                else
                    phaseDemolishedIndex = phasesOrdered.IndexOf(phaseDemolishedName);

                // check phases order and return results
                if (phaseCreatedIndex == -1)
                {
                    result.Add("doesBelongToPhase", null);
                    result.Add("exceptions", "Rooms's Phase not found in the list. The room's document likely contains different phases than the host one");
                    return result;
                }
                else
                {
                    int controlPhaseIndex = phasesOrdered.IndexOf(controlPhase);

                    if ((phaseCreatedIndex <= controlPhaseIndex && phaseDemolishedIndex == -1) || (phaseCreatedIndex <= controlPhaseIndex && phaseDemolishedIndex > controlPhaseIndex))
                    {
                        result.Add("doesBelongToPhase", true);
                        result.Add("exceptions", "Everything seems to be fine");
                        return result;
                    }
                    else
                    {
                        result.Add("doesBelongToPhase", false);
                        result.Add("exceptions", "Everything seems to be fine");
                        return result;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the name of the specified phase
        /// </summary>
        /// <param name="phase"> Revit.Elements.Phase | Revit phase, wrapped through Dynamo </param>
        /// <returns> string | The name of the phase as string </returns>
        /// <search> phase, phase name, name </search>
        public static string PhaseName(Revit.Elements.Element phase)
        {
            try
            {
                return phase.InternalElement.Name;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the bounding box for the specified room. It will be oriented in accordance with Project North
        /// </summary>
        /// <param name="room"> </param>
        /// <returns> Autodesk.DesignScript.Geometry.BoundingBox | Dynamo Bounding Box </returns>
        /// <search> room, bounding box </search>>
        public static BoundingBox RoomBoundingBox(Revit.Elements.Room room)
        {
            try
            {
                return room.BoundingBox;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /*
        public static List<Autodesk.DesignScript.Geometry.Curve> GetRoomCurves(Revit.Elements.Room room, double height)
        {
            try
            {
                // unwrap Dynamo rooms to get Revit rooms
                Autodesk.Revit.DB.Architecture.Room revitRoom = (Autodesk.Revit.DB.Architecture.Room)room.InternalElement;

                // get room's curves from the specified room
                SpatialElementBoundaryOptions options = new SpatialElementBoundaryOptions();
                options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center;

                List<Autodesk.DesignScript.Geometry.Curve> roomCurves = new List<Autodesk.DesignScript.Geometry.Curve>();

                foreach (BoundarySegment segment in revitRoom.GetBoundarySegments(options)[0])
                    roomCurves.Add(segment.GetCurve().ToProtoType());

                return roomCurves;
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        /// <summary>
        /// Gets a FamilyInstance and returns the name of its Family
        /// </summary>
        /// <param name="familyInstance"> Revit.Elements.FamilyInstance || FamilyInstance in Dynamo </param>
        /// <returns> String || The name of the Family </returns>
        public static string FamilyInstanceFamilyName(Revit.Elements.FamilyInstance familyInstance)
        {
            try
            {
                return familyInstance.GetFamily.Name;
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
        /// <param name="list"> [object] | List of elements </param>
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

        /// <summary>
        /// A List.Clean node, that actually works... unlike the OOTB. Removes null values from the input list
        /// </summary>
        /// <param name="inputList"> [object] | List of elements </param>
        /// <returns> [clean list] | The input list, properly cleaned </returns>
        /// <search> list, clean, clean list, list.clean </search>>
        public static List<object> ActuallyWorkingListClean(List<object> inputList)
        {
            try
            {
                List<object> outputList = new List<object>();

                foreach (object obj in inputList)
                    if (obj != null)
                        outputList.Add(obj);

                return outputList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// If an item (either an individual or a one in a list) is equal to the specified value, it will be replaced
        /// </summary>
        /// <param name="item"> object | Could be either an individual item or a list of items </param>
        /// <param name="isEqualTo"> object | The value to search for. Could be any type of variable </param>
        /// <param name="changeWith"> object | The value to replace with. Could be any type of variable </param>
        /// <returns> object | Replaced value </returns>
        /// <search> replace with, replace if, replace, find </search>>
        public static object ReplaceWithIf(object item, object isEqualTo, object changeWith)
        {
            try
            {
                if (item == isEqualTo)
                    return changeWith;
                else
                    return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns the indexes of the maximal value in an input list. If the value occurs only once, the final list will contain a single index
        /// </summary>
        /// <param name="inputList"> [object] | List of elements </param>
        /// <returns> [int] | A List of integers </returns>
        /// <search> indexes of, maximal value, indexesofminvalue, IndexesOfMinValue </search>
        public static List<int> IndexesOfMaxValue(List<double> inputList)
        {
            List<int> outputList = new List<int>();

            if (inputList == null || !inputList.Any())
                outputList.Add(-1);
            else
            {
                double maxValue = inputList.Max();

                for (int i = 0; i < inputList.Count; i++)
                    if (inputList[i] == maxValue)
                        outputList.Add(i);
            }

            return outputList;
        }

        /// <summary>
        /// Returns the indexes of the minimal value in an input list. If the value occurs only once, the final list will contain a single index
        /// </summary>
        /// <param name="inputList"> [object] | List of elements </param>
        /// <returns> [int] | A List of integers </returns>
        /// <search> indexes of, minimal value, IndexesOfMaxValue, indexesofmaxvalue </search>
        public static List<int> IndexesOfMinValue(List<double> inputList)
        {
            List<int> outputList = new List<int>();

            if (inputList == null || !inputList.Any())
                outputList.Add(-1);
            else
            {
                double minValue = inputList.Min();

                for (int i = 0; i < inputList.Count; i++)
                    if (inputList[i] == minValue)
                        outputList.Add(i);
            }

            return outputList;
        }

        /// <summary>
        /// Combines lists by placing elements with identical indices in sublists. If lists have different length, the last sublists will only contain a single element
        /// </summary>
        /// <param name="listA"> [object] | List of elements </param>
        /// <param name="listB"> [object] | List of elements </param>
        /// <returns> [[object]] | A list of lists of combined objects </returns>>
        /// <search> list, combine, combinator, consistent, ListCombinatorConsistent, listcombinatorconsistent </search>
        public static List<List<object>> ListCombinatorConsistent(List<object> listA, List<object> listB)
        {
            List<List<object>> outputList = new List<List<object>>();

            int length;

            if (listA.Count >= listB.Count)
                length = listA.Count;
            else
                length = listB.Count;

            for (int i = 0; i < length; i++)
            {
                List<object> tempList = new List<object>();
                if (i < listA.Count)
                    tempList.Add(listA[i]);
                if (i < listB.Count)
                    tempList.Add(listB[i]);
                outputList.Add(tempList);
            }

            return outputList;
        }
    }

    /// <summary>
    /// Retrieving information from either linked or current document
    /// </summary>
    public class DocumentData
    {
        private DocumentData() { }

        /// <summary>
        /// Returns all phases of the active document, ordered chronologically. Result is return as a list of strings
        /// </summary>
        /// <returns> [string] | List of phase names, ordered chronologically </returns>
        /// <search> phases, get phases, getphases, pahses chronological, chronological </search>>
        public static List<string> GetPhasesInChronologicalOrder()
        {
            try
            {
                Document doc = DocumentManager.Instance.CurrentDBDocument;
                List<string> phaseNames = new List<string>();

                FilteredElementCollector collector = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Phases);
                List<Phase> phases = new List<Phase>();

                foreach (Phase phaseElement in collector)
                {
                    if (phaseElement != null)
                    {
                        phases.Add(phaseElement);
                    }
                }

                // Sort phases by sequence number
                phases.Sort((x, y) => x.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger() - y.get_Parameter(BuiltInParameter.PHASE_SEQUENCE_NUMBER).AsInteger());

                // Get phase names in chronological order
                foreach (Phase phase in phases)
                {
                    phaseNames.Add(phase.Name);
                }

                return phaseNames;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    /// <summary>
    /// Retrieve Geometric Elements' Data
    /// </summary>
    public class GeometryData
    {
        private GeometryData() { }

        /// <summary>
        /// Gets a point and returns all three of its components
        /// </summary>
        /// <param name="point"> Autodesk.DesignScript.Geometry.Point || Dynamo Point </param>
        /// <returns> </returns>
        /// <search> point, coordinates </search>
        [MultiReturn(new[] { "X", "Y", "Z" })]
        public static Dictionary<string, object> ReturnPointsCoordinates(Autodesk.DesignScript.Geometry.Point point)
        {
            try
            {
                Dictionary<string, object> returnDict = new Dictionary<string, object>();

                returnDict.Add("X", point.X);
                returnDict.Add("Y", point.Y);
                returnDict.Add("Z", point.Z);

                return returnDict;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns both the start and end point of a line
        /// </summary>
        /// <param name="line"> Autodesk.DesignScript.Geometry.Line || Dynamo Line </param>
        /// <returns> </returns>
        /// <search> return, startpoint, endpoint, start point, end point </search>>
        [MultiReturn(new[] { "StartPoint", "EndPoint" })]
        public static Dictionary<string, object> ReturnLineStartPointEndPoint(Autodesk.DesignScript.Geometry.Line line)
        {
            try
            {
                Dictionary<string, object> returnDict = new Dictionary<string, object>();

                returnDict.Add("StartPoint", line.StartPoint);
                returnDict.Add("EndPoint", line.EndPoint);

                return returnDict;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Checks if a curve is horizontal
        /// </summary>
        /// <param name="curve"> [object] | List of elements </param>
        /// <returns> bool | true or false </returns>
        /// <search> curve, ishorizontal, isHorizontal, IsCurveHorizontal, iscurvehorizontal </search>>
        public static bool IsCurveHorizontal(Autodesk.DesignScript.Geometry.Curve curve)
        {
            Autodesk.DesignScript.Geometry.Point startPoint = curve.StartPoint;
            Autodesk.DesignScript.Geometry.Point endPoint = curve.EndPoint;

            return (startPoint.Z == endPoint.Z);
        }

        /// <summary>
        /// Checks if a Surface is horizontal
        /// </summary>
        /// <param name="surface"> Autodesk.DesignScript.Geometry.Surface | Dynamo Surface </param>
        /// <returns> bool | true or false </returns>
        /// <search> surface, horizontal, is, is surface, isSurface, isSurfaceHorizontal, is surface horizontal </search>>
        public static bool IsSurfaceHorizontal(Autodesk.DesignScript.Geometry.Surface surface)
        {
            Autodesk.DesignScript.Geometry.Point point0 = surface.PointAtParameter(0, 0);
            Autodesk.DesignScript.Geometry.Point point1 = surface.PointAtParameter(1, 1);

            return (point0.Z == point1.Z);
        }
    }
}

namespace Generate

{
    /// <summary>
    /// Generate geometric elements
    /// </summary>
    public class Geometry
    {
        private Geometry() { }

        /// <summary>
        /// Creates a vector from a curve's Start and End Point
        /// </summary>
        /// <param name="curve"> Autodesk.DesignScript.Geometry.Curve | Dynamo Curve </param>
        /// <returns> Vector | Dynamo Vector </returns>
        /// <search> vector, by, curve, vectorbycurve, VectorByCurve, bycurve, byCurve </search>
        public static Vector VectorByCurve(Autodesk.DesignScript.Geometry.Curve curve)
        {
            Autodesk.DesignScript.Geometry.Point startPoint = curve.StartPoint;
            Autodesk.DesignScript.Geometry.Point endPoint = curve.EndPoint;

            return Vector.ByTwoPoints(startPoint, endPoint);
        }
    }
}