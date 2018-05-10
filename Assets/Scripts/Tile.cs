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
    //private TileType type;
    private string type;
    private string text;
    private string specialCommand;
    private const float TileWidth = 0.05f;
    //public static readonly string[] colors = { "orange", "blue", "green", "red", "purple", "yellow" };
    //public enum TileType { Start = 0, Neutral, Good, Bad, Special, End };

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

    public void SetTileType(string tileType)
    {
        // sets the tile type
        type = tileType;
    }

    public string GetTileType()
    {
        return type;
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

    public string GetText()
    {
        // returns the text on the tile
        return text;
    }

    public void SetText(string tileText)
    {
        // sets the text on the tile
        text = tileText;
    }

    public string GetSpecialCommand()
    {
        // returns the special command as string
        return specialCommand;
    }

    public void SetSpecialCommand(string command)
    {
        // sets the special command
        specialCommand = command;
    }

    public void DisplayImage(string image)
    {
        // displays image on tile
        Material material = Resources.Load<Material>(GameData.GetMaterialsDir() + image);
        GetComponent<Renderer>().material = material;
    }

    private void Awake()
    {
        //next = new List<Tile>();
    }
}