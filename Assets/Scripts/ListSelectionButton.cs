using TMPro;
using UnityEngine;

public class ListSelectionButton : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    public void SetTitle(string title)
    {
        this.title.text = title;
    }
    public void SetDescription(string description)
    {
        this.description.text = description;
    }
}
