using Gengine.State;
using Gengine.UI;
using Microsoft.Xna.Framework;

namespace Drakborgen.States {
    public class MainMenu : BaseMenuState {
        public override void Init() {
            SetAction(HandleCommand);
            SetTitle(new MenuOption("text", "Drakborgen", Color.Red, new Vector2(World.View.Center.X - 50, World.View.Center.Y - 50)));
            AddOption(new MenuOption("text", "Exit", Color.White, new Vector2(World.View.Center.X - 40, World.View.Center.Y + 40)));
        }

        private void HandleCommand(string command) {
            if (command == "Escape") {
                StateManager.ClearStates();
            }
            if (command == "Enter") {
                if (SelectedOption == "Exit") {
                    StateManager.ClearStates();
                    //StateManager.PushState("game");
                }
            }
        }
    }
}
