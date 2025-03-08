using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public int Points { get => energyBar.Value; set => energyBar.Value = value; }

    public string teamName = "Team";
    public CharacterType characterType = CharacterType.None;
    [SerializeField] private List<Character> characters = new List<Character>();
    public Transform opponentGate = null;
    public Color color = Color.white;
    [SerializeField] private EnergyBar energyBar = null;

    protected void SpawnCharacter(Vector3 point)
    {
        if (GameManager.Instance.States != GameStates.Active) return;

        Character character = PoolingManager.Instance.GetCharacter(characterType);
        if (Points < character.Points)
        {
            character.Clear();
            return;
        }

        Points -= character.Points;
        character.Activate(this, point);
        characters.Add(character);
    }

    public Character GetNextCharacter(Character character) 
    {
        Character _ = characters.Find(c => c != character && c.Active);
        if (_ is null) return null;
        float dis = Vector3.Distance(character.transform.position, _.transform.position);
        foreach (Character c in characters)
        {
            if (c != character && c.Active && Vector3.Distance(c.transform.position, _.transform.position) < dis)
                _ = c;
        }

        return _;
    }

    public virtual void StartRound() 
    {

        if (characterType == CharacterType.Attacker)
            characterType = CharacterType.Defender;
        else
            characterType = CharacterType.Attacker;

        energyBar.Title = $"{teamName} ( {characterType.ToString()} )";
        energyBar.Activate();
        OnStartRound();
    }
    protected abstract void OnStartRound();

    public void ClearCharacter()
    {
        characters.ForEach(c => c.Clear());
        characters.Clear();
    }

    public void StopEnargyBar() => energyBar.Stop();
}
