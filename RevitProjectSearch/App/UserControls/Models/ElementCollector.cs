using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using RevitProjectSearch.App.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RevitProjectSearch.App.UserControls.Models
{
    public static class ElementCollector
    {
        public static List<SearchItem> CollectProjectElements(Document doc)
        {
            List<SearchItem> resultList = new List<SearchItem>();

            IList<View> views = new FilteredElementCollector(doc).OfClass(typeof(View)).Cast<View>().ToList();
            IList<ElementType> familySymbols = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Cast<ElementType>().ToList();
            IList<ElementType> conduitTypes = new FilteredElementCollector(doc).OfClass(typeof(ConduitType)).Cast<ElementType>().ToList();
            IList<ElementType> groupTypes = new FilteredElementCollector(doc).OfClass(typeof(GroupType)).Cast<ElementType>().ToList();

            resultList.AddRange(CreateSearchItems_View(views));
            resultList.AddRange(CreateSearchItems_ElementType(familySymbols));
            resultList.AddRange(CreateSearchItems_ElementType(conduitTypes));
            resultList.AddRange(CreateSearchItems_ElementType(groupTypes));

            return resultList;
        }


        // Views

        public static List<SearchItem> CreateSearchItems_View(IList<View> viewList)
        {
            List<SearchItem> resultList = new List<SearchItem>();

            foreach (View view in viewList)
            {
                ViewType viewType = view.ViewType;
                string viewTypeName = SearchHelpers.GetViewTypeName(viewType);
                int itemSortOrder = SearchHelpers.GetViewSortingOrder(viewType);

                string viewName = view.Name;

                string sheetNumber = "";
                if (viewType is ViewType.DrawingSheet)
                {
                    ViewSheet viewSheet = view as ViewSheet;
                    sheetNumber = viewSheet.SheetNumber;
                    viewName = $"{sheetNumber} - {viewName}";
                }

                if (viewType is ViewType.ProjectBrowser ||
                    viewType is ViewType.SystemBrowser) continue;

                if (view.IsTemplate) continue;
                if (view.ViewSpecific) continue;

                resultList.Add(new SearchItem
                (
                    itemElement: view,
                    itemName: viewName,
                    itemIcon: SearchHelpers.GetViewIcon(viewType),
                    itemDescription: viewTypeName,
                    itemSearchTags: $"Вид {view.Name} {viewTypeName} {sheetNumber}",
                    itemGroup: viewTypeName,
                    itemSortingOrder: itemSortOrder
                ));
            }

            return resultList;
        }


        // ElementTypes

        public static List<SearchItem> CreateSearchItems_ElementType(IList<ElementType> elementTypeList)
        {
            List<SearchItem> resultList = new List<SearchItem>();

            foreach (ElementType elementType in elementTypeList)
            {
                // ADSK_Наименование
                Parameter nameParameter = elementType.get_Parameter(new Guid("e6e0f5cd-3e26-485b-9342-23882b20eb43"));
                string nameParameterValue = "";

                if (nameParameter != null)
                    nameParameterValue = nameParameter.AsString();

                string category = elementType.get_Parameter(BuiltInParameter.ELEM_CATEGORY_PARAM).AsValueString();
                int itemSortOrder = 10;

                resultList.Add(new SearchItem
                (
                    itemElement: elementType,
                    itemName: elementType.Name,
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: category,
                    itemSearchTags: $"Семейство Тип {elementType.Name} {elementType.FamilyName} {category} {nameParameterValue}",
                    itemGroup: elementType.FamilyName,
                    itemSortingOrder: itemSortOrder
                ));
            }

            return resultList;
        }


        // Collectors

        public static List<Element> CollectElementsInModel(ElementType elementType, Document document, View view = null)
        {
            List<Element> resultList = new List<Element>();

            List<Type> typesList = new List<Type>
            {
                typeof(FamilyInstance),
                typeof(Conduit)
            };

            if (view != null)
            {
                resultList = new FilteredElementCollector(document, view.Id)
                    .WherePasses(new ElementMulticlassFilter(typesList))
                    .ToElements()
                    .Where(instance => instance.GetTypeId() == elementType.Id)
                    .ToList();
            }
            else
            {
                resultList = new FilteredElementCollector(document)
                    .WherePasses(new ElementMulticlassFilter(typesList))
                    .ToElements()
                    .Where(instance => instance.GetTypeId() == elementType.Id)
                    .ToList();
            }

            return resultList;
        }
    }
}
