using System;
using System.Diagnostics;

namespace SpaceInvaders
{
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
        static public void Process()
        {
            DelayedObjectManager pDelayMan = DelayedObjectManager.privGetInstance();

            ColObserver pNode = pDelayMan.pHeadColObserver;

            while (pNode != null)
            {
                // Fire off listener
                pNode.Execute();

                pNode = (ColObserver)pNode.pMNext;
            }


            // remove
            pNode = pDelayMan.pHeadColObserver;
            ColObserver pTmp = null;

            while (pNode != null)
            {
                pTmp = pNode;
                pNode = (ColObserver)pNode.pMNext;

                // remove
                pDelayMan.privDetach(pTmp, ref pDelayMan.pHeadColObserver);
            }
        }



    }
}