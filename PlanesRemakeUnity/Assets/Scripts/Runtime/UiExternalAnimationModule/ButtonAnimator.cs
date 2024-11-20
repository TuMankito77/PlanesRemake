namespace PlanesRemake.Runtime.UiExternalAnimationModule
{
    using System;
    
    using UnityEngine;

    using DG.Tweening;

    using PlanesRemake.Runtime.UI.CoreElements;
    using UnityEngine.InputSystem.EnhancedTouch;

    public class ButtonAnimator : MonoBehaviour, ISeletableElementAnimator
    {
        private event Action onSubmitAnimationStart = null;
        private event Action onSubmitAnimationEnd = null;

        [SerializeField]
        private float animationDuration = 0.25f;

        [SerializeField]
        private AnimationCurve selectAnimationCurve = AnimationCurve.Linear(0, 1, 1, 1.2f);

        [SerializeField]
        private AnimationCurve submitAnimationCurve = AnimationCurve.Linear(0, 0.5f, 1, 1);

        [SerializeField]
        private AnimationCurve submitAnimationWhenTouchEnabled = AnimationCurve.Linear(0, 1, 1, 1);

        private bool isDoingSubmitAnimation = false;
        private Tweener currentTween = null;
        private TweenCallback TweenCompletedCallback = null;
        private BaseButton baseButton = null;
        private float timeInAnimationCurve = 0;
        private AnimationCurve submitAnimationCurveChosen = null;

        #region ISelectableElementAnimator

        public event Action OnSubmitAnimationStart
        {
            add => onSubmitAnimationStart += value;
            remove => onSubmitAnimationStart -= value;
        }

        public event Action OnSubmitAnimationEnd
        {
            add => onSubmitAnimationEnd += value;
            remove => onSubmitAnimationEnd -= value;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            baseButton = GetComponent<BaseButton>();
            baseButton.SetAnimationExternalModule(this);
            
            if(EnhancedTouchSupport.enabled)
            {
                submitAnimationCurveChosen = submitAnimationWhenTouchEnabled;
            }
            else
            {
                baseButton.onSelect += OnElementSelected;
                baseButton.onDeselect += OnElementDeselected;
                submitAnimationCurveChosen = submitAnimationCurve;
            }

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

            onSubmitAnimationStart?.Invoke();

            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }

            isDoingSubmitAnimation = true;
            float startValue = submitAnimationCurveChosen.keys[0].time;
            float targetValue = submitAnimationCurveChosen.keys[submitAnimationCurveChosen.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * submitAnimationCurveChosen.Evaluate(timeInAnimationCurve);
            currentTween.onComplete += OnTweenAnimationCompleted;
        }

        private void OnTweenAnimationCompleted()
        {
            isDoingSubmitAnimation = false;
            transform.localScale = Vector3.one;
            onSubmitAnimationEnd?.Invoke();
            currentTween.onComplete -= OnTweenAnimationCompleted;
        }

        private void UpdateTimeInAnimationCurve(float time)
        {
            timeInAnimationCurve = time;
        }
    }
}
