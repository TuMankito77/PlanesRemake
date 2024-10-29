namespace PlanesRemake.Runtime.UiExternalAnimationModule
{
    using System;

    using UnityEngine;

    using PlanesRemake.Runtime.UI.CoreElements;
    using DG.Tweening;

    public class ViewAnimator : MonoBehaviour, IViewAnimator
    {
        [SerializeField]
        private RectTransform elementsContainerParent = null;

        [SerializeField]
        private RectTransform elementsContainer = null;

        [SerializeField, Min(0.01f)]
        private float animationDuration = 1f;

        [SerializeField]
        private AnimationCurve transitionAnimaitonCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField, Range(-1, 1)]
        private int horizontalEntranceDirection = 1;

        [SerializeField, Range(-1, 1)]
        private int verticalEntranceDirection = 0;

        private Tweener currentTween = null;
        private TweenCallback tweenCallback = null;
        private float timeInAnimationCurve = 0;

        #region Unity Methods

        private void OnDestroy()
        {
            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }
        }

        #endregion

        #region IViewAnimator

        public event Action OnTransitionInAnimationCompleted;
        public event Action OnTransitionOutAnimatonCompleted;

        public void PlayTransitionIn()
        {
            tweenCallback += OnTransitionInTweenAnimationCompleted;
            Vector2 startPosition = GetOffScreenPosition(elementsContainer);
            Vector2 targetPosition = elementsContainer.anchoredPosition;
            elementsContainer.anchoredPosition = startPosition;
            PlayTweenAnimation(startPosition, targetPosition);
        }
        public void PlayTransitionOut()
        {
            tweenCallback += OnTransitionOutTweenAnimationCompleted;
            Vector2 startPosition = elementsContainer.anchoredPosition;
            Vector2 targetPosition = GetOffScreenPosition(elementsContainer);
            PlayTweenAnimation(startPosition, targetPosition);
        }

        #endregion

        private void OnTransitionInTweenAnimationCompleted()
        {
            tweenCallback -= OnTransitionInTweenAnimationCompleted;
            OnTransitionInAnimationCompleted?.Invoke();
        }

        private void OnTransitionOutTweenAnimationCompleted()
        {
            tweenCallback -= OnTransitionOutTweenAnimationCompleted;
            OnTransitionOutAnimatonCompleted?.Invoke();
        }

        private void PlayTweenAnimation(Vector2 startPosition, Vector2 targetPosition)
        {
            float startValue = transitionAnimaitonCurve.keys[0].time;
            float targetValue = transitionAnimaitonCurve.keys[transitionAnimaitonCurve.keys.Length - 1].time;
            tweenCallback += OnPlayTweenAnimationFinished;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onComplete += tweenCallback;
            currentTween.onUpdate += () =>
            {
                float magnitude = Vector2.Distance(startPosition, targetPosition);
                Vector2 direction = (targetPosition - startPosition).normalized;
                elementsContainer.anchoredPosition = startPosition + (direction * magnitude * transitionAnimaitonCurve.Evaluate(timeInAnimationCurve));
            };
        }

        private void UpdateTimeInAnimationCurve(float time)
        {
            timeInAnimationCurve = time;
        }

        private void OnPlayTweenAnimationFinished()
        {
            tweenCallback -= OnPlayTweenAnimationFinished;
            timeInAnimationCurve = 0;
        }

        private Vector2 GetOffScreenPosition(RectTransform rectTransform)
        {
            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, elementsContainerParent);
            float horizontalStartPosition = rectTransform.anchoredPosition.x;
            float verticalStartPosition = rectTransform.anchoredPosition.y;

            if (horizontalEntranceDirection != 0)
            {
                float distanceToHorizontalCorner = Mathf.Abs(bounds.center.x - (horizontalEntranceDirection * elementsContainerParent.rect.width / 2));
                distanceToHorizontalCorner += rectTransform.rect.width / 2;
                horizontalStartPosition = horizontalStartPosition - (horizontalEntranceDirection * distanceToHorizontalCorner);
            }

            if (verticalEntranceDirection != 0)
            {
                float distanceToVerticalCorner = Mathf.Abs(bounds.center.y - (verticalEntranceDirection * elementsContainerParent.rect.height / 2));
                distanceToVerticalCorner += rectTransform.rect.height / 2;
                verticalStartPosition = verticalStartPosition - (verticalEntranceDirection * distanceToVerticalCorner);
            }

            return new Vector2(horizontalStartPosition, verticalStartPosition);
        }
    }
}
