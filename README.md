# DynaBIMToolbox

> C# nodes for Dynamo

> Version 1.0.0

> Main objectives of this package version is performing Boolean and Solid operations directly in the API, aimed for improved performance of time - consuming tasks

> Secondary objective is optimised data extraction from host model and linked elements

# GeometryAPI Category
Consists of 3 subcategories - BooleanAPI, SolidsAPI and SurfacesAPI

> SolidsAPI -> ReturnDynamoSolid
> Basic wrapper: Gets an Autodesk.Revit.DB.Solid and returns Dynamo Solid


> SolidsAPI -> ElementOrientedBoundingBox
> Returns element oriented bounding box for the specified element, represented as Revit API solid. A handy fix for a popular issue with the API, as the built in method returns a BoundingBox, oriented towards Project North

![ElementOrientedBoundingBox](/images/ElementOrientedBoundingBox02.png)
![ElementOrientedBoundingBox](/images/ElementOrientedBoundingBox03.png)


> SolidsAPI -> GetAndUniteElementSolids
> Get all the solids of an Element, unite them and return their united geometry as RevitAPI Solid. Works for all objects, classified as Dynamo Elements, such as Windows, Doors, etc.

![GetAndUniteElementSolids](/images/GetAndUniteElementSolids01.png)


> SolidsAPI -> GetWallSolid
> Get Dynamo wall and return its solid geometry as Revit API solid

![GetWallSolids](/images/GetWallSolids01.png)


> SolidsAPI -> RevitAPIExtrusionFromCurve
> Create Autodesk.Revit.DB.Solid from a single curve, by offseting the curve in both direction, creating closed curveloop and extruding it

![RevitApiExtrusionFromCurve](/images/RevitApiExtrusionFromCurve01.png)


> SolidsAPI -> TranslateSolidVertically
> Translate Autodesk.revit.DB.Solid along the Z axis at the specified distance

![TranslateSolidVertically](/images/TranslateSolidVertically01.png)


> BooleanAPI -> RoomFacesSolidIntersection
> Gets a list of Rooms, as well as Revit API Solids. Constructs room finish faces, based on the input base offset and height and returns all the face intersections. This was the original operation, that inspired the whole package, as doing the Boolean operations in Dynamo with large amounts of data is extremely slow and heavy process. Calculating those intersections, using formulas, on the other hand, does not work when the room bounding geometry is in a link instance

> BooleanAPI -> RoomSurfaceIntersectionAreas
> Similar node to the previous one, but returns all the intersection areas instead

![RoomSurfaceIntersectionAreas](/images/RoomSurfaceIntersectionAreas01.png)


> SurfacesAPI -> ReturnDynamoFaces
> Similar node to the ReturnDynamoSolid. Converts Autodesk.Revit.DB.PlanarFace to Dynamo Surface

> SurfacesAPI -> WallSurfacesFromRooms
> Get a list of Autodesk.Revit.DB.PlanarFace, representing all the wall faces of a room, based on specified height and base offset

![WallSurfacesFromRooms](/images/WallSurfacesFromRooms01.png)


# Inspect Category
Consists of 3 subcategories - DocumentData, ElementsData, Lists

> DocumentData -> GetPhasesInChronologicalOrder
> Returns all phases of the active document, ordered chronologically. Result is return as a list of strings

> ElementData -> IsElementInControlPhase
> Determines whether or not an element could be applied to the control phase. If an element was created in an earlier phase, than the one specified and was either not demolished or demolished in a later phase, the node returns True. Otherwise, it returns False

![IsElementInControlPhase](/images/IsElementInControlPhase01.png)