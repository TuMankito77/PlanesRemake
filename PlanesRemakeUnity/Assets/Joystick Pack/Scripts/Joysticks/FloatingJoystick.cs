using UnityEngine.InputSystem.EnhancedTouch;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
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
}