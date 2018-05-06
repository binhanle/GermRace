using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private readonly Character piece;
    private readonly int pathIndex;

    public Move(Character playerPiece, int index)
    {
        // initialize the move object
        piece = playerPiece;
        pathIndex = index;
    }

    public Character GetPiece()
    {
        // get the piece that's moving
        return piece;
    }

    public int GetPathIndex()
    {
        // get the piece's path index
        return pathIndex;
    }
}