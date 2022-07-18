using Code.Ecs.Components;
using Code.Ecs.Systems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

namespace Code.Ecs
{
    class Startup : MonoBehaviour {
        EcsWorld _world;
        EcsSystems _systems;

        void Start () {
            // create ecs environment.
            _world = new EcsWorld ();
            _systems = new EcsSystems(_world)
                .ConvertScene() // Этот метод сконвертирует GO в Entity
                .Add(new PlayerHealthSystem())
                .Add(new InputSystem())
                .Add(new PlayerMovableSystem());
            _systems.Init ();
        }
    
        void Update () {
            // process all dependent systems.
            _systems.Run ();
        }

        void OnDestroy () {
            // destroy systems logical group.
            _systems.Destroy ();
            // destroy world.
            _world.Destroy ();
        }
    }
}