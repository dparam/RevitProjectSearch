using Autodesk.Revit.UI;
using RevitProjectSearch.App.Helpers;
using RevitProjectSearch.App.UserControls.Views;
using System;


namespace RevitProjectSearch.App.Pages
{
    public class PageManager
    {
        private UIControlledApplication _uiControlledApp = null;
        private DockablePage _page = null;


        public PageManager(UIControlledApplication uiControlledApp)
        {
            _uiControlledApp = uiControlledApp;
        }


        public DockablePage Register()
        {
            _page = new DockablePage();
            DockablePaneId id = new DockablePaneId(GlobalData.PAGE_GUID);

            _uiControlledApp.RegisterDockablePane(id, "Поиск в проекте", _page);

            return _page;
        }


        public static bool CheckDockableVisibility(UIControlledApplication uiControlledApp)
        {
            DockablePaneId id = new DockablePaneId(GlobalData.PAGE_GUID);
            return uiControlledApp.GetDockablePane(id).IsShown();
        }
    }
}
