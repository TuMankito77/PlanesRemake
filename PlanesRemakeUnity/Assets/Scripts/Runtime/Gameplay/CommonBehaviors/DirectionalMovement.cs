namespace PlanesRemake.Runtime.Gameplay.CommonBehaviors
{
    using UnityEngine;

    public class DirectionalMovement : MonoBehaviour
    {
        [SerializeField]
        private Vector3 velocityVector = Vector3.zero;

        private void Update()
        {
            Vector3 newPosition = transform.position + (velocityVector * Time.deltaTime);
            transform.position = newPosition;
        }

        public void ChangeVelocityVector(Vector3 newVelocityVector)
        {
            velocityVector = newVelocityVector;
        }
    }
}
