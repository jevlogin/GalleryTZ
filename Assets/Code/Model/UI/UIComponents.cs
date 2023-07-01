using System;
using System.Collections.Generic;


namespace WORLDGAMEDEVELOPMENT
{
    [Serializable]
    public sealed class UIComponents
    {
        #region Fields

        internal CanvasView CanvasView;
        internal UIPanelMenuView PanelMenuUIView;
        internal UIPanelLoadingView PanelLoadingUIView;
        internal UIPanelGalleryView PanelGalleryUIView;
        internal UIPanelImageView PanelImageUIView;
        internal List<IPanelUIView> ListPanelViews;

        #endregion
    }
}