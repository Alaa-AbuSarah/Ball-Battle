using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action<Team> FinishRound = null;

    public GameStates States = GameStates.Active;

    [Space]

    [SerializeField] private Team player = null;
    [SerializeField] private Team enemy = null;
    public void FinishTheRound(Team team, bool win = true)
    {
        FinishRound(win ? team : Other(team));
        States = GameStates.End;
        Debug.Log(win ? team.name : Other(team).name);
    }

    private Team Other(Team team) 
    {
        if (team == player) return enemy;
        else return player;
    }
}
