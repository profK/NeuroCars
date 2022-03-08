using UnityEngine;
using UnityEngine.UI;

public class AnalogSpeedometer : MonoBehaviour
{
    [HideInInspector]
    public AnyCarController ACC;

    private float minNeedleAngle = 30f;
    private float maxNeedleAngle = -210f;

    private Transform needleObj;
    [HideInInspector]
    public Transform speedType;
    [HideInInspector]
    public Transform currentGear;
    [HideInInspector]
    public Transform lightsImg;

    void Start()
    {
        var rootParent = transform.parent;
        ACC = rootParent.parent.GetComponent<AnyCarController>();

        needleObj = transform.GetChild(7);
        speedType = transform.GetChild(4);
        currentGear = transform.GetChild(6);
        lightsImg = transform.GetChild(5);
    }
    void Update()
    {
        needleObj.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, GetSpeedRotation());

        if (ACC.RPM > 0.97f)
        {
            currentGear.GetComponent<Text>().color = Color.red;
        }
        else
        {
            currentGear.GetComponent<Text>().color = Color.white;
        }
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = minNeedleAngle - maxNeedleAngle;
        float normalizedSpeed = ACC.currentSpeed / 240;

        float finalSpeedRotation = minNeedleAngle - normalizedSpeed * totalAngleSize;

        return finalSpeedRotation;
    }
}
