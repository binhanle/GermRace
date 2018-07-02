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
    private static Dictionary<string, TileEffectInterface> tileEffects = new Dictionary<string, TileEffectInterface>();
    //private int turn;
    //private const string startTileName = "start";
    private int currPlayerIndex;
    private Queue<Player> nextToRollQueue;
    private static readonly string[] ordinals = { "first", "second", "third", "fourth" };
    //public TextAsset TilesXML;
    

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

    public void LoadTileAnimations()
    {
        //loading all the effects
        HeavyRainEffect rainEffect = GameObject.Find("HeavyRainEffect").GetComponent<HeavyRainEffect>();
        tileEffects.Add("rain", rainEffect);
        FallingObject fallingObject = GameObject.Find("FallingObject").GetComponent<FallingObject>();
        tileEffects.Add("fall", fallingObject);
        RisingObject risingObject = GameObject.Find("JerryCan").GetComponent<RisingObject>();
        tileEffects.Add("jerry", risingObject);
        LeftMovingObject slideObject = GameObject.Find("LeftMovingObject").GetComponent<LeftMovingObject>();
        tileEffects.Add("slide", slideObject);
        SmokeEffect smokeEffect = GameObject.Find("SmokeEffect").GetComponent<SmokeEffect>();
        tileEffects.Add("smoke", smokeEffect);
        LeftMovingObject fishObject = GameObject.Find("FishEffect").GetComponent<LeftMovingObject>();
        tileEffects.Add("fish", fishObject);
        LeftMovingObject screenObject = GameObject.Find("SlidingScreenEffect").GetComponent<LeftMovingObject>();
        tileEffects.Add("screen", screenObject);
        FallingObject soapObject = GameObject.Find("FallingSoapEffect").GetComponent<FallingObject>();
        tileEffects.Add("soap", soapObject);
        WinEffect winEffect = GameObject.Find("WinEffect").GetComponent<WinEffect>();
        tileEffects.Add("win", winEffect);

        //resizing and hiding the effects
        foreach (string effect in tileEffects.Keys)
        {
            //set parameters of effects
            float[] animParams = GameData.GetAnimationParams();
            tileEffects[effect].setParameters(animParams[0], animParams[1], animParams[2], animParams[3]);

            //resize effects
            tileEffects[effect].resizeEffect();

            //hide effects
            tileEffects[effect].hideEffect();
        }
    }

    private void hideAllEffects()
    {
        foreach (string effect in tileEffects.Keys)
        {
            tileEffects[effect].hideEffect();
        }
        }

    public void LoadTiles()
    {
        // load the tiles xml file
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(GameData.GetTextAssetHolder().getTiles().ToString());
        XDocument xDoc = XDocument.Parse(xmlDoc.OuterXml);
        IEnumerable<XElement> items = xDoc.Descendants("tiles").Elements();
        foreach (var item in items)
        {
            // create the tile
            GameObject tileObject = Instantiate(GameData.GetPrefabAssetHolder().GetTile());
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
            /*
            if (item.Element("image") != null)
            {
                string image = item.Element("image").Value.Trim();
                tile.DisplayImage(image);
            }
            */
            float xPosition = float.Parse(item.Element("xPosition").Value.Trim());
            float yPosition = float.Parse(item.Element("yPosition").Value.Trim());
            string landAnimKey = item.Element("landAnimKey").Value.Trim();
            string landAnimOption = item.Element("landAnimOption").Value.Trim();

            

            // apply its attributes
            //tile.SetColor(color);
            tile.SetTileType(type);
            tile.SetPosition(xPosition, yPosition);
            tile.SetText(text);
            tile.SetLandAnimKey(landAnimKey);
            tile.SetLandAnimOption(landAnimOption);
            

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

            //Makes our tiles invisible on the board
            tile.hideTileVisual();

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
        createBourdBackGround();
    }

    public void createBourdBackGround()
    {
        //Finds the center position for the board
        foreach (string key in tiles.Keys)
        {
            Debug.Log(tiles[key].GetPosition());
        }
    }

    public Dictionary<string, Tile> GetTiles()
    {
        // gets the tile dictionary
        return tiles;
    }

    public void SetupPlayers(int numPlayers)
    {
        // sets up the players
        /*players = new Player[GameData.GetNumPlayers()];
        for (int i = 0; i < GameData.GetNumPlayers(); i++)
        {
            GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
            Player player = playerObject.GetComponent<Player>();
            players[i] = player;
        }*/
        //hides player choice buttons
        GameGUI.HidePlayerCountScreen();

        //sets the amount of players
        GameData.SetNumPlayers(numPlayers);

        //keeps track of whih player has been set up so far
        currPlayerIndex = 0;

        //resets players array


        //ADD EXPLANATION HERE
        GameGUI.ShowExplanationScreen(GameData.GetTextAssetHolder().GetChooseRollOrderText(), GameGUI.HideExplanationShowSetup);

        // the first player in the list goes firstsetup
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
            GameGUI.ShowCurrentPieceColor();
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

        //ends tile effect
        hideAllEffects();
        
        if (delay == 0)
        {
            // since no jump happened 

            //if collision, display the collision screen
            bool collision = CheckCollision();
            bool merge = CheckMerge();

            //if n collision move on to next turn 
            if (!collision && !merge)
            {
                // hide message screen
                GameGUI.HideMessageScreen();

                //check for winner
                StartCoroutine(CheckWinner(delay));
            }            
        }
    }

    public bool CheckCollision()
    {
        Vector3 activePosition = GameData.GetActivePiece().transform.position;
        Vector3 startPosition = new Vector3(-4, 0, -5.5f);

        if (activePosition == startPosition)
        {
            return false;
        }

        foreach (Player player in players)
        {
            if (!player.Equals(GameData.GetCurrPlayer()))
            {
                foreach (Character piece in player.getPieces())
                {
                    if (piece.transform.position == activePosition)
                    {
                        //determine smaller piece (active piece wins in case of tie)
                        Character smallerPiece = null;
                        if (piece.getSize() > GameData.GetActivePiece().getSize())
                        {
                            smallerPiece = GameData.GetActivePiece();
                        }
                        else
                        {
                            smallerPiece = piece;
                        }
                        
                        GameGUI.ShowCollisionScreen(smallerPiece, player);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CheckMerge()
    {
        Character[] pieces = GameData.GetCurrPlayer().getPieces();
        Vector3 activePosition = GameData.GetActivePiece().transform.position;
        Character activePiece = GameData.GetActivePiece();
        Vector3 startPosition = new Vector3(-4, 0, -5.5f);

        foreach (Character piece in pieces)
        {
            if (activePiece.transform.position != startPosition && !piece.Equals(activePiece) 
                && piece.transform.position == activePosition )
            {
                int prevSize = activePiece.getSize();
                activePiece.adjustSize(piece.getSize());
                piece.gameObject.SetActive(false);
                GameData.GetCurrPlayer().removePiece(piece);
                activePiece.transform.localScale = activePiece.getSize()/prevSize * activePiece.transform.localScale;
                GameGUI.ShowMergeScreen(activePiece.GetCurrTile());
                return true;
            }
        }

        return false; 

        //HEREHERE display merge screen
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
            tileEffects["win"].activateEffect(new Vector2(0,0));

            // celebrate
            GameData.GetCurrPlayer().Celebrate();
            Audio.PlayVictory();
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
        //hides the tile effect
        hideAllEffects();

        // executes the special command 
        GameGUI.HideMessageScreen();
        CheckCollision();
        CheckMerge();
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
        //HEREHERE ADD COLLISION LOGIC
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

    public static Dictionary<string, TileEffectInterface> GetTileEffects()
    {
        return tileEffects;
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

    public void Setup2Players()
    {
        // sets up 2 players
        GameGUI.HidePlayerCountScreen();
        SetupPlayers(2);
    }

    public void Setup3Players()
    {
        // sets up 3 players
        GameGUI.HidePlayerCountScreen();
        SetupPlayers(3);
    }

    public void Setup4Players()
    {
        // sets up 4 players
        GameGUI.HidePlayerCountScreen();
        SetupPlayers(4);
    }

    //public void ShowRules()
    //{
        // shows the rules
    //    GameGUI.HideMainScreen();
    //    GameGUI.ShowInfoScreen(GameData.GetTextAssetHolder().GetRulesText());
    //}

    //public void ShowCredits()
    //{
        // shows the credits
    //    GameGUI.HideMainScreen();
    //    GameGUI.ShowInfoScreen(GameData.GetTextAssetHolder().GetCreditsText());
    //}

    public void ShowMainMenu()
    {
        // shows the main menu
        GameData.SetGameMode(GameData.Mode.Home);
        GameGUI.HideEveryScreen();
        GameGUI.ShowMainScreen();
        tileEffects["win"].hideEffect();
    }

    public void AskHowManyPlayers()
    {
        // shows the player count screen
        GameGUI.HideMainScreen();
        GameGUI.ShowPlayerCountScreen();
    }

    public void ShowOptions()
    {
        // shows the options screen
        GameGUI.HideMainScreen();
        GameGUI.ShowOptionsScreen();
    }

    public void newGame()
    {
        //remove all pieces on board 
        foreach (Player player in players)
        {
            foreach (Character piece in player.getPieces())
            {
                Destroy(piece.gameObject);
            }
        }

        //reset players in game
        players = new List<Player>();

        //reset piece color choices
        GameData.resetAvailableColors();
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
        LoadTileAnimations();
        LoadTiles();
        //SetupDie();
        //RollDie();
        //GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
        //Player player = playerObject.GetComponent<Player>();
        //GameData.SetGameMode(GameData.Mode.Home);
        //SetupPlayers();
        //DetermineMoveOrder();
        //RollDie(GameData.Mode.NormalRoll);
    }
}