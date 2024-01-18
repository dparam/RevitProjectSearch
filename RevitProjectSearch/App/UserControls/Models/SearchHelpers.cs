using Autodesk.Revit.DB;
using RevitProjectSearch.App.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace RevitProjectSearch.App.UserControls.Models
{
    public static class SearchHelpers
    {
        public static List<string> CreateSearchStringList(string searchString)
        {
            char[] splits = new char[] { ' ', '_', ',', '.', ':', '"', '*', '+', '(', ')', '[', ']' };
            List<string> searchList = searchString.ToLower().Split(splits).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return searchList;
        }


        public static List<string> CreateItemStringList(string searchString)
        {
            char[] splits = new char[] { ' ', '_', '-', ',', '.', ':', '"', '*', '+', '(', ')', '[', ']' };
            List<string> searchList = searchString.ToLower().Split(splits).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            return searchList;
        }


        public static bool CompareStringLists(List<string> searchStringList, List<string> elementStringList)
        {
            foreach (string search in searchStringList)
            {
                //if (search.StartsWith("-") && search.Length <= 2) continue;

                //if (search.StartsWith("-") && search.Length > 2)
                //{
                //    if (elementStringList.Any(element => element.Contains(search.TrimStart('-'))))
                //        return false;
                //}

                if (elementStringList.Any(element => element.Contains(search)) == false)
                    return false;
            }

            return true;
        }


        public static string GetViewTypeName(ViewType viewType)
        {
            string viewTypeName = "Вид";

            if (viewType is ViewType.FloorPlan) viewTypeName = "План этажа";
            if (viewType is ViewType.CeilingPlan) viewTypeName = "План потолка";
            if (viewType is ViewType.DraftingView) viewTypeName = "Чертёжный вид";
            if (viewType is ViewType.DrawingSheet) viewTypeName = "Лист";
            if (viewType is ViewType.Legend) viewTypeName = "Легенда";
            if (viewType is ViewType.ThreeD) viewTypeName = "3D вид";
            if (viewType is ViewType.Schedule) viewTypeName = "Ведомость";
            if (viewType is ViewType.Section) viewTypeName = "Разрез";
            if (viewType is ViewType.Elevation) viewTypeName = "Фасад";

            return viewTypeName;
        }


        public static int GetViewSortingOrder(ViewType viewType)
        {
            int sortingOrder = 0;

            if (viewType is ViewType.FloorPlan) sortingOrder = 0;
            if (viewType is ViewType.CeilingPlan) sortingOrder = 8;
            if (viewType is ViewType.DraftingView) sortingOrder = 3;
            if (viewType is ViewType.DrawingSheet) sortingOrder = 6;
            if (viewType is ViewType.Legend) sortingOrder = 5;
            if (viewType is ViewType.ThreeD) sortingOrder = 1;
            if (viewType is ViewType.Schedule) sortingOrder = 4;
            if (viewType is ViewType.Section) sortingOrder = 2;
            if (viewType is ViewType.Elevation) sortingOrder = 7;

            return sortingOrder;
        }


        public static string GetViewIcon(ViewType viewType)
        {
            string viewTypeIcon = $"{GlobalData.RESOURCES}icon_View.ico";

            if (viewType is ViewType.FloorPlan) viewTypeIcon = $"{GlobalData.RESOURCES}icon_View.ico";
            if (viewType is ViewType.CeilingPlan) viewTypeIcon = $"{GlobalData.RESOURCES}icon_View.ico";
            if (viewType is ViewType.DraftingView) viewTypeIcon = $"{GlobalData.RESOURCES}icon_DraftingView.ico";
            if (viewType is ViewType.DrawingSheet) viewTypeIcon = $"{GlobalData.RESOURCES}Icon_Sheet.ico";
            if (viewType is ViewType.Legend) viewTypeIcon = $"{GlobalData.RESOURCES}Icon_Legend.ico";
            if (viewType is ViewType.ThreeD) viewTypeIcon = $"{GlobalData.RESOURCES}icon_3D.ico";
            if (viewType is ViewType.Schedule) viewTypeIcon = $"{GlobalData.RESOURCES}Icon_Schedule.ico";
            if (viewType is ViewType.Section) viewTypeIcon = $"{GlobalData.RESOURCES}Icon_Section.ico";
            if (viewType is ViewType.Elevation) viewTypeIcon = $"{GlobalData.RESOURCES}Icon_Elevation.ico";

            return viewTypeIcon;
        }
    }
}
