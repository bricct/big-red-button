using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
using Random = System.Random;

namespace BigRedButton.Behaviors
{
    public class BigRedButtonController : NetworkBehaviour
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

        public void Trigger(PlayerControllerB player) => PlaySoundServerRpc();


        [ServerRpc(RequireOwnership = false)]
        public void PlaySoundServerRpc()
        {
            var playSpecial = random.Next(0, 100) < 4;
            if (playSpecial)
            {
                Debug.Log("Special Event Triggered");
                PlaySoundClientRpc(-1);
            }
            else
            {
                PlaySoundClientRpc(random.Next(0, audioClips.Length));
            }
        }

        [ClientRpc]
        public void PlaySoundClientRpc(int index)
        {
            AudioClip clip;
            if (index == -1)
            {
                clip = special;
            }
            else
            {
                clip = audioClips[index];
            }

            audioSource.PlayOneShot(clip);
            WalkieTalkie.TransmitOneShotAudio(audioSource, clip);
        }
    }
}