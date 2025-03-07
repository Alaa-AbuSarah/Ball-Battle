using System;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int Points = 10;

    [SerializeField] private CharacterType characterType = CharacterType.None;
    [SerializeField] private List<Character> characters = new List<Character>();
    public Transform enemyGate = null;
    private void OnEnable()
    {
        PlayerInput.PlayerClicksOnField -= OnPlayerClicksOnField;
        PlayerInput.PlayerClicksOnField += OnPlayerClicksOnField;
    }
    private void OnDisable() => PlayerInput.PlayerClicksOnField -= OnPlayerClicksOnField;

    private void OnPlayerClicksOnField(Vector3 point)
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
}
