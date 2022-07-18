using Code.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Ecs.Systems
{
    internal class InputSystem : IEcsRunSystem 
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        private EcsWorld _world = null;
        private EcsFilter<InputComponent> _filter = null;

        public void Run()
        {
            foreach (var i in _filter) 
            {
                ref var input = ref _filter.Get1 (i);
                input.Horizontal = Input.GetAxisRaw(Horizontal);
                input.Vertical = Input.GetAxisRaw(Vertical);
                
                Debug.Log(input.Horizontal);
            }
        }
    }
}