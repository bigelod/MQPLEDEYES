using System.Collections.Generic;
using UnityEngine;

public class ReadBlendShapes : MonoBehaviour
{
    public SkinnedMeshRenderer sourceMesh;
    public DisplayTextLog displayText;

    public SimEyeAnim EyeL;
    public SimEyeAnim EyeR;

    private List<blndShp> blndShps;

    public blndShp closedL; //12
    public blndShp closedR; //13
    public blndShp lookDownL; //14
    public blndShp lookDownR; //15
    public blndShp lookLeftL; //16
    public blndShp lookLeftR; //17
    public blndShp lookRightL; //18
    public blndShp lookRightR; //19
    public blndShp lookUpL; //20
    public blndShp lookUpR; //21

    public float animTime = 1f;

    public int vrFrame = 0;
    public float vrTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        blndShps = new List<blndShp>();
        blndShps.Add(closedL);
        blndShps.Add(closedR);
        blndShps.Add(lookDownL);
        blndShps.Add(lookDownR);
        blndShps.Add(lookLeftL);
        blndShps.Add(lookLeftR);
        blndShps.Add(lookRightL);
        blndShps.Add(lookRightR);
        blndShps.Add(lookUpL);
        blndShps.Add(lookUpR);
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceMesh != null && displayText != null)
        {
            foreach (blndShp shp in blndShps)
            {
                try
                {
                    float weight = sourceMesh.GetBlendShapeWeight(shp.index);

                    shp.rawVal = weight;

                    if (weight >= shp.maxWeight)
                    {
                        shp.currVal = 3;
                    }
                    else if (weight >= shp.midWeight)
                    {
                        shp.currVal = 2;
                    }
                    else if (weight >= shp.minWeight)
                    {
                        shp.currVal = 1;
                    }
                    else
                    {
                        shp.currVal = 0;
                    }
                }
                catch
                {

                }
            }

            //Map to frames
            int lEye = CalcEyeFrame(lookUpL, lookDownL, lookLeftL, lookRightL);
            int rEye = CalcEyeFrame(lookUpR, lookDownR, lookLeftR, lookRightR);
            string lBlink = "OPEN (" + closedL.rawVal.ToString() + ")";
            string rBlink = "OPEN (" + closedR.rawVal.ToString() + ")";


            //Closed eyes take priority otherwise
            if (closedL.currVal > 0)
            {
                lEye = 0;
                lBlink = "CLOSED (" + closedL.rawVal.ToString() + ")";
            }

            if (closedR.currVal > 0)
            {
                rEye = 0;
                rBlink = "CLOSED (" + closedR.rawVal.ToString() + ")";
            }

            //Update raw frame values
            displayText.leRaw = lEye;
            displayText.reRaw = rEye;

            //Update display text
            displayText.LeftEye = "L:" + lEye.ToString() + " (" + lookLeftL.rawVal.ToString() + "," + lookRightL.rawVal.ToString() + "," + lookUpL.rawVal.ToString() + "," + lookDownL.rawVal.ToString() + ")";
            displayText.RightEye = "R:" + rEye.ToString() + " (" + lookLeftR.rawVal.ToString() + "," + lookRightR.rawVal.ToString() + "," + lookUpR.rawVal.ToString() + "," + lookDownR.rawVal.ToString() + ")";
            displayText.BlinkStatus = lBlink + "," + rBlink;

            //Inject VR Mode to sim display
            if (displayText.VRMode)
            {
                switch (vrFrame)
                {
                    case 0:
                        //V R
                        lEye = 26;
                        rEye = 28;
                        break;
                    case 1:
                        //V R (outline)
                        lEye = 27;
                        rEye = 29;
                        break;
                    case 2:
                        //O N
                        lEye = 30;
                        rEye = 32;
                        break;
                    case 3:
                        //O N (outline)
                        lEye = 31;
                        rEye = 33;
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
                vrFrame = 0;
                vrTime = animTime;
            }

            //Update sim display
            if (EyeL != null) EyeL.newMat = lEye;
            if (EyeR != null) EyeR.newMat = rEye;

        }
    }

    private int CalcEyeFrame(blndShp N, blndShp S, blndShp W, blndShp E)
    {
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

        if (N.currVal == S.currVal && E.currVal == W.currVal)
        {
            return 1;
        }
        else if (N.currVal >= S.currVal)
        {
            if (E.currVal > W.currVal)
            {
                if (E.currVal > 0)
                {
                    if (N.currVal > 0)
                    {
                        blndShp strongest = PickShp(E, N);

                        return 16 + strongest.currVal;
                    }
                    else
                    {
                        return 10 + E.currVal;
                    }
                }
                else
                {
                    return 1 + N.currVal;
                }
            }
            else
            {
                if (W.currVal > 0)
                {
                    if (N.currVal > 0)
                    {
                        blndShp strongest = PickShp(W, N);
                        return 13 + strongest.currVal;
                    }
                    else
                    {
                        return 7 + W.currVal;
                    }
                }
                else
                {
                    return 1 + N.currVal;
                }
            }
        }
        else
        {
            if (E.currVal > W.currVal)
            {
                if (E.currVal > 0)
                {
                    if (S.currVal > 0)
                    {
                        blndShp strongest = PickShp(E, S);
                        return 22 + strongest.currVal;
                    }
                    else
                    {
                        return 10 + E.currVal;
                    }
                }
                else
                {
                    return 4 + S.currVal;
                }
            }
            else
            {
                if (W.currVal > 0)
                {
                    if (S.currVal > 0)
                    {
                        blndShp strongest = PickShp(W, S);
                        return 19 + strongest.currVal;
                    }
                    else
                    {
                        return 7 + W.currVal;
                    }
                }
                else
                {
                    return 4 + S.currVal;
                }
            }
        }
    }

    private blndShp PickShp(blndShp A, blndShp B)
    {
        if (A != null && B != null)
        {
            if (A.currVal >= B.currVal)
            {
                return A;
            }
            else
            {
                return B;
            }
        }

        return new blndShp();
    }
}

[System.Serializable]
public class blndShp
{
    public int index = 0;
    public float minWeight = 25f;
    public float midWeight = 50f;
    public float maxWeight = 75f;
    public float rawVal = 0f;
    public int currVal = 0;
}
