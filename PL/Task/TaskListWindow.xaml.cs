using PL.Task;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Status Status { get; set; } = BO.Status.All;
        public TaskListWindow()
        {
            InitializeComponent();
            var temp = s_bl?.Task.ReadAll().ToList();
            temp.ForEach(t => t.ToString());
            TaskList = temp == null ? new() : new(temp);
        }
        /* public ObservableCollection<BO.TaskInList> TaskList
         {
             get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
             set { SetValue(TaskListProperty, value); }
         }

         public static readonly DependencyProperty TaskListProperty =
             DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
 */


        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tempTaskList = Status == BO.Status.All ?
            s_bl?.Task.ReadAll() :
            s_bl?.Task.ReadAll(taskInList => taskInList.Status == Status);
            TaskList = tempTaskList == null ? new() : new(tempTaskList);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            /*         InitializeComponent();*/
            var tempTaskList = s_bl?.Task.ReadAll();
            TaskList = tempTaskList == null ? new() : new(tempTaskList!);
        }

        private void Update_SelectionChanged(object sender, MouseButtonEventArgs m)
        {
            
                BO.TaskInList? taskInlist = (sender as ListView)?.SelectedItem as BO.TaskInList;
                if (taskInlist != null)
                    new TaskWindow(taskInlist!.Id).ShowDialog();
                InitializeComponent();
                var temptaskInlist = s_bl?.Task.ReadAll();
                TaskList = temptaskInlist == null ? new() : new(temptaskInlist);
           
        }
    }
}
