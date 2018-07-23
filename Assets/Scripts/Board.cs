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
    // A list which contains all Players in the game
    private List<Player> players;

    //A map of Tile's used for the game board
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    //A Map from keys to TileEffectInterfaces so that Tile's can access different visual effects with a specified key
    private static Dictionary<string, TileEffectInterface> tileEffects = new Dictionary<string, TileEffectInterface>();

    //An integer to keep track of which Player is playing the current turn
    private int currPlayerIndex;

    //A Queue to keep track of which Player rolls next
    private Queue<Player> nextToRollQueue;

    
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
        //access method to return the start tile
        return tiles["start"];
    }

    public Tile GetFinishTile()
    {
        //access method to return the finish tile
        return tiles["finish"];
    }

    public void LoadTileAnimations()
    {
        //This function loads all tile animations into the tileEffects map and then hides the visual effects
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

    private void HideAllEffects()
    {
        //this function hides all tileEffects
        foreach (string effect in tileEffects.Keys)
        {
            tileEffects[effect].hideEffect();
        }
    }

    public void LoadTiles()
    {
        // Load the Tiles XML file into Tile classes and puts them in the tile map
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(GameData.GetTextAssetHolder().GetTiles().ToString());
        XDocument xDoc = XDocument.Parse(xmlDoc.OuterXml);
        IEnumerable<XElement> items = xDoc.Descendants("tiles").Elements();
        foreach (var item in items)
        {
            // create the tile
            GameObject tileObject = Instantiate(GameData.GetPrefabAssetHolder().GetTile());

            //Get the next tile component from the XML file
            Tile tile = tileObject.GetComponent<Tile>();

            //Store tile elements into temporary varaiables
            string name = item.Element("name").Value.Trim();
            //string color = item.Element("color").Value.Trim();
            string type = item.Element("type").Value.Trim();
            string text = item.Element("text").Value.Trim();
            string language1Text = item.Element("language1").Value.Trim();
            string language2Text = item.Element("language2").Value.Trim();
            string language3Text = item.Element("language3").Value.Trim();

            //Special case for some tiles which have a "special" element 
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
            //
            //Store tile elements into temporary varaiables
            float xPosition = float.Parse(item.Element("xPosition").Value.Trim());
            float yPosition = float.Parse(item.Element("yPosition").Value.Trim());
            string landAnimKey = item.Element("landAnimKey").Value.Trim();
            string landAnimOption = item.Element("landAnimOption").Value.Trim();

            

            // apply its attributes, create the tile from the temporary variables
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
            tile.SetColor(GameData.GetColorScheme()[type]);

            //Makes our tiles invisible on the board
            tile.HideTileVisual();

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
                tile.SetLandNext(tiles[item.Element("jumpToTile").Value.Trim()]);
            }

        }

        // set the start tile
        Player.SetStartTile(tiles[GameData.GetStartTileName()]);
    }

    public void PrintTilePositions()
    {
        //prints the position of all tiles
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
        //Prepares the game to assign Players their Characeter and turn order
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

        //keeps track of which player has been set up so far
        currPlayerIndex = 0;

        //resets players array


        //Transition to an explanation screen of how players will roll to decide turn order
        GameGUI.ShowExplanationScreen("ChooseRollOrderText", GameGUI.HideExplanationShowSetup);

        // the first player in the list goes firstsetup
        //currPlayerIndex = 0;
        //GameData.SetCurrPlayer(players[0]);
    }

    public void AddPlayer(string name, string color)
    {
        // Adds a Player to the Board's list of players from the setup screen initalizing the Player's name and color. It then makes that Player's color unavailable to other Players.
        // Adds a player from the setup screen initalizing the Player's name and color.
        GameObject playerObject = Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
        Player player = playerObject.GetComponent<Player>();
        player.SetName(name);
        player.SetupPieces(GameData.GetColorMap()[color]);
        players.Add(player);

        // remove player's color from available colors
        GameData.RemoveAvailableColor(color);
    }

    public void SetupNextPlayer()
    {
        // Prepares the game to setup the next character or determine move order if all Player's are set up.
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
        // Switches to the roll screen
        
        // switch view (main camera position)
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
        // this function is used to process the endin of a turn after a normal tile

        // jumps if needed and proceeds to next turn
        float delay = GameData.GetActivePiece().JumpIfNeeded();

        //ends tile effect
        HideAllEffects();
        
        if (delay == 0)
        {
            // since no jump happened 

            //if collision, display the collision screen
            bool collision = CheckCollision();
            bool merge = CheckMerge();

            //if no collision move on to next turn 
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
        //Checks if there is a collision and enacts the collision process if there is one 

        //gets position of active piece
        Vector3 activePosition = GameData.GetActivePiece().transform.position;

        //Position of the starting tile 
        Vector3 startPosition = GetStartTile().transform.position;

        //collisions should not happen at the starting position 
        if (activePosition == startPosition)
        {
            return false;
        }


        //check each piece for a collision
        foreach (Player player in players)
        {
            if (!player.Equals(GameData.GetCurrPlayer()))
            {
                foreach (Character piece in player.GetPieces())
                {
                    if (piece.GetCurrTile() == GameData.GetActivePiece().GetCurrTile() && piece.GetCurrTile() != GetStartTile())
                    {
                        //determine smaller piece (active piece wins in case of tie)
                        Character smallerPiece = null;
                        if (piece.GetSize() > GameData.GetActivePiece().GetSize())
                        {
                            smallerPiece = GameData.GetActivePiece();
                        }
                        else
                        {
                            smallerPiece = piece;
                        }
                        
                        //do the collision 
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
        //Checks if a merge should happen and enacts it 

        //gets all pieces of the current player
        Character[] pieces = GameData.GetCurrPlayer().GetPieces();
        //gets position of the active piece
        Vector3 activePosition = GameData.GetActivePiece().transform.position;
        //gets the active piece
        Character activePiece = GameData.GetActivePiece();
        //the position of the starting tile
        Vector3 startPosition = GetStartTile().transform.position;


        //checks every piece of the current player for a merge
        foreach (Character piece in pieces)
        {
            //merges cannot occure at start or with a piece and itself
            if (activePiece.GetCurrTile() != GetStartTile() && !piece.Equals(activePiece) 
                && piece.GetCurrTile() == activePiece.GetCurrTile())
            {
                //adjusting the size of the character after the merge
                int prevSize = activePiece.GetSize();
                activePiece.AdjustSize(piece.GetSize());

                //remove one of the pieces after the merge
                piece.gameObject.SetActive(false);
                GameData.GetCurrPlayer().RemovePiece(piece);

                //adjusting the visual size of the piece
                activePiece.transform.localScale = activePiece.GetSize()/prevSize * activePiece.transform.localScale;

                //show merge screen for explanation 
                GameGUI.ShowMergeScreen(activePiece.GetCurrTile());
                return true;
            }
        }

        return false; 
    }

    public IEnumerator CheckWinner(float delay)
    {
        // checks for winner
        if (GameData.GetCurrPlayer().IsAllDone())
        {
            // we have a winner, set the mode to win
            yield return new WaitForSeconds(delay);
            GameData.SetGameMode(GameData.Mode.Winner);

            //show the winning screen
            GameGUI.ShowWinScreen();

            //activate the winning visual effect
            tileEffects["win"].activateEffect(new Vector2(0,0));

            // celebrate
            GameData.GetCurrPlayer().Celebrate();

            //play winning sound
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
        HideAllEffects();

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
        //access method for tileEffects map 
        return tileEffects;
    }

    public string GetMoveOrderString()
    {
        // returns a string describing the move order
        string text = "";
        for (int i = 0; i < players.Count; i++)
        {
            text += players[i].GetName() + " " + GameData.GetCurrentLanguage().GetMoveText() + " " + GameData.GetOrdinals()[i] + "!\n";
        }
        return text;
    }

    /*public void Setup2Players()
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
    }*/

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

    public void NewGame()
    {
        //remove all pieces on board 
        foreach (Player player in players)
        {
            foreach (Character piece in player.GetPieces())
            {
                Destroy(piece.gameObject);
            }
        }

        //reset players in game
        players = new List<Player>();

        //reset piece color choices
        GameData.ResetAvailableColors();
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
        //Initializes visual effects map
        LoadTileAnimations();
        //reads and loads Tiles XML
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