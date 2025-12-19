using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    public string mainSceneName;
    

    public void Start()
    {
        //прогружаем новую сцену
        SceneManager.LoadScene(mainSceneName,LoadSceneMode.Additive);
        //создаем игрока
        //подгружаем сохраненые данные
        //генерируем первую мапу


    }
}
