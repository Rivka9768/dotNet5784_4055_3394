using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>

    public partial class EngineerListWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;
        private void UpdateTheList(Object sender, EventArgs e)
        {
            var temp = s_bl?.Engineer.ReadAll().ToList();
            EngineerList = temp == null ? new() : new(temp);
        }
        public EngineerListWindow()
        {
            InitializeComponent();
            Activated += UpdateTheList!;
        }
        public ObservableCollection<BO.EngineerInList> EngineerList
        {
            get { return (ObservableCollection<BO.EngineerInList>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.EngineerInList>), typeof(EngineerListWindow), new PropertyMetadata(null));

        private void ExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tempEngineerList = Experience == BO.EngineerExperience.All ?
                        s_bl?.Engineer.ReadAll() :
                        s_bl?.Engineer.ReadAll(engineer => engineer.Level == Experience);
            EngineerList = tempEngineerList == null ? new() : new(tempEngineerList);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void Update_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            BO.EngineerInList? engineerInlist = (sender as ListView)?.SelectedItem as BO.EngineerInList;
            if(engineerInlist!=null)
                new EngineerWindow(engineerInlist!.Id).ShowDialog();
        }
    }
}
