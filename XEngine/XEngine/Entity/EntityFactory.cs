using System;
using System.Collections.Generic;
using EntityPipeline;
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
                    IEntityAttribute attribute = null;
                    if ( attributeData.Key == Attributes.TRANSFORM ) {
                        attribute = new TransformAttribute( attributeData.Value as TransformData );
                    } else {
                        Type attributeDataType = Type.GetType( "XEngine.EntityAttribute`1" ).MakeGenericType( attributeData.Value.GetType() );
                        attribute = (IEntityAttribute)( System.Activator.CreateInstance( attributeDataType, attributeData.Value ) );
                    }
                    newEntity.AddAttribute( attributeData.Key, attribute );
                }
                // create and load data for all entity components
                foreach ( KeyValuePair<string, ComponentTemplate> componentTemplate in entityTemplate.Components ) {
                    Type componentDataType = Type.GetType( "XEngine." + componentTemplate.Key );
                    IEntityComponent component = (IEntityComponent)( System.Activator.CreateInstance( componentDataType, newEntity ) );
                    if ( componentTemplate.Value != null ) {
                        component.LoadFromTemplate( componentTemplate.Value );
                    }
                    newEntity.AddComponent( component );
                }
            }
            return newEntity;
        }

        static public void LoadEntityDataTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            testGame.InitDelegate = delegate {
                EntityTemplate entityTemplate = ServiceLocator.Content.Load<EntityTemplate>( "Data/EntityTest" );
                System.Diagnostics.Debugger.Break();
            };
            testGame.Run();
        }

        static public void LoadEntityListTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            testGame.InitDelegate = delegate {
                EntityDictionary entityDictionary = ServiceLocator.Content.Load<EntityDictionary>( "Data/EntityTemplates" );
                System.Diagnostics.Debugger.Break();
            };
            testGame.Run();
        }

        static public void CreateEntityTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            EntityFactory entityFactory = new EntityFactory();
            Entity entity = null;
            testGame.InitDelegate = delegate {
                entityFactory.LoadEntityTemplates( "Data/EntityTemplates" );
                entity = entityFactory.CreateEntity( "Ship" );
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
