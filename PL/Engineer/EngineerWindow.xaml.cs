using BO;
using DO;
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
        int ID=0;


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }

            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        public EngineerWindow(int currentId = 0)
        {
            InitializeComponent();
            ID = currentId;
            CurrentEngineer = (currentId == 0) ? new() : s_bl.Engineer.Read(currentId);
            //איך להשתמש עם חריגות ואיך לומר אם הוא נל????
        }

        private void AddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    this.Close();
                    MessageBox.Show("Engineer added succesfully.");
                }
                else
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    this.Close();
                    MessageBox.Show("Engineer updated succesfully.");
                }
              
            }
            catch(DalDoesNotExistException ex)
            {
                this.Close();
                MessageBox.Show(ex.Message,"update error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            catch(BlAlreadyExistsException ex)
            {
                this.Close();
                MessageBox.Show(ex.Message, "add error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(BlInValidInput ex)
            {
                MessageBox.Show(ex.Message, (ID==0)? "add error":"update error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
