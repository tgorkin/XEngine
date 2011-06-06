using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XEngine {
    class SettingsManager {

        private static readonly bool SETTING_MODEL_DEBUG_VIEW = false;

        public bool ShowModelDebugPrimitives {
            get { return SETTING_MODEL_DEBUG_VIEW; }
        }
    }
}
