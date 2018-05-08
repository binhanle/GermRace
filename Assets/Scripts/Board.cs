using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private Player[] players;
    //private List<Tile> tiles;
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    //private int turn;
    //private const string startTileName = "start";
    private int currPlayerIndex;
    
    public Player[] currentPlayer()
    {
        // returns player whose turn it is
        return players;
    }
    
    /*public void setTiles(Tile[] newTiles)
    {
        // sets Tiles array for this board based on array
        tiles = newTiles;
    }*/
    
    public void setPlayers(Player[] newPlayers)
    {
        // sets Players array for this board based on array
        players = newPlayers;
    }

    public Tile GetStartTile()
    {
        return tiles["start"];
    }

    public void LoadTiles()
    {
        // load the tiles xml file
        XDocument xmlDoc = XDocument.Load(GameData.GetTilesPath());
        IEnumerable<XElement> items = xmlDoc.Descendants("tiles").Elements();
        foreach (var item in items)
        {
            // create the tile
            GameObject tileObject = Instantiate((GameObject)Resources.Load("Prefabs/Tile", typeof(GameObject)));
            Tile tile = tileObject.GetComponent<Tile>();
            string name = item.Element("name").Value.Trim();
            //string color = item.Element("color").Value.Trim();
            string type = item.Element("type").Value.Trim();
            string text = item.Element("text").Value.Trim();
            /*if (item.Element("nextTile") != null)
            {
                string nextTile = item.Element("nextTile").Value.Trim();
            }
            if (item.Element("jumpToTile") != null)
            {
                string jumpToTile = item.Element("jumpToTile").Value.Trim();
            }*/
            float xPosition = float.Parse(item.Element("xPosition").Value.Trim());
            float yPosition = float.Parse(item.Element("yPosition").Value.Trim());

            // apply its attributes
            //tile.SetColor(color);
            tile.SetTileType(type);
            tile.SetPosition(xPosition, yPosition);
            tile.SetText(text);

            // set the type based on its color
            /*for (int i = 0; i < Tile.colors.Length; i++)
            {
                if (color == Tile.colors[i])
                {
                    tile.SetTileType((Tile.TileType)i);
                }
            }*/

            // set the color based on its 
            //Debug.Log(type);
            tile.SetColor(GameData.GetColorScheme()[type]);

            // add the tile to dictionary
            tiles.Add(name, tile);
        }

        // link tiles
        foreach (var item in items)
        {
            // get the tile
            Tile tile = tiles[item.Element("name").Value.Trim()];

            // set the next tiles if they exist
            IEnumerable<XElement> nextTileElements = item.Elements("nextTile");

            foreach (var nextTileElement in nextTileElements)
            {
                tile.AddNext(tiles[nextTileElement.Value.Trim()]);
            }

            // set jump to tile if it exists
            if (item.Element("jumpToTile") != null)
            {
                //Debug.Log("attempting to add " + item.Element("jumpToTile"));
                tile.SetLandNext(tiles[item.Element("jumpToTile").Value.Trim()]);
            }
        }

        // set the start tile
        Player.SetStartTile(tiles[GameData.GetStartTileName()]);
    }

    public Dictionary<string, Tile> GetTiles()
    {
        // gets the tile dictionary
        return tiles;
    }

    public void SetupPlayers()
    {
        // sets up the players
        players = new Player[GameData.GetNumPlayers()];
        for (int i = 0; i < GameData.GetNumPlayers(); i++)
        {
            GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
            Player player = playerObject.GetComponent<Player>();
            players[i] = player;
        }

        // the first player in the list goes first
        currPlayerIndex = 0;
        GameData.SetCurrPlayer(players[0]);
    }

    /*public void SetupDie()
    {
        // sets up the die
    }*/

    /*public void SetupCameras()
    {
        // sets up the cameras
        GameData.SetMainCamera(Camera.allCameras[0]);
        GameData.SetDieCamera(Camera.allCameras[1]);
    }*/

    public void RollDie()
    {
        // rolls the die
        // switch view
        GameData.SetGameMode(GameData.Mode.NormalRoll);
        Camera.main.transform.position = GameData.GetDieCameraPosition();
        Camera.main.transform.eulerAngles = GameData.GetDieCameraRotation();

        // show title and roll button
        GameGUI.ShowRollScreen();
    }

    /*public void CreateButton(string text, Vector3 position)
    {
        GameObject button = new GameObject();
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.transform.position = position;
    }*/

    public void PostTurn()
    {
        // jumps if needed and proceeds to next turn
        float delay = GameData.GetActivePiece().JumpIfNeeded();

        // hide message screen
        GameGUI.HideMessageScreen();

        // check for winner
        StartCoroutine(CheckWinner(delay));
    }

    IEnumerator CheckWinner(float delay)
    {
        // checks for winner
        if (GameData.GetCurrPlayer().IsAllDone())
        {
            // we have a winner, set the mode to win
            yield return new WaitForSeconds(delay);
            GameData.SetGameMode(GameData.Mode.Winner);
            GameGUI.ShowWinScreen();

            // celebrate
            GameData.GetCurrPlayer().Celebrate();
        }
        else
        {
            // go to next player
            StartCoroutine(NextTurn(delay));
        }
    }

    IEnumerator NextTurn(float delay)
    {
        // goes to next player
        yield return new WaitForSeconds(delay);
        currPlayerIndex = (currPlayerIndex + 1) % players.Length;

        // set new current player
        GameData.SetCurrPlayer(players[currPlayerIndex]);

        // show die roll screen
        RollDie();
    }

    public void NextTurn()
    {
        // goes to next player
        currPlayerIndex = (currPlayerIndex + 1) % players.Length;

        // set new current player
        GameData.SetCurrPlayer(players[currPlayerIndex]);

        // hide select move screen
        GameGUI.HideSelectMoveScreen();

        // show die roll screen
        RollDie();
    }

    private void Start()
    {
        //tiles = new List<Tile>();
        //tiles = new Dictionary<string, Tile>();
        //SetupCameras();
        LoadTiles();
        //SetupDie();
        //RollDie();
        //GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
        //Player player = playerObject.GetComponent<Player>();
        SetupPlayers();
    }
}