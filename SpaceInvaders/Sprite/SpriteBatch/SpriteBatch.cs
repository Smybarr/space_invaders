using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class SpriteBatchNode : CLink
    {
        // Data: ----------------------------------------------
        public SpriteBase pSpriteBase;

        public SpriteBatchNode()
            : base()
        {
            this.pSpriteBase = null;
        }

        public void Set(GameSprite.Name targetSpriteName, Boolean render = true)
        {
            // Go find it
            this.pSpriteBase = GameSpriteManager.Find(targetSpriteName);
            Debug.Assert(this.pSpriteBase != null);
            this.pSpriteBase.render = render;
        }
        public void Set(BoxSprite.Name targetBoxName)
        {
            // Go find it
            this.pSpriteBase = BoxSpriteManager.Find(targetBoxName);
            Debug.Assert(this.pSpriteBase != null);
        }
        public void WashNodeData()
        {
            this.pSpriteBase = null;
        }
    }


    public class SpriteBatch : Container
    {
        public enum Name
        {
            GameSprites,
            Shields,

            SpriteBoxes,

            TextLetters,

            Blank,
        }

        private static SpriteBatchNode pSpriteBatchNodeRef = new SpriteBatchNode();
        public Name name;


        public SpriteBatch(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            this.name = Name.Blank;
        }
        public void Set(Name spriteBatchName, int startReserveSize, int refillSize)
        {
            this.name = spriteBatchName;

            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            this.baseSetReserve(startReserveSize, refillSize);
        }
        public void WashSpriteBatchData()
        {
            //wash name and data;
            this.name = Name.Blank;
        }

        public SpriteBatchNode Attach(GameSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SpriteBatchNode pSpriteBatchNode = (SpriteBatchNode)this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name);

            return pSpriteBatchNode;
        }
        public SpriteBatchNode Attach(BoxSprite.Name name)
        {
            // Go to Man, get a node from reserve, add to active, return it
            SpriteBatchNode pSpriteBatchNode = (SpriteBatchNode) this.baseAddToFront();
            Debug.Assert(pSpriteBatchNode != null);

            // Initialize SpriteBatchNode
            pSpriteBatchNode.Set(name);

            return pSpriteBatchNode;
        }
        public void Remove(SpriteBatchNode pNode)
        {
            Debug.Assert(pNode != null);
            this.baseRemoveNode(pNode);
        }

        public void DumpSpriteBatchData()
        {
            Debug.WriteLine("------ SpriteBatch ------");
            this.baseDumpAll();
            //---------------------
        }


        //----------------------------------------------------------------------
        // 4 Override Abstract Methods (From Base Manager)
        //----------------------------------------------------------------------
        protected override Boolean derivedCompareNodes(CLink pLinkA, CLink pLinkB)
        {
            //this function is stubbed out....

            Boolean status = false;

            //// This is used in baseFindNode() 
            //Debug.Assert(pLinkA != null);
            //Debug.Assert(pLinkB != null);
            //
            //SpriteBatch pDataA = (SpriteBatch)pLinkA;
            //SpriteBatch pDataB = (SpriteBatch)pLinkB;
            //
            //Boolean status = false;
            //
            //if (pDataA.name == pDataB.name)
            //{
            //    status = true;
            //}

            return status;
        }
        protected override CLink derivedCreateNode()
        {
            CLink pNode = new SpriteBatchNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(CLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatchNode pNode = (SpriteBatchNode)pLink;
            //pNode.DumpNodeData();
        }
        protected override void derivedWashNode(CLink pLink)
        {
            Debug.Assert(pLink != null);
            SpriteBatchNode pNode = (SpriteBatchNode)pLink;
            pNode.WashNodeData();
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

        public static Boolean renderBoxes = false;
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
            SpriteBatch pSpriteBatch = (SpriteBatch)pSpriteBatchMan.pActive;
            SpriteBatchNode pSpriteBatchNode = null;

            while (pSpriteBatch != null)
            {
                pSpriteBatchNode = (SpriteBatchNode)pSpriteBatch.pActive;

                while (pSpriteBatchNode != null)
                {
                    // Assumes someone before here called update() on each sprite
                    // Draw me.
                    pSpriteBatchNode.pSpriteBase.Draw();

                    pSpriteBatchNode = (SpriteBatchNode)pSpriteBatchNode.pCNext;
                }

                pSpriteBatch = (SpriteBatch)pSpriteBatch.pMNext;
            }
        }


        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

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
            pBatchRef.name = name;

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

            if (pDataA.name == pDataB.name)
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
