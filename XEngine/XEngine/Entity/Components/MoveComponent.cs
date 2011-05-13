using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class MoveComponent : BaseComponent, IEntityComponent {

        private EntityAttribute<Transform> m_transform;

        private EntityAttribute<Vector3> m_velocity;

        public MoveComponent( Entity entity ) : base( entity ) { }

        override public void Initialize() {
            m_transform = this.Entity.GetAttribute( Attributes.TRANSFORM ) as EntityAttribute<Transform>;
            m_velocity = this.Entity.GetAttribute( Attributes.VELOCITY ) as EntityAttribute<Vector3>;
        }

        override public void Update( GameTime gameTime ) {
            m_transform.Value.Position += Vector3.Multiply( m_velocity.Value, (float)gameTime.ElapsedGameTime.Milliseconds / 1000 );
        }

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

        static public void AddTestComponent(Entity entity) {
            // Setup a velocity attribute
            EntityAttribute<Vector3> velocity = new EntityAttribute<Vector3>();
            velocity.Value = new Vector3(0, 0, -1.0f);
            entity.AddAttribute( Attributes.VELOCITY, velocity );

            // Add the test component
            MoveComponent moveComponent = new MoveComponent( entity );
            entity.AddComponent( moveComponent );
        }

    }
}
