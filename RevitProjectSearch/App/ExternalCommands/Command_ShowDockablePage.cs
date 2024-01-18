using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitProjectSearch.App.Helpers;
using System;


namespace RevitProjectSearch.App.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    public class Command_ShowDockablePage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                DockablePaneId id = new DockablePaneId(GlobalData.PAGE_GUID);
                DockablePane dockablePane = commandData.Application.GetDockablePane(id);
                dockablePane.Show();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Title", $"{ex}");
            }

            return Result.Succeeded;
        }
    }
}
