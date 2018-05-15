using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Line : VolumetricLines.VolumetricLineBehavior
{
    private Move move;

    public void SetMove(Move lineMove)
    {
        // set the move highlighted by the line
        move = lineMove;
    }

    public void SetStartAndEnd(Vector2 start, Vector2 end)
    {
        // set the start and end points of the line
        // get the distance
        float distance = Vector2.Distance(start, end);

        // get the midpoint
        Vector2 midpoint = new Vector2((start.x + end.x) / 2, (start.y + end.y) / 2);

        // get the angle
        float angle = -Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

        // get the line width
        float width = 0.5f / distance;

        // set them all
        transform.localScale = new Vector3(distance, 1, 1);
        transform.position = new Vector3(midpoint.x, 0, midpoint.y);
        transform.eulerAngles = new Vector3(0, angle, 0);
        LineWidth = width;
    }

    void OnMouseOver()
    {
        // make line shine when mouse is over it
        LightSaberFactor = 0.5f;
        GameGUI.PreviewMessageScreen(move.GetDestTile());
        
    }

    void OnMouseExit()
    {
        // make line normal when mouse is not over it
        LightSaberFactor = 1;
        GameGUI.HideMessageScreen();
    }

    void OnMouseDown()
    {
        // make move when line is clicked
        // destroy the lines and outlines
        GameData.GetCurrPlayer().DestroyLegalMoves();

        // start walking
        move.GetPiece().WalkToTile(move.GetDestTile());
    }
}
