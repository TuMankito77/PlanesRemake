namespace PlanesRemake.Runtime.UiExternalAnimationModule
{
    using System;
    
    using UnityEngine;

    using DG.Tweening;
    using DG.Tweening.Core;
    using DG.Tweening.Plugins.Options;

    using PlanesRemake.Runtime.UI.CoreElements;

    public class ButtonAnimator : MonoBehaviour, ISeletableElementAnimator
    {
        private event Action OnSubmitAnimationFinished = null;

        [SerializeField]
        private float selectScaleSize = 1.2f;

        [SerializeField]
        private float animationDuration = 0.25f;

        private bool isDoingSubmitAnimation = false;
        private TweenerCore<Vector3, Vector3, VectorOptions> currentTween = null;
        private TweenCallback TweenCompletedCallback = null;
        private BaseButton baseButton = null;

        #region ISelectableElementAnimator

        public event Action OnSubmitAnimationCompleted
        {
            add => OnSubmitAnimationFinished += value;
            remove => OnSubmitAnimationFinished -= value;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            baseButton = GetComponent<BaseButton>();
            baseButton.SetAnimationExternalModule(this);
            baseButton.onSelect += OnElementSelected;
            baseButton.onDeselect += OnElementDeselected;
            baseButton.onSubmit += OnElementSubmit;
            TweenCompletedCallback += OnTweenAnimationCompleted;
        }

        private void OnDestroy()
        {
            baseButton.onSelect -= OnElementSelected;
            baseButton.onDeselect -= OnElementDeselected;
            baseButton.onSubmit -= OnElementSubmit;
            TweenCompletedCallback -= OnTweenAnimationCompleted;

            if(currentTween.IsActive())
            {
                currentTween.Kill();
            }
        }

        #endregion

        private void OnElementSelected()
        {
            if(isDoingSubmitAnimation)
            {
                return;
            }

            if(currentTween.IsActive())
            {
                currentTween.Kill();
            }

            currentTween = transform.DOScale(Vector3.one * selectScaleSize, animationDuration);
        }

        private void OnElementDeselected()
        {
            if (isDoingSubmitAnimation)
            {
                return;
            }

            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }

            currentTween = transform.DOScale(Vector3.one, animationDuration);
        }

        private void OnElementSubmit()
        {
            if(isDoingSubmitAnimation)
            {
                return;
            }

            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }

            isDoingSubmitAnimation = true;
            currentTween = transform.DOScale(Vector3.one, animationDuration);
            currentTween.onComplete += OnTweenAnimationCompleted;
        }

        private void OnTweenAnimationCompleted()
        {
            isDoingSubmitAnimation = false;
            OnSubmitAnimationFinished?.Invoke();
            currentTween.onComplete -= OnTweenAnimationCompleted;
        }
    }
}
