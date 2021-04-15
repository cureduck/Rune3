using Unity.Entities;
using Unity.Mathematics;

namespace Game.Component
{
    public struct Path : IBufferElementData
    {
        public int2 node;

        public static implicit operator Path(int2 coord)
        {
            return new Path{node = coord};
        }
    }
}