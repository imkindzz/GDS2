using UnityEngine;

public class PlayerSwapManager : MonoBehaviour
{
    public PlayerMovement heart;
    public PlayerMovement soul;

    private AudioSource audioSource;

    // private bool controlsSwapped = false;

    void Start()
    {
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // swap pos. with spacebar
            Vector3 temp = heart.transform.position;
            heart.transform.position = soul.transform.position;
            soul.transform.position = temp;

            SoundManager.instance.PlaySfxSound(SfxSoundName.PlayerWarp, audioSource);

            /* swap input
            if (!controlsSwapped)
            {
                heart.horizontalAxis = "Horizontal2";
                heart.verticalAxis = "Vertical2";

                soul.horizontalAxis = "Horizontal";
                soul.verticalAxis = "Vertical";
            }
            else
            {
                heart.horizontalAxis = "Horizontal";
                heart.verticalAxis = "Vertical";

                soul.horizontalAxis = "Horizontal2";
                soul.verticalAxis = "Vertical2";
            }

            controlsSwapped = !controlsSwapped;
            */
        }
    }
}
