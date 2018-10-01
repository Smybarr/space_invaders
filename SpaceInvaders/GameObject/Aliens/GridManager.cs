using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class GridManager
    {
        // Data: ----------------------------------------------
        private static GridManager pInstance = null;

        //grid pointers
        private Grid pCurrentGrid;
        private Grid playerOneGrid;
        private Grid playerTwoGrid;

        //private Bomb pBomb;

        //Reference Objects (State Objects)

        //Other Data
        private int randomInt = 9;
        private Random r = new Random();


        private GridManager()
        {
            this.pCurrentGrid = null;
            this.playerOneGrid = null;
            this.playerTwoGrid = null;
        }


        public static void CreateGridManager(Grid pGrid)
        {
            // make sure its the first time
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new GridManager();

                Debug.Assert(pInstance != null);

                // Stuff to initialize after the instance was created

                //Create the Ship Tree
                //pInstance.pCurrentGrid = CreateGrid();
                //pInstance.pCurrentGrid.SetState(GridManager.State.Ready);

                //Grid pGrid = (Grid)GameObjectManager.Find(GameObject.Name.Grid);
                //Debug.Assert(pGrid != null);
                
                //set the grid manager's grid to existing grid;
                pInstance.pCurrentGrid = pGrid;
            }
        }

        private static GridManager privInstance()
        {
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static Grid GetCurrentGrid()
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            return pGridMan.pCurrentGrid;
        }

        public static void SetCurrentGrid(Grid pGrid)
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            pGridMan.pCurrentGrid = pGrid;
        }


        public static Grid GetPlayerOneGrid()
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            return pGridMan.playerOneGrid;
        }

        public static Grid GetPlayerTwoGrid()
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            return pGridMan.playerTwoGrid;
        }

        //call during the update loop
        //todo - drop the bomb depending on the number of steps taken OR number of aliens left! 
        public static void UpdateBombDrop()
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            //get the total time
            float time = Simulation.GetTotalTime();
            if (time > 5.0f)
            {

                if (pGridMan.randomInt / 2 == 0)
                {
                    pGridMan.pCurrentGrid.DropBomb();
                }
                //if the time is even, generate a new random number
                if ((int)time % 2 == 0)
                {
                    pGridMan.privGenerateNewRandomNumber();
                }
            }



        }

        private void privGenerateNewRandomNumber()
        {
            GridManager pGridMan = GridManager.privInstance();

            Debug.Assert(pGridMan != null);
            Debug.Assert(pGridMan.pCurrentGrid != null);

            pGridMan.randomInt = r.Next(0, 500);
        }


        //private static Grid CreateGrid()
        //{

        //}


    }
}
