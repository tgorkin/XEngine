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
                // create and load data for all entity attributes
                foreach ( KeyValuePair<string, object> attributeData in entityTemplate.Attributes ) {
                    Type attributeDataType = Type.GetType( "XEngine.EntityAttribute`1" ).MakeGenericType( attributeData.Value.GetType() );
                    IEntityAttribute attribute = (IEntityAttribute)( System.Activator.CreateInstance( attributeDataType, attributeData.Value ) );
                    newEntity.AddAttribute( attributeData.Key, attribute );
                }
                // create and load data for all entity components
                foreach ( KeyValuePair<string, ComponentData> componentTemplate in entityTemplate.Components ) {
                    Type componentType = Type.GetType( "XEngine." + componentTemplate.Key );
                    IEntityComponent component = (IEntityComponent)( System.Activator.CreateInstance( componentType, newEntity ) );
                    if ( componentTemplate.Value != null ) {
                        component.LoadData( componentTemplate.Value );
                    }
                    newEntity.AddComponent( component );
                }
            }
            return newEntity;
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
            };
            testGame.DrawDelegate = delegate( GameTime gameTime ) {
                entity.Draw( gameTime );
            };
            testGame.Run();
        }
    }
}
