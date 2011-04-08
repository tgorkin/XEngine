using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace TerrainPipeline {

    [ContentImporter(".jpg", DisplayName = "Terrain Importer", DefaultProcessor = "TerrainProcessor")]
    public class TerrainImporter : ContentImporter<TerrainContent> {

        public override TerrainContent Import(string filename, ContentImporterContext context) {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filename);

            TerrainContent terrain = new TerrainContent(bitmap);
            return terrain;
        }

    }
}
