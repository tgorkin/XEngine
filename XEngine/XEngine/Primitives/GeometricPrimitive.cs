using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace XEngine {

    public class GeometricPrimitive : PositionedObject {

        private Color m_color;

        // During the process of constructing a primitive model, vertex
        // and index data is stored on the CPU in these managed lists.
        protected List<VertexPositionNormal> m_vertices = new List<VertexPositionNormal>();

        protected List<ushort> m_indices = new List<ushort>();

        // Once all the geometry has been specified, the InitializePrimitive
        // method copies the vertex and index data into these buffers, which
        // store it on the GPU ready for efficient rendering.
        protected VertexBuffer m_vertexBuffer;
        protected IndexBuffer m_indexBuffer;
        protected BasicEffect m_basicEffect;

        public GeometricPrimitive(Game game, Color color) : base(game) {
            m_color = color;
        }

        /// <summary>
        /// Adds a new vertex to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddVertex(Vector3 position, Vector3 normal) {
            m_vertices.Add(new VertexPositionNormal(position, normal));
        }


        /// <summary>
        /// Adds a new index to the primitive model. This should only be called
        /// during the initialization process, before InitializePrimitive.
        /// </summary>
        protected void AddIndex(int index) {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            m_indices.Add((ushort)index);
        }

        /// <summary>
        /// Queries the index of the current vertex. This starts at
        /// zero, and increments every time AddVertex is called.
        /// </summary>
        protected int CurrentVertex {
            get { return m_vertices.Count; }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize() {
            GenerateGeometry();
            InitializeBuffers();
            base.Initialize();
        }

        virtual protected void GenerateGeometry() {
            // do nothing by default
        }

        /// <summary>
        /// Once all the geometry has been specified by calling AddVertex and AddIndex,
        /// this method copies the vertex and index data into GPU format buffers, ready
        /// for efficient rendering.
        protected void InitializeBuffers() {
            // Create a vertex declaration, describing the format of our vertex data.

            // Create a vertex buffer, and copy our vertex data into it.
            m_vertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionNormal), m_vertices.Count, BufferUsage.None);
            m_vertexBuffer.SetData(m_vertices.ToArray());

            // Create an index buffer, and copy our index data into it.
            m_indexBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(ushort), m_indices.Count, BufferUsage.None);
            m_indexBuffer.SetData(m_indices.ToArray());

            // Create a BasicEffect, which will be used to render the primitive.
            m_basicEffect = new BasicEffect(Game.GraphicsDevice);
            m_basicEffect.EnableDefaultLighting();
        }

        /// <summary>
        /// Frees resources used by this object.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (m_vertexBuffer != null)
                    m_vertexBuffer.Dispose();

                if (m_indexBuffer != null)
                    m_indexBuffer.Dispose();

                if (m_basicEffect != null)
                    m_basicEffect.Dispose();
            }
        }

        /// <summary>
        /// Draws the primitive model, using a BasicEffect shader with default
        /// lighting. Unlike the other Draw overload where you specify a custom
        /// effect, this method sets important renderstates to sensible values
        /// for 3D model rendering, so you do not need to set these states before
        /// you call it.
        /// </summary>
        public override void Draw(GameTime gameTime) {
            // Set BasicEffect parameters.
            m_basicEffect.World = this.getWorld();
            m_basicEffect.View = Camera.Instance.View;
            m_basicEffect.Projection = Camera.Instance.Projection;
            m_basicEffect.DiffuseColor = m_color.ToVector3();
            m_basicEffect.Alpha = m_color.A / 255.0f;

            GraphicsDevice graphicsDevice = m_basicEffect.GraphicsDevice;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (m_color.A < 255) {
                // Set renderstate for alpha blended rendering
                graphicsDevice.BlendState = BlendState.AlphaBlend;
            } else {
                // Set renderstate for opaque rendering
                graphicsDevice.BlendState = BlendState.Opaque;
            }
            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(m_vertexBuffer);
            graphicsDevice.Indices = m_indexBuffer;

            foreach (EffectPass effectPass in m_basicEffect.CurrentTechnique.Passes) {
                effectPass.Apply();

                int primitiveCount = m_indices.Count / 3;

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_vertices.Count, 0, primitiveCount);

            }
        }

        static public void TestGeometricPrimitives() {
            Cube cube = new Cube( XEngineComponentTest.TestGame, Color.Purple, 1.0f );
            XEngineComponentTest.TestGame.Components.Add( cube );
            Sphere sphere = new Sphere( XEngineComponentTest.TestGame, Color.Green );
            sphere.position = new Vector3( 2.0f, 0, 0 );
            XEngineComponentTest.TestGame.Components.Add( sphere );
            Torus torus = new Torus( XEngineComponentTest.TestGame, Color.Gold );
            torus.position = new Vector3( -2.0f, 0, 0 );
            XEngineComponentTest.TestGame.Components.Add( torus );
            Cylinder cylinder = new Cylinder( XEngineComponentTest.TestGame, Color.Red );
            cylinder.position = new Vector3( 0, 2.0f, 0 );
            XEngineComponentTest.TestGame.Components.Add( cylinder );
            XEngineComponentTest.StartTest();
        }
    }
}
