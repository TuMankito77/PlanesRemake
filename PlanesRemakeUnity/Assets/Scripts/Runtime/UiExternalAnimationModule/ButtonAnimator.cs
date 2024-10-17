namespace PlanesRemake.Runtime.UiExternalAnimationModule
{
    using System;
    
    using UnityEngine;

    using DG.Tweening;

    using PlanesRemake.Runtime.UI.CoreElements;

    public class ButtonAnimator : MonoBehaviour, ISeletableElementAnimator
    {
        private event Action OnSubmitAnimationFinished = null;

        [SerializeField]
        private float animationDuration = 0.25f;

        [SerializeField]
        private AnimationCurve selectAnimationCurve = AnimationCurve.Linear(0, 1, 1, 1.2f);

        [SerializeField]
        private AnimationCurve submitAnimationCurve = AnimationCurve.Linear(0, 0.5f, 1, 1);

        private bool isDoingSubmitAnimation = false;
        private Tweener currentTween = null;
        private TweenCallback TweenCompletedCallback = null;
        private BaseButton baseButton = null;
        private float timeInAnimationCurve = 0;

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

            float startValue = selectAnimationCurve.keys[0].time;
            float targetValue = selectAnimationCurve.keys[selectAnimationCurve.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * selectAnimationCurve.Evaluate(timeInAnimationCurve);
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

            float startValue = selectAnimationCurve.keys[selectAnimationCurve.keys.Length - 1].time;
            float targetValue = selectAnimationCurve.keys[0].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * selectAnimationCurve.Evaluate(timeInAnimationCurve);
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
            float startValue = submitAnimationCurve.keys[0].time;
            float targetValue = submitAnimationCurve.keys[selectAnimationCurve.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * submitAnimationCurve.Evaluate(timeInAnimationCurve);
            currentTween.onComplete += OnTweenAnimationCompleted;
        }

        private void OnTweenAnimationCompleted()
        {
            isDoingSubmitAnimation = false;
            transform.localScale = Vector3.one;
            OnSubmitAnimationFinished?.Invoke();
            currentTween.onComplete -= OnTweenAnimationCompleted;
        }

        private void UpdateTimeInAnimationCurve(float time)
        {
            timeInAnimationCurve = time;
        }
    }
}
