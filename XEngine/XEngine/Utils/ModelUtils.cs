using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {
    class ModelUtils {

        public static BoundingSphere GetGlobalBoundingSphere( Model model ) {
            Matrix[] absoluteBoneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo( absoluteBoneTransforms );
            BoundingSphere globalBoundingSphere = new BoundingSphere();
            foreach( ModelMesh mesh in model.Meshes) {
                CustomMeshData meshData = mesh.Tag as CustomMeshData;
                BoundingSphere meshBoundingSphere = new BoundingSphere();
                if ( meshData != null ) {
                    meshBoundingSphere = meshData.BoundingSphere;
                } else {
                    meshBoundingSphere = mesh.BoundingSphere;
                }
                meshBoundingSphere = meshBoundingSphere.Transform( absoluteBoneTransforms[mesh.ParentBone.Index] );
                globalBoundingSphere = BoundingSphere.CreateMerged( globalBoundingSphere, meshBoundingSphere );
            }
            return globalBoundingSphere;
        }

        public static BoundingBox GetGlobalBoundingBox( Model model ) {
            Matrix[] absoluteBoneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo( absoluteBoneTransforms );
            BoundingBox globalBoundingBox = new BoundingBox();
            foreach ( ModelMesh mesh in model.Meshes ) {
                CustomMeshData meshData = mesh.Tag as CustomMeshData;
                if ( meshData != null ) {
                    BoundingBox meshBoundingBox = ModelUtils.TransformBoundingBox( meshData.BoundingBox, absoluteBoneTransforms[mesh.ParentBone.Index]);
                    globalBoundingBox = BoundingBox.CreateMerged( globalBoundingBox, meshBoundingBox );
                }
            }
            return globalBoundingBox;
        }

        public static BoundingBox TransformBoundingBox( BoundingBox boundingBoxIn, Matrix transform ) {
            Vector3 min = boundingBoxIn.Min;
            Vector3 max = boundingBoxIn.Max;
            return new BoundingBox( Vector3.Transform( min, transform ), Vector3.Transform( max, transform ) );
        }

    }
}
