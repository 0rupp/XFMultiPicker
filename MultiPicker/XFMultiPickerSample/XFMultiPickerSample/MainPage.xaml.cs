using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;
using Xamarin.Forms;
using XFMultiPickerSample.Annotations;

namespace XFMultiPickerSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ObservableCollection<MyType> items = BuildData();

            BindingContext = new MainViewModel
            {
                AvailableItems = items,
                SelectedItems = new ObservableCollection<MyType>(new[] {items[0]})
            };
        }

        private static ObservableCollection<MyType> BuildData()
        {
            return new ObservableCollection<MyType>(new[]
            {
                new MyType
                {
                    Id = 1,
                    Name = "A"
                },
                new MyType
                {
                    Id = 2,
                    Name = "B"
                },
                new MyType
                {
                    Id = 3,
                    Name = "C"
                },
                new MyType
                {
                    Id = 4,
                    Name = "D"
                }
            });
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MyType> _selectedItems;
        public ObservableCollection<MyType> AvailableItems { get; set; }

        public ObservableCollection<MyType> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (Equals(value, _selectedItems)) return;
                if (_selectedItems != null)
                    _selectedItems.CollectionChanged -= SelectedItemsCollectionChanged;
                _selectedItems = value;
                if (value != null)
                    _selectedItems.CollectionChanged += SelectedItemsCollectionChanged;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedItems));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}