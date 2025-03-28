using Avalonia.Controls;
using ivgr1UserManager.ViewModels;

namespace ivgr1UserManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new UserViewModel();
    }
}