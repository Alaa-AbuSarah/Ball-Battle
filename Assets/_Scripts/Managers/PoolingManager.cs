using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField] private Character attacker = null;
    [SerializeField] private Character defender = null;

    [Space]

    [SerializeField] protected List<Character> attackers = new List<Character>();
    [SerializeField] protected List<Character> defenders = new List<Character>();

    public Character GetCharacter(CharacterType type)
    {
        Character _ = null;

        switch (type)
        {
            case CharacterType.Attacker:
                _ = attackers.Find(c => c.IsClear);

                if (_ is null)
                {
                    _ = Instantiate(attacker);
                    attackers.Add(_);
                }
                break;
            case CharacterType.Defender:
                _ = defenders.Find(c => c.IsClear);

                if (_ is null)
                {
                    _ = Instantiate(defender);
                    defenders.Add(_);
                }
                break;
        }

        return _;
    }
}
