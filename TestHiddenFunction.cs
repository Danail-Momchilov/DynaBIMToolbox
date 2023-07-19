using Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;

namespace Solids
{
    // with the IsVisibleInDynamoLibrary function, the class remains hidden and can only be used internally
    [IsVisibleInDynamoLibrary(false)]
    public class TestHiddenFunction
    {
        private TestHiddenFunction() { }
    }
}
