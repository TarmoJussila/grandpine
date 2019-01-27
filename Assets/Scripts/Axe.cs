using UnityEngine;

public class Axe : MonoBehaviour
{
    public bool IsCollected { get; private set; }

    public void Collect()
    {
        if (IsCollected)
        {
            return;
        }

        IsCollected = true;
        PlayerController.Instance.EnableAxe();
        gameObject.SetActive(false);
    }
}