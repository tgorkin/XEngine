using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class PositionAttribute : IEntityAttribute {

        public PositionAttribute() : this(new Vector3(), Matrix.Identity, new Vector3(1.0f)) { }

        public PositionAttribute( Vector3 position, Matrix rotation, Vector3 scale ) {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3 Position {
            get;
            set;
        }

        public Matrix Rotation {
            get;
            set;
        }

        public Vector3 Scale {
            get;
            set;
        }

        public Matrix World {
            get { return Matrix.Identity * Matrix.CreateScale( Scale ) * Rotation * Matrix.CreateTranslation( Position ); }
        }
    }
}
