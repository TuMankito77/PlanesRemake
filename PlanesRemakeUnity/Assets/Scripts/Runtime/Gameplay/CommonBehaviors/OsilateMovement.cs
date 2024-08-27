namespace PlanesRemake.Runtime.Gameplay.CommonBehaviors
{
    using UnityEngine;

    public class OsilateMovement : MonoBehaviour
    {
        [SerializeField]
        private float osilationDistance = 10;
        
        [SerializeField]
        private float speed = 1;

        private float timeTranscurred = 0;
        
        private void Update()
        {
            float position = Mathf.Sin(timeTranscurred);
            position *= osilationDistance;
            Vector3 newPosition = new Vector3(transform.position.x, position, transform.position.z);
            transform.position = newPosition;
            timeTranscurred += (Time.deltaTime * speed);
        }

        public void ChangeOsilationDistance(float newOsilationDistance)
        {
            osilationDistance = Mathf.Max(0, newOsilationDistance);
        }

        public void ChangeSpeed(float newSpeed)
        {
            osilationDistance = Mathf.Max(0, newSpeed);
        }
    }
}
