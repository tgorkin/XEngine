using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {
    public class EntityInstance {

        public string Name;

        public string Template;

        [ContentSerializer( Optional = true )]
        public string Parent;

        [ContentSerializer( Optional = true, FlattenContent = true)]
        public EntityTemplate EntityData;

    }
}
