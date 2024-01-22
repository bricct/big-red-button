using GameNetcodeStuff;
using UnityEngine;
using Random = System.Random;

public class BigRedButtonController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioClip special;
    public AudioSource audioSource;
    public InteractTrigger trigger;

    private Random random = new Random();

    void Start()
    {
        Debug.Log("subscribing to trigger event");
        trigger.onInteract.AddListener(Trigger);
    }
    public void Trigger(PlayerControllerB player)
    {
        if (audioClips != null)
        {
            var playSpecial = random.Next(0, 100) < 4;
            if (playSpecial)
            {
                Debug.Log("Special Event Triggered");
            }
            var clip = playSpecial ? special : audioClips[random.Next(0, audioClips.Length)];

            audioSource.PlayOneShot(clip);
            WalkieTalkie.TransmitOneShotAudio(audioSource, clip);
        }
    }
}