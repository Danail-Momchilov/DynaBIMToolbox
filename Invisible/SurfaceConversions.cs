using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaBIMToolbox.Invisible
{
    // with the IsVisibleInDynamoLibrary function, the class remains hidden and can only be used internally
    [IsVisibleInDynamoLibrary(false)]
    internal class SurfaceConversions
    {
        public SurfaceConversions() { }

        public static bool isFaceVertical(Autodesk.Revit.DB.PlanarFace face)
        {
            try
            {
                if (Math.Round(face.FaceNormal.Z, 2, MidpointRounding.AwayFromZero) == 1 || Math.Round(face.FaceNormal.Z, 2, MidpointRounding.AwayFromZero) == -1)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static PlanarFace largestVerticalFace(FaceArray faces)
        {
            try
            {
                double largestArea = 0;
                List<PlanarFace> largestFaces = new List<PlanarFace>();

                foreach (PlanarFace face in faces)
                {
                    if (face.FaceNormal.Z != 1 && face.FaceNormal.Z != -1)
                        if (face.Area >= largestArea)
                        {
                            largestArea = face.Area;
                            largestFaces.Add(face);
                        }
                }

                return largestFaces.Last();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static PlanarFace lowerMostFace(List<PlanarFace> faces)
        {
            try
            {
                PlanarFace lowerMostFace = faces[0];

                foreach (PlanarFace face in faces)
                {
                    if (face.Origin.Z < lowerMostFace.Origin.Z)
                    {
                        lowerMostFace = face;
                    }
                }

                return lowerMostFace;
            }
            catch (Exception e)
            { 
                throw e; 
            }
        }
    }
}
