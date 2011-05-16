using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XEngineTypes {

    public class ModelBoneControllerData : ComponentData {

        private static readonly float DEFAULT_SPEED = 1.0f;

        public string BoneName;

        public TransformType TransformType;

        [ContentSerializer( Optional = true )]
        public float Speed = DEFAULT_SPEED;

    }
}
