using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XEngineTypes;

namespace XEngine {

    class LevelManager {

        private EntityFactory m_entityFactory;

        private LevelData m_levelData;

        public LevelManager() {
            m_entityFactory = new EntityFactory();
        }

        public void LoadEntityTemplates( string entityTemplates ) {
            m_entityFactory.LoadEntityTemplates( entityTemplates );
        }

        public void LoadLevel(string levelFile) {
            EntityManager entityManager = ServiceLocator.EntityManager;
            ScenegraphManager scenegraph = ServiceLocator.ScenegraphManager;

            // clear any existing entities
            entityManager.ClearEntities();
            // load level data
            m_levelData = ServiceLocator.Content.Load<LevelData>( levelFile );
            // create new entities
            foreach ( EntityInstance entityData in m_levelData.Entities ) {
                Entity newEntity = m_entityFactory.CreateEntityWithData( entityData.Template, entityData );
                // add new entities to the entity manager and scenegraph
                entityManager.AddEntity( entityData.Name, newEntity );
                scenegraph.AddEntity( newEntity, entityManager.FindEntity( entityData.Parent ) );
            }
            // initialize all new entities
            entityManager.Initialize();
        }

        static public void LoadLevelData() {
            XEngineComponentTest testGame = new XEngineComponentTest();

            LevelManager levelManager;
            testGame.InitDelegate = delegate {
                levelManager = new LevelManager();
                levelManager.LoadEntityTemplates( "Data/EntityTemplates" );
                levelManager.LoadLevel( "Data/Level_Test" );
            };
            testGame.Run();
        }
    }
}
