using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public Button button;
    public Sprite playSprite;
    public Sprite stopSprite;

    private float _timeStep;
    private Orbit _orbit;

    private void Awake()
    {
        _timeStep = Manager.TimeStep;
    }

    public void ChangeImage()
    {
        if(button.image.sprite == playSprite)
        {
            button.image.sprite = stopSprite;
            Manager.TimeStep = 0;
            Orbit.mainPhysics = false;
        }
        else
        {
            button.image.sprite = playSprite;
            Manager.TimeStep = _timeStep;
            Orbit.mainPhysics = true;

        }
        
    }
}
