using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class TimerEventManager : Manager
    {
        //----------------------------------------------------------------------
        // Data: unique data for this manager here
        //----------------------------------------------------------------------
        private static TimerEvent pTimerEventRef = new TimerEvent();
        private static TimerEventManager pInstance = null;
        protected float currTime;

        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        private TimerEventManager(int startReserveSize = 3, int refillSize = 1)
            : base(startReserveSize, refillSize)
        {
            // do nothing
            this.currTime = 0.0f;
        }
        public static void Create(int startReserveSize = 3, int refillSize = 1)
        {
            // make sure values are ressonable 
            Debug.Assert(startReserveSize > 0);
            Debug.Assert(refillSize > 0);

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new TimerEventManager(startReserveSize, refillSize);
            }

            Debug.WriteLine("------TimerEvent Manager Initialized-------");
        }
        private static TimerEventManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        ~TimerEventManager()
        {
#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("~TimerMan():{0} ", this.GetHashCode());
#endif
            TimerEventManager.pTimerEventRef = null;
            TimerEventManager.pInstance = null;
        }
        public static void Destroy()
        {
            // Get the instance
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.WriteLine("--->TimerMan.Destroy()");
            pMan.baseDestroy();

#if (TRACK_DESTRUCTOR)
            Debug.WriteLine("     {0} ({1})", TimerMan.pTimerEventRef, TimerMan.pTimerEventRef.GetHashCode());
            Debug.WriteLine("     {0} ({1})", TimerMan.pInstance, TimerMan.pInstance.GetHashCode());
#endif
            TimerEventManager.pTimerEventRef = null;
            TimerEventManager.pInstance = null;
        }


        public static void LoadAlienAnimations()
        {
            //----------------------
            //Animated Sprite (Attached to TimerManager
            //Each animation is a TimerEvent following command pattern;

            // Create alien animation sprites
            AnimationSprite pAnim_Squid = new AnimationSprite(GameSprite.Name.Squid);
            AnimationSprite pAnim_Crab = new AnimationSprite(GameSprite.Name.Crab);
            AnimationSprite pAnim_Octo = new AnimationSprite(GameSprite.Name.Octopus);

            //explosion animation 
            AnimationSprite pAnim_AlienExplosion_Pop = new AnimationSprite(GameSprite.Name.AlienExplosion);


            // attach to death manager for garbage collection management
            DeathManager.Attach(pAnim_Squid);
            DeathManager.Attach(pAnim_Crab);
            DeathManager.Attach(pAnim_Octo);

            DeathManager.Attach(pAnim_AlienExplosion_Pop);

            // attach alternating images to animation cycle

            pAnim_Squid.Attach(Image.Name.SquidClosed);
            pAnim_Squid.Attach(Image.Name.SquidOpen);


            pAnim_Crab.Attach(Image.Name.CrabClosed);
            pAnim_Crab.Attach(Image.Name.CrabOpen);


            pAnim_Octo.Attach(Image.Name.OctopusClosed);
            pAnim_Octo.Attach(Image.Name.OctopusOpen);

            // add AnimationSprite to timer

            //set the interval between events and add sprite animation event objects to timer manager;
            float animInterval = 1.0f;
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Squid, animInterval);
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Crab, animInterval);
            TimerEventManager.Add(TimerEvent.Name.SpriteAnimation, pAnim_Octo, animInterval);

        }

        public static void LoadBombAnimations()
        {
            //-----------------

            // Create bomb animation sprites
            AnimationSprite pAnim_CrossAlienBomb = new AnimationSprite(GameSprite.Name.CrossAlienBomb);
            AnimationSprite pAnim_ZigZagAlienBomb = new AnimationSprite(GameSprite.Name.ZigZagAlienBomb);
            AnimationSprite pAnim_RollingAlienBomb = new AnimationSprite(GameSprite.Name.RollingAlienBomb);

            // attach to death manager for garbage collection management
            DeathManager.Attach(pAnim_CrossAlienBomb);
            DeathManager.Attach(pAnim_ZigZagAlienBomb);
            DeathManager.Attach(pAnim_RollingAlienBomb);

            // attach alternating images to animation cycle

            pAnim_CrossAlienBomb.Attach(Image.Name.AlienBombCross_Two);
            pAnim_CrossAlienBomb.Attach(Image.Name.AlienBombCross_Three);
            pAnim_CrossAlienBomb.Attach(Image.Name.AlienBombCross_Four);
            pAnim_CrossAlienBomb.Attach(Image.Name.AlienBombCross_One);

            pAnim_ZigZagAlienBomb.Attach(Image.Name.AlienBombZigZag_Two);
            pAnim_ZigZagAlienBomb.Attach(Image.Name.AlienBombZigZag_Three);
            pAnim_ZigZagAlienBomb.Attach(Image.Name.AlienBombZigZag_Four);
            pAnim_ZigZagAlienBomb.Attach(Image.Name.AlienBombZigZag_One);

            pAnim_RollingAlienBomb.Attach(Image.Name.AlienBombRolling_Three);
            pAnim_RollingAlienBomb.Attach(Image.Name.AlienBombRolling_Two);

            //set the interval between events and add sprite animation event objects to timer manager;
            float animInterval = 0.01f;
            TimerEventManager.Add(TimerEvent.Name.BombAnimation, pAnim_CrossAlienBomb, animInterval);
            TimerEventManager.Add(TimerEvent.Name.BombAnimation, pAnim_ZigZagAlienBomb, animInterval);
            TimerEventManager.Add(TimerEvent.Name.BombAnimation, pAnim_RollingAlienBomb, animInterval);

        }



        //----------------------------------------------------------------------
        // Methods
        //----------------------------------------------------------------------
        public static TimerEvent Add(TimerEvent.Name timeName, Command pCommand, float deltaTimeToTrigger)
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            //pull a resereved node
            TimerEvent pNode = (TimerEvent)pMan.basePopReserve();
            Debug.Assert(pNode != null);

            Debug.Assert(pCommand != null);
            Debug.Assert(deltaTimeToTrigger >= 0.0f);

            //set the data
            pNode.Set(timeName, pCommand, deltaTimeToTrigger);

            //add the newly set timer event node to the list in sorted order
            pMan.baseAddSorted(pNode);

            return pNode;
        }
        public static TimerEvent Find(TimerEvent.Name name)
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);
            // Compare functions only compares two Nodes

            // So:  Use a reference node
            //      fill in the needed data
            //      use in the Compare() function
            Debug.Assert(pTimerEventRef != null);
            pTimerEventRef.Wash();
            pTimerEventRef.SetName(name);

            TimerEvent pData = (TimerEvent)pMan.baseFindNode(pTimerEventRef);
            return pData;
        }
        public static void Remove(TimerEvent pNode)
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.Assert(pNode != null);
            pMan.baseRemoveNode(pNode);
        }
        public static void DumpAll()
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ TimerEvent Manager Dump All ------");
            pMan.baseDumpAll();
        }
        public static void DumpStats()
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ TimerEvent Manager Stats ------");
            pMan.baseDumpStats();
        }
        public static void DumpLists()
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ TimerEvent Manager Lists ------");
            pMan.baseDumpLists();
        }

        public static float GetCurrTime()
        {
            // Get the instance
            TimerEventManager pTimerEventManager = TimerEventManager.privGetInstance();

            // return time
            return pTimerEventManager.currTime;
        }

        public static void Update(float totalTime)
        {
            // Get the instance
            TimerEventManager pTimerEventManager = TimerEventManager.privGetInstance();

            // store the current time
            pTimerEventManager.currTime = totalTime;

            // walk the event list
            TimerEvent pEvent = (TimerEvent)pTimerEventManager.baseGetActive();
            TimerEvent nextEvent = null;

            // Walk the list until currTime is greater than timeEvent triggerTime
            while (pEvent != null)
            {
                // get the next event early in case this event executes and is removed
                nextEvent = (TimerEvent)pEvent.pMNext;

                if (pTimerEventManager.currTime >= pEvent.triggerTime)
                {
                    //Debug.WriteLine("{0} Event Triggered!", pEvent.GetName());
                    //Debug.WriteLine("Trigger Time:{0}", pTimerEventManager.currTime);
                    // execute the event
                    pEvent.Process();

                    // remove event from list after execution
                    pTimerEventManager.baseRemoveNode(pEvent);
                }
                else
                {
                    // early out, since the list is sorted
                    break;
                }

                // advance the pointer
                pEvent = nextEvent;
            }
        }

        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected MLink derivedCreateNode()
        {
            MLink pNode = new TimerEvent();
            Debug.Assert(pNode != null);

            return pNode;
        }
        override protected Boolean derivedCompareNodes(MLink pLinkA, MLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            TimerEvent pDataA = (TimerEvent)pLinkA;
            TimerEvent pDataB = (TimerEvent)pLinkB;

            Boolean status = false;

            if (pDataA.GetName() == pDataB.GetName())
            {
                status = true;
            }

            return status;
        }
        override protected void derivedWashNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            TimerEvent pNode = (TimerEvent)pLink;
            pNode.Wash();
        }
        override protected void derivedDumpNode(MLink pLink)
        {
            Debug.Assert(pLink != null);
            TimerEvent pData = (TimerEvent)pLink;
            pData.Dump();
        }

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------


    }
}
