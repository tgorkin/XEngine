using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XEngine {
    class TerrainReader : ContentTypeReader<Terrain>{

        protected override Terrain Read(ContentReader input, Terrain existingInstance) {
            return new Terrain(input);
        }
    }
}
