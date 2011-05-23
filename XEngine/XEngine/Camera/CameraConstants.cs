using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    public enum CameraType {
        CAMERA_TYPE_FREE,
        CAMERA_TYPE_FIRST_PERSON,
        CAMERA_TYPE_TOP_DOWN
    };

    class CameraConstants {
        
        static public readonly float NEAR_PLANE = 0.1f;

        static public readonly float FAR_PLANE = 1000f;

        static public readonly float FIELD_OF_VIEW_DEGREES = 45;

        static public readonly Vector3 DEFAULT_POSITION = new Vector3(0, 2, 5);

        static public readonly Vector3 DEFAULT_LOOK_AT = new Vector3(0, 0, 0);

        static public readonly Vector3 DEFAULT_UP = Vector3.Up;
    }
}
