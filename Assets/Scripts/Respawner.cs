using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public static Respawner Instance;

    #region Variables
    public List<Transform> RespawnPoints;

    private const string path = "Assets/Resources/Save.txt";
    private int index;
    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < RespawnPoints.Count; i++)
            RespawnPoints[i].GetComponent<RespawnPoint>().Index = i;
    }

    public void UpdateIndex(int _index)
    {
        StreamWriter save;

        if(File.Exists(path))
            File.Delete(path);

        save = File.CreateText(path);
        save.WriteLine(_index.ToString());
        save.Close();
    }

    public void Respawn(Transform _player)
    {
        string test = File.ReadAllLines(path)[0];
        _player.position = RespawnPoints[int.Parse(test)].position;
    }
}
