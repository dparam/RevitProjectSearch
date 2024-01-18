using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitProjectSearch.App.Pages;
using RevitProjectSearch.App.Ribbons;
using RevitProjectSearch.App.Updaters;
using RevitProjectSearch.App.UserControls.ViewModels;
using RevitProjectSearch.App.UserControls.Views;
using System;


namespace RevitProjectSearch.App
{
    public class MainApp : IExternalApplication
    {
        private DockablePage _page = null;
        private UpdaterManager _updaterManager = null;
        private EventHandler<ViewActivatedEventArgs> _eventHandler = null;


        public Result OnStartup(UIControlledApplication uiControlledApp)
        {
            CreateRibbonPanel(uiControlledApp);
            RegisterDockablePage(uiControlledApp);
            SubscribePageEvents(uiControlledApp);
            RegisterUpdaters(uiControlledApp);

            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication uiControlledApp)
        {
            UnsubscribePageEvents(uiControlledApp);
            UnregisterUpdaters(uiControlledApp);

            return Result.Succeeded;
        }


        //


        private void CreateRibbonPanel(UIControlledApplication uiControlledApp)
        {
            string tabName = "RPS";
            uiControlledApp.CreateRibbonTab(tabName);
            RibbonPanel ribbonPanel = uiControlledApp.CreateRibbonPanel(tabName, "RPS");

            RibbonPanelHelpers.CreateButton(
                ribbonPanel,
                "BTN_PanelShow",
                "Show Panel",
                "RevitProjectSearch.App.ExternalCommands.Command_ShowDockablePage",
                "icon_Default.ico");
        }


        private void RegisterDockablePage(UIControlledApplication uiControlledApp)
        {
            PageManager pageManager = new PageManager(uiControlledApp);
            _page = pageManager.Register();
        }


        private void SubscribePageEvents(UIControlledApplication uiControlledApp)
        {
            if (_page == null) return;

            MainViewModel mainViewModel = _page.DataContext as MainViewModel;
            _eventHandler = new EventHandler<ViewActivatedEventArgs>(mainViewModel.OnViewActivated);

            uiControlledApp.ViewActivated += _eventHandler;
        }

        private void UnsubscribePageEvents(UIControlledApplication uiControlledApp)
        {
            if (_page == null) return;

            uiControlledApp.ViewActivated -= _eventHandler;
        }


        private void RegisterUpdaters(UIControlledApplication uiControlledApp)
        {
            if (_page == null) return;

            _updaterManager = new UpdaterManager(uiControlledApp, _page);
            _updaterManager.Register();
        }


        private void UnregisterUpdaters(UIControlledApplication uiControlledApp)
        {
            if (_updaterManager == null) return;

            _updaterManager.Unregister();
        }
    }
}
