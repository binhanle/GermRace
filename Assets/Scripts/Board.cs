using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEngine.UI;
using System;

public class Board : MonoBehaviour
{
    //private Player[] players;
    private List<Player> players;
    //private List<Tile> tiles;
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    //private int turn;
    //private const string startTileName = "start";
    private int currPlayerIndex;
    private Queue<Player> nextToRollQueue;
    private static readonly string[] ordinals = { "first", "second", "third", "fourth" };

    /*public Player[] currentPlayer()
    {
        // returns player whose turn it is
        return players;
    }*/
    
    /*public void setTiles(Tile[] newTiles)
    {
        // sets Tiles array for this board based on array
        tiles = newTiles;
    }*/
    
    /*public void setPlayers(Player[] newPlayers)
    {
        // sets Players array for this board based on array
        players = newPlayers;
    }*/

    public Tile GetStartTile()
    {
        return tiles["start"];
    }

    public Tile GetFinishTile()
    {
        return tiles["finish"];
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
            if (item.Element("special") != null)
            {
                string command = item.Element("special").Value.Trim();
                tile.SetSpecialCommand(command);
            }
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

            // set the color based on its type
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
        /*players = new Player[GameData.GetNumPlayers()];
        for (int i = 0; i < GameData.GetNumPlayers(); i++)
        {
            GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
            Player player = playerObject.GetComponent<Player>();
            players[i] = player;
        }*/
        currPlayerIndex = 0;
        GameGUI.ShowSetupScreen("Player1");
        ShowCurrentPieceColor();

        // the first player in the list goes first
        //currPlayerIndex = 0;
        //GameData.SetCurrPlayer(players[0]);
    }

    public void AddPlayer(string name, string color)
    {
        // adds a player
        GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
        Player player = playerObject.GetComponent<Player>();
        player.SetName(name);
        player.SetupPieces(color);
        players.Add(player);

        // remove player's color from available colors
        GameData.RemoveAvailableColor(color);
    }

    public void SetupNextPlayer()
    {
        // sets up the next player
        currPlayerIndex++;
        if (currPlayerIndex < GameData.GetNumPlayers())
        {
            GameGUI.ShowSetupScreen("Player" + (currPlayerIndex + 1));
        }
        else
        {
            // hide the setup screen
            GameGUI.HideSetupScreen();

            // determine the move order
            DetermineMoveOrder();
        }
    }

    public void ProcessPlayer()
    {
        // adds the current player and sets up the next player
        AddPlayer(GameGUI.GetInputFieldName(), GameGUI.GetDropDownColor());

        // set up the next player
        SetupNextPlayer();
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

    public void RollDie(GameData.Mode mode)
    {
        // rolls the die
        // switch view
        GameData.SetGameMode(mode);
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

    public IEnumerator CheckWinner(float delay)
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
        currPlayerIndex = (currPlayerIndex + 1) % players.Count;

        // set new current player
        GameData.SetCurrPlayer(players[currPlayerIndex]);

        // show die roll screen
        RollDie(GameData.Mode.NormalRoll);
    }

    public void NextTurn()
    {
        // goes to next player
        currPlayerIndex = (currPlayerIndex + 1) % players.Count;

        // set new current player
        GameData.SetCurrPlayer(players[currPlayerIndex]);

        // hide select move screen
        GameGUI.HideSelectMoveScreen();

        // show die roll screen
        RollDie(GameData.Mode.NormalRoll);
    }

    public void DoSpecialCommand()
    {
        // executes the special command
        GameGUI.HideMessageScreen();
        Invoke(GameData.GetActivePiece().GetCurrTile().GetSpecialCommand(), 0);
    }

    public void RollAgain()
    {
        // shows roll screen again
        RollDie(GameData.Mode.NormalRoll);
    }

    public void RollSixOrDie()
    {
        // rerolls for a six
        RollDie(GameData.Mode.RollSixOrDie);
    }

    public void BringPiece()
    {
        // brings another piece to current tile
        GameData.GetCurrPlayer().BringPiece(GameData.GetActivePiece().GetCurrTile());
    }

    public void DetermineMoveOrder()
    {
        // determines move order by dice roll
        // initialize the roll queue
        foreach (Player player in players)
        {
            nextToRollQueue.Enqueue(player);
        }

        // start rolling
        RollFromQueue();
    }

    public void RollFromQueue()
    {
        // makes the first player in the queue roll
        if (nextToRollQueue.Count != 0)
        {
            Player currPlayer = nextToRollQueue.Dequeue();
            GameData.SetCurrPlayer(currPlayer);

            // start rolling
            RollDie(GameData.Mode.InitialRoll);
        }
        else
        {
            // order players by highest dice roll
            //Array.Sort(players);
            //Array.Reverse(players);
            players.Sort();
            players.Reverse();

            // check for ties
            DoTieBreakers();
        }
    }

    public void DoTieBreakers()
    {
        // checks for ties in dice rolls and does tiebreakers if needed
        bool tied = false;
        int index = 0;
        while (!tied && index < players.Count - 1)
        {
            if (players[index].CompareTo(players[index + 1]) > 0)
            {
                // if current player is higher than next player, add 6 to initial roll
                players[index].AddToInitialRoll(6);
            }
            else
            {
                // if tied, add players to roll queue and stop iterating
                nextToRollQueue.Enqueue(players[index]);
                nextToRollQueue.Enqueue(players[index + 1]);
                tied = true;
            }
            index++;
        }

        // add tied players to the queue
        while (index < players.Count - 1 && players[index].CompareTo(players[index + 1]) == 0)
        {
            nextToRollQueue.Enqueue(players[index + 1]);
            index++;
        }

        // reroll if necessary
        if (nextToRollQueue.Count > 0)
        {
            RollFromQueue();
        }
        else
        {
            // hide the roll screen
            GameGUI.HideRollScreen();

            // show the move order screen
            GameGUI.ShowMoveOrderScreen();
        }
    }

    public void StartGame()
    {
        // starts the game
        // hide the move order screen
        GameGUI.HideMoveOrderScreen();

        // the first player in the list goes first
        currPlayerIndex = 0;
        GameData.SetCurrPlayer(players[0]);
        RollDie(GameData.Mode.NormalRoll);
    }

    public string GetMoveOrderString()
    {
        // returns a string describing the move order
        string text = "";
        for (int i = 0; i < players.Count; i++)
        {
            text += players[i].GetName() + " moves " + ordinals[i] + "!\n";
        }
        return text;
    }

    public void ShowPieceColor(string pieceColor)
    {
        // shows demo piece of specific color and hides the rest
        Dictionary<string, GameObject> demoPieces = GameData.GetDemoPieces();
        foreach (string color in demoPieces.Keys)
        {
            if (color.Equals(pieceColor))
            {
                demoPieces[color].SetActive(true);
            }
            else
            {
                demoPieces[color].SetActive(false);
            }
        }
    }

    public void ShowCurrentPieceColor()
    {
        // shows demo piece of color specified by dropdown
        ShowPieceColor(GameGUI.GetDropDownColor());
    }

    private void Awake()
    {
        // initialize player list
        players = new List<Player>();

        // initialize initial roll queue
        nextToRollQueue = new Queue<Player>();
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
        GameData.SetGameMode(GameData.Mode.Home);
        //SetupPlayers();
        //DetermineMoveOrder();
        //RollDie(GameData.Mode.NormalRoll);
    }
}