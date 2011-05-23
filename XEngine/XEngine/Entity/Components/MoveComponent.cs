using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class MoveComponent : BaseComponent, IEntityComponent {

        private Transform m_transform;

        private Vector3 m_velocity;

        public MoveComponent( Entity entity ) : base( entity ) { }

        override public void Initialize() {
            m_transform = this.Entity.GetAttribute<Transform>( Attributes.TRANSFORM );
            m_velocity = this.Entity.GetAttribute<Vector3>( Attributes.VELOCITY );
        }

        override public void Update( GameTime gameTime ) {
            m_transform.Position += Vector3.Multiply( m_velocity, (float)gameTime.ElapsedGameTime.Milliseconds / 1000 );
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
            Vector3 velocity = new Vector3(0, 0, -1.0f);
            entity.AddAttribute( Attributes.VELOCITY, velocity );

            // Add the test component
            MoveComponent moveComponent = new MoveComponent( entity );
            entity.AddComponent( moveComponent );
        }

    }
}
