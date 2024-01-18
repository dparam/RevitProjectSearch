using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitProjectSearch.App.UserControls.Views;

namespace RevitProjectSearch.App.Updaters
{
    public class UpdaterManager
    {
        private static Updater_CreateRenameDelete _updater = null;
        private UIControlledApplication _uiControlledApp = null;
        private DockablePage _page = null;


        public UpdaterManager(UIControlledApplication uiControlledApp, DockablePage page)
        {
            _uiControlledApp = uiControlledApp;
            _page = page;
        }


        public void Register()
        {
            if (_updater != null) return;

            _updater = new Updater_CreateRenameDelete(_uiControlledApp, _page);

            UpdaterRegistry.RegisterUpdater(_updater);

            ElementClassFilter elementClassFilter_FamilySymbol = new ElementClassFilter(typeof(FamilySymbol));
            ElementClassFilter elementClassFilter_View = new ElementClassFilter(typeof(View));
            ElementClassFilter elementClassFilter_ConduitType = new ElementClassFilter(typeof(ConduitType));
            ElementClassFilter elementClassFilter_GroupType = new ElementClassFilter(typeof(GroupType));
            //ElementClassFilter elementClassFilter_CableTrayType = new ElementClassFilter(typeof(CableTrayType));
            //ElementClassFilter elementClassFilter_DuctType = new ElementClassFilter(typeof(DuctType));

            ElementId familyNameId = new ElementId(BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM);
            ElementId typeNameId = new ElementId(BuiltInParameter.ALL_MODEL_TYPE_NAME);
            ElementId viewNameId = new ElementId(BuiltInParameter.VIEW_NAME);

            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_FamilySymbol, Element.GetChangeTypeParameter(familyNameId));
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_FamilySymbol, Element.GetChangeTypeParameter(typeNameId));
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_View, Element.GetChangeTypeParameter(viewNameId));
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_ConduitType, Element.GetChangeTypeParameter(typeNameId));
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_GroupType, Element.GetChangeTypeParameter(typeNameId));
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_CableTrayType, Element.GetChangeTypeParameter(typeNameId));
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_DuctType, Element.GetChangeTypeParameter(typeNameId));

            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_FamilySymbol, Element.GetChangeTypeElementAddition());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_View, Element.GetChangeTypeElementAddition());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_ConduitType, Element.GetChangeTypeElementAddition());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_GroupType, Element.GetChangeTypeElementAddition());
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_CableTrayType, Element.GetChangeTypeElementAddition());
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_DuctType, Element.GetChangeTypeElementAddition());

            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_FamilySymbol, Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_View, Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_ConduitType, Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_GroupType, Element.GetChangeTypeElementDeletion());
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_CableTrayType, Element.GetChangeTypeElementDeletion());
            //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), elementClassFilter_DuctType, Element.GetChangeTypeElementDeletion());

            //TaskDialog.Show("Title", $"Updater Registred");
        }


        public void Unregister()
        {
            if (_updater == null) return;

            UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());
            _updater = null;

            //TaskDialog.Show("Title", $"Updater Unregistred");
        }
    }
}
