using UnityEngine;
using UnityEngine.UI;


public sealed class UIPanelGalleryView : UIPanelView
{
    #region Fields

    public string UrlBase;
    public string ImagePrefix;
    public int CountImages;

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RawImage[] _images;
    [SerializeField] private float _imageHeight;
    [SerializeField] private int _constraintCount;

    #endregion

    #region Properties

    public ScrollRect ScrollRect => _scrollRect;
    public GridLayoutGroup GridLayoutGroup { get; set; }
    public RawImage[] Images { get => _images; set => _images = value; }
    public int Top { get; set; }
    public int Bottom { get; set; }
    public float SpassingY { get; set; }
    public float ImageHeight { get => _imageHeight; set => _imageHeight = value; }
    public int ConstraintCount { get => _constraintCount; set => _constraintCount = value; }

    #endregion


    private void Start()
    {
        GridLayoutGroup = _scrollRect.content.GetComponent<GridLayoutGroup>();
        Top = GridLayoutGroup.padding.top;
        Bottom = GridLayoutGroup.padding.bottom;
        SpassingY = GridLayoutGroup.spacing.y;
        ImageHeight = GridLayoutGroup.cellSize.y;
        ConstraintCount = GridLayoutGroup.constraintCount;
    }
}
