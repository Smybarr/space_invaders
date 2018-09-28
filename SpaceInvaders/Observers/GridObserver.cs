using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class GridObserver : ColObserver
    {
        public GridObserver()
        {

        }
        public override void Notify()
        {
            Debug.WriteLine("GridObserver: {0} {1}", this.pSubject.pObjA, this.pSubject.pObjB);

            // OK do some magic
            Grid pGrid = (Grid)this.pSubject.pObjA;

            WallCategory pWall = (WallCategory)this.pSubject.pObjB;
            if (pWall.GetWallType() == WallCategory.Type.Right)
            {
                pGrid.SetDelta(-2.0f);
            }
            else if (pWall.GetWallType() == WallCategory.Type.Left)
            {
                pGrid.SetDelta(2.0f);
            }
            else
            {
                Debug.Assert(false);
            }

        }
    }
}