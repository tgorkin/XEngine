using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace EntityPipeline {
    public class PositionAttributeData {

        [ContentSerializer( Optional = true )]
        public Vector3 Position;

        [ContentSerializer( Optional = true )]
        public Matrix Rotation;

        [ContentSerializer( Optional = true )]
        public Vector3 Scale;
    }
}
