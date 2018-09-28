using System;
using System.Diagnostics;


namespace SpaceInvaders
{
    public class InputSubject
    {


        public void Attach(InputObserver observer)
        {
            // protection
            Debug.Assert(observer != null);

            observer.pSubject = this;

            // add to front
            if (head == null)
            {
                head = observer;
                observer.pMNext = null;
                observer.pMPrev = null;
            }
            else
            {
                observer.pMNext = head;
                observer.pMPrev = null;
                head.pMPrev = observer;
                head = observer;
            }
        }


        public void Notify()
        {
            InputObserver pNode = this.head;

            while (pNode != null)
            {
                // Fire off listener
                pNode.Notify();

                pNode = (InputObserver)pNode.pMNext;
            }
        }

        public void Detach()
        {
        }


        // Data: ------------------------
        private InputObserver head;



    }
}