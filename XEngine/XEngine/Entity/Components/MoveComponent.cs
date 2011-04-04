using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class MoveComponent : BaseComponent, IEntityComponent {

        BaseAttribute<Vector3> m_velocity;

        PositionAttribute m_position;

        public MoveComponent( Entity entity ) : base( entity ) {
            m_velocity = entity.getAttribute( Attributes.VELOCITY ) as BaseAttribute<Vector3>;
            m_position = entity.getAttribute( Attributes.POSITION ) as PositionAttribute;
        }

        override public void Update( GameTime gameTime ) {
            m_position.Position += Vector3.Multiply( m_velocity.Value, (float)gameTime.ElapsedGameTime.Milliseconds / 1000 );
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

        static public void AddTestComponent(Entity entity) {
            // Setup a velocity attribute
            BaseAttribute<Vector3> velocity = new BaseAttribute<Vector3>();
            velocity.Value = new Vector3(0, 0, -1.0f);
            entity.addAttribute( Attributes.VELOCITY, velocity );

            // Add the test component
            MoveComponent moveComponent = new MoveComponent( entity );
            entity.AddComponent( moveComponent );
        }

    }
}
