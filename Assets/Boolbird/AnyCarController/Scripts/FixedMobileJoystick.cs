using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum MobileJoystickDirections
{
    Horizontal,
    Vertical,
    Both
}

public class FixedMobileJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public RectTransform mobileJoystickBase;

    [HideInInspector]
    public RectTransform handleObj;

    [HideInInspector]
    public MobileJoystickDirections mobileJoystickDirection = MobileJoystickDirections.Both;

    [HideInInspector]
    public float handleLimit = 0.5f;
    [HideInInspector]
    public Vector2 mobileJoystickInput = Vector2.zero;

    //Output

    [HideInInspector]
    public float Vertical { get { return mobileJoystickInput.y; } }
    [HideInInspector]
    public float Horizontal { get { return mobileJoystickInput.x; } }

    public void Start()
    {
        mobileJoystickBase = transform.GetComponent<RectTransform>();
        handleObj = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 myDirection = eventData.position - RectTransformUtility.WorldToScreenPoint(new Camera(), mobileJoystickBase.position);
        mobileJoystickInput = (myDirection.magnitude > mobileJoystickBase.sizeDelta.x / 2f) ? myDirection.normalized :
            myDirection / (mobileJoystickBase.sizeDelta.x / 2f);

        if (mobileJoystickDirection == MobileJoystickDirections.Horizontal)
        {
            mobileJoystickInput = new Vector2(mobileJoystickInput.x, 0);
        }
        if (mobileJoystickDirection == MobileJoystickDirections.Vertical)
        {
            mobileJoystickInput = new Vector2(0, mobileJoystickInput.y);
        }

        handleObj.anchoredPosition = (mobileJoystickInput * mobileJoystickBase.sizeDelta.x / 2f) * handleLimit;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mobileJoystickInput = Vector2.zero;
        handleObj.anchoredPosition = Vector2.zero;
    }
}
