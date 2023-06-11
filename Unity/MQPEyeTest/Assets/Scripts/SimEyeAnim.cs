using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimEyeAnim : MonoBehaviour
{
    public MeshRenderer myRenderer;

    public List<Material> materialList = new List<Material>();

    public int newMat = 0;
    public int currMat = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currMat != newMat)
        {
            if (newMat < materialList.Count)
            {
                currMat = newMat;

                if (myRenderer != null)
                {
                    myRenderer.material = materialList[currMat];
                }
            }
        }
    }
}
