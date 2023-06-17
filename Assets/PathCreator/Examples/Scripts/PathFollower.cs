using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        bool sub = false;
        Vector3 startPos;


        private void Start()
        {
            startPos = transform.position;
        }
        void Update()
        {
            if (pathCreator != null && sub)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }


        public void addPath()
        {
            if (pathCreator != null)
            {
                sub = true;
                var emission = FindObjectOfType<ParticleSystem>().emission;
                emission.enabled = true;
                pathCreator.pathUpdated += OnPathChanged;
            }
        }
        public void removePath()
        {
            
            sub = false;
            var emission = FindObjectOfType<ParticleSystem>().emission;
            emission.enabled = false;
            distanceTravelled = 0;
            transform.position = startPos;
            pathCreator.pathUpdated -= OnPathChanged;
        }
    }
}