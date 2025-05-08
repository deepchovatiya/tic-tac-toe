using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class plays : MonoBehaviour
{
    GameObject btn;
    Text txt, sx, scoreXText, scoreOText;
    PhotonView vi;
    string plname;
    public GameObject pX, pO, drw;
    bool isPlayerATurn = true;
    int scoreX, scoreO;

    void Start()
    {
        txt = GameObject.Find("Player Name").GetComponent<Text>();
        plname = PhotonNetwork.NickName;
        txt.text = plname;
        vi = GetComponent<PhotonView>();
        btn = GameObject.Find("btn");
        isPlayerATurn = true;
        scoreX = PlayerPrefs.GetInt("scoreX", 0);
        scoreO = PlayerPrefs.GetInt("scoreO", 0);
      //sx = GameObject.Find("sx").GetComponent<Text>();
        scoreXText = GameObject.Find("ScoreXText").GetComponent<Text>();
        scoreOText = GameObject.Find("ScoreOText").GetComponent<Text>();
        UpdateScoreUI();
    }

    void Update()
    {
        UpdateScoreUI();
    }

    public void btncliclevent(int n)
    {
        if ((isPlayerATurn && plname == "Player A") || (!isPlayerATurn && plname == "Player B"))
        {
            string s = "";
            if (plname == "Player A")
            {
                s = "O";
            }
            else
            {
                s = "X";
            }
            btn.transform.GetChild(n).GetComponentInChildren<Text>().text = s;
            btn.transform.GetChild(n).GetComponentInChildren<Button>().interactable = false;
            vi.RPC("views", RpcTarget.All, n, s, !isPlayerATurn);
        }
    }

    [PunRPC]
    void views(int n, string s, bool call)
    {
        btn.transform.GetChild(n).GetComponentInChildren<Text>().text = s;
        btn.transform.GetChild(n).GetComponentInChildren<Button>().interactable = false;

        if (CheckForWin(s))
        {
            print(s + " is the winner!");
            EndGame();
            if (s == "X")
            {
                pX.SetActive(true);
                scoreX += 10; 
                PlayerPrefs.SetInt("scoreX", scoreX); 
            }
            else
            {
                pO.SetActive(true);
                scoreO += 10; 
                PlayerPrefs.SetInt("scoreO", scoreO); 
            }
            UpdateScoreUI();
        }
        else if (IsBoardFull())
        {
            print("It's a draw!");
            EndGame();
            drw.SetActive(true);
        }

        isPlayerATurn = call;
    }

    void UpdateScoreUI()
    {
        scoreXText.text = "Player X Score: " + scoreX.ToString();
        scoreOText.text = "Player O Score: " + scoreO.ToString();
    }

    bool CheckForWin(string playerSymbol)
    {
        if (btn.transform.GetChild(0).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(1).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(2).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        if (btn.transform.GetChild(3).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(4).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(5).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        if (btn.transform.GetChild(6).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(7).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(8).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }

        if (btn.transform.GetChild(0).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(3).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(6).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        if (btn.transform.GetChild(1).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(4).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(7).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        if (btn.transform.GetChild(2).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(5).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(8).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }

        if (btn.transform.GetChild(0).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(4).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(8).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        if (btn.transform.GetChild(2).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(4).GetComponentInChildren<Text>().text == playerSymbol &&
            btn.transform.GetChild(6).GetComponentInChildren<Text>().text == playerSymbol)
        {
            return true;
        }
        return false;
    }

    bool IsBoardFull()
    {
        for (int i = 0; i < 9; i++)
        {
            if (btn.transform.GetChild(i).GetComponentInChildren<Text>().text == "")
            {
                return false;
            }
        }
        return true;
    }

    void EndGame()
    {
        for (int i = 0; i < 9; i++)
        {
            btn.transform.GetChild(i).GetComponentInChildren<Button>().interactable = false;
        }
    }

    [PunRPC]
    void RestartGameForAll()
    {
        for (int i = 0; i < 9; i++)
        {
            btn.transform.GetChild(i).GetComponentInChildren<Text>().text = "";
            btn.transform.GetChild(i).GetComponentInChildren<Button>().interactable = true;
        }
        isPlayerATurn = true;
        pX.SetActive(false);
        pO.SetActive(false);
        drw.SetActive(false);
    }

    public void RestartGame()
    {
        vi.RPC("RestartGameForAll", RpcTarget.All);
    }
}
