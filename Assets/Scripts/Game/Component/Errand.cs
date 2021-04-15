using Unity.Entities;

namespace Game.Component
{
    public struct Errand : IComponentData
    {
        public STATE State;

        public Entity provider;
        public Entity consumer;
    }

    public enum STATE
    {
        REACHING,
        WORKING
    }
}