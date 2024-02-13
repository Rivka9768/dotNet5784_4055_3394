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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>

    public partial class TaskWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.Novice;
        public BO.Status Status { get; set; } = BO.Status.Unscheduled;
        int ID = 0;


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }

            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));


        public TaskWindow(int currentId = 0)
        {
            InitializeComponent();
            ID = currentId;
            CurrentTask = (currentId == 0) ? new() : s_bl.Task.Read(currentId);
            //איך להשתמש עם חריגות ואיך לומר אם הוא נל????
        }

        private void AddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ID == 0)
                {
                    s_bl.Task.Create(CurrentTask);
                    this.Close();
                    MessageBox.Show("Task added succesfully.");
                }
                else
                {
                    s_bl.Task.Update(CurrentTask);
                    this.Close();
                    MessageBox.Show("Task updated succesfully.");
                }

            }
            catch (DalDoesNotExistException ex)
            {
                this.Close();
                MessageBox.Show(ex.Message, "update error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlAlreadyExistsException ex)
            {
                this.Close();
                MessageBox.Show(ex.Message, "add error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlInValidInput ex)
            {
                MessageBox.Show(ex.Message, (ID == 0) ? "add error" : "update error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
