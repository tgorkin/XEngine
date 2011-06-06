using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class VectorUtils {

        public static Vector3 VectorRecipricol( Vector3 orig ) {
            return new Vector3( 1 / orig.X, 1 / orig.Y, 1 / orig.Z );
        }
    }
}
