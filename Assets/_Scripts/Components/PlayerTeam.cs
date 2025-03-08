using UnityEngine;

public class PlayerTeam : Team
{
    private void OnEnable()
    {
        PlayerInput.PlayerClicksOnField -= OnPlayerClicksOnField;
        PlayerInput.PlayerClicksOnField += OnPlayerClicksOnField;
    }
    private void OnDisable() => PlayerInput.PlayerClicksOnField -= OnPlayerClicksOnField;

    private void OnPlayerClicksOnField(Vector3 point)
    {
        if (GameManager.Instance.States != GameStates.Active) return;
        SpawnCharacter(point);
    }
}
