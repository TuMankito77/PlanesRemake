namespace PlanesRemake.Runtime.Gameplay
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Gameplay.CommonBehaviors;

    [RequireComponent(typeof(DirectionalMovement), typeof(OsilateMovement))]
    public class Obstacle : MonoBehaviour
    {
        private DirectionalMovement directionalMovement = null;
        private OsilateMovement osilateMovement = null;

        #region Unity Methods

        private void Awake()
        {
            directionalMovement = GetComponent<DirectionalMovement>();
            osilateMovement = GetComponent<OsilateMovement>();
        }

        #endregion

        //NOTE: We can create a container that has all of this information so that it gets passed in
        //and we do not have this long list of parameters 
        public void Initialize(float movementSpeed, float osilationSpeed, float osilationDistance, Vector3 startingPosition, Quaternion startingRotation)
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Vector3 velocityVector = new Vector3(-movementSpeed, 0, 0);
            directionalMovement.ChangeVelocityVector(velocityVector);
            osilateMovement.ChangeOsilationDistance(osilationDistance);
            osilateMovement.ChangeSpeed(osilationSpeed);
        }
    }
}
