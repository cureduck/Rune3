using Game.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public class RunSystem : SystemBase
    {
        
        private static NativeArray<int> walkableMap = new NativeArray<int>(10000, Allocator.Persistent);
        private EntityQuery _eq;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            for (int i = 0; i < 10000; i++)
            {
                walkableMap[i] = 1;
            }

            _eq = GetEntityQuery(typeof(Path));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            walkableMap.Dispose();
        }
        
        [BurstCompile]
        struct PathFindJob : IJobChunk
        {
            [ReadOnly] public NativeArray<int> walkableMap;

            public ComponentTypeHandle<Movement> CoordHandle;
            public BufferTypeHandle<Path> PathHandle;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var paths = chunk.GetBufferAccessor(PathHandle);

                for (int c = 0; c < chunk.Count; c++)
                {
                    var pathArray = AStarPathfinding.FindPath(new int2(0, 0), new int2(10, 10), walkableMap);
                    var path = paths[c];
                    path.Clear();
                    for (int i = 0; i < pathArray.Length; i++)
                    {
                        path.Add(pathArray[i]);
                    }

                    pathArray.Dispose();
                }
            }
        }
        
        protected override void OnUpdate()
        {
            var pathJob = new PathFindJob();
            pathJob.PathHandle = GetBufferTypeHandle<Path>();
            pathJob.walkableMap = walkableMap;

            Dependency = pathJob.ScheduleParallel(_eq, Dependency);
            
        }
    }
}