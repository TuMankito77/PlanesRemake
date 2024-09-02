namespace PlanesRemake.Runtime.Gameplay.CommonBehaviors
{
    using UnityEngine;

    public class DirectionalSpinning : MonoBehaviour
    {
        [SerializeField]
        private float spinningSpeed = 100;

        [SerializeField]
        private Vector3 rotatingDirection = Vector3.forward;

        #region Unity Methods

        void Update()
        {
            transform.Rotate(rotatingDirection, Time.deltaTime * spinningSpeed);
        }

        #endregion
    }
}
