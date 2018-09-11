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
        private SBNodeManager pSBNodeManRef;

        public SpriteBatch()
            : base()
        {
            //set the default blank name and create a new SBNodeManager
            this.name = Name.Blank;
            this.pSBNodeManRef = new SBNodeManager();
            Debug.Assert(this.pSBNodeManRef != null);
        }

        public void Set(Name spriteBatchName, int startReserveSize = 3, int refillSize = 1)
        {
            this.name = spriteBatchName;

            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            this.pSBNodeManRef.Set(spriteBatchName, startReserveSize, refillSize);
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
            return this.pSBNodeManRef;
        }

        public SBNode Attach(GameSprite.Name name)
        {
            SBNode pSBNode = this.pSBNodeManRef.Attach(name);

            return pSBNode;
        }

        public SBNode Attach(BoxSprite.Name name)
        {
            SBNode pSBNode = this.pSBNodeManRef.Attach(name);

            return pSBNode;
        }

        public void WashSpriteBatchData()
        {
            //wash name and data;
            this.name = Name.Blank;
        }

        public void DumpSpriteBatchData()
        {
            Debug.WriteLine("------ SpriteBatch ------");

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
        private static SpriteBatch pBatchRef = new SpriteBatch();
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
        }
        private static SpriteBatchManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //Draw function draws all sprite batches and is called in Game.Draw();
        public static void Draw()
        {
            // FIX: this screams - iterators...

            // get the singleton
            SpriteBatchManager pSpriteBatchMan = SpriteBatchManager.privGetInstance();
            SpriteBatch pSpriteBatch = pSpriteBatchMan.GetActive();
            SBNodeManager pSBNodeManager = null;          
            SBNode pSBNode = null;

            while (pSpriteBatch != null)
            {
                //get the sprite batch node manager reference attached to this sprite batch link
                pSBNodeManager = pSpriteBatch.GetSBNodeManager();
                Debug.Assert(pSBNodeManager != null);

                //get the first active SBNode link on that manager;
                pSBNode = (SBNode)pSBNodeManager.GetActive();

                while (pSBNode != null)
                {
                    // Assumes someone before here called update() on each sprite
                    // Draw me.
                    pSBNode.pSpriteBase.Draw();

                    //get the next sprite batch node on this manager
                    pSBNode = (SBNode)pSBNode.pMNext;
                }

                //get the next sprite batch
                pSpriteBatch = (SpriteBatch)pSpriteBatch.pMNext;
            }
        }


        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------


        public SpriteBatch GetActive()
        {
            return (SpriteBatch)this.baseGetActive();
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
            Debug.Assert(pBatchRef != null);
            pBatchRef.WashSpriteBatchData();

            //find the node by name
            pBatchRef.SetName(name);

            SpriteBatch pData = (SpriteBatch)pMan.baseFindNode(pBatchRef);

            return pData;
        }
        public static void Remove(SpriteBatch pNode)
        {
            //get the singleton
            SpriteBatchManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void Dump()
        {
            SpriteBatchManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ SpriteBatchNode Manager ------");
            pMan.baseDumpAll();
        }

        //----------------------------------------------------------------------
        // 4 Override Abstract Methods (From Base Manager)
        //----------------------------------------------------------------------
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
}
