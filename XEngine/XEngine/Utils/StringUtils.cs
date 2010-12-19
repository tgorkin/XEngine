using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class StringUtils {

        public static string Vec2ToString(Vector2 vec) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("[ {0}x, {1}y ]", vec.X, vec.Y);
            return stringBuilder.ToString();
        }

        public static string Vec3ToString(Vector3 vec) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("[ {0}x, {1}y, {2}z ]", vec.X, vec.Y, vec.Z);
            return stringBuilder.ToString();
        }
    }
}
