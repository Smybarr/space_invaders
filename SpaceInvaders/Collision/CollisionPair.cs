using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ColPair : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */


        public enum Name
        {
            Alien_Missile,

            Alien_Wall,

            Missile_Wall,
            Misslie_Shield,

            Bomb_Wall,
            Bomb_Shield,
            Bomb_Ship,


            NullObject,
            Blank,

        }

        // Data: ---------------
        public ColPair.Name name;
        public int index;
        public GameObject treeA;
        public GameObject treeB;
        public ColSubject poSubject;

        //EDIT THE FOLLOWING METHODS---------------------
        public ColPair()
        : base()
        {
            this.treeA = null;
            this.treeB = null;
            this.name = ColPair.Name.Blank;
            this.index = 0;
            this.poSubject = new ColSubject();
        }

        ~ColPair()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~ColPair():{0}", this.GetHashCode());
            #endif

            this.treeA = null;
            this.treeB = null;
            this.name = ColPair.Name.Blank;
            this.index = 0;
            this.poSubject = null;
        }


        public void WashNodeData()
        {
            //wash name and data;
            this.treeA = null;
            this.treeB = null;
            this.name = ColPair.Name.Blank;
            this.index = 0;


            Debug.Assert(this.poSubject != null);

            // Need more dev on Subjects... current number should be 0
        }

        //SetData here;
        public void Set(ColPair.Name colpairName, int index, GameObject pTreeRootA, GameObject pTreeRootB)
        {
            Debug.Assert(pTreeRootA != null);
            Debug.Assert(pTreeRootB != null);

            this.treeA = pTreeRootA;
            this.treeB = pTreeRootB;
            this.name = colpairName;
            this.index = index;


            Debug.Assert(this.poSubject != null);

        }

        public void SetName(Name name)
        {
            this.name = name;
        }

        public ColPair.Name GetName()
        {
            return this.name;
        }

        public void Process()
        {
            Collide(this.treeA, this.treeB);
        }

        public static void Collide(GameObject pSafeTreeA, GameObject pSafeTreeB)
        {
            // A vs B
            GameObject pNodeA = pSafeTreeA;
            GameObject pNodeB = pSafeTreeB;

            //     Debug.WriteLine("\nColPair: start {0}, {1}", pNodeA.name, pNodeB.name);
            while (pNodeA != null)
            {
                // Restart compare
                pNodeB = pSafeTreeB;

                while (pNodeB != null)
                {
                    // who is being tested?
                    //Debug.WriteLine("ColPair: collide:  {0}, {1}", pNodeA.GetName(), pNodeB.GetName());

                    // Get rectangles
                    ColRect rectA = pNodeA.GetColObject().poColRect;
                    ColRect rectB = pNodeB.GetColObject().poColRect;

                    // test them
                    if (ColRect.Intersect(rectA, rectB))
                    {
                        // Boom - it works (Visitor in Action)
                        pNodeA.Accept(pNodeB);
                        break;
                    }

                    pNodeB = (GameObject)pNodeB.pSibling;
                }
                pNodeA = (GameObject)pNodeA.pSibling;
            }
        }

        public void Attach(ColObserver observer)
        {
            this.poSubject.Attach(observer);
        }

        public void NotifyListeners()
        {
            this.poSubject.Notify();
        }

        public void SetCollision(GameObject pObjA, GameObject pObjB)
        {
            Debug.Assert(pObjA != null);
            Debug.Assert(pObjB != null);

            // GameObject pAlien = AlienCategory.GetAlien(objA, objB);
            this.poSubject.pObjA = pObjA;
            this.poSubject.pObjB = pObjB;
        }



        public void DumpNodeData()
        {
            // we are using HASH code as its unique identifier 
            Debug.WriteLine("CollisionPairNode: {0}, hashcode: ({1})", this.name, this.GetHashCode());
            if (this.pMNext == null)
            {
                Debug.WriteLine("      next: null");
            }
            else
            {
                ColPair pTmp = (ColPair) this.pMNext;
                Debug.WriteLine("      next: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            if (this.pMPrev == null)
            {
                Debug.WriteLine("      prev: null");
            }
            else
            {
                ColPair pTmp = (ColPair) this.pMPrev;
                Debug.WriteLine("      prev: {0}, hashcode: ({1})", pTmp.name, pTmp.GetHashCode());
            }

            // Print Unique Node Data:         
            Debug.WriteLine("   CollisionPairNode Name: {0}", this.name);
            Debug.WriteLine("TODO - Enter Print For ColPair Node Data\n");
        }

    }



    public class ColPairManager : Manager
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

        //----------------------------------------------------------------------
        // Data: unique data for this manager here
        //----------------------------------------------------------------------

        
        private ColPair pRefNode;
        private ColPair pActiveColPair;
        //singleton reference ensures only one manager is created;
        private static ColPairManager pInstance = null;

        //----------------------------------------------------------------------
        // Constructor - Singleton Instantiation
        //----------------------------------------------------------------------
        private ColPairManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            /*delegate to parent manager*/
            // do nothing
            this.pRefNode = (ColPair)this.derivedCreateNode();
            this.pActiveColPair = null;
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
                pInstance = new ColPairManager(startReserveSize, refillSize);
            }

        }
        private static ColPairManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~ColPairManager()
        {
            #if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~ColPairManager():{0}", this.GetHashCode());
            #endif
            ColPairManager.pInstance = null;
            this.pActiveColPair = null;
        }
        public static void Destroy()
        {
            // Get the instance
            ColPairManager pMan = ColPairManager.privGetInstance();
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("--->ColPairManager.Destroy()");
            #endif
            pMan.baseDestroy();
        }


        static public ColPair GetActiveColPair()
        {
            // get the singleton
            ColPairManager pMan = ColPairManager.privGetInstance();

            return pMan.pActiveColPair;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        public static ColPair Add(ColPair.Name colpairName, GameObject treeRootA, GameObject treeRootB)
        {
            return ColPairManager.Add(colpairName, 0, treeRootA, treeRootB);
        }
        public static ColPair Add(ColPair.Name colpairName, int index, GameObject treeRootA, GameObject treeRootB)
        {
            // Get the instance
            ColPairManager pMan = ColPairManager.privGetInstance();

            // Go to Man, get a node from reserve, add to active, return it
            ColPair pColPair = (ColPair)pMan.baseAddToFront();
            Debug.Assert(pColPair != null);

            // Initialize Image
            pColPair.Set(colpairName, index, treeRootA, treeRootB);

            return pColPair;
        }

        public static ColPair Find(ColPair.Name name, int index = 0)
        {
            //get the singleton
            ColPairManager pMan = ColPairManager.privGetInstance();

            // Compare functions only compares two Nodes
            pMan.pRefNode.SetName(name);

            ColPair pData = (ColPair)pMan.baseFindNode(pMan.pRefNode);
            return pData;
        }

        public static void Remove(ColPair pNode)
        {
            //get the singleton
            ColPairManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }


        public static void Process()
        {
            // get the singleton
            ColPairManager pMan = ColPairManager.privGetInstance();

            ColPair pColPair = (ColPair)pMan.baseGetActive();

            while (pColPair != null)
            {
                pMan.pActiveColPair = pColPair;

                // do the check for a single pair
                pColPair.Process();

                // advance to next
                pColPair = (ColPair)pColPair.pMNext;
            }

        }



        public static void DumpAll()
        {
            ColPairManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ CollisionPair Manager Dump All------");
            pMan.baseDumpAll();
        }

        public static void DumpStats()
        {
            ColPairManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ CollisionPair Manager Dump Stats------");
            pMan.baseDumpStats();
        }

        public static void DumpLists()
        {
            ColPairManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ CollisionPair Manager Dump Lists------");
            pMan.baseDumpLists();
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            ColPair pDataA = (ColPair) pLinkA;
            ColPair pDataB = (ColPair) pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }

        protected override MLink derivedCreateNode()
        {
            MLink pNode = new ColPair();
            Debug.Assert(pNode != null);

            return pNode;
        }
        //---------------------



        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            ColPair pNode = (ColPair) pLink;
            pNode.DumpNodeData();
        }

        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            ColPair pNode = (ColPair) pLink;
            pNode.WashNodeData();
        }


    }
}