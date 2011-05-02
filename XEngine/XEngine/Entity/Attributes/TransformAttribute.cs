using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EntityPipeline;

namespace XEngine {
    class TransformAttribute : IEntityAttribute {

        public Vector3 Position = new Vector3();

        public Vector3 Scale = new Vector3(1.0f);

        public Matrix Rotation = Matrix.Identity;

        public TransformAttribute() { }

        public TransformAttribute( TransformData data ) {
            this.Position = data.Position;
            this.Scale = data.Scale;
            this.Rotation = data.Rotation;
        }

        public Matrix World {
            get { return Matrix.CreateScale( Scale ) * Rotation * Matrix.CreateTranslation( Position ); }
        }
    }
}
