using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{

    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowGameWinUI()
    {
        gameWinUI.SetActive(true);
    }

    void ShowGameLoseUI()
    {
        gameLoseUI.SetActive(true);
    }
}
