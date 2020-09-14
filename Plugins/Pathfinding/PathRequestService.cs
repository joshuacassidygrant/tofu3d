using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TofuConfig;
using TofuCore.Events;
using TofuCore.Service;
using UnityEngine;

namespace TofuPlugin.Pathfinding
{
    public class PathRequestService : AbstractService
    {
        Queue<PathResult> results = new Queue<PathResult>();
#pragma warning disable 649
        [Dependency] private Pathfinder _pathfinder;
#pragma warning restore 649

        public override void Initialize()
        {
            base.Initialize();
            BindListener(EventKey.FrameUpdate, OnFrameUpdate, _eventContext);
        }

        public void RequestPath(PathRequest request)
        {
            ThreadStart threadStart = delegate { _pathfinder.FindPath(request, FinishProcessingPath); };
            threadStart.Invoke();
        }

        void OnFrameUpdate(EventPayload payload)
        {
            if (results.Count > 0)
            {
                int itemsInQueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < itemsInQueue; i++)
                    {
                        PathResult result = results.Dequeue();
                        result.Callback(result.Path, result.Success);
                    }
                }
            }
        }

        public void FinishProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }
    }

    public struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
        {
            this.pathStart = start;
            this.pathEnd = end;
            this.callback = callback;

        }
    }

    public struct PathResult
    {
        public Vector3[] Path;
        public bool Success;
        public Action<Vector3[], bool> Callback;

        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
        {
            Path = path;
            Success = success;
            Callback = callback;
        }

    }
}
