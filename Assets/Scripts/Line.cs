using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Line : VolumetricLines.VolumetricLineBehavior
{
    public void SetStartAndEnd(Vector2 start, Vector2 end)
    {
        // set the start and end points of the line
        // get the distance
        float distance = Vector2.Distance(start, end);

        // get the midpoint
        Vector2 midpoint = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);

        // get the angle
        float angle = Mathf.Atan2(end.x - start.x, end.y - start.y);

        // set them all
        transform.localScale = new Vector3(distance, 1, 1);
        transform.position = new Vector3(midpoint.x, 0, midpoint.y);
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

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
