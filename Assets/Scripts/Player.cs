using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour, IComparable<Player>
{
    //private static int playerCount = 0;
    //private int playerID;
    private string playerName;
    private Character[] playerPieces;
    //private static Character activePiece;
    //private const int NumPieces = 1;
    //private const string charDir = "Mushroomboypack1.2/3D/";
    private static Tile startTile;
    private static int numPlayers;
    private Stack<Line> lines;
    private Stack<Tile> destTiles;
    private int initialRoll = 0;
    private List<UnityEngine.Color> colors = new List<UnityEngine.Color> { UnityEngine.Color.green
        , UnityEngine.Color.gray, UnityEngine.Color.yellow, UnityEngine.Color.blue};
    private string charType;

    public string GetName()
    {
        // returns the name of the player
        return playerName;
    }

    public Character[] GetPieces()
    {
        return playerPieces;
    }

    public void RemovePiece(Character piece)
    {
        //var isRemoved = Array.remove(playerPieces, piece);
        List<Character> temp = new List<Character>();
        foreach (Character character in playerPieces)
        {
            if (!character.Equals(piece))
            {
                temp.Add(character);
            }
        }

        playerPieces = temp.ToArray();
    }

    public void SetName(string name)
    {
        // sets the name of the player
        playerName = name;
    }

    public void SetupPieces(string charName)
    {
        // sets up the player's pieces using specified character
        for (int i = 0; i < GameData.GetNumPiecesPerPlayer(); i++)
        {
            // create the piece
            GameObject pieceObject = Instantiate(GameData.GetCharDir()[charName]);
            pieceObject.AddComponent<Character>();
            Character piece = pieceObject.GetComponent<Character>();

            // rotate the piece 90 degrees clockwise
            pieceObject.transform.Rotate(new Vector3(0, 90, 0));

            // add piece to the piece list
            playerPieces[i] = piece;
            piece.MoveToTile(startTile);
            //piece.MoveToNextTile(0);
            //piece.JumpToNextTile(0);
            //piece.DoFinish();
            //piece.MoveSpaces(6, 0);
        }
        charType = charName + "Character";
    }
    
    /*public Character getCharacter()
    {
        // returns the character of this player
        return playerPiece;
    }*/
    
    /*public void Move(int numSpaces, int pathIndex)
    {
        // moves character based on die roll
        //playerPiece.Move(x, y);
        activePiece = playerPieces[0];
        GameData.SetActivePiece(activePiece);
        activePiece.MoveSpaces(numSpaces, pathIndex);
    }*/

    public static void SetStartTile(Tile tile)
    {
        // sets start tile
        startTile = tile;
    }

    /*public static Character GetActivePiece()
    {
        // gets the active piece
        return activePiece;
    }

    public static void SetActivePiece(Character piece)
    {
        // sets the active piece
        activePiece = piece;
    }*/

    public bool IsAllDone()
    {
        // returns true if all of the player's pieces reached the end
        foreach (Character piece in playerPieces)
        {
            if (!piece.IsDone())
            {
                return false;
            }
        }
        return true;
    }

    public void Celebrate()
    {
        // makes all the player's pieces dance
        foreach (Character piece in playerPieces)
        {
            piece.DoHappy();
        }
    }

    public List<Move> GetLegalMoves(int numSpaces)
    {
        // returns all legal moves
        Dictionary<Tile, Character> origTiles = new Dictionary<Tile, Character>();
        List<Move> moves = new List<Move>();

        // loop through each piece
        foreach (Character piece in playerPieces)
        {
            // if move is legal, add to list
            Tile currTile = piece.GetCurrTile();
            if (!origTiles.ContainsKey(currTile))
            {
                origTiles.Add(currTile, piece);
                for (int pathIndex = 0; pathIndex < currTile.GetNext().Count; pathIndex++)
                {
                    Tile destTile = currTile;
                    if (piece.IsLegalMove(numSpaces, pathIndex, ref destTile))
                    {
                        moves.Add(new Move(piece, destTile));
                    }
                }
            }
        }
        return moves;
    }

    public void DisplayLegalMoves(int numSpaces)
    {
        // displays all legal moves on the board
        GameData.SetGameMode(GameData.Mode.SelectMove);

        // get legal moves
        List<Move> moves = GetLegalMoves(numSpaces);

        //keep track of piece that is associated with each color
        Dictionary<Character, int> colorMap = new Dictionary<Character, int>();

        //assign colors to each piece
        int piecesCount = 0;
        foreach( Move move in moves)
        {
            if (!colorMap.ContainsKey(move.GetPiece()))
            {
                colorMap.Add(move.GetPiece(), piecesCount);
                piecesCount++;
            }
        }

        

        // display them
        foreach (Move move in moves)
        {
            // draw line from start to end tile
            GameObject lineObject = Instantiate(GameData.GetPrefabAssetHolder().GetLine());
            Line line = lineObject.GetComponent<Line>();
            Debug.Log(line);
            Debug.Log(move.GetPiece().GetCurrTile().GetPosition());
            Debug.Log(move.GetDestTile().GetPosition());
            Debug.Log(colors[colorMap[move.GetPiece()]]);
            line.SetStartAndEnd(move.GetPiece().GetCurrTile().GetPosition(), move.GetDestTile().GetPosition(), colors[colorMap[move.GetPiece()]]);
            lines.Push(line);

            // store move in line
            line.SetMove(move);

            // highlight destination tile
            Tile destTile = move.GetDestTile();
            destTile.GetComponent<Outline>().enabled = true;
            destTiles.Push(destTile);
        }

        var allLines = FindObjectsOfType<Line>();

        foreach(Line line in allLines)
        {
            line.SetOtherLines();
        }

        // show the select move GUI
        GameGUI.ShowSelectMoveScreen(moves.Count > 0);
    }

    public void DestroyLegalMoves()
    {
        // erases all lines and outlines highlighting moves
        // hide the select move GUI
        GameGUI.HideSelectMoveScreen();

        // destroy the lines
        while (lines.Count > 0)
        {
            Destroy(lines.Pop().gameObject);
        }

        // destroy the outlines
        while (destTiles.Count > 0)
        {
            destTiles.Pop().GetComponent<Outline>().enabled = false;
        }
    }

    public void BringPiece(Tile destTile)
    {
        // brings a piece from start to destination tile
        Character pieceOnStart = null;
        foreach (Character piece in playerPieces)
        {
            // check if any pieces are on start tile
            if (piece.IsOnStart())
            {
                pieceOnStart = piece;
            }
        }

        // if there is a piece at start, move it to destination
        if (pieceOnStart != null)
        {
            pieceOnStart.JumpToTile(destTile);

            // Check for winner (wait 2 seconds to jump, 2 seconds to settle down) HEREHERE
            //StartCoroutine(GameData.GetBoard().CheckWinner(4));
            bool merge = GameData.GetBoard().CheckMerge();
        }
        else
        {
            // if not, go to next move
            GameData.GetBoard().NextTurn();
        }
    }

    public void AddToInitialRoll(int value)
    {
        // adds a value to player's initial roll
        initialRoll += value;
    }

    public int CompareTo(Player otherPlayer)
    {
        // returns a positive value if this player has higher dice roll, 0 if tied, negative otherwise
        return initialRoll - otherPlayer.initialRoll;
    }

    //document
    public string GetCharType()
    {
        return charType;
    }

    public void Awake()
    {
        // set up the player
        numPlayers++;
        playerName = "Player " + numPlayers;
        //playerID = playerCount;

        // set up the pieces
        playerPieces = new Character[GameData.GetNumPiecesPerPlayer()];
        //SetupPieces(GameData.PickColor());

        // initialize line stack
        lines = new Stack<Line>();

        // initialize destination tile stack
        destTiles = new Stack<Tile>();

        // test
        //activePiece = playerPieces[0];
        //GameData.SetActivePiece(activePiece);
    }
}