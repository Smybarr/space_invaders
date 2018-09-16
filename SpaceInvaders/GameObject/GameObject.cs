using System;
using System.Diagnostics;

namespace SpaceInvaders
{

    public abstract class GameObject : PCSNode
    {
        public enum Name
        {
            Squid,
            Crab,
            Octopus,

            Grid,

            NullObject,

            Blank

        }

        // Data: ---------------
        private Name name;

        public float x;
        public float y;
        public ProxySprite pProxySprite;

        protected GameObject(GameObject.Name objName)
        {
            this.name = objName;
            this.x = 0.0f;
            this.y = 0.0f;
            this.pProxySprite = null;
        }

        protected GameObject(GameObject.Name objName, GameSprite.Name spriteName)
        {
            this.name = objName;
            this.x = 0.0f;
            this.y = 0.0f;
            //this.pProxySprite = new ProxySprite(spriteName);
            this.pProxySprite = ProxySpriteManager.Add(spriteName);
            Debug.Assert(this.pProxySprite != null);
        }

        override public Enum getName()
        {
            return this.name;
        }
        public void SetName(Name name)
        {
            this.name = name;
        }


        ~GameObject()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("     ~GameObject():{0}", this.GetHashCode());
#endif
            this.name = GameObject.Name.Blank;
            this.pProxySprite = null;
        }

        public virtual void Update()
        {
            Debug.Assert(this.pProxySprite != null);
            this.pProxySprite.x = this.x;
            this.pProxySprite.y = this.y;
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

        public GameObject.Name GetName()
        {
            return this.name;
        }

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
        public void Set(GameObject pGameObject)
        {
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
            Debug.Assert(this.pGameObj != null);
            Debug.WriteLine("\t\t     GameObject: {0}", this.GetHashCode());

            this.pGameObj.Dump();
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

        
        private GameObjectManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            //default null game object to avoid breaking find;
            GameObject pGameObj = new NullGameObject();
            Debug.Assert(pGameObj != null);
            GameObjectManager.pRefNode.pGameObj = pGameObj;
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

        public static GameObjectNode Attach(GameObject pGameObject)
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            Debug.Assert(pMan != null);

            GameObjectNode pNode = (GameObjectNode)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            pNode.Set(pGameObject);
            return pNode;
        }

        public static void Remove(GameObjectNode pNode)
        {
            Debug.Assert(pNode != null);
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            pMan.baseRemoveNode(pNode);
        }

        public static void Update()
        {
            GameObjectManager pMan = GameObjectManager.privGetInstance();
            Debug.Assert(pMan != null);

            GameObjectNode pNode = (GameObjectNode)pMan.baseGetActive();

            while (pNode != null)
            {
                // Update the node
                Debug.Assert(pNode.pGameObj != null);

                pNode.pGameObj.Update();

                pNode = (GameObjectNode)pNode.pMNext;
            }

        }

        public static GameObject Find(GameObject.Name name)
        {
            //get the singleton
            GameObjectManager pMan = privGetInstance();

            // Compare functions only compares two Nodes
            GameObjectManager.pRefNode.pGameObj.SetName(name);

            GameObjectNode pNode = (GameObjectNode)pMan.baseFindNode(GameObjectManager.pRefNode);
            Debug.Assert(pNode != null);

            return pNode.pGameObj;
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
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------

        //EDIT THE FOLLOWING METHODS---------------------
        protected override Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFindNode() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            GameObjectNode pDataA = (GameObjectNode) pLinkA;
            GameObjectNode pDataB = (GameObjectNode) pLinkB;

            Boolean status = false;

            if (pDataA.pGameObj.GetName() == pDataB.pGameObj.GetName())
            {
                status = true;
            }

            return status;
        }

        protected override MLink derivedCreateNode()
        {
            MLink pNode = new GameObjectNode();
            Debug.Assert(pNode != null);

            return pNode;
        }
        //---------------------



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
}