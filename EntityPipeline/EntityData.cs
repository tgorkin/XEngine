using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace EntityPipeline {
    public class EntityData {

        public string Name;

        [ContentSerializer( Optional = true )]
        public Dictionary<string, object> AttributeData;

        [ContentSerializer( Optional = true )]
        public Dictionary<string, object> ComponentData;

        public static void SerializeTest() {
            Dictionary<string, Dictionary<string, Vector3>> testData = new Dictionary<string, Dictionary<string, Vector3>>();
            // add first inner dictionary
            Dictionary<string, Vector3> innerDict0 = new Dictionary<string, Vector3>();
            innerDict0.Add( "position", Vector3.UnitX );
            innerDict0.Add( "scale", Vector3.UnitY );
            testData.Add( "innerDict0", innerDict0 );
            // add second inner dictionary
            Dictionary<string, Vector3> innerDict1 = new Dictionary<string, Vector3>();
            innerDict1.Add( "blah", Vector3.UnitX );
            innerDict1.Add( "bleh", Vector3.UnitY );
            testData.Add( "innerDict1", innerDict1 );

            /*XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using ( XmlWriter writer = XmlWriter.Create( "test.xml", settings ) ) {
                IntermediateSerializer.Serialize( writer, testData, null );
            }*/
            SaveToXml( testData );
        }

        public static void SaveToXml( object testData ) {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using ( XmlWriter writer = XmlWriter.Create( "test.xml", settings ) ) {
                IntermediateSerializer.Serialize( writer, testData, null );
            }
        }
    }
}
