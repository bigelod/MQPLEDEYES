using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class DisplayTextLog : MonoBehaviour
{
    public TMPro.TextMeshPro textMesh;

    /*
     *  Raw frame numbers for eye positions:
     *  0 - Blink
     *  1 - Open
     *  2 - N1
     *  3 - N2
     *  4 - N3
     *  5 - S1
     *  6 - S2
     *  7 - S3
     *  8 - W1
     *  9 - W2
     *  10 - W3
     *  11 - E1
     *  12 - E2
     *  13 - E3
     *  14 - NW1
     *  15 - NW2
     *  16 - NW3
     *  17 - NE1
     *  18 - NE2
     *  19 - NE3
     *  20 - SW1
     *  21 - SW2
     *  22 - SW3
     *  23 - SE1
     *  24 - SE2
     *  25 - SE3
     *  26 - V
     *  27 - V outline
     *  28 - R
     *  29 - R outline
     *  30 - O
     *  31 - O outline
     *  32 - N
     *  33 - N outline
    */

    public int leRaw = 0;
    public int reRaw = 0;

    public string LeftEye = "";
    public string RightEye = "";
    public string BlinkStatus = "";
    public string InputStatus = "";

    public bool VRMode = false;

    public float animTime = 1f;

    public int vrFrame = 0;
    public float vrTime = 1f;

    public string IP = "192.168.0.1";
    public int port = 10024;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ToggleVRMode()
    {
        VRMode = !VRMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (textMesh != null)
        {
            if (VRMode)
            {
                InputStatus = "VR MODE: ON";

                LeftEye = "L:";
                RightEye = "R:";

                switch (vrFrame)
                {
                    case 0:
                        //V R
                        leRaw = 26;
                        reRaw = 28;
                        LeftEye += "26";
                        RightEye += "28";
                        break;
                    case 1:
                        //V R (outline)
                        leRaw = 27;
                        reRaw = 29;
                        LeftEye += "27";
                        RightEye += "29";
                        break;
                    case 2:
                        //O N
                        leRaw = 30;
                        reRaw = 32;
                        LeftEye += "30";
                        RightEye += "32";
                        break;
                    case 3:
                        //O N (outline)
                        leRaw = 31;
                        reRaw = 33;
                        LeftEye += "31";
                        RightEye += "33";
                        break;
                }

                vrTime -= 1f * Time.deltaTime;

                if (vrTime <= 0)
                {
                    vrFrame += 1;
                    vrTime = animTime;

                    if (vrFrame > 3) vrFrame = 0;
                }
            }
            else
            {
                InputStatus = "VR MODE: OFF";
                vrFrame = 0;
                vrTime = animTime;
            }

            textMesh.text = LeftEye + System.Environment.NewLine + RightEye + System.Environment.NewLine + BlinkStatus + System.Environment.NewLine + InputStatus;

            SendNetworkMessage(leRaw.ToString() + "," + reRaw.ToString());
        }
    }

    public void SendNetworkMessage(string message)
    {
        try
        {
            //Debug.Log("Testing");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress broadcast = IPAddress.Parse(IP);
            //Debug.Log("Created Socket");
            byte[] sendbuf = Encoding.ASCII.GetBytes(message);
            IPEndPoint ep = new IPEndPoint(broadcast, port);

            s.SendTo(sendbuf, ep);

            //Debug.Log("Message Sent");
        }
        catch
        {

        }
    }
}
