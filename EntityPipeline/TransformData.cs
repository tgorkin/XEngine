using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EntityPipeline {
    public class TransformData {

        [ContentSerializer( Optional = true )]
        public Vector3 Position = new Vector3();

        [ContentSerializer( Optional = true )]
        public Vector3 Scale = new Vector3( 1.0f );

        [ContentSerializer( Optional = true )]
        public Matrix Rotation = Matrix.Identity;
    }
}
