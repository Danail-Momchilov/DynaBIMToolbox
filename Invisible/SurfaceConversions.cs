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
                if (face.FaceNormal.Z == 1 || face.FaceNormal.Z == -1)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static double largestVerticalFaceArea(FaceArray faces)
        {
            try
            {
                double largestArea = 0;

                foreach (PlanarFace face in faces)
                {
                    if (face.FaceNormal.Z != 1 && face.FaceNormal.Z != -1)
                        if (face.Area >= largestArea)
                            largestArea = face.Area * 0.092;
                }

                return largestArea;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
