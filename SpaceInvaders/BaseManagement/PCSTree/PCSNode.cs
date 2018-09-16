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

        abstract public Enum GetPCSName();
        abstract public int GetIndex();

        // Methods: Dump ------------------

        public void dumpNode()
        {
            Debug.WriteLine("");
            Debug.WriteLine("    name: {0} {1}", this.GetPCSName(), this.GetHashCode());
            if (this.pParent != null)
            {
                Debug.WriteLine("  parent: {0} {1}", this.pParent.GetPCSName(), this.pParent.GetHashCode());
            }
            else
            {
                Debug.WriteLine("  parent: ------");
            }
            if (this.pChild != null)
            {
                Debug.WriteLine("   child: {0} {1}", this.pChild.GetPCSName(), this.pChild.GetHashCode());
            }
            else
            {
                Debug.WriteLine("   child: ------");
            }
            if (this.pSibling != null)
            {
                Debug.WriteLine(" sibling: {0} {1}", this.pSibling.GetPCSName(), this.pSibling.GetHashCode());
            }
            else
            {
                Debug.WriteLine(" sibling: ------");
            }

        }



    }
}