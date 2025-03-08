using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static Action<Team> FinishRound = null;

    public GameStates States = GameStates.Active;

    [Header("References")]
    [SerializeField] private Team player = null;
    [SerializeField] private Team enemy = null;
    [SerializeField] private Team draw = null;
    [SerializeField] private TMP_Text text = null;
    [SerializeField] private GameObject endGamePanel = null;

    [Space]

    public List<Round> rounds = new List<Round>(5);

    private int roundIndex = 0;

    private void Start()
    {
        StartCoroutine(StartRound());
    }
    public void FinishTheRound(Team team, bool win = true)
    {
        States = GameStates.End;
        RoundTimer.Instance.Stop();
        Team winer = win ? team : Other(team);
        if (team == null) winer = null;
        FinishRound((winer is null) ? draw : winer);
        rounds[roundIndex].Winer = winer;
        roundIndex++;

        text.gameObject.SetActive(true);

        text.text = (winer == player) ? "Win" : "Lose";
        text.color = (winer == player) ? Color.green : Color.red;

        if (winer is null) 
        {
            text.text = "DRAW";
            text.color = Color.black;
        }

        if (roundIndex < rounds.Count)
            StartCoroutine(StartRound(true));
        else
            endGamePanel.SetActive(true);
    }

    private Team Other(Team team) 
    {
        if (team == player) return enemy;
        else return player;
    }

    private IEnumerator StartRound(bool first = false) 
    {
        if (first)
        {
            player.StopEnargyBar();
            enemy.StopEnargyBar();
            yield return new WaitForSeconds(5);
            player.ClearCharacter();
            enemy.ClearCharacter();
        }

        WaitForSeconds wait = new WaitForSeconds(1);

        text.gameObject.SetActive(true);
        text.text = "3";
        text.color = Color.red;
        yield return wait;
        text.text = "2";
        text.color = Color.yellow;
        yield return wait;
        text.text = "1";
        text.color = Color.green;
        yield return wait;
        text.text = "Gooo!";
        yield return wait;
        text.gameObject.SetActive(false);
        
        States = GameStates.Active;

        RoundTimer.Instance.StartTimer(140);

        player.StartRound();
        enemy.StartRound();

        Team attacker = (enemy.characterType == CharacterType.Attacker) ? enemy : player;
        Ball.Instance.Respawn(attacker.transform.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)));
    }
}
