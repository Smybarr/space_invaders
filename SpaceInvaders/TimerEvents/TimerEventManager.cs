using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class TimerEventManager : Manager
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        private TimerEventManager(int reserveNum = 3, int reserveGrow = 1)
            : base(reserveNum, reserveGrow)
        {
            // do nothing
            this.currTime = 0.0f;
        }
        public static void Create(int reserveNum = 3, int reserveGrow = 1)
        {
            // make sure values are ressonable 
            Debug.Assert(reserveNum > 0);
            Debug.Assert(reserveGrow > 0);

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new TimerEventManager(reserveNum, reserveGrow);
            }
        }

        //----------------------------------------------------------------------
        // Methods
        //----------------------------------------------------------------------
        public static TimerEvent Add(TimerEvent.Name timeName, Command pCommand, float deltaTimeToTrigger)
        {
            TimerEventManager pMan = TimerEventManager.privGetInstance();
            Debug.Assert(pMan != null);

            TimerEvent pNode = (TimerEvent)pMan.baseAddToFront();
            Debug.Assert(pNode != null);

            Debug.Assert(pCommand != null);
            Debug.Assert(deltaTimeToTrigger >= 0.0f);

            pNode.Set(timeName, pCommand, deltaTimeToTrigger);
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
            pTimerEventRef.name = name;

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

            // squirrel away
            pTimerEventManager.currTime = totalTime;

            // walk the list
            TimerEvent pEvent = (TimerEvent)pTimerEventManager.pActive;
            TimerEvent nextEvent = null;

            // Walk the list until there is no more list OR currTime is greater than TimerEvent 
            // ToDo Fix: List needs to be sorted
            while (pEvent != null && (pTimerEventManager.currTime >= pEvent.triggerTime))
            {
                // Difficult to walk a list and remove itself from the list
                // so squirrel away the next event now, use it at bottom of while
                nextEvent = (TimerEvent)pEvent.pMNext;

                if (pTimerEventManager.currTime >= pEvent.triggerTime)
                {
                    // call it
                    pEvent.Process();

                    // remove from list
                    pTimerEventManager.baseRemoveNode(pEvent);
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

            if (pDataA.name == pDataB.name)
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
        private static TimerEventManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data: unique data for this manager here
        //----------------------------------------------------------------------
        private static TimerEvent pTimerEventRef = new TimerEvent();
        private static TimerEventManager pInstance = null;
        protected float currTime;
    }
}
