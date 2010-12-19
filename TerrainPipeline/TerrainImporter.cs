using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

// TODO: replace this with the type you want to import.
using TImport = System.String;

namespace TerrainPipeline {
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// 
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".jpg",".bmp", DisplayName = "Terrain Importer", DefaultProcessor = "TerrainProcessor")]
    public class TerrainImporter : ContentImporter<TerrainContent> {

        public override TerrainContent Import(string filename, ContentImporterContext context) {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filename);

            TerrainContent terrain = new TerrainContent(bitmap);
            return terrain;
        }

    }
}
