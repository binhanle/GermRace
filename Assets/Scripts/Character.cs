using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    //private Vector2 position;
    private Tile currTile;
    //private Player owner;
    //private Animation happyAn;
    //private Animation sadAn;
    //private bool isWalking;
    //private bool isJumping;
    private const string AnimPath = "Characters/Mushroomboypack1.2/3D/Mushroomboy";
    //private const string IdleAn = "01idle";
    private const string MoveAn = "02walk";
    private const string JumpAn = "03jump";
    private const string HappyAn = "04dance";
    private const string SadAn = "05tumble";
    private const string FinishAn = "06hooray";
    private const float moveDuration = 2;
    private const float jumpHeight = 0.5f;
    private const float delayAfterJump = 4;

    /*public void Move(float x, float y)
    {
        // sets position to x-y coordinates passed in
        position = new Vector2(x, y);
    }*/

    public bool IsLegalMove(int numSpaces, int pathIndex, ref Tile destTile)
    {
        // checks if move is legal
        destTile = currTile.GetNext()[pathIndex];
        int spacesLeft = numSpaces - 1;
        while (spacesLeft > 0 && destTile.HasNext())
        {
            destTile = destTile.GetNext()[0];
            spacesLeft--;
        }

        return spacesLeft == 0;
    }

    public void MoveSpaces(int numSpaces, int pathIndex)
    {
        // moves a specified number of spaces
        // find the destination tile
        Tile destTile = currTile.GetNext()[pathIndex];
        //Debug.Log(destTile.GetPosition());
        int spacesLeft = numSpaces - 1;
        while (spacesLeft > 0 && destTile.HasNext())
        {
            destTile = destTile.GetNext()[0];
            spacesLeft--;
        }

        // move to the destination
        WalkToTile(destTile);
    }

    public void WalkToTile(Tile tile)
    {
        // moves piece to specified tile
        // focus on piece
        GameData.SetActivePiece(this);
        GameData.SetGameMode(GameData.Mode.MovingPiece);

        // start walking
        Vector3 currPos3d = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
        Vector3 newPos3d = new Vector3(tile.GetPosition().x, 0, tile.GetPosition().y);
        DoMove(currPos3d, newPos3d);
        currTile = tile;

        // show what's on the tile and react accordingly
        StartCoroutine(DoLand(currTile));

        // jump if necessary
        /*if (tile.HasLandNext())
        {
            Invoke("JumpToLandNextTile", moveDuration);
            //JumpToLandNextTile();
        }*/
    }

    public float JumpIfNeeded()
    {
        // jumps if necessary and returns delay until next turn
        if (currTile.HasLandNext())
        {
            JumpToLandNextTile();
            if (IsDone())
            {
                Invoke("DoFinish", moveDuration);
            }
            return delayAfterJump;
        }
        return 0;
    }

    IEnumerator DoLand(Tile currTile)
    {
        // displays tile text and reacts according to tile type
        yield return new WaitForSeconds(moveDuration);
        GameGUI.ShowMessageScreen(currTile);

        // play corresponding animation
        switch (currTile.GetTileType())
        {
            case "jumpBack":
                DoSad();
                break;
            case "finish":
                DoFinish();
                break;
            case "jumpAhead":
                DoHappy();
                break;
            case "special":
                currTile.DoSpecialCommand();
                break;
            default:
                break;
        }
    }

    public void MoveToTile(Tile tile)
    {
        // moves piece to tile
        transform.position = new Vector3(tile.GetPosition().x, 0, tile.GetPosition().y);
        currTile = tile;
    }

    public void MoveToNextTile(int pathIndex)
    {
        // moves piece to the next tile
        Tile nextTile = currTile.GetNext()[pathIndex];
        Vector3 currPos3d = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
        Vector3 newPos3d = new Vector3(nextTile.GetPosition().x, 0, nextTile.GetPosition().y);
        DoMove(currPos3d, newPos3d);
        currTile = nextTile;
    }

    /*public void JumpToNextTile(int pathIndex)
    {
        // moves piece to another tile after landing
        Tile nextTile = currTile.GetNext()[pathIndex];
        Vector3 currPos3d = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
        Vector3 newPos3d = new Vector3(nextTile.GetPosition().x, 0, nextTile.GetPosition().y);
        DoJump(currPos3d, newPos3d);
        currTile = nextTile;
    }*/

    public void JumpToLandNextTile()
    {
        // moves piece to another tile after landing
        Tile nextTile = currTile.GetLandNext();
        Vector3 currPos3d = new Vector3(currTile.GetPosition().x, 0, currTile.GetPosition().y);
        Vector3 newPos3d = new Vector3(nextTile.GetPosition().x, 0, nextTile.GetPosition().y);
        DoJump(currPos3d, newPos3d);
        currTile = nextTile;
    }

    /*public void setOwner(Player newOwner)
    {
        owner = newOwner;
    }
    
    public Player getOwner()
    {
        return owner;
    }*/

    /*public Vector2 getPos()
    {
        // gets the position
        return position;
    }*/

    public Tile GetCurrTile()
    {
        // gets the tile where the piece is
        return currTile;
    }

    public void DoMove(Vector3 currPos, Vector3 newPos)
    {
        // runs move animation
        GetComponent<Animator>().Play(MoveAn);
        StartCoroutine(AnimateMove(currPos, newPos, moveDuration));
        //GetComponent<Animator>().Play(IdleAn);
    }

    public void DoJump(Vector3 currPos, Vector3 newPos)
    {
        // runs jump animation
        GetComponent<Animator>().Play(JumpAn);
        StartCoroutine(AnimateJump(currPos, newPos, moveDuration));
    }

    public void DoHappy()
    {
        // runs happy animation
        GetComponent<Animator>().Play(HappyAn);
    }
    
    public void DoSad()
    {
        // runs sad animation
        GetComponent<Animator>().Play(SadAn);
    }

    public void DoFinish()
    {
        // runs finish animation
        GetComponent<Animator>().Play(FinishAn);
    }

    public bool IsDone()
    {
        // returns true if character is on finish tile (nowhere else to go)
        return !currTile.HasNext();
    }

    public void Awake()
    {

    }

    /*public void FixedUpdate()
    {
        Vector3 target = new Vector3(2f, 0f, 0f);
        float smoothTime = 0.3f;
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }*/

    IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration)
    {
        // linearly move from origin to target
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.SmoothStep(0, 1, journey / duration);

            transform.position = Vector3.Lerp(origin, target, percent);

            yield return null;
        }
    }

    IEnumerator AnimateJump(Vector3 origin, Vector3 target, float duration)
    {
        // linearly move from origin to target and adjust height quadratically
        float journey = 0f;
        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.SmoothStep(0, 1, journey / duration);
            Vector3 newPos = Vector3.Lerp(origin, target, percent);
            newPos.y = 4 * percent * (1 - percent) * jumpHeight;

            transform.position = newPos;

            yield return null;
        }
    }
}