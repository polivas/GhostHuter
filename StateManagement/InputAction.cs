using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GhosterHunter.StateManagement
{

    public class InputAction
    {
        private readonly Buttons[] _buttons;
        private readonly Keys[] _keys;
        private readonly bool _firstPressOnly;

        private delegate bool ButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex player);
        private delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        /// <summary>
        /// Constructs a new InputMapping, binding the suppled triggering input options to the action
        /// </summary>
        /// <param name="triggerButtons">The buttons that trigger this action</param>
        /// <param name="triggerKeys">The keys that trigger this action</param>
        /// <param name="firstPressOnly">If this action only triggers on the initial key/button press</param>
        public InputAction(Buttons[] triggerButtons, Keys[] triggerKeys, bool firstPressOnly)
        {

            _buttons = triggerButtons != null ? triggerButtons.Clone() as Buttons[] : new Buttons[0];
            _keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            _firstPressOnly = firstPressOnly;
        }

        /// <summary>
        /// Determines if he action has occured. If playerToTest is null, the player parameter will be the player that performed the action
        /// </summary> 
        /// <param name="stateToTest">The InputState object to test</param>
        /// <param name="playerToTest">If not null, specifies the player (0-3) whose input should be tested</param>
        /// <param name="player">The player (0-3) who triggered the action</param>
        public bool Occurred(InputState stateToTest, PlayerIndex? playerToTest, out PlayerIndex player)
        {
            ButtonPress buttonTest;
            KeyPress keyTest;

            if (_firstPressOnly)
            {
                buttonTest = stateToTest.IsNewButtonPress;
                keyTest = stateToTest.IsNewKeyPress;
            }
            else
            {
                buttonTest = stateToTest.IsButtonPressed;
                keyTest = stateToTest.IsKeyPressed;
            }

            foreach (var button in _buttons)
            {
                if (buttonTest(button, playerToTest, out player))
                    return true;
            }
            foreach (var key in _keys)
            {
                if (keyTest(key, playerToTest, out player))
                    return true;
            }
            player = PlayerIndex.One;
            return false;
        }
    }
}
