using Gengine.Animation;
using Gengine.Input;

namespace Drakborgen.Systems {
    public class AnimationMapper : IAnimationMapper {
        public string GetAnimationId(InputComponent input) {
            if (input.Input.HasFlag(InputKey.Left)) {
                return "moveleft";
            }
            if (input.Input.HasFlag(InputKey.Right)) {
                return "moveright";
            }
            if (input.Input.HasFlag(InputKey.Up)) {
                return "moveup";
            }
            if (input.Input.HasFlag(InputKey.Down)) {
                return "movedown";
            }
            return "idle";
        }
    }
}
