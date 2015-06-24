using System.Collections.Generic;
using Gengine.Entities;
using Gengine.EntityComponentSystem;

namespace Drakborgen.Systems {
    public class RenderSystem {
        public IEnumerable<IRenderable> GetAllRenderables(){
            return Entity.GetAllComponents<TestComponent>(TestComponent.ComponentId);
        }
    }
}
