using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class OrbitComponent : BaseComponent {

        private PositionAttribute m_position;

        public float Radius { get; set; }

        public float CycleTime { get; set; }

        public OrbitComponent( Entity entity )
            : base( entity ) {
            m_position = entity.getAttribute( Attributes.POSITION ) as PositionAttribute;
        }

        override public void Update( GameTime gameTime ) {
            Vector3 origPostion = m_position.Position;
            float cycleProgress = (float)gameTime.TotalGameTime.TotalMilliseconds / CycleTime;

            origPostion.X = (float)Math.Cos( cycleProgress ) * Radius;
            origPostion.Y = (float)Math.Sin( cycleProgress ) * Radius;

            m_position.Position = origPostion;
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity = null;
            testGame.InitDelegate = delegate {
                entity = new Entity();
                ModelRenderComponent.AddShipTestComponent( entity );
                AddTestComponent( entity );
            };
            testGame.UpdateDelegate = delegate( GameTime gameTime ) {
                entity.Update( gameTime );
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }

        static public void AddTestComponent( Entity entity ) {
            OrbitComponent orbitComponent = new OrbitComponent( entity );
            orbitComponent.Radius = 10f;
            orbitComponent.CycleTime = 2000f;
            entity.AddComponent( orbitComponent );
        }

    }
}
