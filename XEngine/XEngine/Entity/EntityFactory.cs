using System;
using System.Collections.Generic;
using XEngineTypes;
using Microsoft.Xna.Framework;

namespace XEngine {
    class EntityFactory {

        private EntityDictionary m_entityDictionary = new EntityDictionary();

        public void LoadEntityTemplates(string entityTemplateAssetPath) {
            EntityDictionary loadedDictionary = ServiceLocator.Content.Load<EntityDictionary>( entityTemplateAssetPath );
            m_entityDictionary.Merge( loadedDictionary );
        }

        public Entity CreateEntity( string entityTemplateName ) {
            Entity newEntity = new Entity();
            if ( m_entityDictionary.ContainsKey( entityTemplateName ) ) {
                EntityTemplate entityTemplate = m_entityDictionary[entityTemplateName];
                AddAttributes( entityTemplate.Attributes, newEntity );
                AddComponents( entityTemplate.Components, newEntity );
            }
            return newEntity;
        }

        public Entity CreateEntityWithData( string entityTemplateName, EntityInstance data ) {
            Entity newEntity = CreateEntity( entityTemplateName );
            AddAttributes( data.EntityData.Attributes, newEntity );
            AddComponents( data.EntityData.Components, newEntity );
            return newEntity;
        }

        private void AddAttributes( Dictionary<string, object> attributes, Entity entity ) {
            // create and load data for all entity attributes
            foreach ( KeyValuePair<string, object> attributeData in attributes ) {
                // TODO: make sure these attributes are deep copied
                entity.AddAttribute( attributeData.Key, attributeData.Value );
            }
        }

        private void AddComponents( Dictionary<string, ComponentData> components, Entity entity ) {
            // create and load data for all entity components
            foreach ( KeyValuePair<string, ComponentData> componentTemplate in components ) {
                Type componentType = Type.GetType( "XEngine." + componentTemplate.Key );
                IEntityComponent component = (IEntityComponent)( System.Activator.CreateInstance( componentType, entity ) );
                if ( componentTemplate.Value != null ) {
                    component.LoadData( componentTemplate.Value );
                }
                entity.AddComponent( component );
            }
        }

        static public void LoadEntityListTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            testGame.InitDelegate = delegate {
                EntityDictionary entityDictionary = ServiceLocator.Content.Load<EntityDictionary>( "Data/EntityTemplates" );
                System.Diagnostics.Debugger.Break();
            };
            testGame.Run();
        }

        static public void EntityTest(string entityName) {
            XEngineComponentTest testGame = new XEngineComponentTest();

            EntityFactory entityFactory = new EntityFactory();
            Entity entity = null;
            testGame.InitDelegate = delegate {
                entityFactory.LoadEntityTemplates( "Data/EntityTemplates" );
                entity = entityFactory.CreateEntity( entityName );
                entity.Initialize();
            };
            testGame.UpdateDelegate = delegate( GameTime gameTime ) {
                entity.Update( gameTime );
                Transform transform = entity.GetAttribute<Transform>( Attributes.TRANSFORM );
                if ( transform != null ) {
                    transform.UpdateWorld( null );
                }
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
