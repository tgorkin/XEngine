using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class OrbitComponent : BaseComponent {

        private static readonly string COMPONENT_DATA_RADIUS = "Radius";

        private static readonly string COMPONENT_DATA_PERIOD = "Period";

        private EntityAttribute<Transform> m_transform;

        public float Radius { get; set; }

        public float Period { get; set; }

        public OrbitComponent( Entity entity ) : base( entity ) { }

        override public void Initialize() {
            m_transform = this.Entity.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform>;
        }

        override public void Update( GameTime gameTime ) {
            Vector3 origPostion = m_transform.Value.Position;
            float cycleProgress = (float)gameTime.TotalGameTime.TotalMilliseconds / Period;

            origPostion.X = (float)Math.Cos( cycleProgress ) * Radius;
            origPostion.Y = (float)Math.Sin( cycleProgress ) * Radius;

            m_transform.Value.Position = origPostion;
        }

        /*override public void LoadFromTemplate( ComponentTemplate componentTemplate ) {
            if ( componentTemplate.ComponentData.ContainsKey( COMPONENT_DATA_RADIUS ) ) {
                this.Radius = (float)componentTemplate.ComponentData[COMPONENT_DATA_RADIUS];
            }
            if ( componentTemplate.ComponentData.ContainsKey( COMPONENT_DATA_PERIOD ) ) {
                this.Period = (float)componentTemplate.ComponentData[COMPONENT_DATA_PERIOD];
            }
        }*/

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            Entity entity = null;
            testGame.InitDelegate = delegate {
                entity = new Entity();
                ModelRenderComponent.AddShipTestComponent( entity );
                AddTestComponent( entity );
                entity.Initialize();
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
            orbitComponent.Period = 2000f;
            entity.AddComponent( orbitComponent );
        }

    }
}
