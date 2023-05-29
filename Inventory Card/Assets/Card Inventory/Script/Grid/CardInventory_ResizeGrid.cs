using UnityEngine;
using UnityEngine.UI;

namespace CardInventory
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class CardInventory_ResizeGrid : MonoBehaviour
    {
        //How many elements will have in the line
        [SerializeField] int _numberOfElements = 3;
        //Width of the elements
        [SerializeField] float _elementHeight = 1.4f;

        new RectTransform transform;
        [SerializeField] GridLayoutGroup grid;

        void Awake()
        {
            transform = (RectTransform)base.transform;
        }
        void Start()
        {
            UpdateCellSize();
        }

        void OnRectTransformDimensionsChange()
        {
            UpdateCellSize();
        }

#if UNITY_EDITOR
        [ExecuteAlways]
        void Update()
        {
            UpdateCellSize();
        }
#endif

        void OnValidate()
        {
            transform = (RectTransform)base.transform;
            grid = GetComponent<GridLayoutGroup>();
            UpdateCellSize();
        }

        void UpdateCellSize()
        {
            if (grid.transform.childCount > 0)
            {
                float spacing = (_numberOfElements - 1) * grid.spacing.x;
                float contentSize = transform.rect.width - grid.padding.left - grid.padding.right - spacing;
                float sizePerCell = contentSize / _numberOfElements;
                /*
                 * To leave the "cellsize. Y" smaller than the "cellsize. X" just change the
                 * multiplication sign "*" to split "/"
                 * Example: grid.cellSize = new Vector2(sizePerCell, (sizePerCell / _elementHeight));
                 */
                grid.cellSize = new Vector2(sizePerCell, (sizePerCell * _elementHeight));
            }
        }
    }
}
