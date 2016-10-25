using UnityEngine;
using UnityEngine.UI;

public class ButtonTapCount : MonoBehaviour {

    public int tapCount = 0;
    //public Text text;
    
    public void IncrementTapCount() {
        tapCount++;
        //text.text = tapCount.ToString();
    }
}
