using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace XEngine {
    class Origin : DrawableGameComponent {

        private BasicEffect m_basicEffect;

        private VertexPositionColor[] m_vertices;

        private float m_scale;

        public float scale {
            get { return m_scale; }
            set { 
                m_scale = value;
                m_vertices = GenerateVertices();
            }
        }

        public Origin(XEngineGame game)
            : base(game) {
        }

        override public void Initialize() {
            m_basicEffect = new BasicEffect(Game.GraphicsDevice);
            m_basicEffect.VertexColorEnabled = true;
            m_vertices = GenerateVertices();            
        }

        private VertexPositionColor[] GenerateVertices() {
            if (m_scale <= 0) {
                m_scale = 1.0f;
            }

            VertexPositionColor[] vertices = new VertexPositionColor[] {
                new VertexPositionColor(Vector3.Zero, Color.Red),
                new VertexPositionColor(Vector3.UnitX*m_scale, Color.Red),
                new VertexPositionColor(Vector3.Zero, Color.Green),
                new VertexPositionColor(Vector3.UnitY*m_scale, Color.Green),
                new VertexPositionColor(Vector3.Zero, Color.Blue),
                new VertexPositionColor(Vector3.UnitZ*m_scale, Color.Blue),
            };

            return vertices;
        }

        public override void Draw(GameTime gameTime) {
            ICamera camera = (ICamera)Game.Services.GetService( typeof( ICamera ) );

            m_basicEffect.World = Matrix.Identity;
            m_basicEffect.View = camera.View;
            m_basicEffect.Projection = camera.Projection;

            foreach (EffectPass pass in m_basicEffect.CurrentTechnique.Passes) {
                pass.Apply();

                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList,
                    m_vertices,
                    0, // vertex offset
                    3); // primitive count
            }
        }

        static public void TestOrigin() {
            XEngineComponentTest testGame = new XEngineComponentTest();
            testGame.BindGameComponent( new Camera( testGame ), typeof( ICamera ) );
            testGame.BindGameComponent( new Origin( testGame ) );
            testGame.Run();
        }

    }
}
