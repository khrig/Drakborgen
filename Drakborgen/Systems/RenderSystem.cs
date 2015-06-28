using Drakborgen.Components;
using Gengine.EntityComponentSystem;
using Gengine.Rendering;

namespace Drakborgen.Systems {
    public class RenderSystem : EntityProcessingSystem{
        public override void Process(Entity entity, float dt){
            var physics = entity.GetComponent<PhysicsComponent>();
            var render = entity.GetComponent<RenderComponent>();

            render.RenderPosition = physics.Position;
        }
    }
}
