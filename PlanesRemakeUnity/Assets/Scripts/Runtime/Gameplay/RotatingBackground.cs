namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;

    public class RotatingBackground : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float rotatingSpeed = 2;

        [SerializeField]
        private bool turnRight = false;

        private int direction = 1;

        #region Unity Methods

        private void Start()
        {
            direction = turnRight ? 1 : -1;
        }

        void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotatingSpeed * direction);        
        }

        #endregion
    }
}
