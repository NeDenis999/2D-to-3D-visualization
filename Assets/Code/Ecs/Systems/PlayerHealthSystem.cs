using Code.Ecs.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Ecs.Systems
{
    class PlayerHealthSystem : IEcsInitSystem, IEcsRunSystem 
    {
        // Переменная _world автоматически инициализируется
        private EcsWorld _world = null;
        // В фильтре просто описываем с каким компонентом 
        //будет работать система 
        private EcsFilter<HealthComponent> _filter = null;

        public void Init () {
            // Сработает на старте
            foreach (var i in _filter) 
            {
                // entity которые содержат PlayerComponent.
                ref var entity = ref _filter.GetEntity (i); 

                // Get1 вернет ссылку на "PlayerComponent".
                ref var player = ref _filter.Get1 (i);
                player.Health = 1000000; 
                Debug.Log("Init");
            }
        }

        public void Run () {
            foreach (var i in _filter) 
            {
                // entity которые содержат PlayerComponent.
                ref var entity = ref _filter.GetEntity (i); 

                // Get1 вернет ссылку на "PlayerComponent".
                ref var player = ref _filter.Get1 (i);
                player.Health = player.Health - 10; 
                //Debug.Log(player.Health);
            }
        }
    }
}