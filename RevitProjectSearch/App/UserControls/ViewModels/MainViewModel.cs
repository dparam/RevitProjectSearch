using System.Collections.Generic;
using System;
using System.ComponentModel;
using RevitProjectSearch.App.Helpers;
using RevitProjectSearch.App.UserControls.Models;
using System.Windows.Input;
using RevitProjectSearch.App.UserControls.Commands;
using System.Windows.Data;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using Autodesk.Revit.DB.Electrical;
using System.Windows;
using System.Linq;
using System.IO;

namespace RevitProjectSearch.App.UserControls.ViewModels
{
    // ExampleCollection

    public class ExampleCollection
    {
        public SmartCollection<SearchItem_Example> ExampleSearchItemsCollection { get; set; }
        public ICollectionView ExampleCollectionView { get; set; }

        public ExampleCollection()
        {
            ExampleSearchItemsCollection = new SmartCollection<SearchItem_Example>();
            ExampleCollectionView = CollectionViewSource.GetDefaultView(ExampleSearchItemsCollection);

            ExampleCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SearchItem_Example.itemGroup)));
            ExampleCollectionView.SortDescriptions.Add(new SortDescription(nameof(SearchItem_Example.itemSortingOrder), ListSortDirection.Ascending));
            ExampleCollectionView.SortDescriptions.Add(new SortDescription(nameof(SearchItem_Example.itemName), ListSortDirection.Ascending));

            List<SearchItem_Example> ExampleSearchItems = new List<SearchItem_Example>();

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "TSL_Общая спецификация",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "Ведомости",
                    itemSortingOrder: 1
                ));

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "TSL_Спецификация",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "Ведомости",
                    itemSortingOrder: 1
                ));

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "TSL_Спецификация квартирная",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "Ведомости",
                    itemSortingOrder: 1
                ));

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "Уровень 1",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "План этажа",
                    itemSortingOrder: 0
                ));

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "Лист 1",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "Лист",
                    itemSortingOrder: 3
                ));

            ExampleSearchItems.Add(
                new SearchItem_Example
                (
                    itemElement: "itemElement",
                    itemName: "Лист 2",
                    itemIcon: $"{GlobalData.RESOURCES}icon_Family.ico",
                    itemDescription: "itemDescription",
                    itemSearchTags: $"itemSearchTags",
                    itemGroup: "Лист",
                    itemSortingOrder: 3
                ));

            ExampleSearchItemsCollection.Reset(ExampleSearchItems);
        }
    }


    public class MainViewModel
    {
        private UIControlledApplication _uiControlledApp = null;
        private Document _doc = null;
        private UIDocument _uiDoc = null;


        public ICommand Command_SelectInRevit { get; set; }


        public SmartCollection<SearchItem> SearchItemsCollection { get; set; }
        public ICollectionView CollectionView { get; set; }


        private string _searchString;
        public string SearchPredicate
        {
            get => _searchString;
            set
            {
                _searchString = value;
                FilterCollectionView(value);
            }
        }


        // Constructor

        public MainViewModel()
        {
            SearchItemsCollection = new SmartCollection<SearchItem>();
            CollectionView = CollectionViewSource.GetDefaultView(SearchItemsCollection);

            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SearchItem.itemGroup)));
            CollectionView.SortDescriptions.Add(new SortDescription(nameof(SearchItem.itemSortingOrder), ListSortDirection.Ascending));
            CollectionView.SortDescriptions.Add(new SortDescription(nameof(SearchItem.itemName), ListSortDirection.Ascending));

            Command_SelectInRevit = new UICommand(SelectInRevit, CanExecute);
        }


        // Commands

        private bool CanExecute(object obj)
        {
            return true;
        }

        private void SelectInRevit(object obj)
        {
            return;
        }


        //


        public void OnViewActivated(object sender, RevitAPIPostDocEventArgs e)
        {
            _uiControlledApp = sender as UIControlledApplication;
            _doc = e.Document;

            UpdateCollection();
        }


        public void UpdateCollection()
        {
            if (_doc == null) return;
            if (!_doc.IsValidObject) return;

            //MessageBox.Show("UpdateCollection");

            SearchItemsCollection.Reset(ElementCollector.CollectProjectElements(_doc));
        }


        private void FilterCollectionView(string filterString)
        {
            CollectionView.Filter = item => (item as SearchItem).itemElement.IsValidObject;
            CollectionView.Filter = item => (item as SearchItem).itemName.Contains(filterString);
            CollectionView.Filter = item => (item as SearchItem).itemName.Contains(filterString);

            IEqualityComparer<String> comparer = StringComparer.InvariantCultureIgnoreCase;

            CollectionView.Filter = item =>
            {
                SearchItem searchItem = item as SearchItem;

                List<string> searchStringList = SearchHelpers.CreateSearchStringList(filterString);
                List<string> itemStringList = SearchHelpers.CreateItemStringList(searchItem.itemSearchTags);

                return SearchHelpers.CompareStringLists(searchStringList, itemStringList);
            };
        }


        // Events

        //public void OnClearTextbox(object sender, RoutedEventArgs e)
        //{
        //    SearchPredicate = "";
        //}


        public void OnSelected(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            SearchItem searchItem = item.Content as SearchItem;

            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            _uiDoc = new UIDocument(_doc);
            List<ElementId> elementIdList = new List<ElementId>();
            elementIdList.Add(searchItem.itemElement.Id);

            _uiDoc.Selection.SetElementIds(elementIdList);
        }


        public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            SearchItem searchItem = item.Content as SearchItem;

            if (searchItem == null) return;

            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            if (searchItem.itemElement is View)
            {
                View view = searchItem.itemElement as View;
                _uiDoc = new UIDocument(_doc);
                _uiDoc.ActiveView = view;
                return;
            }

            if (searchItem.itemElement is FamilySymbol)
            {
                FamilySymbol familySymbol = searchItem.itemElement as FamilySymbol;
                _uiDoc = new UIDocument(_doc);
                _uiDoc.PostRequestForElementTypePlacement(familySymbol);
                return;
            }

            if (searchItem.itemElement is ConduitType)
            {
                ConduitType conduitType = searchItem.itemElement as ConduitType;
                _uiDoc = new UIDocument(_doc);
                _uiDoc.PostRequestForElementTypePlacement(conduitType);
                return;
            }
        }


        public void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            SearchItem searchItem = item.Content as SearchItem;

            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            CreateContextMenuItems(searchItem, item.ContextMenu, e);
        }


        // Context Menu

        public void CreateContextMenuItems(SearchItem searchItem, ContextMenu contextMenu, ContextMenuEventArgs e)
        {
            contextMenu.Items.Clear();

            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            if (searchItem.itemElement is View)
            {
                var menuItem_OpenView = new MenuItem();
                menuItem_OpenView.Header = "Открыть вид";
                menuItem_OpenView.Click += context_OnOpenView;
                menuItem_OpenView.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_OpenView);

                var menuItem_PlaceVirewOnSheet = new MenuItem();
                menuItem_PlaceVirewOnSheet.Header = "Разместить вид на листе";
                menuItem_PlaceVirewOnSheet.Click += context_OnPlaceVirewOnSheet;
                menuItem_PlaceVirewOnSheet.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_PlaceVirewOnSheet);

                return;
            }

            if (searchItem.itemElement is FamilySymbol)
            {
                var menuItem_EditFamily = new MenuItem();
                menuItem_EditFamily.Header = "Редактировать семейство";
                menuItem_EditFamily.Click += context_OnEditFamily;
                menuItem_EditFamily.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_EditFamily);

                var menuItem_PlaceInstance = new MenuItem();
                menuItem_PlaceInstance.Header = "Создать экземпляр";
                menuItem_PlaceInstance.Click += context_OnPlaceInstance;
                menuItem_PlaceInstance.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_PlaceInstance);

                var menuItem_SelectAllInView = new MenuItem();
                menuItem_SelectAllInView.Header = "Выделить видимые на виде";
                menuItem_SelectAllInView.Click += context_OnSelectAllInView;
                menuItem_SelectAllInView.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_SelectAllInView);

                var menuItem_SelectAllInModel = new MenuItem();
                menuItem_SelectAllInModel.Header = "Выделить во всём проекте";
                menuItem_SelectAllInModel.Click += context_OnSelectAllInModel;
                menuItem_SelectAllInModel.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_SelectAllInModel);

                return;
            }

            if (searchItem.itemElement is ConduitType || 
                searchItem.itemElement is GroupType)
            {
                var menuItem_PlaceInstance = new MenuItem();
                menuItem_PlaceInstance.Header = "Создать экземпляр";
                menuItem_PlaceInstance.Click += context_OnPlaceInstance;
                menuItem_PlaceInstance.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_PlaceInstance);

                var menuItem_SelectAllInView = new MenuItem();
                menuItem_SelectAllInView.Header = "Выделить видимые на виде";
                menuItem_SelectAllInView.Click += context_OnSelectAllInView;
                menuItem_SelectAllInView.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_SelectAllInView);

                var menuItem_SelectAllInModel = new MenuItem();
                menuItem_SelectAllInModel.Header = "Выделить во всём проекте";
                menuItem_SelectAllInModel.Click += context_OnSelectAllInModel;
                menuItem_SelectAllInModel.CommandParameter = contextMenu;
                contextMenu.Items.Add(menuItem_SelectAllInModel);

                return;
            }

            e.Handled = true;
        }

        
        private static ListViewItem FindClickedItem(object sender)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
            {
                MessageBox.Show("MenuItem");
                return null;
            }

            ContextMenu contextMenu = menuItem.CommandParameter as ContextMenu;
            if (contextMenu == null)
            {
                MessageBox.Show("ContextMenu");
                return null;
            }

            ListViewItem item = contextMenu.PlacementTarget as ListViewItem;
            if (item == null)
            {
                MessageBox.Show("ListViewItem");
                return null;
            }

            return item;
        }


        // Context Menu Events

        private void context_OnOpenView(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;

            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            View view = searchItem.itemElement as View;
            _uiDoc = new UIDocument(_doc);
            _uiDoc.ActiveView = view;
        }


        private void context_OnPlaceVirewOnSheet(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;

            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            View view = searchItem.itemElement as View;
            _uiDoc = new UIDocument(_doc);
            _uiDoc.PromptToPlaceViewOnSheet(view, false);
        }


        private void context_OnSelectAllInModel(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;
            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            ElementType elementType = searchItem.itemElement as ElementType;
            if (elementType == null) return;

            _uiDoc = new UIDocument(_doc);

            List<Element> elementsList = ElementCollector.CollectElementsInModel(elementType, _doc);
            List<ElementId> elementIdList = new List<ElementId>();

            foreach (var item in elementsList)
                elementIdList.Add(item.Id);

            _uiDoc.Selection.SetElementIds(elementIdList);
        }


        private void context_OnSelectAllInView(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;
            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            ElementType elementType = searchItem.itemElement as ElementType;
            if (elementType == null) return;

            _uiDoc = new UIDocument(_doc);
            View activeView = _uiDoc.ActiveView;

            List<Element> elementsList = ElementCollector.CollectElementsInModel(elementType, _doc, activeView);
            List<ElementId> elementIdList = new List<ElementId>();

            foreach (var item in elementsList)
                elementIdList.Add(item.Id);

            _uiDoc.Selection.SetElementIds(elementIdList);
        }


        private void context_OnPlaceInstance(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;
            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            if (searchItem.itemElement is FamilySymbol)
            {
                FamilySymbol familySymbol = searchItem.itemElement as FamilySymbol;
                _uiDoc = new UIDocument(_doc);
                _uiDoc.PostRequestForElementTypePlacement(familySymbol);
            }

            if (searchItem.itemElement is ConduitType)
            {
                ConduitType conduitType = searchItem.itemElement as ConduitType;
                _uiDoc = new UIDocument(_doc);
                _uiDoc.PostRequestForElementTypePlacement(conduitType);
            }
        }


        private void context_OnEditFamily(object sender, RoutedEventArgs e)
        {
            ListViewItem clickedItem = FindClickedItem(sender);
            if (clickedItem == null) return;

            SearchItem searchItem = clickedItem.Content as SearchItem;
            if (searchItem == null) return;
            if (!searchItem.itemElement.IsValidObject)
            {
                UpdateCollection();
                return;
            }

            if (searchItem.itemElement is FamilySymbol)
            {
                FamilySymbol familySymbol = searchItem.itemElement as FamilySymbol;

                try
                {
                    _uiDoc = new UIDocument(_doc);

                    Family family = familySymbol.Family;
                    Document familyDoc = _doc.EditFamily(family);

                    string name = family.Name;
                    string path = Path.GetTempPath();
                    string fName = name + ".rfa";
                    string fPath = path + fName;

                    familyDoc.SaveAs(fPath);
                    familyDoc.Close();
                    _uiDoc.Application.OpenAndActivateDocument(fPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex}");
                }
            }
        }
    }
}
