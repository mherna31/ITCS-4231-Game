using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] Text deathDisplay;
    [SerializeField] Text endDisplay;
    int deaths;
    int mTokens;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deaths = gameObject.GetComponent<PlayerMove>().deathCounter;
        mTokens = gameObject.GetComponent<PlayerMove>().currentMuddle;

        deathDisplay.text = "Deaths x " + deaths + " M Tokens x " + mTokens;

        if (gameObject.GetComponent<PlayerMove>().endGame)
        {
            deathDisplay.text = " ";
            endDisplay.text = "Congratulations!\n\n" + "You died " + deaths + " times.";

        }

    }
}
