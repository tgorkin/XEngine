using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {

    public class EntityTemplate {

        [ContentSerializer( Optional = true, FlattenContent = true, CollectionItemName = "Attribute" )]
        public Dictionary<string, object> Attributes;

        [ContentSerializer( Optional = true, FlattenContent = true, CollectionItemName = "Component" )]
        public Dictionary<string, ComponentData> Components;

    }

}
