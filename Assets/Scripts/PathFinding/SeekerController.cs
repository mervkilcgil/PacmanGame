using UnityEngine;

namespace Astar2DPathFinding.Mika
{
    [RequireComponent(typeof(CountPath))]
    public class SeekerController : MonoBehaviour
    {
        private CountPath counter;

        void Start()
        {
            counter = GetComponent<CountPath>();
        }

        public void AddTarget(Vector2 target)
        {
            counter.FindPath(transform, target);
        }
    }
}