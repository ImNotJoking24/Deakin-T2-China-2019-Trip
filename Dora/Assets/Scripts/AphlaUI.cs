using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For the Unity UI components

public class AphlaUI : MonoBehaviour
{
    public float AlphaThreshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    
}
