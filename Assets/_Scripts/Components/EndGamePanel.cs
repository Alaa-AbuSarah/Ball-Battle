using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text text = null;

    [Space]

    [SerializeField] private Sprite winIcon = null;
    [SerializeField] private Sprite loseIcon = null;
    [SerializeField] private Sprite drawIcon = null;

    [Space]

    [SerializeField] private List<Image> icons = new List<Image>(5);
    [SerializeField] private GameObject mazeButton = null;

    [Space]

    [SerializeField] private List<GameObject> roundUI = new List<GameObject>();
    [SerializeField] private List<GameObject> mazeUI = new List<GameObject>();

    [Space]

    [SerializeField] private GameObject mazeAttacker = null;

    private int playerPoints = 0;
    private int enemyPoints = 0;
    private void OnEnable()
    {
        playerPoints = 0;
        enemyPoints = 0;
        Team team = null;

        for (int i = 0; i < icons.Count; i++)
        {
            team = GameManager.Instance.rounds[i].Winer;
            if(team is null)
                icons[i].sprite = drawIcon;
            else if (team.teamName == "Player")
            {
                icons[i].sprite = winIcon;
                playerPoints++;
            }
            else if (team.teamName == "Enemy")
            {
                icons[i].sprite = loseIcon;
                enemyPoints++;
            }
        }

        if (playerPoints > enemyPoints) text.text = "Win";
        else if (enemyPoints > playerPoints) text.text = "Lose";
        else 
        {
            text.text = "DRAW";
            mazeButton.SetActive(true);
        }
    }

    public void PlayMaze() 
    {
        roundUI.ForEach(u => u.gameObject.SetActive(false));
        mazeUI.ForEach(u => u.gameObject.SetActive(true));

        mazeAttacker.SetActive(true);
        gameObject.SetActive(false);
    }
}
