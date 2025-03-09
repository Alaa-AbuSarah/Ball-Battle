using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickAudioPlayer : MonoBehaviour
{
    public void play() => AudioManager.Instance?.PlayClick();
}
