using System;
using System.Diagnostics;


namespace SpaceInvaders
{

    public abstract class ColVisitor : PCSNode
    {

        //-----------------------------------------------------------------------------------------
        //all ColVisitor objects must implement;
        //-------------------------------------
        public abstract void Accept(ColVisitor other);

        //-----------------------------------------------------------------------------------------
        //visit null game object
        //-------------------------------------
        public virtual void VisitNullGameObject(NullGameObject n)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by NullGameObject not implemented");
            Debug.Assert(false);
        }

        //-----------------------------------------------------------------------------------------
        //visit alien tree
        //-------------------------------------
        //root, grid, column, crab/squid/octopus

        ////-------------------------------------
        //public virtual void VisitAlienRoot(AlienRoot a)
        //{
        //    // no differed to subcass
        //    Debug.WriteLine("Visit by AlienRoot not implemented");
        //    Debug.Assert(false);
        //}

        //-------------------------------------
        public virtual void VisitGrid(Grid a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Grid not implemented");
            Debug.Assert(false);
        }
        //-------------------------------------
        public virtual void VisitColumn(Column a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Column not implemented");
            Debug.Assert(false);
        }

        //-------------------------------------
        public virtual void VisitCrab(Crab a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Crab not implemented");
            Debug.Assert(false);
        }
        //-------------------------------------
        public virtual void VisitSquid(Squid a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Squid not implemented");
            Debug.Assert(false);
        }
        //-------------------------------------
        public virtual void VisitOctopus(Octopus a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Octopus not implemented");
            Debug.Assert(false);
        }
        public virtual void VisitExplodingAlien(ExplodingAlien a)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by ExplodingAlien not implemented");
            Debug.Assert(false);
        }
        //-----------------------------------------------------------------------------------------
        //visit alien ship
        //-------------------------------------
        //root, alienShip

        ////-------------------------------------
        //public virtual void VisitAlienShipRoot(AlienShipRoot a)
        //{
        //    // no differed to subcass
        //    Debug.WriteLine("Visit by AlienShipRoot not implemented");
        //    Debug.Assert(false);
        //}
        ////-------------------------------------
        //public virtual void VisitAlienShip(AlienShip a)
        //{
        //    // no differed to subcass
        //    Debug.WriteLine("Visit by AlienShip not implemented");
        //    Debug.Assert(false);
        //}


        //-----------------------------------------------------------------------------------------
        //visit bomb tree
        //-------------------------------------
        //root, bomb

        //-------------------------------------
        public virtual void VisitBombRoot(BombRoot b)
        {
            Debug.WriteLine("Visit by BombRoot not implemented");
            Debug.Assert(false);
        }
        //-------------------------------------
        public virtual void VisitBomb(Bomb b)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Bomb not implemented");
            Debug.Assert(false);
        }


        //-----------------------------------------------------------------------------------------
        //visit Shield tree
        //-------------------------------------
        //root, grid, column, brick

        //-------------------------------------
        public virtual void VisitShieldRoot(ShieldRoot s)
        {
            Debug.WriteLine("Visit by ShieldRoot not implemented");
            Debug.Assert(false);
        }

        //-------------------------------------
        public virtual void VisitShieldGrid(ShieldGrid s)
        {
            Debug.WriteLine("Visit by ShieldGrid not implemented");
            Debug.Assert(false);
        }

        //-------------------------------------
        public virtual void VisitShieldColumn(ShieldColumn s)
        {
            Debug.WriteLine("Visit by ShieldColumn not implemented");
            Debug.Assert(false);
        }

        //-------------------------------------
        public virtual void VisitShieldBrick(ShieldBrick s)
        {
            Debug.WriteLine("Visit by ShieldBrick not implemented");
            Debug.Assert(false);
        }



        //-----------------------------------------------------------------------------------------
        //visit missile tree
        //-------------------------------------
        //root, missile

        //-------------------------------------

        public virtual void VisitMissileRoot(MissileRoot m)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by MissileRoot not implemented");
            Debug.Assert(false);
        }
        //-------------------------------------
        public virtual void VisitMissile(Missile m)
        {
            // no differed to subcass
            Debug.WriteLine("Visit by Missile not implemented");
            Debug.Assert(false);
        }

        //-----------------------------------------------------------------------------------------
        //visit ship tree
        //-------------------------------------
        //root, ship

        //-------------------------------------
        public virtual void VisitShip(Ship s)
        {
            Debug.WriteLine("Visit by Ship not implemented");
            Debug.Assert(false);
        }
        public virtual void VisitShipRoot(ShipRoot s)
        {
            Debug.WriteLine("Visit by ShipRoot not implemented");
            Debug.Assert(false);
        }



        //-----------------------------------------------------------------------------------------
        //visit wall tree
        //-------------------------------------
        //root, wallRight, wallLeft, wallTop, wallBottom

        //-------------------------------------
        public virtual void VisitWallRoot(WallRoot w)
        {
            Debug.WriteLine("Visit by WallRoot not implemented");
            Debug.Assert(false);
        }
        public virtual void VisitWallRight(WallRight w)
        {
            Debug.WriteLine("Visit by WallRight not implemented");
            Debug.Assert(false);
        }
        public virtual void VisitWallLeft(WallLeft w)
        {
            Debug.WriteLine("Visit by WallLeft not implemented");
            Debug.Assert(false);
        }

        public virtual void VisitWallTop(WallTop w)
        {
            Debug.WriteLine("Visit by WallTop not implemented");
            Debug.Assert(false);
        }

        public virtual void VisitWallBottom(WallBottom w)
        {
            Debug.WriteLine("Visit by WallBottom not implemented");
            Debug.Assert(false);
        }








 
    }

}