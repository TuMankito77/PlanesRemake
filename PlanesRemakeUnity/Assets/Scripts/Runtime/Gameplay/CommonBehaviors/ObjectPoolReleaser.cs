namespace PlanesRemake.Runtime.Gameplay.CommonBehaviors
{
    using UnityEngine;
    
    using PlanesRemake.Runtime.Utils;

    public class ObjectPoolReleaser : MonoBehaviour
    {
        [SerializeField]
        private Vector2 cameraBoundariesOffset = Vector2.zero;

        [SerializeField]
        private bool checkHorizontalBoundaries = true;

        [SerializeField]
        private bool checkVerticalBoundaries = true;

        private BasePoolableObject poolableObject = null;
        private CameraExtensions.Boundaries? cameraBoundaries = null;

        #region Unity Region

        private void Awake()
        {
            poolableObject = GetComponent<BasePoolableObject>();
        }

        void Update()
        {
            CheckHorizontalContraints();
            CheckVerticalConstraints();
        }

        #endregion

        public void SetCameraBoundaries(CameraExtensions.Boundaries sourceCameraBoundaries)
        {
            cameraBoundaries = sourceCameraBoundaries;
        }

        public void CheckHorizontalContraints()
        {
            if (!checkHorizontalBoundaries || !cameraBoundaries.HasValue)
            {
                return;
            }

            if(transform.position.x > cameraBoundaries.Value.right + cameraBoundariesOffset.x ||
                transform.position.x < cameraBoundaries.Value.left - cameraBoundariesOffset.x)
            {
                poolableObject.ReleaseObject();
            }
        }

        public void CheckVerticalConstraints()
        {
            if(!checkVerticalBoundaries || !cameraBoundaries.HasValue)
            {
                return;
            }

            if(transform.position.y > cameraBoundaries.Value.top + cameraBoundariesOffset.y ||
                transform.position.y < cameraBoundaries.Value.bottom - cameraBoundariesOffset.y)
            {
                poolableObject.ReleaseObject();
            }
        }
    }
}
