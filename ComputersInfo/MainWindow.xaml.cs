using System.Windows;

namespace ComputersInfo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            FrameClass.fr = FrameHead;
            FrameClass.fr.Navigate(new MainPage());
        }


    }
}
