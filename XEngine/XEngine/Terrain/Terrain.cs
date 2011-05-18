using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XEngine {
    class Terrain {

        private int m_xLength;

        private int m_zLength;

        private VertexPositionNormalTexture[] m_verts;

        private int[] m_indices;

        private float m_heightOffset;

        private float m_heightScale;

        private float m_gridSpacing;

        private float m_textureSpacing;

        private Matrix m_objectMat;

        private BasicEffect m_effect;

        private VertexBuffer m_vertexBuffer;

        private IndexBuffer m_indexBuffer;

        private Texture2D m_texture;

        public Terrain( ContentReader input) {
            m_xLength = input.ReadInt32();
            m_zLength = input.ReadInt32();
            m_objectMat = Matrix.Identity;

            m_verts = new VertexPositionNormalTexture[m_xLength * m_zLength];
            int i = 0;
            for (int x = 0; x < m_xLength; x++) {
                for (int z = 0; z < m_zLength; z++) {
                    Vector3 point = input.ReadVector3();
                    Vector2 texCoord = new Vector2(x, z);
                    m_verts[i++] = new VertexPositionNormalTexture(point, Vector3.Up, texCoord);
                }
            }
            GenerateIndices();
            VertexUtils.GenerateNormalsForTriangleStrip( m_verts, m_indices );
            CreateRenderData();
        }

        public void ScaleTerrain(float heightScale, float heightOffset, float gridSpacing) {
            m_heightScale = heightScale;
            m_heightOffset = heightOffset;
            m_gridSpacing = gridSpacing;
            //m_objectMat = Matrix.CreateScale(new Vector3(m_gridSpacing, m_heightScale, m_gridSpacing)) * Matrix.CreateTranslation(0, m_heightOffset, 0);
            m_objectMat = Matrix.CreateTranslation(0, m_heightOffset, 0);

            for (int i = 0; i < m_verts.Length; i++) {
                m_verts[i].Position.X *= m_gridSpacing;
                m_verts[i].Position.Y *= m_heightScale;
                m_verts[i].Position.Z *= m_gridSpacing;
            }

            VertexUtils.GenerateNormalsForTriangleStrip( m_verts, m_indices );
            m_vertexBuffer.SetData(m_verts);
        }

        public void LoadTexture(string textureName, float texSpacing) {
            m_texture = ServiceLocator.Content.Load<Texture2D>("Textures\\" + textureName);
            m_textureSpacing = texSpacing;
            for (int i = 0; i < m_verts.Length; i++) {
                m_verts[i].TextureCoordinate /= texSpacing;
            }
            m_vertexBuffer.SetData(m_verts);
        }

        private void GenerateIndices() {
            m_indices = new int[m_xLength * 2 * (m_zLength - 1)];
            int i = 0;
            int z = 0;
            while (z < m_zLength - 1) {
                // create a rows worth of indices going from left to right
                for (int x = 0; x < m_xLength; x++) {
                    m_indices[i++] = x + z * m_xLength; // lower vert
                    m_indices[i++] = x + (z + 1) * m_xLength; // upper vert
                }
                z++;

                // create another rows worth of indices going from right to left
                if (z < m_zLength - 1) {
                    for (int x = m_xLength-1; x >= 0; x--) {
                        m_indices[i++] = x + (z + 1) * m_xLength; // upper vert
                        m_indices[i++] = x + z * m_xLength; // lower vert
                    }
                    z++;
                }
            }
        }

        private void CreateRenderData() {
            GraphicsDevice graphicsDevice = ServiceLocator.Graphics;

            m_vertexBuffer = new VertexBuffer( graphicsDevice, typeof( VertexPositionNormalTexture ), m_verts.Length, BufferUsage.WriteOnly );
            m_vertexBuffer.SetData(m_verts);

            m_indexBuffer = new IndexBuffer( graphicsDevice, typeof( int ), m_indices.Length, BufferUsage.WriteOnly );
            m_indexBuffer.SetData(m_indices);

            m_effect = new BasicEffect( graphicsDevice );
        }

        public void Draw() {
            ICamera camera = ServiceLocator.Camera;

            m_effect.World = Matrix.Identity;
            m_effect.View = camera.View;
            m_effect.Projection = camera.Projection;
            m_effect.Texture = m_texture;
            m_effect.TextureEnabled = true;

            m_effect.EnableDefaultLighting();
            m_effect.DirectionalLight0.Direction = new Vector3(1, -1, 1);
            m_effect.DirectionalLight0.Enabled = true;
            m_effect.DirectionalLight1.Enabled = false;
            m_effect.DirectionalLight2.Enabled = false;
            m_effect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
            m_effect.SpecularColor = Vector3.Zero;

            GraphicsDevice graphics = m_effect.GraphicsDevice;
            graphics.SetVertexBuffer( m_vertexBuffer );
            graphics.Indices = m_indexBuffer;

            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.BlendState = BlendState.Opaque;
            graphics.SamplerStates[0] = SamplerState.AnisotropicWrap;
            //graphics.RenderState.FillMode = FillMode.WireFrame;

            foreach (EffectPass effectPass in m_effect.CurrentTechnique.Passes) {
                effectPass.Apply();

                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleStrip,  0, 0, m_xLength * m_zLength,  0,  m_xLength * 2 * (m_zLength - 1) - 2);

            }
            //graphics.RenderState.FillMode = FillMode.Solid;

        }

        static public void TestTerrain() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Terrain terrain = null;
            testGame.InitDelegate = delegate {
                    terrain = testGame.Content.Load<Terrain>( "HeightMaps\\testheightmap" );
                    terrain.ScaleTerrain(5.0f, -2.0f, 0.5f);
                    terrain.LoadTexture("grass", 10);
               };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                terrain.Draw();
            };
            testGame.Run();
        }
    }
}
