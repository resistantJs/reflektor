using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* The code in this script is based on a publically available YouTube tutorial produced by user Brackeys (2017)
 * For the full reference to this source, along with others used for this Design Challenge 01, see 'Mechanics Prototype\Documentation\AINT254 - Coursework 01 - Reference List.pdf'
 */

/* Manager responsible for all audio playback, such as sound effects, music, and voice
 * Contains methods for playing, pausing, and stopping audio audio
 */
public class AudioManager : Manager
{
    /* Nested class defining a sound to be operated on by AudioManager's methods 
     */
    [System.Serializable]
    private class Sound
    {
        // Name of the sound. Used to reference it in AudioManager methods
        public string name = null;

        // AudioClip for the sound. Contains actual sound file to be played
        public AudioClip clip = null;

        // Controls volume of the AudioClip
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        // Controls pitch of the AudioClip
        [Range(0.1f, 3f)]
        public float pitch = 1.0f;

        // Controls whether or not the AudioClip should loop when played
        public bool loop = false;
        // Controls whether or not the sound should begin playing immediately upon a scene loading
        public bool playOnAwake = false;

        // AudioSource in which to play the AudioClip
        // Initialised by code
        [HideInInspector]
        public AudioSource source = null;
    }

    // Singleton instance for the AudioManager
    // Initialised inside InitManager
    private static AudioManager m_instance = null;
    // List of currently playing Sound instances
    // When a sound instance is played using Play, it is added to this list
    private List<string> m_currentlyPlayingSounds = new List<string>();

    // Stores all Sound instances
    // Initialised in Inspector
    [SerializeField]
    private Sound[] m_sounds;

    // Calls initialisation code for the manager
    private void Awake()
    {
        InitManager();
        SetReferences();
        AddSounds();
    }

    // Subscribes to sceneLoad event to enable the manager to respond to scene changes
    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewLevelLoaded;
    }

    // Unsubscribes to sceneLoad when manager is disabled
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= NewLevelLoaded;
    }

    // Property to access the manager's singleton instance outside this class
    // Can be set privately only
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

    // Creates AudioSource components for each Sound created in the Inspector
    private void AddSounds()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            m_sounds[i].source = gameObject.AddComponent<AudioSource>();
            // Initialises the properties of the newly created AudioSources with the values of each Sound instance's properties
            m_sounds[i].source.clip = m_sounds[i].clip;
            m_sounds[i].source.volume = m_sounds[i].volume;
            m_sounds[i].source.pitch = m_sounds[i].pitch;
            m_sounds[i].source.loop = m_sounds[i].loop;
            m_sounds[i].source.playOnAwake = m_sounds[i].playOnAwake;
        }
    }

    // Plays the specified sound, if it is not already playing
    public void Play(string _name)
    {
        // Iterates through all Sound instances to find one with a matching name
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                // If the specified sound is not already playing and has a non-null AudioSource, plays it
                if (m_sounds[i].source != null && !m_sounds[i].source.isPlaying)
                {
                    // Access the AudioSource of the Sound instance to play the AudioClip
                    m_sounds[i].source.Play();
                    Debug.Log("Playing audio: " + _name);

                    // Adds the Sound to the list of currently playing sounds
                    m_currentlyPlayingSounds.Add(_name);
                    Debug.Log("Audio added: " + _name);
                }
                // If the specified sound is already playing, logs a message
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

    // Stops the specified sound if it is playing
    public void Stop(string _name)
    {
        // Iterates through all Sound instances to find one with a matching name
        // Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                // If the sound is playing, stops it
                if (m_sounds[i].source.isPlaying)
                {
                    // Access the AudioSource of the Sound instance to stop it
                    m_sounds[i].source.Stop();
                    Debug.Log("Audio Stopped: " + _name);

                    // Removes the sound from the currently playing list
                    if (m_currentlyPlayingSounds.Contains(_name))
                    {
                        m_currentlyPlayingSounds.Remove(_name);
                        Debug.Log("Audio Removed: " + _name);
                    }
                }
                // If the specified sound is not playing, logs a message
                else
                {
                    Debug.Log("Sound: " + _name + " is not playing");
                }
            }
        }
    }

    // Pauses the specified sound
    public void Pause(string _name)
    {
        // Iterates through all Sound instances to find one with a matching name
        // Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                // If the specified sound is playing, pauses it
                if (m_sounds[i].source.isPlaying)
                {
                    // Access the AudioSource of the Sound instance to pause it 
                    m_sounds[i].source.Pause();
                    Debug.Log("Sound: " + _name + " paused");
                }
                // If the specified sound is not playing, logs a message
                else
                {
                    Debug.Log("Sound: " + _name + " is not playing");
                }
            }
        }
    }

    // Stops all currently playing sounds
    public void StopAll()
    {
        // Iterates through all sound instances, stopping each one
        // Performance could be improved by iterating through only the sounds that are currently playing
        for (int i = 0; i < m_sounds.Length; i++)
        {
            // If a Sound instance's AudioSource is playing, stops it
            if (m_sounds[i].source.isPlaying)
            {
                // Accesses the AudioSource to stop it playing
                m_sounds[i].source.Stop();
                // Clears the currently playing sounds list
                m_currentlyPlayingSounds.Clear();
            }
        }

        // Logs a message upon stoppig all audio
        Debug.Log("All audio stopped");
    }

    // Initialises the singleton instance of the manager and sets it not be destroyed when a new scene is loaded
    // The singleton pattern is used here as there should not be multiple of a single manager type in a scene
    protected override void InitManager()
    {
        // Instantiates the instance if has not been
        if (m_instance == null)
        {
            m_instance = this;
        }
        // If an instance already exists, destroys this one
        else
        {
            Destroy(gameObject);
        }

        // Sets this instance to be not be destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    // Reacquires references to dependencies upon loading a new scene
    protected override void NewLevelLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SetReferences();
    }

    // Sets references to dependencies
    protected override void SetReferences()
    {
        Debug.Log("AudioManager: " + NO_REFERENCES_MESSAGE);
    }
}