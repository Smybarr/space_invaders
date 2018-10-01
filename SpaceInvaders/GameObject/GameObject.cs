using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    public abstract class GameObject : ColVisitor
    {
        public enum Name
        {
            Root,

            Grid,
            Column,

            Squid,
            Crab,
            Octopus,
            ExplodingAlien,

            AlienUFO,


            ShipRoot,
            Ship,

            MissileRoot,
            Missile,

            BombRoot,
            Bomb,

            ShieldRoot,
            ShieldGrid,
            ShieldColumn,
            ShieldBrick,

            WallRoot,

            WallTop,
            WallRight,
            WallLeft,
            WallBottom,


            NullObject,
            Blank,

        }

        // Data: ---------------
        private Name name;
        public ProxySprite pProxySprite;
        protected ColObject poColObj;
        public int index;

        public float x;
        public float y;

        public bool markForDeath;




        protected GameObject(GameObject.Name objName, GameSprite.Name spriteName, int _index)
        {
            this.name = objName;
            this.index = _index;
            this.x = 0.0f;
            this.y = 0.0f;

            this.markForDeath = false;

            //this.pProxySprite = new ProxySprite(spriteName);
            this.pProxySprite = ProxySpriteManager.Add(spriteName);
            Debug.Assert(this.pProxySprite != null);

            this.poColObj = new ColObject(this.pProxySprite);
            Debug.Assert(this.poColObj != null);
        }
        ~GameObject()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~GameObject():{0}", this.GetHashCode());
            #endif
            this.name = GameObject.Name.Blank;
            this.pProxySprite = null;
            this.poColObj = null;
        }

        override public Enum GetPCSName()
        {
            return this.name;
        }

        public Name GetName()
        {
            return this.name;
        }

        public void SetCollisionColor(float red, float green, float blue)
        {
            Debug.Assert(this.poColObj != null);
            Debug.Assert(this.poColObj.pColSprite != null);

            this.poColObj.pColSprite.SetLineColor(red, green, blue);
        }

        public void ChangeImage(Image.Name imageName)
        {
            this.pProxySprite.ChangeImage(imageName);
        }


        public override int GetIndex()
        {
            return this.index;
        }
        public void SetName(Name name)
        {
            this.name = name;
        }

        public ColObject GetColObject()
        {
            Debug.Assert(this.poColObj != null);
            return this.poColObj;
        }


        public virtual void Remove()
        {
            Debug.WriteLine("REMOVE GAME OBJECT: {0}", this);

            // Remove proxy sprite from SpriteBatch
            Debug.Assert(this.pProxySprite != null);
            SBNode pSBNode = this.pProxySprite.GetSBNode();

            Debug.Assert(pSBNode != null);
            SpriteBatchManager.Remove(pSBNode);

            // Remove collision sprite box from spriteBatch
            Debug.Assert(this.poColObj != null);
            Debug.Assert(this.poColObj.pColSprite != null);
            pSBNode = this.poColObj.pColSprite.GetSBNode();

            Debug.Assert(pSBNode != null);
            SpriteBatchManager.Remove(pSBNode);

            // Remove from GameObjectMan
            GameObjectManager.Remove(this);

            //add to the ghost manager since it's a dead object
            GhostManager.Add(this);

        }



        public void ActivateGameSprite(SpriteBatch pSpriteBatch)
        {
            Debug.Assert(pSpriteBatch != null);
            pSpriteBatch.Attach(this.pProxySprite);
        }
        public void ActivateCollisionSprite(SpriteBatch pSpriteBatch)
        {
            Debug.Assert(pSpriteBatch != null);
            Debug.Assert(this.poColObj != null);
            pSpriteBatch.Attach(this.poColObj.pColSprite);
        }

        protected void baseUpdateBoundingBox()
        {
            //go to the first child in the PCS tree;
            PCSNode pcsNode = (PCSNode)this;
            pcsNode = pcsNode.pChild;

            //cast the pcsNode as a GameObject
            GameObject gameObject = (GameObject)pcsNode;

            //check to see if pcsNode is null (has it been removed?)
            if (pcsNode != null)
            {
                Debug.Assert(this.poColObj != null);
                Debug.Assert(this.poColObj.poColRect != null);
                ColRect collisionTotal = this.poColObj.poColRect;

                Debug.Assert(this.poColObj != null);
                Debug.Assert(this.poColObj.poColRect != null);
                collisionTotal.Set(gameObject.poColObj.poColRect);

                //loop through the siblings to get the total collisionRectSize;
                while (pcsNode != null)
                {
                    //cast each sibling pcsNode as a GameObject for consistency;
                    gameObject = (GameObject)pcsNode;
                    //total will be the size of the Union / combination of all the collisionRect sizes of the siblings;
                    collisionTotal.Union(gameObject.poColObj.poColRect);

                    //point to the next sibling;
                    //if the next sibling is null, then the loop will break out automatically;
                    pcsNode = pcsNode.pSibling;
                }

                this.x = this.poColObj.poColRect.x;
                this.y = this.poColObj.poColRect.y;
            }

        }


        public virtual void Update()
        {
            //update the proxy sprite coordinates
            Debug.Assert(this.pProxySprite != null);
            this.pProxySprite.x = this.x;
            this.pProxySprite.y = this.y;

            //update the collision box coordinates
            Debug.Assert(this.poColObj != null);
            //UpdatePos will push new x/y vals and call update
            this.poColObj.UpdatePos(this.x, this.y);
        }

        public void Dump()
        {
            // Data:
            Debug.WriteLine("\t\t\t       name: {0} ({1})", this.name, this.GetHashCode());

            if (this.pProxySprite != null)
            {
                Debug.WriteLine("\t\t   pProxySprite: {0}", this.pProxySprite.name);
                Debug.WriteLine("\t\t    pRealSprite: {0}", this.pProxySprite.pSprite.GetSpriteName());
            }
            else
            {
                Debug.WriteLine("\t\t   pProxySprite: null");
                Debug.WriteLine("\t\t    pRealSprite: null");
            }
            Debug.WriteLine("\t\t\t      (x,y): {0}, {1}", this.x, this.y);

        }


        //todo streamline this GameObject/PCSNode GetName function
        //public GameObject.Name GetName()
        //{
        //    return this.name;
        //}

    }




    public class GameObjectNode : MLink
    {
        /* All nodes inheriting from DLink should contain AT
         * LEAST the following components:
         *      - enum Name (to identify subtype of node - Name.ShipSprite for example)
         *      - constructor(s)
         *      - Setter (to set unique fields of the node)
         *      - Wash() (to clear the data fields of the node for recycling)
         *      - Dump/Print methods for debugging
         */

        // Data: ------------------
        public GameObject pGameObj;
        //private PCSTree pTree;


        public GameObjectNode()
            : base()
        {
            this.pGameObj = null;

        }

        public void WashNodeData()
        {
            //wash name and data;
            this.pGameObj = null;          
        }

        //SetData here;
        //public void Set(GameObject pGameObject, PCSTree pTree)
        //{
        //    this.pGameObj = pGameObject;

        //    Debug.Assert(pTree != null);
        //    this.pTree = pTree;
        //}

        public void Set(GameObject pGameObject)
        {
            Debug.Assert(pGameObject != null);
            this.pGameObj = pGameObject;
        }

        public void SetName(GameObject.Name name)
        {
            Debug.Assert(this.pGameObj != null);
            this.pGameObj.SetName(name);
        }

        public Enum GetName()
        {
            return this.pGameObj.GetName();
        }

        public void DumpNodeData()
        {
            if (this.pGameObj == null)
            {
                Debug.WriteLine("\t\t     GameObject: NULL");
            }
            else
            {
                Debug.WriteLine("\t\t     GameObject: {0}", this.GetHashCode());

                this.pGameObj.Dump();
            }

        }

    }



    public class GameObjectManager : Manager
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
       
        private static GameObjectNode pRefNode = new GameObjectNode();
        private static GameObjectManager pInstance = null;
        private PCSTree pRootTree;

        private GameObjectManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            //default null game object as ref node to avoid breaking find;
            GameObject pGameObj = new NullGameObject();
            Debug.Assert(pGameObj != null);
            GameObjectManager.pRefNode.pGameObj = pGameObj;


            //Todo - GameObjectManager Constructor - used in remove: HACK HACK, need a better way
            this.pRootTree = new PCSTree();
            //make sure tree and its root were created
            Debug.Assert(this.pRootTree != null);
            Debug.Assert(this.pRootTree.GetRoot() != null);
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
                pInstance = new GameObjectManager(startReserveSize, refillSize);
            }

            // Add a NULL Sprite into the Manager, allows find 
            GameSprite pGSprite = GameSpriteManager.Add(GameSprite.Name.NullObject, Image.Name.NullObject, 0, 0, 1, 1);
            Debug.Assert(pGSprite != null);

            Debug.WriteLine("------GameObject Manager Initialized-------");

        }
        private static GameObjectManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        ~GameObjectManager()
        {
            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GameObjectMan():{0}", this.GetHashCode());
            #endif

            GameObjectManager.pRefNode = null;
            GameObjectManager.pInstance = null;
            this.pRootTree = null;
        }
        public static void Destroy()
        {
            // Get the instance
            GameObjectManager pMan = GameObjectManager.privGetInstance();

            #if(TRACK_DESTRUCTOR)
            Debug.WriteLine("--->GameObjectMan.Destroy()");
            #endif

            pMan.baseDestroy();
            GameObjectManager.pRefNode = null;
            GameObjectManager.pInstance = null;
        }

        //----------------------------------------------------------------------
        // 4 Wrapper methods: baseAdd, baseFind, baseRemove, baseDump
        //----------------------------------------------------------------------

        //Remove GameObject Node 
        public static void Remove(GameObjectNode pNode)
        {
            Debug.Assert(pNode != null);
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            pMan.baseRemoveNode(pNode);
        }

        //Remove from GameObject PCSTree
        public static void Remove(GameObject pNode)
        {
            Debug.Assert(pNode != null);
            GameObjectManager pMan = GameObjectManager.privGetInstance();

            GameObject pSafetyNode = pNode;

            // OK so we have a linked list of trees (Remember that)

            // 1) find the tree root (we already know its the most parent)

            GameObject pTmp = pNode;
            GameObject pRoot = null;
            while (pTmp != null)
            {
                pRoot = pTmp;
                pTmp = (GameObject)pTmp.pParent;
            }

            // 2) pRoot is the tree we are looking for
            // now walk the active list looking for pRoot

            GameObjectNode pTree = (GameObjectNode)pMan.baseGetActive();

            while (pTree != null)
            {
                if (pTree.pGameObj == pRoot)
                {
                    // found it
                    break;
                }
                // Goto Next tree
                pTree = (GameObjectNode)pTree.pMNext;
            }

            // 3) pTree is the tree that holds pNode
            //  Now remove it

            Debug.Assert(pTree != null);
            Debug.Assert(pTree.pGameObj != null);
            pMan.pRootTree.SetRoot(pTree.pGameObj);
            pMan.pRootTree.Remove(pNode);

        }

        public static PCSTree GetRootTree()
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            Debug.Assert(pMan.pRootTree != null);
            return pMan.pRootTree;
        }


        public static void Update()
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();

            //get the root game object
            GameObjectNode pRoot = (GameObjectNode)pMan.baseGetActive();

            while (pRoot != null)
            {
                // OK at this point, I have a Root tree,
                // need to walk the tree completely before moving to next tree

                //todo double check on forward or reverse iteration
                //PCSTreeIterator pIterator = new PCSTreeIterator(pRoot.pGameObj);
                PCSTreeForwardIterator pIterator = new PCSTreeForwardIterator(pRoot.pGameObj);
                //PCSTreeReverseIterator pIterator = new PCSTreeReverseIterator(pRoot.pGameObj);


                // Initialize the first game object pointer
                GameObject pGameObj = (GameObject)pIterator.First();

                //iterate through and update all GameObjects in this tree/subtree
                //Debug.WriteLine("-------");
                while (!pIterator.IsDone())
                {
                    //Debug.WriteLine("  {0}", pGameObj.GetName());
                    pGameObj.Update();

                    // Advance
                    pGameObj = (GameObject)pIterator.Next();
                }

                // Go to next tree
                pRoot = (GameObjectNode)pRoot.pMNext;
            }
        }




        //public static GameObjectNode AttachTree(GameObject pGameObject, PCSTree pTree)
        //{
        //    //safety first
        //    Debug.Assert(pGameObject != null);

        //    GameObjectManager pMan = GameObjectManager.privGetInstance();
        //    Debug.Assert(pMan != null);

        //    GameObjectNode pNode = (GameObjectNode)pMan.baseAddToFront();
        //    Debug.Assert(pNode != null);


        //    Debug.Assert(pTree != null);
        //    pNode.Set(pGameObject, pTree);

        //    return pNode;
        //}

        public static GameObjectNode AttachTree(GameObject pGameObject)
        {
            Debug.Assert(pGameObject != null);

            GameObjectManager pMan = GameObjectManager.privGetInstance();

            GameObjectNode pNode = (GameObjectNode)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(pGameObject);
            return pNode;
        }

        //public static GameObject Find(GameObject.Name name)
        //{
        //    //get the singleton
        //    GameObjectManager pMan = privGetInstance();

        //    // Compare functions only compares two Nodes
        //    GameObjectManager.pRefNode.pGameObj.SetName(name);

        //    GameObjectNode pNode = (GameObjectNode)pMan.baseFindNode(GameObjectManager.pRefNode);
        //    Debug.Assert(pNode != null);

        //    return pNode.pGameObj;
        //}
        public static GameObject Find(GameObject.Name name, int index = 0)
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();

            // Compare functions only compares two Nodes
            GameObjectManager.pRefNode.pGameObj.SetName(name);
            GameObjectManager.pRefNode.pGameObj.index = index;

            GameObjectNode pRoot = (GameObjectNode)pMan.baseGetActive();
            GameObject pGameObj = null;

            bool found = false;
            while (pRoot != null && found == false)
            {
                // OK at this point, I have a Root tree,
                // need to walk the tree completely before moving to next tree
                //forward navigation
                PCSTreeForwardIterator pIterator = new PCSTreeForwardIterator(pRoot.pGameObj);

                // Initialize
                pGameObj = (GameObject)pIterator.First();
                //while (pGameObj != null)
                while (!pIterator.IsDone())
                {
                    //check for both matching name and index
                    if (pGameObj.GetName() == GameObjectManager.pRefNode.pGameObj.GetName() &&
                        pGameObj.index == GameObjectManager.pRefNode.pGameObj.index)
                    {
                        found = true;
                        break;
                    }

                    // Advance
                    pGameObj = (GameObject)pIterator.Next();
                }

                // Goto Next tree
                pRoot = (GameObjectNode)pRoot.pMNext;
            }

            return pGameObj;
        }

        public static void Insert(GameObject pGameObj, GameObject pParent)
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            Debug.Assert(pGameObj != null);

            if (pParent == null)
            {
                //GameObjectManager.AttachTree(pGameObj, null);
                GameObjectManager.AttachTree(pGameObj);
            }
            else
            {
                //parent shouldn't be null if here
                Debug.Assert(pParent != null);

                //set the root as the parent and insert pGamObj as 
                //a child of the parent in the same tree;
                pMan.pRootTree.SetRoot(pParent);
                pMan.pRootTree.Insert(pGameObj, pParent);
            }
        }



        public static void DumpAll()
        {
            GameObjectManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ GameObject Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            GameObjectManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ GameObject Manager Stats ------");
            pMan.baseDumpStats();
        }
        public static void DumpLists()
        {
            GameObjectManager pMan = privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ GameObject Manager Lists ------");
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

            //GameObjectNode pDataA = (GameObjectNode) pLinkA;
            //GameObjectNode pDataB = (GameObjectNode) pLinkB;

            //Boolean status = false;

            //if (pDataA.pGameObj.GetName() == pDataB.pGameObj.GetName())
            //{
            //    status = true;
            //}

            //return status;

            Debug.Assert(false);
            return false;
        }
        protected override MLink derivedCreateNode()
        {
            MLink pNode = new GameObjectNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        protected override void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GameObjectNode pNode = (GameObjectNode) pLink;
            pNode.DumpNodeData();
        }
        protected override void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            GameObjectNode pNode = (GameObjectNode) pLink;
            pNode.WashNodeData();
        }


    }
/*GameObjectManager.Update() - OLD
    //public static void Update()
    //{
    //    GameObjectManager pMan = GameObjectManager.privGetInstance();
    //    Debug.Assert(pMan != null);

    //    GameObjectNode pNode = (GameObjectNode)pMan.baseGetActive();

    //    while (pNode != null)
    //    {
    //        // Update the node
    //        Debug.Assert(pNode.pGameObj != null);

    //        pNode.pGameObj.Update();

    //        pNode = (GameObjectNode)pNode.pMNext;
    //    }

    //}
*/
}



