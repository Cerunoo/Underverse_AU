using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public class AsyncLoading : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;
    [SerializeField] private Button buttonActiveScene;

    [SerializeField] private float timeToLoad;
    
    private AsyncOperation operation;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Screen.fullScreen = true;
            StartCoroutine(LoadAsync(1));
        }
    }

    public IEnumerator LoadAsync(int indexScene)
    {
        // if (SceneManager.GetActiveScene().buildIndex != 0)
        // {
        //     Button[] allButton = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        //     for (int i = 0; i < allButton.Length; i++) allButton[i].interactable = false;
        //     GetComponent<Animator>().SetBool("Show", true);
        // }
        
        yield return new WaitForSeconds(timeToLoad);

        operation = SceneManager.LoadSceneAsync(indexScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            progressSlider.value = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.text = (progressSlider.value * 100).ToString("F0") + "%";
            yield return null;

            if (progressSlider.value == 1)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    buttonActiveScene.interactable = true;

                    StartCoroutine(animationProgressText());
                    IEnumerator animationProgressText()
                    {
                        string[] frames = new string[]{ "", "p", "pr", "pre", "pres", "press", "press.", "press.." };

                        string step = "  ";
                        for (int i = 0; i < 12; i++)
                        {
                            foreach (string fr in frames)
                            {
                                progressText.text = step + fr;
                                yield return new WaitForSeconds(0.36f);
                            }
                            step += "   ";
                        }
                    }
                }
                // else
                // {
                //     operation.allowSceneActivation = true;
                // }
                break;
            }
        }
    }
    public void pressActiveScene() { operation.allowSceneActivation = true; }
}
