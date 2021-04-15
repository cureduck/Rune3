using Game.Component;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class LoadManager : MonoBehaviour
    {
        private EntityArchetype unitArch;
        private EntityManager em;
        public int unitCount;
    
        void Start()
        {
            em = World.DefaultGameObjectInjectionWorld.EntityManager;
            unitArch = em.CreateArchetype(typeof(Path));
        
            em.CreateEntity(unitArch, unitCount);
        }
    }
}