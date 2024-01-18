using Autodesk.Revit.UI;
using RevitProjectSearch.App.Helpers;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;


namespace RevitProjectSearch.App.Ribbons
{
    public static class RibbonPanelHelpers
    {
        private static string _location = Assembly.GetExecutingAssembly().Location;

        public static void CreateButton(
            RibbonPanel ribbonPanel,
            string buttonName,
            string buttonText,
            string buttonCommand,
            string buttonIconFileName)
        {
            PushButtonData pushButtonData = new PushButtonData(buttonName, buttonText, _location, buttonCommand);

            BitmapImage iconImage = new BitmapImage(new Uri($"{GlobalData.RESOURCES}{buttonIconFileName}"));
            pushButtonData.LargeImage = iconImage;

            ribbonPanel.AddItem(pushButtonData);
        }
    }
}
