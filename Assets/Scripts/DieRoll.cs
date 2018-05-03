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
        if (GameData.GetGameMode() == GameData.Mode.NormalRoll)
        {
            // Hide the menu and move the active piece
            GameGUI.HideRollScreen();
            GameData.SetGameMode(GameData.Mode.MovingPiece);
            GameData.GetActivePiece().MoveSpaces(dieScript.value, 0);
        }
        RollCheckIsOccuring = false;
    }
}