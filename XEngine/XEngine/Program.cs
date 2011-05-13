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

            //Terrain.TestTerrain();

            //ModelRenderComponent.ComponentTest();
            //MoveComponent.ComponentTest();
            //OrbitComponent.ComponentTest();
            //PrimitiveRenderComponent.ComponentTest();

            //EntityFactory.LoadEntityDataTest();
            //EntityFactory.LoadEntityListTest();
            //EntityFactory.CreateEntityTest();

            //ScenegraphManager.ComponentTest();
            ComponentTests.TankTurretTest();
        }
    }
#endif
}

