using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {
    class ScenegraphNode {

        public ScenegraphNode Parent;

        public List<ScenegraphNode> Children;

        private Transform m_transform;

        public ScenegraphNode(Transform transform) {
            m_transform = transform;
            Children = new List<ScenegraphNode>();
        }

        public void AddChild( ScenegraphNode child ) {
            Children.Add( child );
            child.Parent = this;
        }

        public void Update() {
            if ( Parent != null ) {
                //m_transform.UpdateWorld( Parent.m_transform );
            }
            foreach ( ScenegraphNode childNode in Children ) {
                childNode.Update();
            }
        }
    }
}
