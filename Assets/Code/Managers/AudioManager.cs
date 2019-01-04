using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager responsible for all audio playback, such as sound effects, music, and voice
/// Contains methods for playing, pausing, and stopping audio audio
/// All managers implement the singleton pattern to prevent the existance of multiple instances of the same manager in the scene
/// </summary>
/// <remarks>
/// The code in this script is based on a publically available YouTube tutorial produced by user Brackeys (2017). For the full reference to this source, along with others used for this project, see 'Mechanics Prototype\Documentation\AINT254 - Coursework 01 - Reference List.pdf'
/// </remarks>
public class AudioManager : Manager
{
    /// <summary>
    /// Nested class defining a sound to be operated on by AudioManager's methods 
    /// </summary>
    [System.Serializable]
    private class Sound
    {
        /// <summary>
        /// Represents the name of the sound. Used to reference it in AudioManager methods
        /// </summary>
        public string name = null;

        /// <summary>
        /// AudioClip for the sound. Contains actual sound file to be played
        /// </summary>
        public AudioClip clip = null;

        /// <summary>
        /// Controls volume of the AudioClip
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        /// <summary>
        /// Controls pitch of the AudioClip
        /// </summary>
        [Range(0.1f, 3f)]
        public float pitch = 1.0f;

        /// <summary>
        /// Controls whether or not the AudioClip should loop when played
        /// </summary>
        public bool loop = false;
        /// <summary>
        /// Controls whether or not the sound should begin playing immediately upon a scene loading
        /// </summary>
        public bool playOnAwake = false;

        /// <summary>
        /// AudioSource in which to play the AudioClip
        /// Initialised by code
        /// </summary>
        [HideInInspector]
        public AudioSource source = null;
    }

    /// <summary>
    /// Singleton instance for the manager
    /// Initialised inside InitManager
    /// </summary>
    private static AudioManager m_instance = null;
    /// <summary>
    /// List of currently playing Sound instances
    /// When a sound instance is played using Play, it is added to this list
    /// </summary>
    private List<string> m_currentlyPlayingSounds = new List<string>();

    /// <summary>
    /// Stores all Sound instances
    /// Initialised in Inspector
    /// </summary>
    [SerializeField]
    private Sound[] m_sounds;

    /// <summary>
    /// Subscribes to relevant events
    /// </summary>
    private void OnEnable()
    {
        /// Subscribes to sceneLoad event to enable the manager to respond to scene changes
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    /// <summary>
    /// Unsubscribes from events when the Manager is disabled
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    /// <summary>
    /// Calls initialisation methods for the manager
    /// </summary>
    private void Awake()
    {
        InitManager();
        SetReferences();
        AddSounds();
    }

    /// <summary>
    /// Property to access the manager's singleton instance outside this class
    /// Can be set privately only
    /// </summary>
    /// <value>Publically gets and privately sets the value of the class's singleton instance, m_instance</value>
    public static AudioManager Instance
    {
        get
        {
            return m_instance;
        }

        private set
        {
            m_instance = value;
        }
    }

    /// <summary>
    /// Creates AudioSource components for each Sound created in the Inspector
    /// </summary>
    private void AddSounds()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            m_sounds[i].source = gameObject.AddComponent<AudioSource>();
            /// Initialises the properties of the newly created AudioSources with the values of each Sound instance's properties
            m_sounds[i].source.clip = m_sounds[i].clip;
            m_sounds[i].source.volume = m_sounds[i].volume;
            m_sounds[i].source.pitch = m_sounds[i].pitch;
            m_sounds[i].source.loop = m_sounds[i].loop;
            m_sounds[i].source.playOnAwake = m_sounds[i].playOnAwake;
        }
    }
 
    /// <summary>
    /// Plays the specified sound, if it is not already playing
    /// </summary>
    /// <param name="_name">The name of the sound to play</param>
    public void Play(string _name)
    {
        /// Iterates through all Sound instances to find one with a matching name
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                /// If the specified sound is not already playing and has a non-null AudioSource, plays it
                if (m_sounds[i].source != null && !m_sounds[i].source.isPlaying)
                {
                    /// Access the AudioSource of the Sound instance to play the AudioClip
                    m_sounds[i].source.Play();
                    Debug.Log("Playing audio: " + _name);

                    /// Adds the Sound to the list of currently playing sounds
                    m_currentlyPlayingSounds.Add(_name);
                    Debug.Log("Audio added: " + _name);
                }
                /// If the specified sound is already playing, logs a message
                else if (m_sounds[i].source.isPlaying)
                {
                    Debug.Log("Sound: " + _name + " is already playing");
                }
                else
                {
                    Debug.Log("Sound: " + _name + " was not found.");
                }
            }
        }
    }
 
    /// <summary>
    /// Stops the specified sound if it is playing
    /// </summary>
    /// <param name="_name">The name of the sound to stop</param>
    public void Stop(string _name)
    {
        /// Iterates through all Sound instances to find one with a matching name
        /// Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                /// If the sound is playing, stops it
                if (m_sounds[i].source.isPlaying)
                {
                    /// Access the AudioSource of the Sound instance to stop it
                    m_sounds[i].source.Stop();
                    Debug.Log("Audio Stopped: " + _name);

                    /// Removes the sound from the currently playing list
                    if (m_currentlyPlayingSounds.Contains(_name))
                    {
                        m_currentlyPlayingSounds.Remove(_name);
                        Debug.Log("Audio Removed: " + _name);
                    }
                }
                /// If the specified sound is not playing, logs a message
                else
                {
                    Debug.Log("Sound: " + _name + " is not playing");
                }
            }
        }
    }

    /// <summary>
    /// Pauses the specified sound
    /// </summary>
    /// <param name="_name">The name of the sound to pause</param>
    public void Pause(string _name)
    {
        /// Iterates through all Sound instances to find one with a matching name
        /// Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                /// If the specified sound is playing, pauses it
                if (m_sounds[i].source.isPlaying)
                {
                    /// Access the AudioSource of the Sound instance to pause it 
                    m_sounds[i].source.Pause();
                    Debug.Log("Sound: " + _name + " paused");
                }
                /// If the specified sound is not playing, logs a message
                else
                {
                    Debug.Log("Sound: " + _name + " is not playing");
                }
            }
        }
    }

    /// <summary>
    /// Stops all currently playing sounds
    /// </summary>
    public void StopAll()
    {
        /// Iterates through all sound instances, stopping each one
        /// Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            /// If a Sound instance's AudioSource is playing, stops it
            if (m_sounds[i].source.isPlaying)
            {
                /// Accesses the AudioSource to stop it playing
                m_sounds[i].source.Stop();
                /// Clears the currently playing sounds list
                m_currentlyPlayingSounds.Clear();
            }
        }

        /// Logs a message upon stoppig all audio
        Debug.Log("All audio stopped");
    }

    /// <summary>
    /// Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    /// Contains other initialisation code for a manager
    /// </summary>
    protected override void InitManager()
    {
        /// Instantiates the instance if has not been already
        if (m_instance == null)
        {
            m_instance = this;
        }
        /// If an instance already exists, destroys this one
        else
        {
            Destroy(gameObject);
        }

        /// Sets this instance to be not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Method compatible with SceneManager.sceneLoad event
    /// Enables managers to respond to a scene being loaded, for example by reacquiring references to objects in the scene
    /// </summary>
    /// <param name="_scene">Scene reference</param>
    /// <param name="_mode">LoadSceneMode reference</param>
    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    /// <summary>
    /// Contains code to get references to objects the manager depends on
    /// Should be called inside NewLevelLoaded
    /// </summary>s
    protected override void SetReferences()
    {
        Debug.Log("AudioManager: " + NO_REFERENCES_MESSAGE);
    }
}