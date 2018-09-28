using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ColSubject
    {
        public ColSubject()
        {
            this.pObjB = null;
            this.pObjA = null;
            this.pHead = null;
        }

        ~ColSubject()
        {
            this.pObjB = null;
            this.pObjA = null;
            // ToDo
            // Need to walk and nuke the list
            this.pHead = null;
        }

        public void Attach(ColObserver observer)
        {
            // protection
            Debug.Assert(observer != null);

            observer.pSubject = this;

            // add to front
            if (pHead == null)
            {
                pHead = observer;
                observer.pMNext = null;
                observer.pMPrev = null;
            }
            else
            {
                observer.pMNext = pHead;
                pHead.pMPrev = observer;
                pHead = observer;
            }

        }


        public void Notify()
        {
            ColObserver pNode = this.pHead;

            while (pNode != null)
            {
                // Fire off listener
                pNode.Notify();

                pNode = (ColObserver)pNode.pMNext;
            }

        }

        public void Detach()
        {
        }


        // Data: ------------------------
        private ColObserver pHead;
        public GameObject pObjA;
        public GameObject pObjB;


    }
}