using System;
using System.Collections.Generic;
using System.Text;

namespace SceenGame.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
