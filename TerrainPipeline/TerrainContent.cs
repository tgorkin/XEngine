using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace TerrainPipeline {
    public class TerrainContent {

        public Bitmap m_bitmap;

        public int m_width;

        public int m_height;

        public Vector3[,] m_verts;

        public TerrainContent(Bitmap bitmap) {
            m_bitmap = bitmap;
        }

        public void process() {
            m_width = m_bitmap.Width;
            m_height = m_bitmap.Height;
            generateVerts();
            
        }

        private void generateVerts() {
            m_verts = new Vector3[m_width, m_height];

            for (int x = 0; x < m_width; x++) {
                for (int y = 0; y < m_height; y++) {
                   System.Drawing.Color pixelColor = m_bitmap.GetPixel(x, y);
                    float height = (float)(pixelColor.R + pixelColor.G + pixelColor.B) / (3 * 255);
                    m_verts[x, y] = new Vector3(x-m_width/2, height, y-m_height/2);
                }
            }
        }

        public void write(ContentWriter output) {
            output.Write(m_width);
            output.Write(m_height);
            for (int x = 0; x < m_width; x++) {
                for (int y = 0; y < m_height; y++) {
                    output.Write(m_verts[x, y]);
                }
            }
        }
    }
}
