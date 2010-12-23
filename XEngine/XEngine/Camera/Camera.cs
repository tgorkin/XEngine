using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    public class Camera : GameComponent, ICamera {

        private Matrix m_view;

        private Matrix m_projection;

        private Vector3 m_cameraPosition;

        private Vector3 m_lookAt;

        private Vector3 m_up;

        public Camera(XEngineGame game) : base(game) {
            
        }

        public override void Initialize () {
            this.Position = CameraConstants.DEFAULT_POSITION;
            this.LookAt = CameraConstants.DEFAULT_LOOK_AT;
            this.Up = CameraConstants.DEFAULT_UP;
            UpdateView();
            UpdateProj();
        }

        public override void Update(GameTime gameTime) {
            UpdateView();
        }

        virtual public void UpdateView() {
            m_view = Matrix.CreateLookAt(m_cameraPosition, m_lookAt, m_up);
        }

        public void UpdateProj() {
            m_projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(CameraConstants.FIELD_OF_VIEW_DEGREES),  // 45 degree angle
                Game.GraphicsDevice.Viewport.AspectRatio,
                CameraConstants.NEAR_PLANE, // near plane
                CameraConstants.FAR_PLANE); // far plane
        }

        public Matrix View {
            get { return m_view; }
        }

        public Matrix Projection {
            get { return m_projection; }
        }

        public Vector3 Position {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; }
        }

        public Vector3 LookAt {
            get { return m_lookAt; }
            set { m_lookAt = value; }
        }

        public Vector3 LookDirection {
            get { return m_lookAt - m_cameraPosition; }
        }

        public Vector3 Up {
            get { return m_up; }
            set { m_up = value; }
        }

        public Matrix InverseView {
            // TODO: optimize this to not use Matrix.Invert, use rotation matrix transpose method instead
            get { return Matrix.Invert(this.View); }
        }

        static public void TestCamera() {
            XEngineComponentTest testGame = new XEngineComponentTest();
            testGame.BindGameComponent( new Camera( testGame ), typeof( ICamera ) );
            testGame.Run();
        }
    }
}
