using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public Button button;
    public Sprite playSprite;
    public Sprite stopSprite;

    public void ChangeImage()
    {
        if(button.image.sprite == playSprite)
        {
            button.image.sprite = stopSprite;
        }
        else
        {
            button.image.sprite = playSprite;
        }
        
    }
}
