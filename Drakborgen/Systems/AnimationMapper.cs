using Gengine.Animation;
using Gengine.Input;

namespace Drakborgen.Systems {
    public class AnimationMapper : IAnimationMapper {
        public string GetAnimationId(InputComponent input) {
            if (input.DirectionX < 0) {
                return "moveleft";
            }
            if (input.DirectionX > 0) {
                return "moveright";
            }
            if (input.DirectionY < 0) {
                return "moveup";
            }
            if (input.DirectionY > 0) {
                return "movedown";
            }
            return "idle";
        }
    }
}
