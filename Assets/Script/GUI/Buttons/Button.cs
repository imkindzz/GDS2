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

        if (sceneName == "Menu" || sceneName == "Difficulty")
        {
            SoundManager.instance.PlayMusic(MusicName.Menu);
        }
        else if (sceneName == "Hard" || sceneName == "Easy")
        {
            SoundManager.instance.PlayMusic(MusicName.GoblinNormal);
            SoundManager.instance.PreloadMusic(MusicName.GoblinBoss);
        }
        else if (sceneName == "Level 2" || sceneName == "Level 2 Easy")
        {
            SoundManager.instance.PlayMusic(MusicName.VillageNormal);
            SoundManager.instance.PreloadMusic(MusicName.VillageBoss);
        }
        else if (sceneName == "Level 3" || sceneName == "Level 3 Easy")
        {
            SoundManager.instance.PlayMusic(MusicName.CastleNormal);
            SoundManager.instance.PreloadMusic(MusicName.CastleBoss);
        }
    }
}
