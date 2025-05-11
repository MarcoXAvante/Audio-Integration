using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSurfaceDetector : MonoBehaviour
{
    public string currentSurfaceTag = "";

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wood") || other.CompareTag("Stone"))
        {
            currentSurfaceTag = other.tag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == currentSurfaceTag)
        {
            currentSurfaceTag = "";
        }
    }
}
