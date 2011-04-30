using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XEngine {
    class BaseAttribute<T> : IEntityAttribute {

        private T m_data;

        public T Value {
            get { return m_data; }
            set { m_data = value; }
        }

        public void LoadData( object data ) {

        }
    }
}
