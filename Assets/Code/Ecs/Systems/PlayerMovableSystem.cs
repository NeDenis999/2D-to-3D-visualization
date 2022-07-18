using Code.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Ecs.Systems
{
    public class PlayerMovableSystem : IEcsRunSystem 
    {
        private EcsWorld _world = null;
        private EcsFilter<InputComponent, TransformComponent, MovableSpeedComponent> _filter = null;
        
        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var playerTransform = ref _filter.Get2 (i);
                ref var movableSpeed = ref _filter.Get3(i);
                ref var inputComponent = ref _filter.Get1(i);

                playerTransform.Transform.position +=
                    new Vector3(inputComponent.Horizontal, 0, inputComponent.Vertical) * movableSpeed.Speed;
            }
        }
    }
}