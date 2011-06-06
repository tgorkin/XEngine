using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {

    public class ModelRenderData : ComponentData {

        public string Model;

        [ContentSerializer( Optional = true )]
        public Transform Transform = new Transform();
    }
}
