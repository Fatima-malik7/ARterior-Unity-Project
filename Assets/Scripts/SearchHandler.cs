using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SearchHandler : MonoBehaviour
{
    public TMP_InputField searchInput;
    public List<GameObject> productItems; // Your cards or panels

    void Start()
    {
        searchInput.onValueChanged.AddListener(OnSearchChanged);
    }

    void OnSearchChanged(string value)
    {
        string searchText = value.ToLower();

        foreach (GameObject item in productItems)
        {
            // Match by name or tag or label (you can extend this)
            bool match = item.name.ToLower().Contains(searchText);

            // Instead of disabling GameObject, fade out via CanvasGroup
            CanvasGroup cg = item.GetComponent<CanvasGroup>();

            if (cg != null)
            {
                if (match)
                {
                    cg.alpha = 1;
                    cg.blocksRaycasts = true;
                    cg.interactable = true;
                }
                else
                {
                    cg.alpha = 0;
                    cg.blocksRaycasts = false;
                    cg.interactable = false;
                }
            }
        }
    }
}
