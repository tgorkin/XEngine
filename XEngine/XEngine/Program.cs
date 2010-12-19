using System;

namespace XEngine {
#if WINDOWS || XBOX
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args) {

            //XEngineGame.RunGame();
            //Camera.TestCamera();
            //Origin.TestOrigin();
            //DebugHUD.TestDebugHUD();

            //InputManager.TestInputManager();
            //CameraController.TestCameraController();

            //GeometricPrimitive.TestGeometricPrimitives();
            Terrain.TestTerrain();
            //XEngineTest.StartTest();
        }
    }
#endif
}

