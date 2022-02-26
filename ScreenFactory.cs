using System;
using System.Collections.Generic;
using System.Text;
using SceenGame.StateManagement;

namespace SceenGame
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
