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
    [SerializeField] private Shaker cameraShaker = null;

    [Header("SFX")]
    [SerializeField] private AudioClip startRoundClip = null;
    [SerializeField] private AudioClip winRoundClip = null;
    [SerializeField] private AudioClip loseRoundClip = null;
    [SerializeField] private AudioClip drawRoundClip = null;

    [Space]

    public List<Round> rounds = new List<Round>(5);

    private int roundIndex = 0;

    private void Start() => StartCoroutine(StartRound());
    public void FinishTheRound(Team team, bool win = true)
    {
        if (cameraShaker != null) cameraShaker.Shake();
        States = GameStates.End;
        RoundTimer.Instance.Stop();
        Team winer = win ? team : Other(team);
        if (team == null) winer = null;
        FinishRound?.Invoke((winer is null) ? draw : winer);
        rounds[roundIndex].Winer = winer;
        roundIndex++;

        text.gameObject.SetActive(true);

        if (winer == player)
        {
            text.text = "Win";
            text.color = Color.green;
            AudioManager.Instance?.PlaySFX(winRoundClip, 0.75f);
        }
        else if (win == enemy)
        {
            text.text = "Lose";
            text.color = Color.red;
            AudioManager.Instance?.PlaySFX(loseRoundClip, 0.75f);

        }
        else if (winer is null)
        {
            text.text = "DRAW";
            text.color = Color.black;
            AudioManager.Instance?.PlaySFX(drawRoundClip, 0.75f);
        }

        if (roundIndex < rounds.Count)
            StartCoroutine(StartRound(true));
        else 
        {
            player.ClearCharacter();
            enemy.ClearCharacter();
            endGamePanel.SetActive(true);
        }
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
        AudioManager.Instance?.PlayClick();
        yield return wait;
        text.text = "2";
        AudioManager.Instance?.PlayClick();
        text.color = Color.yellow;
        yield return wait;
        text.text = "1";
        AudioManager.Instance?.PlayClick();
        text.color = Color.green;
        yield return wait;
        AudioManager.Instance?.PlaySFX(startRoundClip);
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
