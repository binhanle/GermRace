using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoll : MonoBehaviour
{
    public float speed = 5f;
    public float rollSpeed = 5f;
    public Rigidbody rb;
    public bool roll = false;
    public Die_d6 dieScript;
    //public GameControllerScript gcs;
    public Vector3 startPosition;

    private bool RollCheckIsOccuring = false;
    private AudioSource source;

    void Start()
    {
        //source = GetComponent<AudioSource>();
        startPosition = transform.position;
        transform.rotation = Random.rotationUniform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularVelocity = Random.insideUnitSphere * rollSpeed;
    }

    void OnCollisionEnter()
    {
        //source.Play();
    }


    public void ResetDie()
    {
        transform.position = startPosition;
        transform.rotation = Random.rotationUniform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularVelocity = Random.insideUnitSphere * rollSpeed;
        StopAllCoroutines();
        RollCheckIsOccuring = false;
    }

    public void Roll()
    {
        roll = true;
        rb.useGravity = true;
        rb.velocity = (Vector3.left + 3 * Vector3.up) * speed;
        Rolling();
    }

    public void Rolling()
    {
        if (!RollCheckIsOccuring)
        {
            StartCoroutine(RollingCoroutine());
        }
        else
        {
            Debug.Log("Rolling COROUTINE IS ALREADY IN PROGRESS ERROR!!!!");
        }
    }

    IEnumerator RollingCoroutine()
    {
        RollCheckIsOccuring = true;
        while (dieScript.rolling)
        {
            yield return null;
        }
        //source.Play();
        yield return new WaitForSeconds(2);
        //gcs.MovePlayer(dieScript.value);

        // Hide the menu
        GameGUI.HideRollScreen();

        // Reset the die
        ResetDie();

        // Proceed based on game mode
        Board board = GameData.GetBoard();
        if (GameData.GetGameMode() == GameData.Mode.NormalRoll)
        {
            //GameData.SetGameMode(GameData.Mode.MovingPiece);
            //GameData.GetActivePiece().MoveSpaces(dieScript.value, 0);
            //Debug.Log(GameData.GetCurrPlayer().GetLegalMoves(dieScript.value).Count);
            //GameData.GetCurrPlayer().Move(dieScript.value, 0);

            // Display the player's legal moves
            GameData.GetCurrPlayer().DisplayLegalMoves(dieScript.value);
        }
        if (GameData.GetGameMode() == GameData.Mode.RollSixOrDie)
        {
            // If six, jump to finish, else jump to start
            GameData.SetGameMode(GameData.Mode.MovingPiece);
            if (dieScript.value == 6)
            {
                Tile finishTile = board.GetFinishTile();
                GameData.GetActivePiece().JumpToTile(finishTile);
            }
            else
            {
                Tile startTile = board.GetStartTile();
                GameData.GetActivePiece().JumpToTile(startTile);
            }

            // Check for winner (wait 2 seconds to jump, 2 seconds to settle down)
            StartCoroutine(board.CheckWinner(4));
        }
        if (GameData.GetGameMode() == GameData.Mode.InitialRoll)
        {
            // record die roll
            //board.RecordDieRoll(dieScript.value);
            GameData.GetCurrPlayer().AddToInitialRoll(dieScript.value);

            // next player in queue rolls
            board.RollFromQueue();
        }
        RollCheckIsOccuring = false;
    }
}