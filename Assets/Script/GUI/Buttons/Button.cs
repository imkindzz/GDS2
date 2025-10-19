using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        switch (sceneName)
        {
            case "Menu":
            case "Difficulty":
                SoundManager.instance.PlayMusic(MusicName.Menu);
                break;
            case "Hard":
            case "Easy":
                SoundManager.instance.PlayMusic(MusicName.GoblinNormal);
                SoundManager.instance.PreloadMusic(MusicName.GoblinBoss);
                break;
            case "Level 2":
            case "Level 2 Easy":
                SoundManager.instance.PlayMusic(MusicName.VillageNormal);
                SoundManager.instance.PreloadMusic(MusicName.VillageBoss);
                break;
            case "Level 3":
            case "Level 3 Easy":
                break;
            default:
                break;
        }
    }
}
