using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitProjectSearch.App.UserControls.Models
{
    public class SearchItem_Example
    {
        public string itemElement { get; set; }
        public string itemName { get; set; }
        public string itemIcon { get; set; }
        public string itemDescription { get; set; }
        public string itemSearchTags { get; set; }
        public string itemGroup { get; set; }
        public int itemSortingOrder { get; set; }


        public SearchItem_Example(string itemElement, string itemName, string itemIcon, string itemDescription, string itemSearchTags, string itemGroup, int itemSortingOrder)
        {
            this.itemElement = itemElement;
            this.itemName = itemName;
            this.itemIcon = itemIcon;
            this.itemDescription = itemDescription;
            this.itemSearchTags = itemSearchTags;
            this.itemGroup = itemGroup;
            this.itemSortingOrder = itemSortingOrder;
        }
    }
}
