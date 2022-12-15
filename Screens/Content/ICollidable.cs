using System;
using System.Collections.Generic;
using System.Text;

namespace GhostHunter.Screens.Content
{
    public interface ICollidable
    {
        void OnCollide(Sprite sprite);
    }
}
