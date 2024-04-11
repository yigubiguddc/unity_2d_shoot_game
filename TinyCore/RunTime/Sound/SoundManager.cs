using UnityEngine;
using UnityEngine.Events;


//Sound Manager
namespace Bronya
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        //Instance
        //public static SoundManager Instance; No need anymore

        //Music
        public AudioSource musicSource;
        private float musicVolume = 1.0f;
        //In noamal condition, the BGM just need only one musicSource but not an array.



        //Sound
        public AudioSource[] soundSources;
        private float soundVolume = 1.0f;
        private int soundSourceIndex;


        //In noarmal contidion, the Sound needs a an array to playe the sounds

        protected override void Awake()
        {
            //Instance = this;    //单例模式
            base.Awake();
            // Music
            GameObject newMusicSource = new GameObject { name = "Music Source"};    //动态创建新的音频游戏物体
            musicSource = newMusicSource.AddComponent<AudioSource>();               //为游戏物体添加组件AudioSource
            newMusicSource.transform.SetParent(transform);                          //将这个物体设置为本物体的子物体（本物体指挂载新音频和本脚本的物品）

            // Sound
            soundSources = new AudioSource[7]; 
            for(int i=0;i<soundSources.Length;i++)
            {
                GameObject newSoundSource = new GameObject { name = $"Sound Source{i + 1}" };
                soundSources[i] = newSoundSource.AddComponent<AudioSource>();
                newSoundSource.transform.SetParent(transform);
            }

        }

        #region Music
        //This is very easy to use this function,just need to input the sound's name as practical paramenters
        public void PlayMusic(string musicName)
        {
            SourceManager.Instance.LoadAsync<AudioClip>($"Music/{musicName}",clip=>
            {
                musicSource.clip = clip;
                musicSource.loop = true;
                musicSource.volume = musicVolume;
                musicSource.Play();
            });   //异步加载
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void ChangeMusicVolume(float volume)
        {
            musicVolume = volume;
            musicSource.volume = musicVolume;
        }

        #endregion

        #region Sound
        public void PlaySound(string soundName,bool isLoop = false,UnityAction<AudioSource> callback = null)
        {
            SourceManager.Instance.LoadAsync<AudioClip>($"Sound/{soundName}", clip =>
             {
                 AudioSource audioSource = soundSources[soundSourceIndex];
                 soundSourceIndex++;
                 soundSourceIndex %= soundSources.Length;   //限定长度
                 audioSource.clip = clip;
                 audioSource.loop = isLoop;
                 audioSource.volume = soundVolume;
                 audioSource.Play();
                 callback?.Invoke(audioSource);
              });
        }

        public void StopSound(string soundName)
        {
            foreach(AudioSource audioSource in soundSources)
            {
                if(audioSource.isPlaying&&audioSource.clip.name == soundName)
                {
                    audioSource.Stop();
                }

            }
        }


        public void StopAllSound()
        {
            foreach (AudioSource audioSource in soundSources)
            {
                if (audioSource.isPlaying )
                {
                    audioSource.Stop();
                }

            }
        }

        public void ChangeSoundVolume(float volume)
        {
            soundVolume = volume;
            foreach (AudioSource audioSource in soundSources)
            {
                audioSource.volume = soundVolume;
            }
        }


        #endregion


    }
}

