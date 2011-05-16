using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {

    public class PrimitiveRenderData : ComponentData {

        public GeometricPrimitiveType PrimitiveType;

        [ContentSerializer( Optional = true )]
        public Color Color = Color.White;

        [ContentSerializer( Optional = true )]
        public bool Wireframe = false;
    }

}
