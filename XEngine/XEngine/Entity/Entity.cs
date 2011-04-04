using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class Entity : IGameComponent {

        private List<IEntityComponent> m_components = new List<IEntityComponent>();

        private Dictionary<string, IEntityAttribute> m_attributes = new Dictionary<string, IEntityAttribute>();

        public void AddComponent( IEntityComponent newComponent ) {
            m_components.Add( newComponent );
        }

        public IEntityAttribute getAttribute( string attributeName ) {
            IEntityAttribute result = null;
            if ( m_attributes.ContainsKey( attributeName ) ) {
                result = m_attributes[attributeName];
            }
            return result;
        }

        public void addAttribute( string attributeName, IEntityAttribute attribute ) {
            m_attributes[attributeName] = attribute;
        }

        public void Initialize( ) {

        }

        public void Update( GameTime gameTime ) {
            foreach(IEntityComponent component in m_components) {
                component.Update( gameTime );
            }
        }

        public void Draw( GameTime gameTime ) {
            foreach ( IEntityComponent component in m_components ) {
                component.Draw( gameTime );
            }
        }
    }
}
