using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class VertexUtils {

        public static void GenerateNormalsForTriangleStrip( VertexPositionNormalTexture[] verts, int[] indices ) {
            // first reset all normals
            VertexUtils.ResetAllNormals( verts );

            // iterate through and add normals to all vertices for each indexed primitive
            //  because indexed as triange strip, need to keep track of swap vert winding to get normals pointing in correct direction
            bool swapWinding = false;
            for ( int i = 2; i < indices.Length; i++ ) {
                Vector3 firstVec = verts[indices[i - 1]].Position - verts[indices[i]].Position;
                Vector3 secondVec = verts[indices[i - 2]].Position - verts[indices[i]].Position;
                Vector3 normal = Vector3.Cross( firstVec, secondVec );
                normal.Normalize();

                if ( swapWinding ) {
                    normal *= -1;
                }

                verts[indices[i]].Normal += normal;
                verts[indices[i - 1]].Normal += normal;
                verts[indices[i - 2]].Normal += normal;

                swapWinding = !swapWinding;
            }

            VertexUtils.NormalizeAllNormals( verts );
        }

        public static void ResetAllNormals( VertexPositionNormalTexture[] verts ) {
            for ( int i = 0; i < verts.Length; i++ ) {
                verts[i].Normal = Vector3.Zero;
            }
        }

        public static void NormalizeAllNormals( VertexPositionNormalTexture[] verts ) {
            for ( int i = 0; i < verts.Length; i++ ) {
                verts[i].Normal.Normalize();
            }
        }
    }
}
