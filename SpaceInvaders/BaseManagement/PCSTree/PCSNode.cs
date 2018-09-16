using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public abstract class PCSNode
    {
        // data ----------------------
        public PCSNode pParent;
        public PCSNode pChild;
        public PCSNode pSibling;

        // Constructors: --------------------------------
        public PCSNode()
        {
            this.pParent = null;
            this.pChild = null;
            this.pSibling = null;
        }

        public PCSNode(PCSNode pNode)
        {
            this.pParent = pNode.pParent;
            this.pChild = pNode.pChild;
            this.pSibling = pNode.pSibling;
        }

        public PCSNode(PCSNode pParent, PCSNode pChild, PCSNode pSibling)
        {
            this.pParent = pParent;
            this.pChild = pChild;
            this.pSibling = pSibling;
        }

        // Methods: set/get -------------------------------

        abstract public Enum getName();

        // Methods: Dump ------------------

        public void dumpNode()
        {
            Debug.WriteLine("");
            Debug.WriteLine("    name: {0} {1}", this.getName(), this.GetHashCode());
            if (this.pParent != null)
            {
                Debug.WriteLine("  parent: {0} {1}", this.pParent.getName(), this.pParent.GetHashCode());
            }
            else
            {
                Debug.WriteLine("  parent: ------");
            }
            if (this.pChild != null)
            {
                Debug.WriteLine("   child: {0} {1}", this.pChild.getName(), this.pChild.GetHashCode());
            }
            else
            {
                Debug.WriteLine("   child: ------");
            }
            if (this.pSibling != null)
            {
                Debug.WriteLine(" sibling: {0} {1}", this.pSibling.getName(), this.pSibling.GetHashCode());
            }
            else
            {
                Debug.WriteLine(" sibling: ------");
            }

        }



    }
}