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
        public static void Dump()
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            Debug.WriteLine("------ Timer Manager ------");
            pMan.baseDumpAll();
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
                    Debug.WriteLine("{0} Event Triggered!", pEvent.GetName());
                    Debug.WriteLine("Trigger Time:{0}", pTimerEventManager.currTime);
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
