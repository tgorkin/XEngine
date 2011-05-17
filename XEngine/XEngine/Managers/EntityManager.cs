using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {

    class EntityManager : DrawableGameComponent {

        private Dictionary<string, Entity> m_entities = new Dictionary<string, Entity>();

        public EntityManager( Game game ) : base( game ) { }

        public void ClearEntities() {
            m_entities = new Dictionary<string, Entity>();
        }

        public void AddEntity( string entityName, Entity entity ) {
            m_entities.Add( entityName, entity );
        }

        public Entity FindEntity( string entityName ) {
            Entity result = null;
            if ( entityName != null && m_entities.ContainsKey( entityName ) ) {
                result = m_entities[entityName];
            }
            return result;
        }

        override public void Initialize() {
            foreach ( Entity entity in m_entities.Values ) {
                entity.Initialize();
            }
        }

        override public void Update( GameTime gameTime ) {
            foreach ( Entity entity in m_entities.Values ) {
                entity.Update( gameTime );
            }
        }

        override public void Draw( GameTime gameTime ) {
            foreach ( Entity entity in m_entities.Values ) {
                entity.Draw( gameTime );
            }
        }
    }
}
