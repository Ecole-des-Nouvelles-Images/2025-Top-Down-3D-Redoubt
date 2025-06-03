using System.Collections;
using System.Collections.Generic;
using Hugo.I.Scripts.Utils;
using UnityEngine;

namespace Hugo.I.Scripts.Sounds
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        [Header("<size=14><color=#E74C3C>   SOUNDS</color></size>")]
        [Space(3)]
        [Header("<size=13><color=#58D68D>Ambiance</color></size>")]
        [SerializeField] public List<AudioClip> AmbianceSounds;
        
        [Header("<size=13><color=#58D68D>Player</color></size>")]
        [SerializeField] public List<AudioClip> PlayerFootStepSounds;
        [SerializeField] public List<AudioClip> PlayerCollectSounds;
        
        [Header("<size=13><color=#58D68D>Enemy</color></size>")]
        [SerializeField] public List<AudioClip> EnemyFootStepSounds;
        [SerializeField] public List<AudioClip> EnemyAttackImpactSounds;

        [Header("<size=13><color=#58D68D>Tower</color></size>")]
        [SerializeField] public List<AudioClip> LevelUpSounds;
        [SerializeField] public List<AudioClip> HealingSounds;
        [SerializeField] public List<AudioClip> ReloadingSounds;
        
        [Header("<size=13><color=#58D68D>Resource</color></size>")]
        [SerializeField] public List<AudioClip> BreakingSounds;
        
        public void PlaySound(GameObject obj, List<AudioClip> clips)
        {
            AudioSource audio = obj.AddComponent<AudioSource>();
            audio.clip = clips[Random.Range(0, clips.Count)];
            audio.Play();
            float durationClip = audio.clip.length;
            StartCoroutine(RemoveAudioSource(durationClip, audio));
        }
        
        public IEnumerator RemoveAudioSource(float timer, AudioSource source)
        {
            yield return new WaitForSeconds(timer);
            Destroy(source);
        }
    }
}
