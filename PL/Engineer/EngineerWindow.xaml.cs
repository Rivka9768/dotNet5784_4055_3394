using System;
using System.Collections.Generic;
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
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
  
    public partial class EngineerWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.Novice;



        //public int MyProperty
        //{
        //    get { return (int)GetValue(MyPropertyProperty); }
        //    set { SetValue(MyPropertyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyPropertyProperty =
        //    DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }

            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        public EngineerWindow(int currentId=0)
        {
            InitializeComponent();
            CurrentEngineer = (currentId == 0) ? new () : s_bl.Engineer.Read(currentId);
            //איך להשתמש עם חריגות ואיך לומר אם הוא נל????
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddUpdate_Click(object sender, RoutedEventArgs e)
        {

            s_bl.Engineer.Create(CurrentEngineer);
            Console.ReadLine();
           
        }
    }
}
