using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public int Points = 10;

    [SerializeField] private CharacterType characterType = CharacterType.None;
    [SerializeField] private List<Character> characters = new List<Character>();
    public Transform opponentGate = null;
    public Color color = Color.white;

    protected void SpawnCharacter(Vector3 point)
    {
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
        return characters.Find(c => c != character && c.Active);
    }
}
