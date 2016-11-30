using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace GeoBis.Mobile.UI.CustomControls
{
    public class SelectMultipleBasePage<T> : PopupPage
    {
        private IList<T> _items;
        private IList<T> _selectedItems;

        private IList<WrappedSelection<T>> _wrappedItems;
        private readonly ListView _listView;

        public SelectMultipleBasePage()
        {
            HasSystemPadding = true;
            Padding = new Thickness(200, 50);

            _listView = new ListView
            {
                BackgroundColor = Color.FromRgb(240, 240, 240),
                ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate))
            };

            Content = new Frame
            {
                Padding = 10,
                BackgroundColor = Color.Silver,
                Content = _listView
            };
        }

        public IList<T> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                WrappedItems = _items.Select(item => new WrappedSelection<T> {Item = item, IsSelected = false}).ToList();
            }
        }

        public IList<T> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                SetSelection(value);
            }
        }

        public IList<WrappedSelection<T>> WrappedItems
        {
            get { return _wrappedItems; }
            set
            {
                _wrappedItems = value;
                _listView.ItemsSource = value;
            }
        }

        protected override void OnDisappearing()
        {
            _selectedItems.Clear();
            foreach (T item in GetSelection())
                    _selectedItems.Add(item);
            base.OnDisappearing();
        }

        private IList<T> GetSelection()
        {
            return WrappedItems.Where(item => item.IsSelected).Select(wrappedItem => wrappedItem.Item).ToList();
        }

        private void SetSelection(IList<T> selectedItems)
        {
            foreach (WrappedSelection<T> wrappedItem in WrappedItems)
                wrappedItem.IsSelected = selectedItems.Contains(wrappedItem.Item);
        }

        public class WrappedSelection<T> : INotifyPropertyChanged
        {
            private bool _isSelected;
            public T Item { get; set; }

            public bool IsSelected
            {
                get { return _isSelected; }
                set
                {
                    if (_isSelected != value)
                    {
                        _isSelected = value;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }

        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate()
            {
                var name = new Label();
                name.SetBinding(Label.TextProperty, new Binding("Item.Name"));

                var mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));

                var layout = new RelativeLayout();
                layout.Children.Add(name,
                    Constraint.Constant(5),
                    Constraint.Constant(5),
                    Constraint.RelativeToParent(p => p.Width - 60),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );

                layout.Children.Add(mainSwitch,
                    Constraint.RelativeToParent(p => p.Width - 55),
                    Constraint.Constant(5),
                    Constraint.Constant(50),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );

                View = layout;
            }
        }
    }
}