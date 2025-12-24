using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "SampleScene";

    [SerializeField] private GameObject playerPrefab = null;

    private AsyncOperation _asyncOperation;
    private SceneContext _mainSceneContext;
    private DiContainer _container;

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }

    public void Start()
    {
        Debug.Log("Game Bootstrapper Started");
        StartCoroutine(LoadGameRoutine());

        //Инициализация игровых систем
        //Загружаем персонажа
        //Загружаем основной уровень
        //Загружаем сохраненные данные
        //Инициализируем UI
    }
    private IEnumerator LoadGameRoutine()
    {
        yield return StartCoroutine(LoadSceneAsync());

        CreatePlayer();

        Debug.Log("Game Bootstrapper: All systems initialized");
    }

    private IEnumerator LoadSceneAsync()
    {
        Debug.Log("Start loading scene: " + mainSceneName);

        _asyncOperation = SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Additive);
        _asyncOperation.allowSceneActivation = true;

        while (!_asyncOperation.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));

        Debug.Log($"Scene '{mainSceneName}' loaded and activated");
    }

    private void CreatePlayer()
    {
        Debug.Log("Character initialization...");
        try
        {
            if (playerPrefab == null)
                throw new ArgumentNullException($"Failed to load SceneContext!");

            var player = _container.InstantiatePrefab(playerPrefab);
            player.name = "Player";
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(mainSceneName));

            Debug.Log("Player created through main scene container");
        }
        catch (ArgumentNullException ane)
        {
            Debug.LogError($"An error occurred while creating your character: {ane.Message}");
            throw;
        }

    }
}
