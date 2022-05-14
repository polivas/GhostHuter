using System;
using System.Collections.Generic;
using System.Text;

namespace GhosterHunter.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
