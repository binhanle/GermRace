using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private readonly Character piece;
    //private readonly int pathIndex;
    private readonly Tile destTile;

    public Move(Character playerPiece, Tile tile)
    {
        // initialize the move object
        piece = playerPiece;
        //pathIndex = index;
        destTile = tile;
    }

    public Character GetPiece()
    {
        // get the piece that's moving
        return piece;
    }

    /*public int GetPathIndex()
    {
        // get the piece's path index
        return pathIndex;
    }*/

    public Tile GetDestTile()
    {
        // return the destination tile of the move
        return destTile;
    }
}