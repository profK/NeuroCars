using UnityEngine;
using UnityEngine.UI;

public class DigitalSpeedometer : MonoBehaviour
{
    [HideInInspector]
    public AnyCarController ACC;

    [HideInInspector]
    public Transform speedType;
    [HideInInspector]
    public Transform currentGear;
    [HideInInspector]
    public Transform lightsImg;
    [HideInInspector]
    public Transform speedNumber;
    [HideInInspector]
    public Transform RPMTicks;

    void Start()
    {
        var rootParent = transform.parent;
        ACC = rootParent.parent.GetComponent<AnyCarController>();

        speedType = transform.GetChild(4);
        currentGear = transform.GetChild(6);
        lightsImg = transform.GetChild(5);
        speedNumber = transform.GetChild(7);
        RPMTicks = transform.GetChild(8);
    }
    void Update()
    {
        speedNumber.GetComponent<Text>().text = ACC.currentSpeed.ToString("0");

        RPMTicksManager();
    }

    public void RPMTicksManager()
    {
        if (ACC.RPM < 0.2)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(false);
            RPMTicks.GetChild(2).gameObject.SetActive(false);
            RPMTicks.GetChild(3).gameObject.SetActive(false);
            RPMTicks.GetChild(4).gameObject.SetActive(false);
            RPMTicks.GetChild(5).gameObject.SetActive(false);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.2 && ACC.RPM < 0.3)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(false);
            RPMTicks.GetChild(3).gameObject.SetActive(false);
            RPMTicks.GetChild(4).gameObject.SetActive(false);
            RPMTicks.GetChild(5).gameObject.SetActive(false);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.3 && ACC.RPM < 0.4)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(false);
            RPMTicks.GetChild(4).gameObject.SetActive(false);
            RPMTicks.GetChild(5).gameObject.SetActive(false);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.4 && ACC.RPM < 0.5)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(false);
            RPMTicks.GetChild(5).gameObject.SetActive(false);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.5 && ACC.RPM < 0.6)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(true);
            RPMTicks.GetChild(5).gameObject.SetActive(false);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.6 && ACC.RPM < 0.7)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(true);
            RPMTicks.GetChild(5).gameObject.SetActive(true);
            RPMTicks.GetChild(6).gameObject.SetActive(false);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.7 && ACC.RPM < 0.8)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(true);
            RPMTicks.GetChild(5).gameObject.SetActive(true);
            RPMTicks.GetChild(6).gameObject.SetActive(true);
            RPMTicks.GetChild(7).gameObject.SetActive(false);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.8 && ACC.RPM < 0.9)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(true);
            RPMTicks.GetChild(5).gameObject.SetActive(true);
            RPMTicks.GetChild(6).gameObject.SetActive(true);
            RPMTicks.GetChild(7).gameObject.SetActive(true);
            RPMTicks.GetChild(8).gameObject.SetActive(false);
        }
        else if (ACC.RPM > 0.9)
        {
            RPMTicks.GetChild(0).gameObject.SetActive(true);
            RPMTicks.GetChild(1).gameObject.SetActive(true);
            RPMTicks.GetChild(2).gameObject.SetActive(true);
            RPMTicks.GetChild(3).gameObject.SetActive(true);
            RPMTicks.GetChild(4).gameObject.SetActive(true);
            RPMTicks.GetChild(5).gameObject.SetActive(true);
            RPMTicks.GetChild(6).gameObject.SetActive(true);
            RPMTicks.GetChild(7).gameObject.SetActive(true);
            RPMTicks.GetChild(8).gameObject.SetActive(true);
        }
    }
}
