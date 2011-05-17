using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {
    public class LevelData {

        [ContentSerializer(CollectionItemName = "Entity" )]
        public List<EntityInstance> Entities;
    }
}
