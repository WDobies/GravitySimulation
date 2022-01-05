using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public Button pauseButton;
    public Sprite playSprite;
    public Sprite stopSprite;

    public Button uiHideButton;
    public GameObject editPanel;
    public Sprite hideSprite;
    public Sprite showSprite;

    private float _timeStep;
    private Orbit _orbit;

    private void Awake()
    {
        _timeStep = Manager.TimeStep;
    }

    public void ChangePauseImage()
    {
        if(pauseButton.image.sprite == playSprite)
        {
            pauseButton.image.sprite = stopSprite;
            Manager.TimeStep = 0;
            Orbit.mainPhysics = false;
        }
        else
        {
            pauseButton.image.sprite = playSprite;
            Manager.TimeStep = _timeStep;
            Orbit.mainPhysics = true;
        }
    }

    public void ChangeUIHideImage()
    {
        if (uiHideButton.image.sprite == hideSprite)
        {
            editPanel.gameObject.SetActive(false);
            uiHideButton.image.sprite = showSprite;
        }
        else
        {
            editPanel.gameObject.SetActive(true);
            uiHideButton.image.sprite = hideSprite;
        }
    }
}
