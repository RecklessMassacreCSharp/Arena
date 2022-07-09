using UnityEngine;

public class SetActive : MonoBehaviour
{
    public void SetAct() {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
