using Autodesk.Revit.UI;
using RevitProjectSearch.App.UserControls.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace RevitProjectSearch.App.UserControls.Views
{
    public partial class DockablePage : Page, IDockablePaneProvider
    {
        public DockablePage()
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }


        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;

            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            };
        }


        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MainViewModel mainViewModel = DataContext as MainViewModel;
                mainViewModel.OnMouseDoubleClick(sender, e);
            }
            catch (System.Exception exception)
            {
                MessageBox.Show($"OnMouseDoubleClick\n{exception}");
            }
        }


        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                MainViewModel mainViewModel = DataContext as MainViewModel;
                mainViewModel.OnContextMenuOpening(sender, e);
            }
            catch (System.Exception exception)
            {
                MessageBox.Show($"OnContextMenuOpening\n{exception}");
            }
        }


        // prevent change selected item when mouse down

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                MainViewModel mainViewModel = DataContext as MainViewModel;
                mainViewModel.OnSelected(sender, e);

                this.mainListView.ReleaseMouseCapture();
            }
            catch (System.Exception exception)
            {
                MessageBox.Show($"OnSelected\n{exception}");
            }
        }


        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem listViewItem = sender as ListViewItem;
            if (listViewItem.IsSelected == true)
            {
                listViewItem.IsSelected = false;
            }
        }

        //private void OnClearTextbox(object sender, RoutedEventArgs e)
        //{
        //    MainViewModel mainViewModel = DataContext as MainViewModel;
        //    mainViewModel.OnClearTextbox(sender, e);
        //}
    }
}
