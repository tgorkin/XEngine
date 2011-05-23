using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    class Entity : IGameComponent {

        private List<IEntityComponent> m_components = new List<IEntityComponent>();

        private Dictionary<string, object> m_attributes = new Dictionary<string, object>();

        public List<IEntityComponent> Components {
            get { return m_components; }
            set { m_components = value; }
        }

        public void AddComponent( IEntityComponent newComponent ) {
            m_components.Add( newComponent );
        }

        public IEntityComponent GetComponentOfType( Type type ) {
            IEntityComponent result = null;
            foreach ( IEntityComponent component in m_components ) {
                if ( component.GetType() == type ) {
                    result = component;
                    break;
                }
            }
            return result;
        }

        public T GetAttribute<T>( string attributeName ) {
            T result = default(T);
            if ( m_attributes.ContainsKey( attributeName ) ) {
                var attribute = m_attributes[attributeName];
                if ( attribute.GetType() == typeof(T) ) {
                    result = (T)attribute;
                }
            }
            return result;
        }

        public void AddAttribute( string attributeName, object attribute ) {
            m_attributes[attributeName] = attribute;
        }

        public void Initialize( ) {
            foreach ( IEntityComponent component in m_components ) {
                component.Initialize();
            }
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
