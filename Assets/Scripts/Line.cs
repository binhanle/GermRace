using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Line : VolumetricLines.VolumetricLineBehavior
{
    void OnMouseOver()
    {
        // make line shine when mouse is over it
        LightSaberFactor = 0.8f;
    }

    void OnMouseExit()
    {
        // make line normal when mouse is not over it
        LightSaberFactor = 1;
    }
}
