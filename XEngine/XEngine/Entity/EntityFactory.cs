using System;
using System.Collections.Generic;
using EntityPipeline;

namespace XEngine {
    class EntityFactory {

        public Entity BuildEntity(EntityData entityData) {
            Entity entity = new Entity();
            // iterate through all the attributes
            foreach ( KeyValuePair<string, object> pair in entityData.AttributeData ) {
                // get the type of attribute to construct
                Type attributeType = Type.GetType( "XEngine." + pair.Key );
                IEntityAttribute attribute = (IEntityAttribute)( System.Activator.CreateInstance( attributeType ) );
                //attribute.Load( pair.Value );
            }
            return entity;
        }

        static public void ComponentTest() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            EntityData entityData;
            testGame.InitDelegate = delegate {
                entityData = ServiceLocator.Content.Load<EntityData>( "Data/entityData" );
                EntityFactory factory = new EntityFactory();
                Entity entity = factory.BuildEntity( entityData );
                System.Diagnostics.Trace.Write( entityData );
            };
            testGame.Run();
        }
    }
}
