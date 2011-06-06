using System;

namespace XEngine {
#if WINDOWS || XBOX
    static class Program {

        static void Main(string[] args) {

            //XEngineGame.RunGame();
            //Camera.ComponentTest();
            //DebugHUD.ComponentTest();

            //InputManager.ComponentTest();
            //CameraController.ComponentTest();
            //FirstPersonCameraController.ComponentTest();
            //TopDownCameraController.ComponentTest();

            //Terrain.TestTerrain();

            //ModelRenderComponent.ComponentTest();
            //MoveComponent.ComponentTest();
            //OrbitComponent.ComponentTest();
            //PrimitiveRenderComponent.ComponentTest();
            //SelectionCubeComponent.ComponentTest();

            //EntityFactory.LoadEntityListTest();
            //EntityFactory.EntityTest( "TestModel" );
            //EntityFactory.EntityTest( "Ship" );
            EntityFactory.EntityTest( "Tank" );
            //EntityFactory.EntityTest( "Sphere" );

            //LevelManager.LoadLevelData();
            //ScenegraphManager.ComponentTest();

            //Origin.ComponentTest();
            
        }
    }
#endif
}

