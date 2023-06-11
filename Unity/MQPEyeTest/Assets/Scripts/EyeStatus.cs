using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeStatus : MonoBehaviour
{
    public bool isLeftEye = false;
    public bool isRightEye = false;

    public DisplayTextLog displayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (displayText != null)
        {
            if (isLeftEye)
            {
                displayText.LeftEye = "Left Eye: " + transform.localEulerAngles.ToString() + " World: " + transform.eulerAngles.ToString();
            }
            else if (isRightEye)
            {
                displayText.RightEye = "Right Eye: " + transform.localEulerAngles.ToString() + " World: " + transform.eulerAngles.ToString();
            }
        }
    }
}
