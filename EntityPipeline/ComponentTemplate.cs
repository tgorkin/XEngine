using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EntityPipeline {
    public class ComponentTemplate {

        [ContentSerializer( Optional = true, FlattenContent = true, CollectionItemName = "ComponentData" )]
        public Dictionary<string, object> ComponentData;

    }
}
