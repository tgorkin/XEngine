using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XEngineTypes;

namespace XEngine {

    class Origin {

        private VertexPositionColor[] m_vertices = new VertexPositionColor[6];

        private BasicEffect m_basicEffect;

        public Transform Transform = new Transform();

        private float m_size;

        public Origin( float size ) 
            : base() {
            m_size = size;
        }

        public Origin( )
            : this(1.0f) {
        }

        public void Initialize() {
            m_vertices[0] = ( new VertexPositionColor( new Vector3( 0, 0, 0 ), Color.Red ) );
            m_vertices[1] = ( new VertexPositionColor( new Vector3( m_size, 0, 0 ), Color.Red ) );
            m_vertices[2] = ( new VertexPositionColor( new Vector3( 0, 0, 0 ), Color.Lime ) );
            m_vertices[3] = ( new VertexPositionColor( new Vector3( 0, m_size, 0 ), Color.Lime ) );
            m_vertices[4] = ( new VertexPositionColor( new Vector3( 0, 0, 0 ), Color.Blue ) );
            m_vertices[5] = ( new VertexPositionColor( new Vector3( 0, 0, m_size ), Color.Blue ) );
            m_basicEffect = new BasicEffect( ServiceLocator.Graphics );
        }

        public void Draw( GameTime gameTime ) {
            ICamera camera = ServiceLocator.Camera;

            m_basicEffect.World = Transform.World;
            m_basicEffect.View = camera.View;
            m_basicEffect.Projection = camera.Projection;
            m_basicEffect.VertexColorEnabled = true;
            m_basicEffect.LightingEnabled = false;
            m_basicEffect.TextureEnabled = false;

            GraphicsDevice graphicsDevice = m_basicEffect.GraphicsDevice;
            graphicsDevice.BlendState = BlendState.Opaque;

            foreach ( EffectPass effectPass in m_basicEffect.CurrentTechnique.Passes ) {
                effectPass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionColor>( PrimitiveType.LineList, m_vertices, 0, 3 );
            }
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Origin origin = null;
            testGame.InitDelegate = delegate {
                origin = new Origin();
                origin.Initialize();
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                origin.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
