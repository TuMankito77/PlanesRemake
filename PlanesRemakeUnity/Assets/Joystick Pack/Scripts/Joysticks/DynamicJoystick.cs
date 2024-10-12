using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(Finger finger)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(finger.screenPosition);
        background.gameObject.SetActive(true);
        base.OnPointerDown(finger);
    }

    public override void OnPointerUp(Finger finger)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(finger);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}