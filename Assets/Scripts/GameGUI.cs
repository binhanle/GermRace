using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUI : MonoBehaviour
{
    private static Canvas rollScreen;
    private static Text titleText;

    public static void ShowRollScreen()
    {
        // Displays the roll screen
        rollScreen.enabled = true;

        // Show who's rolling
        Player currPlayer = GameData.GetCurrPlayer();
        titleText.text = currPlayer.GetName() + " roll";
    }

    public static void HideRollScreen()
    {
        // Hides the roll screen
        rollScreen.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        rollScreen = GameObject.Find("Roll Screen").GetComponent<Canvas>();
        titleText = GameObject.Find("Title Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
