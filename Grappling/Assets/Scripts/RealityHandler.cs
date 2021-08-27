using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityHandler : MonoBehaviour
{
    public static int globalDimension = 1;
    public int dimensionNum = 2;

    private List<GameObject> switchingObjects = new List<GameObject>();

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject o in allObjects)
        {
            if (o.GetComponent<SetDimension>() != null)
            {
                switchingObjects.Add(o);
                if (o.GetComponent<SetDimension>().dimension == globalDimension) o.SetActive(true);
                else o.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            globalDimension++;
            if (globalDimension > dimensionNum) globalDimension = 1;
            //Debug.Log("Dimension: " + globalDimension);

            //Hide/Unhide the appropriate objects
            foreach (GameObject o in switchingObjects)
            {
                if (o.GetComponent<SetDimension>().dimension == globalDimension) o.SetActive(true);
                else o.SetActive(false);
            }
        }
    }
}
