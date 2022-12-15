using System;
using System.Collections.Generic;
using System.Text;
using GhostHunter.StateManagement;


namespace GhostHunter
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
