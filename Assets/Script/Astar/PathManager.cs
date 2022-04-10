using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Astar
{

    public class PathManager : MonoBehaviour
    {
        static Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static PathManager instance;
        PathFinding pathFinding;
        bool isProcessingPath;

        void Awake()
        {
            instance = this;
            pathFinding = GetComponent<PathFinding>();
        }

        public static void Request(Vector3 start, Vector3 end, bool smootherPath, Action<List<Vector3>, bool> callBack)
        {
            var newPathRequest = new PathRequest(start, end, callBack);
            pathRequests.Enqueue(newPathRequest);
            instance.tryProcesNext(smootherPath);
        }

        void tryProcesNext(bool smootherPath)
        {
            if(!isProcessingPath && pathRequests.Count > 0)
            {
                currentPathRequest = pathRequests.Dequeue();
                isProcessingPath = true;
                var result = pathFinding.findPath(currentPathRequest.start, currentPathRequest.end, smootherPath);
                isProcessingPath = false;

                currentPathRequest.callBack(result, result==null?false:true);
                if (result == null)
                {
                    currentPathRequest.callBack(null, false);
                }
                else
                {
                    currentPathRequest.callBack(result, true);
                }
                
                instance.tryProcesNext(true);
            }
        }

        public class PathRequest
        {
            public Vector3 start;
            public Vector3 end;
            public Action<List<Vector3>, bool> callBack;


            public PathRequest(Vector3 _start,Vector3 _end, Action<List<Vector3>, bool> _callBack)
            {
                this.start = _start;
                this.end = _end;
                this.callBack = _callBack;
            }

        }
    }
}
