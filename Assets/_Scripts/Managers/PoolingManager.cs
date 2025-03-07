using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField] private Character Attacker = null;

    [SerializeField] protected List<Character> attackers = new List<Character>();

    public Character GetCharacter(CharacterType type)
    {
        Character _ = null;

        switch (type)
        {
            case CharacterType.Attacker:
                _ = attackers.Find(c => c.IsClear);

                if (_ is null)
                {
                    _ = Instantiate(Attacker);
                    attackers.Add(_);
                }
                break;
            case CharacterType.Defender:
                break;
        }

        return _;
    }
}
