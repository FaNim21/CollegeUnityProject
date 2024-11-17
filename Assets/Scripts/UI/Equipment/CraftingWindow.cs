using Main.Datas;
using System.Collections.Generic;
using UnityEngine;

namespace Main.UI.Equipment
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemData itemData;
        public int amount;
    }

    [System.Serializable]
    public class Recipe
    {
        public ItemData item;
        public Ingredient[] ingredients;
    }

    public class CraftingWindow : MonoBehaviour, IWindowControl
    {
        public bool IsActive => gameObject.activeSelf;

        public RecipeContainer recipePrefab;
        public Transform recipesContent;
        public Inventory inventory;

        [Header("Components")]
        [SerializeField] private CanvasHandle _canvasHandle;

        [Header("Values")]
        [SerializeField] private Recipe[] _recpiesData;

        private readonly List<RecipeContainer> _recipies = new();


        private void Awake()
        {
            _canvasHandle.AddWindowToControl(this);
        }
        private void Start()
        {
            for (int i = 0; i < _recpiesData.Length; i++)
            {
                var container = Instantiate(recipePrefab, recipesContent);
                container.Initialize(this, _recpiesData[i]);
                _recipies.Add(container);
            }
        }

        //i know that ingredients are not updated in all situations, but there is no need for that when it updates when click craft button 
        public void UpdateIngredients()
        {
            for (int i = 0; i < _recipies.Count; i++)
            {
                var current = _recipies[i];
                current.UpdateIngredientsInfo();
            }
        }

        public void OpenWindow()
        {
            gameObject.SetActive(true);
            UpdateIngredients();
        }
        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
