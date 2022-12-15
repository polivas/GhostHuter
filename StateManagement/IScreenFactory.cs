using System;
using System.Collections.Generic;
using System.Text;

namespace GhostHunter.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
