using Drakborgen.Components;
using Gengine.Animation;
using Gengine.EntityComponentSystem;
using Gengine.Input;
using Gengine.Rendering;

namespace Drakborgen.Systems {
    public class AnimationSystem : EntityProcessingSystem {
        private readonly IAnimationMapper _animationMapper;

        public AnimationSystem(IAnimationMapper animationMapper)
            : base(typeof(AnimationComponent), typeof(RenderComponent), typeof(InputComponent)) {
            _animationMapper = animationMapper;
        }

        public override void Process(Entity entity, float dt) {
            var render = entity.GetComponent<RenderComponent>();
            var animation = entity.GetComponent<AnimationComponent>();
            var input = entity.GetComponent<InputComponent>();

            string animationId = _animationMapper.GetAnimationId(input);
            SetRunningAnimation(animation, animationId);

            if (!string.IsNullOrEmpty(animation.CurrentAnimationId)){
                UpdateAnimation(dt, animation);
                render.SourceRectangle = animation.Animations[animation.CurrentAnimationId].SourceRectangle;
            }
        }

        private void UpdateAnimation(float dt, AnimationComponent animationComponent){
            var animation = animationComponent.Animations[animationComponent.CurrentAnimationId];

            if (animation.AnimationFrames.Count == 0) return;

            animationComponent.Elapsed += dt;
            if (!(animationComponent.Elapsed > animation.CurrentFrame.Duration)) return;

            animation.CurrentFrameIndex++;
            if (animation.CurrentFrameIndex >= animation.AnimationFrames.Count)
                animation.CurrentFrameIndex = 0;

            animationComponent.Elapsed = 0;
        }

        private void SetRunningAnimation(AnimationComponent animationComponent, string name) {
            if (animationComponent.Animations.ContainsKey(name)){
                if (animationComponent.CurrentAnimationId != name){
                    var animation = animationComponent.Animations[name];
                    animation.CurrentFrameIndex = 0;
                    animationComponent.Elapsed = 0.0f;
                    animationComponent.CurrentAnimationId = name;
                }
            }
            else{
                animationComponent.CurrentAnimationId = string.Empty;
            }
        }
    }
}