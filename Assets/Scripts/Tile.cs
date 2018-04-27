using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Tile : MonoBehaviour
{
    private Animation landAn;
    private Animation passAn;
    private List<Tile> next = new List<Tile>();
    private Tile landNext;
    private Type tileType;
    private const float TileWidth = 0.05f;
    public static readonly string[] colors = { "orange", "blue", "green", "red", "purple", "yellow" };
    public enum Type { Start = 0, Neutral, Good, Bad, Special, End };

    public void SetPosition(float x, float y)
    {
        // sets the position
        transform.position = new Vector3(x, -TileWidth, y);
    }

    public Vector2 GetPosition()
    {
        // gets the position
        Vector3 position3d = gameObject.transform.position;
        return new Vector2(position3d.x, position3d.z);
    }

    public void SetColor(string colorString)
    {
        // sets the color
        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        GetComponent<Renderer>().material.color = color;
    }

    public void SetTileType(Type type)
    {
        // sets the tile type
        tileType = type;
    }

    /*public Animation getLandAn()
    {
        return landAn;
    }*/

    public Animation GetPassAn()
    {
        // runs pass animation
        return passAn;
    }

    public bool HasNext()
    {
        // returns true if next tile exists, false otherwise
        return next.Count > 0;
    }

    public List<Tile> GetNext()
    {
        // returns possible next tiles
        return next;
    }

    public void AddNext(Tile nextTile)
    {
        // adds possible next tile
        next.Add(nextTile);
    }

    public bool HasLandNext()
    {
        // returns true if jump to tile exists, false otherwise
        return landNext != null;
    }

    public Tile GetLandNext()
    {
        // (for special tiles only) returns next tile if landed upon
        return landNext;
    }

    public void SetLandNext(Tile jumpToTile)
    {
        // (for special tiles only) sets next tile if landed upon
        landNext = jumpToTile;
    }

    private void Awake()
    {
        //next = new List<Tile>();
    }
}