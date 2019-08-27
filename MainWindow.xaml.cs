using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ContextMenuError
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var viewModel = new ExampleViewModel();
            DataContext = viewModel;

            InitializeComponent();
        }
    }

    public class ExampleViewModel : INotifyPropertyChanged
    {
        public ExampleViewModel()
        {
            toggleAndRefreshCommand = new RelayCommand(ToggleAndRefresh);
            Rows = new ObservableCollection<DataRow>
            {
                new DataRow
                {
                    CanDoA = false,
                    ExampleCommand = new RelayCommand(ToggleAndRefresh)
                },
                new DataRow
                {
                    CanDoA = true,
                    ExampleCommand = new RelayCommand(ToggleAndRefresh)
                }
            };
        }

        public ObservableCollection<DataRow> Rows { get; set; }

        private readonly RelayCommand toggleAndRefreshCommand;
        public ICommand ToggleAndRefreshCommand => toggleAndRefreshCommand;

        private void ToggleAndRefresh(object commandParameter)
        {
            Rows = new ObservableCollection<DataRow>(Rows.Select(x => new DataRow
            {
                CanDoA = !x.CanDoA,
                ExampleCommand = new RelayCommand(ToggleAndRefresh)
            }));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rows)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DataRow
    {
        public bool CanDoA { get; set; }
        public ICommand ExampleCommand { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> _action;
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
