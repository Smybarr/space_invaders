using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    //DelayedObjectManager holds all observers and 'Delays' their executions
    //after executing observers on its list it removes them
    public class DelayedObjectManager
    {        
        
        // Data: ------------------------
        private ColObserver pHeadColObserver;
        private static DelayedObjectManager pInstance = null;



        private DelayedObjectManager()
        {
            this.pHeadColObserver = null;
        }
        private static DelayedObjectManager privGetInstance()
        {
            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new DelayedObjectManager();
            }

            // Safety - this forces users to call create first
            Debug.Assert(pInstance != null);

            return pInstance;
        }


        static public void Attach(ColObserver observer)
        {
            // protection
            Debug.Assert(observer != null);

            DelayedObjectManager pDelayMan = DelayedObjectManager.privGetInstance();

            // add to front
            if (pDelayMan.pHeadColObserver == null)
            {
                pDelayMan.pHeadColObserver = observer;
                observer.pMNext = null;
                observer.pMPrev = null;
            }
            else
            {
                observer.pMNext = pDelayMan.pHeadColObserver;
                observer.pMPrev = null;
                pDelayMan.pHeadColObserver.pMPrev = observer;
                pDelayMan.pHeadColObserver = observer;
            }
        }


        private void privDetach(ColObserver node, ref ColObserver head)
        {
            // protection
            Debug.Assert(node != null);

            if (node.pMPrev != null)
            {	// middle or last node
                node.pMPrev.pMNext = node.pMNext;
            }
            else
            {  // first
                head = (ColObserver)node.pMNext;
            }

            if (node.pMNext != null)
            {	// middle node
                node.pMNext.pMPrev = node.pMPrev;
            }
        }

        //process any observers in the update loop of main game
        static public void Process()
        {
            //get the manager instance
            DelayedObjectManager pDelayMan = DelayedObjectManager.privGetInstance();

            //First - Fire off all the observers currently on the list

            //get the head observer
            ColObserver pNode = pDelayMan.pHeadColObserver;

            //iterate through the observer list
            while (pNode != null)
            {
                // Fire off listener
                pNode.Execute();

                //get the next observer
                pNode = (ColObserver)pNode.pMNext;
            }


            //done executing all observers - now remove them
            pNode = pDelayMan.pHeadColObserver;
            ColObserver pTmp = null;

            //iterate through the observer list
            while (pNode != null)
            {
                //hold current observer in pTmp for reference after removal
                pTmp = pNode;
                pNode = (ColObserver)pNode.pMNext;

                // remove observer
                pDelayMan.privDetach(pTmp, ref pDelayMan.pHeadColObserver);
            }
        }



    }
}