using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class PCSTreeIterator : Iterator
    {
        public PCSTreeIterator(PCSNode rootNode)
        {
            Debug.Assert(rootNode != null);
            this.root = rootNode;
            this.current = this.root;
        }

        public override PCSNode First()
        {
            this.current = this.root;
            return this.current;
        }

        public override PCSNode Next()
        {
            this.current = privGetNext(this.current);

            return this.current;
        }

        private PCSNode privGetNext(PCSNode node, bool UseChild = true)
        {
            PCSNode tmp = null;

            if ((node.pChild != null) && UseChild)
            {
                tmp = node.pChild;
            }
            else if (node.pSibling != null)
            {
                tmp = node.pSibling;
            }
            else if (node.pParent != this.root)
            {
                // recurse here
                tmp = this.privGetNext(node.pParent, false);
            }
            else
            {
                tmp = null;
            }
            return tmp;
        }

        public override bool IsDone()
        {
            return (this.current == null);
        }

        public override PCSNode CurrentItem()
        {
            return this.current;
        }

        private PCSNode root;
        private PCSNode current;
    }
}
