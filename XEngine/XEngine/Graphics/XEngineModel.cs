using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {

    class XEngineModel {

        private string m_modelName;

        private Model m_model;

        private Matrix m_localTransform;

        private Matrix[] m_absoluteBoneTransforms;

        private BoundingSphere m_modelBoundingSphere;

        private BoundingBox m_modelBoundingBox;

        private BoundingSpherePrimitive m_modelBoundingSpherePrimitive;

        private BoundingBoxPrimitive m_modelBoundingBoxPrimitive;

        private Dictionary<string, BoundingSphere> m_meshBoundingSpheres;

        private Dictionary<string, BoundingBox> m_meshBoundingBoxes;

        private Dictionary<string, BoundingVolumePrimitive> m_meshBoundingSpherePrimitives;

        private Dictionary<string, BoundingVolumePrimitive> m_meshBoundingBoxPrimitives;

        private Origin[] m_meshOrigins;

        private Dictionary<string, object> m_tagData;

        public XEngineModel( string modelName ) {
            m_modelName = modelName;
        }

        public Model Model {
            get { return m_model; }
        }

        public void Initialize() {
            m_model = ServiceLocator.Content.Load<Model>( m_modelName );
            m_tagData = m_model.Tag as Dictionary<string, object>;
            m_absoluteBoneTransforms = new Matrix[m_model.Bones.Count];
            m_localTransform = Matrix.Identity;

            SetupBoundingVolumes();
            if ( ServiceLocator.SettingsManager.ShowModelDebugPrimitives ) {
                CreateBoundingVolumePrimitives();
                CreateMeshOrigins();
            }
        }

        private void SetupBoundingVolumes() {
            m_meshBoundingSpheres = new Dictionary<string, BoundingSphere>();
            m_meshBoundingBoxes = new Dictionary<string, BoundingBox>();
            foreach ( ModelMesh mesh in m_model.Meshes ) {
                CustomMeshData meshData = mesh.Tag as CustomMeshData;
                if ( meshData != null ) {
                    if ( meshData.BoundingSphere != null ) {
                        m_meshBoundingSpheres.Add( mesh.Name, meshData.BoundingSphere );
                    }
                    if ( meshData.BoundingBox != null ) {
                        m_meshBoundingBoxes.Add( mesh.Name, meshData.BoundingBox );
                    }
                }
            }

            m_modelBoundingSphere = ModelUtils.GetGlobalBoundingSphere( m_model );
            m_modelBoundingBox = ModelUtils.GetGlobalBoundingBox( m_model );
        }

        private void CreateBoundingVolumePrimitives() {
            m_modelBoundingSpherePrimitive = new BoundingSpherePrimitive( m_modelBoundingSphere, Color.SlateGray );
            m_modelBoundingBoxPrimitive = new BoundingBoxPrimitive( m_modelBoundingBox, Color.MediumOrchid );

            m_meshBoundingSpherePrimitives = new Dictionary<string, BoundingVolumePrimitive>();
            foreach ( KeyValuePair<string, BoundingSphere> boundingSphereData in m_meshBoundingSpheres ) {
                BoundingSpherePrimitive boundingSpherePrimitive = new BoundingSpherePrimitive( boundingSphereData.Value, Color.Indigo );
                m_meshBoundingSpherePrimitives.Add( boundingSphereData.Key, boundingSpherePrimitive );
            }

            m_meshBoundingBoxPrimitives = new Dictionary<string, BoundingVolumePrimitive>();
            foreach ( KeyValuePair<string, BoundingBox> boundingBoxData in m_meshBoundingBoxes ) {
                BoundingBoxPrimitive boundingBoxPrimitive = new BoundingBoxPrimitive( boundingBoxData.Value, Color.LawnGreen );
                m_meshBoundingBoxPrimitives.Add( boundingBoxData.Key, boundingBoxPrimitive );
            }
        }

        private void CreateMeshOrigins() {
            m_meshOrigins = new Origin[m_model.Meshes.Count];
            int meshIndex = 0;
            foreach ( ModelMesh mesh in m_model.Meshes ) {
                Origin meshOrigin = new Origin();
                meshOrigin.Initialize();
                m_meshOrigins[meshIndex++] = meshOrigin;
            }
        }

        public void Draw( Matrix world) {
            ICamera camera = ServiceLocator.Camera;
            bool isDebugViewOn = ServiceLocator.SettingsManager.ShowModelDebugPrimitives;

            Matrix modelWorldTransform = m_localTransform * world;
            m_model.CopyAbsoluteBoneTransformsTo( m_absoluteBoneTransforms );

            for ( int meshIdx = 0; meshIdx < m_model.Meshes.Count; meshIdx++ ) {
                ModelMesh mesh = m_model.Meshes[meshIdx];
                Matrix meshWorldTransform = m_absoluteBoneTransforms[mesh.ParentBone.Index] * modelWorldTransform;
                foreach ( BasicEffect effect in mesh.Effects ) {
                    effect.EnableDefaultLighting();
                    effect.GraphicsDevice.BlendState = BlendState.Opaque;
                    effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = meshWorldTransform;
                }
                mesh.Draw();

                // draw debug primitives for each model mesh
                if ( isDebugViewOn ) {
                    DrawMeshBoundingVolumes( m_meshBoundingSpherePrimitives, mesh, meshWorldTransform );
                    DrawMeshBoundingVolumes( m_meshBoundingBoxPrimitives, mesh, meshWorldTransform );
                    m_meshOrigins[meshIdx].Draw( meshWorldTransform );
                }
            }
            // draw debug primitives for the overall model
            if ( isDebugViewOn ) {
                m_modelBoundingSpherePrimitive.Draw( modelWorldTransform );
                m_modelBoundingBoxPrimitive.Draw( modelWorldTransform );
            }
        }

        private void DrawMeshBoundingVolumes( Dictionary<string, BoundingVolumePrimitive> meshBoundingVolumes, ModelMesh mesh, Matrix meshWorldTransform ) {
            if ( meshBoundingVolumes.ContainsKey( mesh.Name ) ) {
                BoundingVolumePrimitive boundingVolume = meshBoundingVolumes[mesh.Name];
                boundingVolume.Draw( meshWorldTransform );
            }
        }
    }
}
