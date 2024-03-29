# DynaBIMToolbox

> C# nodes for Dynamo

> Version 1.0.4

> Main objectives of this package version is performing Boolean and Solid operations directly in the API, aimed for improved performance of time - consuming tasks

> Secondary objective is optimised data extraction from host model and linked elements

# GeometryAPI Category
Consists of 3 subcategories - BooleanAPI, SolidsAPI and SurfacesAPI

> **SolidsAPI -> ReturnDynamoSolid**
> Basic wrapper: Gets an Autodesk.Revit.DB.Solid and returns Dynamo Solid


> **SolidsAPI -> ElementOrientedBoundingBox**  
> Returns element oriented bounding box for the specified element, represented as Revit API solid. A handy fix for a popular issue with the API, as the built in method returns a BoundingBox, oriented towards Project North

![ElementOrientedBoundingBox](/images/ElementOrientedBoundingBox02.png)
![ElementOrientedBoundingBox](/images/ElementOrientedBoundingBox03.png)
![ElementOrientedBoundingBox](/images/ElementOrientedBoundingBox04.png)


> **SolidsAPI -> GetAndUniteElementSolids**  
> Get all the solids of an Element, unite them and return their united geometry as RevitAPI Solid. Works for all objects, classified as Dynamo Elements, such as Windows, Doors, etc.

![GetAndUniteElementSolids](/images/GetAndUniteElementSolids01.png)


> **SolidsAPI -> GetWallSolid**  
> Get Dynamo wall and return its solid geometry as Revit API solid

![GetWallSolids](/images/GetWallSolids01.png)


> **SolidsAPI -> RevitAPIExtrusionFromCurve**  
> Create Autodesk.Revit.DB.Solid from a single curve, by offseting the curve in both direction, creating closed curveloop and extruding it

![RevitApiExtrusionFromCurve](/images/RevitApiExtrusionFromCurve01.png)


> **SolidsAPI -> TranslateSolidVertically**  
> Translate Autodesk.revit.DB.Solid along the Z axis at the specified distance

![TranslateSolidVertically](/images/TranslateSolidVertically01.png)


> **BooleanAPI -> RoomFacesSolidIntersection**  
> Gets a list of Rooms, as well as Revit API Solids. Constructs room finish faces, based on the input base offset and height and returns all the face intersections. This was the original operation, that inspired the whole package, as doing the Boolean operations in Dynamo with large amounts of data is extremely slow and heavy process. Calculating those intersections, using formulas, on the other hand, does not work when the room bounding geometry is in a link instance


> **BooleanAPI -> RoomSurfaceIntersectionAreas**  
> Similar node to the previous one, but returns all the intersection areas instead

![RoomSurfaceIntersectionAreas](/images/RoomSurfaceIntersectionAreas01.png)


> **BooleanAPI -> DoSolidsIntersect (Added in v 1.03)**
> Checks if two API solids intersect. Returns either True or False

![DoSolidsIntersect](/images/DoSolidsIntersect01.png)
![DoSolidsIntersect](/images/DoSolidsIntersect02.png)


> **BooleanAPI -> SolidIntersection (Added in v 1.03)**
> Gets two API Solids and returns the intersection

![SolidsIntersection](/images/SolidIntersection.png)


> **BooleanAPI -> SolidsUnion (Added in v 1.03)**
> Gets a list of Autodesk.Revit.DB.Solid elements and returns a single, united Solid

![SolidsUnion](/images/SolidsUnion.png)


> **SurfacesAPI -> ReturnDynamoFaces**  
> Similar node to the ReturnDynamoSolid. Converts Autodesk.Revit.DB.PlanarFace to Dynamo Surface


> **SurfacesAPI -> WallSurfacesFromRooms**  
> Get a list of Autodesk.Revit.DB.PlanarFace, representing all the wall faces of a room, based on specified height and base offset

![WallSurfacesFromRooms](/images/WallSurfacesFromRooms01.png)


# Inspect Category
Consists of 3 subcategories - DocumentData, ElementsData, GeometryData, Lists

> **DocumentData -> GetPhasesInChronologicalOrder**  
> Returns all phases of the active document, ordered chronologically. Result is return as a list of strings


> **ElementData -> IsElementInControlPhase**  
> Determines whether or not an element could be applied to the control phase. If an element was created in an earlier phase, than the one specified and was either not demolished or demolished in a later phase, the node returns True. Otherwise, it returns False

![IsElementInControlPhase](/images/IsElementInControlPhase01.png)


> **ElementData -> EitherOneOfTwoParametersTypeInstance**  
> This node would check if any of the specified parameters is available in the element, either as type or instance. 
> It returns their value in the order they were specified - if a value is found for the first parameter, it will be returned. 
> If not, it will proceed to the second one. If no value is found for both, it will return null

![EitherOneOfTwoParametersTypeInstance](/images/EitherOneOfTwoParametersTypeInstance01.png)


> **ElementsData -> GetLinkedWallHostElevation**  
> Returns the host model elevation for the specified linked wall. The elevation is measured as the distance between the wall base curve's endpoint and the Project Base Point


> **ElementsData -> GetLinkedWallHostLevelName**  
> Works for Walls, obtained from linked files. The node gets the end point of the wall location curve, gets its Z component and check if the given height corresponds to a level in the host model. 
> It applies a tolerance of a 1 cm. If such level is found, its name will be returned as an output

![GetLinkedWallHostLevelName](/images/GetLinkedWallHostLevelName01.png)


> **ElementsData -> HostFamilyInstanceElevation**  
> Returns the elevation of a family instance from the host model. Works for elements with Location Point, such as Doors, Windows, etc. 
> Returns the elevation as the distance to Project Base Point


> **ElementsData -> levelElevation**  
> Returns the elevation for each specified level. Typically the distance to the Internal Origin 


> **ElementsData -> LinkedDoorWindowHostLevelName**  
> Returns the name of the host model level for a Door or Window in a linked file. The level is calculated, based on the Family Instance location.
> Sill Height is subtracted from the elevation. The node uses a tolerance of 1 cm

![LinkedDoorWindowHostLevelName](/images/LinkedDoorWindowHostLevelName01.png)


> **ElementsData -> PhaseName**  
> Returns the name of the specified phase


> **ElementsData -> ReturnSystemTypeName**  
> Gets an element, belonging to a system family, e.g. Waals, Roofs, Floors, Ceilings. Works for both linked elements as well as elements in the host model

![ReturnSystemTypeName](/images/ReturnSystemTypeName01.png)


> **ElementsData -> RoomBoundingBox**  
> Returns the bounding box for the specified room. It will be oriented in accordance with Project North


> **ElementsData -> FamilyInstanceFamilyName (Added in v 1.03)**
> Gets a FamilyInstance and returns the name of its Family


> **Lists -> ActuallyWorkingListClean**  
> Just as the name suggests: a List.Clean node, that actually works... Removes null values from the input list


> **Lists -> AnyTrue**  
> Checks if any of the bools in a given list is True


> **Lists -> ReplaceWithIf**  
> If an item (either an individual or a one in a list) is equal to the specified value, it will be replaced

![ReplaceWithIf](/images/ReplaceWithIf01.png)


**Lists -> ListCombinatorConsistent (Added in v 1.03)**
> Combines lists by placing elements with identical indices in sublists. If lists have different length, the last sublists will only contain a single element

![ListCombinatorConsistent](/images/ListCombinatorConsistent.png)


**Lists -> IndexesOfMaxValue (Added in v 1.03)**
> Returns the indexes of the maximal value in an input list. If the value occurs only once, the final list will contain a single index


**Lists -> IndexesOfMinValue (Added in v 1.03)**
> Returns the indexes of the minimal value in an input list. If the value occurs only once, the final list will contain a single index


> **GeometryData -> ReturnPointsCoordinates (Added in v 1.03)**
> Gets a point and returns all three of its coordinates

![ReturnPointsCoordinates](/images/ReturnPointsCoordinates.png)


> **GeometryData -> ReturnLineStartPointEndPoint (Added in v 1.03)**
> Returns both the start and end point of a line

![ReturnLineStartPointEndPoint](/images/ReturnLineStartPointEndPoint.png)


**GeometryData -> IsCurveHorizontal (Added in v 1.03)**
> Checks if a curve is horizontal. Returns true or false


# Generate Category
Currently, consists of only one subcategory - Geometry (Added in v 1.03)


**Geometry -> VectorByCurve (Added in v 1.04)**
> Creates a vector from a curve's Start and End Point

![VectorByCurve](/images/VectorByCurve.png)

**Geometry -> RoomSurface (Added in v 1.05)**
> Retrieves room's Geometry and extracts only the lowermost surface from it. Returns it as a Revit API PlanarFace

![RoomSurface](/images/RoomSurface.png)