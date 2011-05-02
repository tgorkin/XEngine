using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace EntityPipeline {

    public class EntityDictionary : Dictionary<string, EntityTemplate> {

       public void Merge( EntityDictionary newEntityDictionary ) {
            foreach ( KeyValuePair<string, EntityTemplate> entityTemplateEntry in newEntityDictionary ) {
                this.Add(entityTemplateEntry.Key, entityTemplateEntry.Value);
            }
        }
    }
}
