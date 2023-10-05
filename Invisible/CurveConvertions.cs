using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaBIMToolbox.Invisible
{
    [IsVisibleInDynamoLibrary(false)]
    public class CurveConvertions
    {
        public CurveConvertions() { }

        public static Curve ReturnTransformedCurve(Curve curve, RevitLinkInstance linkInstance)
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

                return curve.CreateTransformed(coordinateSystemTransform);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
