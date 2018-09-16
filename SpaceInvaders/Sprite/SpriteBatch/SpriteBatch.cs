using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    public class SpriteBatch : MLink
    {
        public enum Name
        {
            GameSprites,
            Shields,

            SpriteBoxes,

            TextLetters,

            Blank,
        }


        private Name name;
        private SBNodeManager pSBNodeMan;

        public SpriteBatch()
            : base()
        {
            //set the default blank name and create a new SBNodeManager
            this.name = Name.Blank;
            this.pSBNodeMan = new SBNodeManager();
            Debug.Assert(this.pSBNodeMan != null);
        }
        ~SpriteBatch()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~SpriteBatch():{0} ", this.GetHashCode());
#endif
            this.name = SpriteBatch.Name.Blank;
            this.pSBNodeMan = null;
        }

        public void Destroy()
        {
            Debug.Assert(this.pSBNodeMan != null);
            this.pSBNodeMan.Destroy();
        }

        public void Set(Name spriteBatchName, int startReserveSize = 3, int refillSize = 1)
        {
            this.name = spriteBatchName;

            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            //attach both the name of the sprite batch and a pointer to batch itself;
            this.pSBNodeMan.Set(spriteBatchName, startReserveSize, refillSize);
            this.pSBNodeMan.SetSpriteBatch(this);
        }
        public void SetName(Name spriteBatchName)
        {
            this.name = spriteBatchName;
        }
        public SpriteBatch.Name GetName()
        {
            return this.name;
        }

        public SBNodeManager GetSBNodeManager()
        {
            return this.pSBNodeMan;
        }

        public void Attach(SpriteBase pNode)
        {
            Debug.Assert(pNode != null);

            // Go to Man, get a node from reserve, add to active, return it
            SBNode pSBNode = (SBNode)this.pSBNodeMan.Add(pNode);
            Debug.Assert(pSBNode != null);

            // Initialize SpriteBatchNode
            pSBNode.Set(pNode, this.pSBNodeMan);
        }

        //public SBNode Attach(GameSprite.Name name)
        //{
        //    SBNode pSBNode = this.pSBNodeMan.Attach(name);

        //    return pSBNode;
        //}
        //public SBNode Attach(BoxSprite.Name name)
        //{
        //    SBNode pSBNode = this.pSBNodeMan.Attach(name);

        //    return pSBNode;
        //}
        //public SBNode Attach(ProxySprite pNode)
        //{
        //    // Go to Man, get a node from reserve, add to active, return it
        //    Debug.Assert(this.pSBNodeMan != null);
        //    SBNode pSBNode = this.pSBNodeMan.Add(pNode);
        //    return pSBNode;
        //}

        public void Draw()
        {
            SBNode pSpriteBatchNode = (SBNode)this.pSBNodeMan.GetActive();
            //int debugPrintCount = 0;
            while (pSpriteBatchNode != null)
            {
                //debugPrintCount++;
                // Assumes someone before here called update() on each sprite
                // OK... data is right so --> Draw me.
                pSpriteBatchNode.GetSpriteBase().Draw();

                pSpriteBatchNode = (SBNode)pSpriteBatchNode.pMNext;
            }
            //Debug.WriteLine("{0} sprites from SpriteBatch({1}) drawn", debugPrintCount, this.name);
        }
        public void Remove(SBNode pSpriteBatchNode)
        {
            Debug.Assert(pSpriteBatchNode != null);
            this.pSBNodeMan.Remove(pSpriteBatchNode);
        }

        public void WashSpriteBatchData()
        {
            //wash name;
            this.name = Name.Blank;
            // make sure it still exists
            Debug.Assert(this.pSBNodeMan != null);
        }

        public void DumpSpriteBatchData()
        {
            Debug.WriteLine("------ SpriteBatch: {0} ------", this.name);
            this.pSBNodeMan.DumpSBNodeData();
            //---------------------
        }

    }


    public class SpriteBatchManager : Manager
    {
        /* All NodeManagers inheriting from Manager should contain AT
         * LEAST the following components:
         *      
         *      - Constructor
         *          !!! IF SINGLETON:
         *               - private constructor
         *               - public static void Create(int resNum, int resGrow)
         *               - private getInstance()
         *               - private static NodeManager pInstance = null; (Field)
         *          ------------------------------
         *      - 4 Base Manager Function Wrappers
         *          - Add (wrap baseAdd)
         *          - Find (wrap baseFind)
         *          - Remove (wrap baseRemove)
         *          - Dump (wrap baseDump)
         *          ------------------------------
         *      - Override of Manager Abstract Methods
         *          - override protected DLink derivedCreateNewNode()
         *          - override protected Boolean derivedCompareNodes(DLink a, DLink b)
         *          - override protected void derivedWashNode(DLink pLink)
         *          - override protected void derivedDumpNode(DLink pLink)
         *         ------------------------------
         *
         * Node Manager can also have its own unique data and functions if necessary
         *
         */

        public static Boolean renderBoxes = true;
        private static SpriteBatch pSpriteBatchRef = new SpriteBatch();
        private static SpriteBatchManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private SpriteBatchManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            /*delegate to parent manager*/
        }
        //public facing constructor for instantiation of the singleton instance
        public static void Create(int startReserveSize = 3, int refillSize = 1)
        {
            // make sure values are reasonable 
            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            // initialize the singleton here:
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                //constructor can only be called here since private access
                pInstance = new SpriteBatchManager(startReserveSize, refillSize);
            }

            //add a default node?

            Debug.WriteLine("------SpriteBatch Manager Initialized-------");
        }
        private static SpriteBatchManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        ~SpriteBatchManager()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~SpriteBatchMan():{0} ", this.GetHashCode());
#endif
            SpriteBatchManager.pSpriteBatchRef = null;
            SpriteBatchManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            SpriteBatchManager pMan = SpriteBatchManager.privGetInstance();
            Debug.WriteLine("--->SpriteBatch.Destroy()");
            pMan.baseDestroy();

#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", SpriteBatchMan.pSpriteBatchRef, SpriteBatchMan.pSpriteBatchRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", SpriteBatchMan.pInstance, SpriteBatchMan.pInstance.GetHashCode());
#endif
            SpriteBatchManager.pSpriteBatchRef = null;
            SpriteBatchManager.pInstance = null;
        }

        public SpriteBatch GetActive()
        {
            return (SpriteBatch)this.baseGetActive();
        }

        //most of Draw is now handled by each individual sprite batch
        public static void Draw()
        {
            // get the singleton
            SpriteBatchManager pSpriteBatchMan = SpriteBatchManager.privGetInstance();
            SpriteBatch pSpriteBatch = (SpriteBatch)pSpriteBatchMan.GetActive();

            //draw all the sprite batches
            while (pSpriteBatch != null)
            {
                pSpriteBatch.Draw();
                pSpriteBatch = (SpriteBatch)pSpriteBatch.pMNext;
            }
        }

        public static SpriteBatch Add(SpriteBatch.Name name, int startReserveSize = 3, int refillSize = 1)
        {
            SpriteBatchManager pMan = SpriteBatchManager.privGetInstance();
            Debug.Assert(pMan != null);

            SpriteBatch pNode = (SpriteBatch)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            // set the data
            pNode.Set(name, startReserveSize, refillSize);

            return pNode;
        }
        public static SpriteBatch Find(SpriteBatch.Name name)
        {
            //get the singleton
            SpriteBatchManager pMan = privGetInstance();

            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pSpriteBatchRef != null);
            pSpriteBatchRef.WashSpriteBatchData();

            //find the node by name
            pSpriteBatchRef.SetName(name);

            SpriteBatch pData = (SpriteBatch)pMan.baseFindNode(pSpriteBatchRef);

            return pData;
        }

        public static void Remove(SBNode pSBNode)
        {
            Debug.Assert(pSBNode != null);
            SBNodeManager pSBNodeMan = pSBNode.GetSBNodeMan();

            Debug.Assert(pSBNodeMan != null);
            pSBNodeMan.Remove(pSBNode);
        }
        public static void Remove(SpriteBatch pNode)
        {
            //get the singleton
            SpriteBatchManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }

        public static void DumpAll()
        {
            SpriteBatchManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ SpriteBatch Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            SpriteBatchManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ SpriteBatch Manager Stats------");
            pMan.baseDumpStats();
        }
        //----------------------------------------------------------------------
        // 4 Override Abstract Methods (From Base Manager)
        //----------------------------------------------------------------------
        protected override void derivedDestroyNode(MLink pLink)
        {
            SpriteBatch pNode = (SpriteBatch)pLink;
            Debug.Assert(pNode != null);
            pNode.Destroy();
        }
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            SpriteBatch pDataA = (SpriteBatch)pLinkA;
            SpriteBatch pDataB = (SpriteBatch)pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new SpriteBatch();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatch pNode = (SpriteBatch)pLink;
            pNode.DumpSpriteBatchData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatch pNode = (SpriteBatch)pLink;
            pNode.WashSpriteBatchData();
        }
    }




/*SpriteBatchManager.Draw() - OLD

    ////Draw is now handled by each individual sprite batch
    //public static void Draw()
    //{
    //    // FIX: this screams - iterators...

    //    // get the singleton
    //    SpriteBatchManager pSpriteBatchMan = SpriteBatchManager.privGetInstance();
    //    SpriteBatch pSpriteBatch = pSpriteBatchMan.GetActive();
    //    SBNodeManager pSBNodeManager = null;          
    //    SBNode pSBNode = null;

    //    while (pSpriteBatch != null)
    //    {
    //        //get the sprite batch node manager reference attached to this sprite batch link
    //        pSBNodeManager = pSpriteBatch.GetSBNodeManager();
    //        Debug.Assert(pSBNodeManager != null);

    //        //get the first active SBNode link on that manager;
    //        pSBNode = (SBNode)pSBNodeManager.GetActive();

    //        while (pSBNode != null)
    //        {
    //            // Assumes someone before here called update() on each sprite
    //            // Draw me.
    //            pSBNode.pSpriteBase.Draw();

    //            //get the next sprite batch node on this manager
    //            pSBNode = (SBNode)pSBNode.pMNext;
    //        }

    //        //get the next sprite batch
    //        pSpriteBatch = (SpriteBatch)pSpriteBatch.pMNext;
    //    }
    //}

 */
}






