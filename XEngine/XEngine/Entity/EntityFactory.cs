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
                Type buildTimeDataType = pair.Value.GetType();
                string buildTimeDataTypeString = buildTimeDataType.ToString();
                string[] splitString = buildTimeDataTypeString.Split('.');
                buildTimeDataTypeString = splitString[splitString.Length - 1];
                int splitIndex = buildTimeDataTypeString.LastIndexOf( "Data" );
                string runTimeDataTypeString = buildTimeDataTypeString.Substring( 0, splitIndex );
                
                //Type attributeType = Type.GetType( "XEngine." + pair.Key );

                Type attributeType = null;
                if ( runTimeDataTypeString == "BaseAttribute" ) {
                    Type dataType = (pair.Value as BaseAttributeData).DataValue.GetType();
                    attributeType = Type.GetType( "XEngine." + runTimeDataTypeString + "`1" ).MakeGenericType( dataType );
                } else {
                    attributeType = Type.GetType( "XEngine." + runTimeDataTypeString );
                }
                IEntityAttribute attribute = (IEntityAttribute)( System.Activator.CreateInstance( attributeType ) );
                attribute.LoadData( pair.Value );
                entity.addAttribute( pair.Key, attribute );
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
