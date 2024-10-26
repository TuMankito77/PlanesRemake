namespace PlanesRemake.Runtime.UiExternalAnimationModule
{
    using System;

    using UnityEngine;
    
    using PlanesRemake.Runtime.UI.CoreElements;
    using DG.Tweening;
    using TMPro;

    public class ViewAnimator : MonoBehaviour, IViewAnimator
    {
        [SerializeField]
        private RectTransform buttonsContainer = null;

        [SerializeField]
        private RectTransform buttonsContainerSize = null;

        [SerializeField]
        private Canvas canvas = null;

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
            Vector2 startPosition = GetStartPosition(buttonsContainer);
            Vector2 targetPosition = buttonsContainer.anchoredPosition;
            buttonsContainer.anchoredPosition = startPosition;
            PlayTweenAnimation(startPosition, targetPosition);
        }
        public void PlayTransitionOut()
        {
            tweenCallback += OnTransitionOutTweenAnimationCompleted;
            Vector2 startPosition = buttonsContainer.anchoredPosition;
            Vector2 targetPosition = GetStartPosition(buttonsContainer);
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
                buttonsContainer.anchoredPosition = startPosition + (direction * magnitude * transitionAnimaitonCurve.Evaluate(timeInAnimationCurve));
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

        //TODO: Improve this function by finding out how to transform the start position from screen coordinates to 
        //local coordinates within a rectangle. (For some reason the rect utility does no return the correct values)
        private Vector2 GetStartPosition(RectTransform rectTransform)
        {
            Rect screenRect = RectTransformUtility.PixelAdjustRect(rectTransform, canvas);
            Vector2 entranceDirection = new Vector2(horizontalEntranceDirection, verticalEntranceDirection).normalized;
            float horizontalStartPosition = rectTransform.anchoredPosition.x;
            float verticalStartPosition = rectTransform.anchoredPosition.y;

            if (entranceDirection.x < 0)
            {
                horizontalStartPosition = screenRect.position.x + ((Screen.width - screenRect.position.x + (screenRect.width/2)) * -entranceDirection.x);
            }
            else if (entranceDirection.x > 0)
            {
                horizontalStartPosition = screenRect.position.x + ((screenRect.position.x + (screenRect.width/2)) * -entranceDirection.x);
            }

            if (entranceDirection.y < 0)
            {
                verticalStartPosition = screenRect.position.y + ((Screen.height - screenRect.position.y + (screenRect.height/2)) * -entranceDirection.y);
            }
            else if (entranceDirection.y > 0)
            {
                verticalStartPosition = screenRect.position.y + ((screenRect.position.y + (screenRect.height/2)) * -entranceDirection.y);
            }

            Vector2 screenSpaceStartPosition = new Vector2(horizontalStartPosition, verticalStartPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(buttonsContainerSize, screenSpaceStartPosition, canvas.worldCamera, out Vector2 localStartPosition);
            
            if(entranceDirection.x == 0)
            {
                localStartPosition.x = rectTransform.anchoredPosition.x;
            }

            if(entranceDirection.y == 0)
            {
                localStartPosition.y = rectTransform.anchoredPosition.y;
            }

            return localStartPosition;
        }
    }
}
