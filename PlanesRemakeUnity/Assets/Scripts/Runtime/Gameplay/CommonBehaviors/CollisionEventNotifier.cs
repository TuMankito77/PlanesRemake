namespace PlanesRemake.Runtime.Gameplay.CommonBehaviors
{
    using System;

    using UnityEngine;

    public class CollisionEventNotifier : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterDetected = null;
        public event Action<Collision> OnCollisionExitDetected = null;
        public event Action<Collider> OnTiggerEnterDetected = null;
        public event Action<Collider> OnTriggerExitDetected = null;

        #region Unity Methods

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterDetected?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnCollisionExitDetected?.Invoke(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTiggerEnterDetected?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitDetected?.Invoke(other);
        }

        #endregion
    }
}
