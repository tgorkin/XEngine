using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XEngine {
    class EntityAttribute<T> : IEntityAttribute {

        private T m_data;

        public EntityAttribute() { }

        public EntityAttribute( T data ) {
            m_data = data;
        }

        public T Value {
            get { return m_data; }
            set { m_data = value; }
        }
    }
}
