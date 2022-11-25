using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Helpers
{
    public static class InputHelpers
    {

        public static bool WasJustClicked(ButtonState currentState,ButtonState prevState)
        {
            if(currentState == ButtonState.Pressed && prevState == ButtonState.Released)
            {
                return true;
            }
            return false;
        }
    }
}
