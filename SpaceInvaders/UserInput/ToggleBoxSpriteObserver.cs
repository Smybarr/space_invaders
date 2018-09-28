using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ToggleBoxSpriteObserver : InputObserver
    {
        public override void Notify()
        {
			//toggle off
            if (SpriteBatchManager.renderBoxes == true)
            {
                Debug.WriteLine("BoxSprite Rendering OFF");
                SpriteBatchManager.renderBoxes = false;
            }

			//toggle on
            else if (SpriteBatchManager.renderBoxes == false)
            {
                Debug.WriteLine("BoxSprite Rendering ON");
                SpriteBatchManager.renderBoxes = true;
            }
        }
    }
}