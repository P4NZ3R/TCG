// /*******************************************************
//  * Copyright (c) 2016 Brain in the Box Games srl <Bitbgames@gmail.com>
//  *
//  * This file is part of Voodoo.
//  *
//  * Voodoo can not be copied and/or distributed without the express
//  * permission of Brain in the Box Games srl
//  *******************************************************/

using UnityEngine;
using UnityEngine.Networking;
using BitB.Player;
using UnityEngine.SceneManagement;

#if !UNITY_EDITOR
using BitB.ReadFile;
#endif

public class NetworkManagerExtended : NetworkManager
{
    /// <summary>
    /// Struttura dati per l'oggetto da spawnare all'inizializzazione del server.
    /// </summary>
    [System.Serializable]
    public struct SpawnObject
    {
        public GameObject prefab;
        public string spawnPointName;
        public Vector3 position;
        public Vector3 rotation;
    }

    static NetworkManagerExtended singleton;

    public static NetworkManagerExtended Instance { get { return singleton; } }

    public string pathKillLog = "NetworkedResources/KillLog";
    public string pathChatLog = "NetworkedResources/ChatLog";
    public string pathRagdollList = "Utility/RagdollList";

    [HideInInspector]public bool spawnServer;
    [HideInInspector]public bool spectator;
    public string seed = "darisa";

    /// <summary>
    /// Array di oggetti da spawnare all'inizializzazione del server.
    /// </summary>
    public SpawnObject[] spawnObjects;

    /// <summary>
    /// Insieme di caratteri che possono venire usati per generare il nuovo seme.
    /// </summary>
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    //    void Awake()
    //    {
    //        // Per scegliere se far fare al giocatore da server.
    //        #if UNITY_EDITOR
    //        spawnServer = PlayerPrefs.GetInt("serverIsPlayer") != 0;
    //        spectator = PlayerPrefs.GetInt("spectator") != 0;
    //        #else
    //            spawnServer = ReadServerSettings.GetSetting("serverIsPlayer", "0") != "0";
    //            spectator = ReadServerSettings.GetSetting("spectator", "0") != "0";
    //        #endif
    //    }

    void OnEnable()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        singleton = this;

        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        SceneManager.sceneUnloaded += OnLevelUnloading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        SceneManager.sceneUnloaded -= OnLevelUnloading;
    }

    void OnDestroy()
    {
        if (singleton == this)
            singleton = null;
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        InitializeServer();
        InitializeSpawns();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == NetworkManagerExtended.Instance.offlineScene && NetworkMasterClient.singleton)
            NetworkMasterClient.singleton.discovery.Initialize();
    }

    void OnLevelUnloading(Scene scene)
    {
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // fai la funzione OnServerAddPlayer di default
        base.OnServerAddPlayer(conn, playerControllerId);

        // manda parametri iniziali degli altri player
        PlayerNew[] players = FindObjectsOfType<PlayerNew>();
        foreach (PlayerNew player in players)
            player.SendInitialParam(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        GameObject ragdollList = Instantiate(Resources.Load<GameObject>(singleton.pathRagdollList));
    }

    /// <summary>
    /// Server objects initialization.
    /// </summary>
    public static void InitializeServer()
    {
        // elimina la camera sul server
        if (Camera.main != null)
        {
            FlareLayer fl = Camera.main.transform.GetComponent<FlareLayer>();
            if (fl != null)
                Destroy(fl);
            GUILayer gl = Camera.main.transform.GetComponent<GUILayer>();
            if (gl != null)
                Destroy(gl);
            Destroy(Camera.main);
        }

        // elimina tutti gli AudioListeners perché deve essercene solo uno
        AudioListener[] audioListeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
        for (int i = 0; i < audioListeners.Length; i++)
            Destroy(audioListeners[i]);

        // inserisce l'unico AudioListener nella camera se esiste o in questo gameObject se non esiste
        if (Camera.main)
            Camera.main.gameObject.AddComponent<AudioListener>();
        else
            singleton.gameObject.AddComponent<AudioListener>();
    }

    /// <summary>
    /// Spawns initialization.
    /// </summary>
    public static void InitializeSpawns()
    {
        // istanzia il killlog
        GameObject killLog = Instantiate(Resources.Load<GameObject>(singleton.pathKillLog));
        NetworkServer.Spawn(killLog);

        // istanzia il chatlog
        GameObject chatLog = Instantiate(Resources.Load<GameObject>(singleton.pathChatLog));
        NetworkServer.Spawn(chatLog);

        // istanzia gli oggetti da istanziare dal server
        foreach (SpawnObject spawnObject in singleton.spawnObjects)
        {
            if (!spawnObject.prefab)
                continue;
            GameObject spawnPoint = null;
            if (!string.IsNullOrEmpty(spawnObject.spawnPointName))
            {
                spawnPoint = GameObject.Find(spawnObject.spawnPointName);
                if (!spawnPoint)
                    continue;
            }
            Vector3 spawnPos;
            Vector3 spawnRot;
            if (spawnPoint)
            {
                spawnPos = spawnPoint.transform.position;
                spawnRot = spawnPoint.transform.rotation.eulerAngles;
            }
            else
            {
                spawnPos = spawnObject.position;
                spawnRot = spawnObject.rotation;
            }
            SpawnPrefab(spawnObject.prefab, spawnPos, spawnRot);
        }
    }

    /// <summary>
    /// Spawns a GameObject.
    /// </summary>
    /// <param name="pref"> Reference GameObject. </param>
    /// <param name="position"> Position. </param>
    /// <param name="rotation"> Rotation. </param>
    static void SpawnPrefab(GameObject pref, Vector3 position = default(Vector3), Vector3 rotation = default(Vector3))
    {
        if (pref != null)
        {
            GameObject go = (GameObject)Instantiate(pref, position, Quaternion.Euler(rotation));
            NetworkServer.Spawn(go);
        }
    }

    /// <summary>
    /// Respawns the player.
    /// </summary>
    /// <param name="player">Player to respawn.</param>
    public static void RespawnPlayer(PlayerNew player)
    {
        if (player)
        {
            NetworkServer.Destroy(player.gameObject);
            Transform spawnPos = singleton.GetStartPosition();
            GameObject go = (GameObject)Instantiate(NetworkManagerExtended.Instance.playerPrefab, spawnPos.position, spawnPos.rotation);
            NetworkServer.ReplacePlayerForConnection(player.connectionToClient, go, 0);
        }
        else
            Debug.LogError("SpawnManager: player to respawn was null.");
    }
}
