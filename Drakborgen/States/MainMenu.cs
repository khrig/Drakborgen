using Gengine.State;
using Gengine.UI;
using Gengine.Utils;
using Microsoft.Xna.Framework;

namespace Drakborgen.States {
    public class MainMenu : BaseMenuState {
        public override void Run() {
            SetAction(HandleCommand);
            SetTitle(new MenuOption("text", "Drakborgen", Color.Red, new Vector2(World.View.Center.X - TextExtensions.GetContentLength("text", "Drakborgen").Length() / 2, World.View.Center.Y - 50)));
            AddOption(new MenuOption("text", "Start", Color.White, new Vector2(World.View.Center.X - TextExtensions.GetContentLength("text", "Start").Length() / 2, World.View.Center.Y + 40)));
            AddOption(new MenuOption("text", "Exit", Color.White, new Vector2(World.View.Center.X - TextExtensions.GetContentLength("text", "Exit").Length() / 2, World.View.Center.Y + 80)));
        }

        private void HandleCommand(string command) {
            if (command == "Escape") {
                StateManager.ClearStates();
            }
            if (command == "Enter") {
                if (SelectedOption == "Exit") {
                    StateManager.ClearStates();
                } else if (SelectedOption == "Start") {
                    StateManager.ClearStates();
                    StateManager.PushAndSetupState("game");                    
                }
            }
        }
    }
}
