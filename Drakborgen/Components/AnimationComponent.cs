using System.Collections.Generic;
using Gengine.Animation;
using Gengine.EntityComponentSystem;

namespace Drakborgen.Components {
    public class AnimationComponent : IComponent {
        public AnimationComponent(Dictionary<string, Animation> animations){
            Animations = animations;
        }

        public string CurrentAnimationId { get; set; }
        public Dictionary<string, Animation> Animations { get; set; }
        public float Elapsed { get; set; }
    }
}
