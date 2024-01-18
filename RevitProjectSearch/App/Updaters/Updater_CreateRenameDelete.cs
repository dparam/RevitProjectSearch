using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitProjectSearch.App.Helpers;
using RevitProjectSearch.App.UserControls.ViewModels;
using RevitProjectSearch.App.UserControls.Views;
using System;


namespace RevitProjectSearch.App.Updaters
{
    public class Updater_CreateRenameDelete : IUpdater
    {
        private AddInId _appId = null;
        private UpdaterId _updaterId = null;
        private UIControlledApplication _uiControlledApp = null;
        private DockablePage _page = null;


        public Updater_CreateRenameDelete(UIControlledApplication uiControlledApp, DockablePage page)
        {
            _uiControlledApp = uiControlledApp;
            _appId = _uiControlledApp.ControlledApplication.ActiveAddInId;
            _page = page;
            _updaterId = new UpdaterId(_appId, GlobalData.UPDATER_GUID);
        }


        public void Execute(UpdaterData data)
        {
            //try
            //{

            if (_page == null) return;
            if (_page.DataContext == null) return;

            MainViewModel mainViewModel = _page.DataContext as MainViewModel;
            mainViewModel.UpdateCollection();

            //TaskDialog.Show("Updater.Execute", "Update Executed");
            //}
            //catch (Exception ex)
            //{
            //    TaskDialog.Show("Updater.Execute", $"{ex}");
            //}
        }


        public string GetAdditionalInformation() => "Info: Updater_CreateRenameDelete";
        public ChangePriority GetChangePriority() => ChangePriority.GridsLevelsReferencePlanes;
        public UpdaterId GetUpdaterId() => _updaterId;
        public string GetUpdaterName() => "Updater_CreateRenameDelete";
    }
}
