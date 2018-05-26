using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    private string landAnimKey;
    private string landAnimOption;
    private TileEffectInterface passAn;
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

    public TileEffectInterface GetPassAn()
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

    public void SetLandAnimKey(string inputKey)
    {
        landAnimKey = inputKey;
    }

    public void SetLandAnimOption(string inputOption)
    {
        landAnimOption = inputOption;
    }
    
    public string GetLandAnimKey()
    {
        return landAnimKey;
    }

    public string GetLandAnimOption()
    {
        return landAnimOption;
    }

    public void playEffect()
    {
        Dictionary<string, TileEffectInterface> effects = Board.GetTileEffects();
        if (effects.ContainsKey((string)landAnimKey))
        {
            effects[(string)landAnimKey].activateEffect(new Vector2(transform.position.x, transform.position.z), landAnimOption);
        }
        else
        {
            Debug.Log("No valid land effect");
            Debug.Log("Given Animation key is: " + landAnimKey);
            foreach (string key in effects.Keys)
            {
                Debug.Log(key);
            }
        }
        
    }

    public void hideEffect()
    {
        Dictionary<string, TileEffectInterface> effects = Board.GetTileEffects();
        if (effects.ContainsKey(landAnimKey))
        {
            effects[landAnimKey].hideEffect();

        }
        else
        {
            
        }        
    }

    public TileEffectInterface test()
    {
        Dictionary<string, TileEffectInterface> effects = Board.GetTileEffects();
        if (effects.ContainsKey(landAnimKey))
        {
            return effects[landAnimKey];
        }
        else
        {
            Debug.Log(landAnimKey + "doesn't exist");
            return null;
        }
    }

    private void Awake()
    {
        //next = new List<Tile>();
    }
}