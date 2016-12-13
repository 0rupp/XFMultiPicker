using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace XFMultiPicker
{
    public class MultiPickerView<T> : Button where T : class
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IList<T>), typeof(MultiPickerView<T>),
                null, propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty SelectedItemsProperty =
            BindableProperty.Create("SelectedItems", typeof(IList<T>), typeof(MultiPickerView<T>),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedItemsChanged);

        public MultiPickerView()
        {
            Command = new Command(() => { PopupNavigation.PushAsync(PopupPage); }, () => SelectedItems != null);
            PopupPage = new MultiPickerPopupPage<T>();
        }

        public MultiPickerPopupPage<T> PopupPage { get; set; }

        public IList<T> ItemsSource
        {
            get { return (IList<T>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public IList<T> SelectedItems
        {
            get { return (IList<T>) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as MultiPickerView<T>;
            var items = newvalue as IList<T>;
            if (items != null)
                picker.PopupPage.Items = items;
        }

        private static void OnSelectedItemsChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as MultiPickerView<T>;
            ((Command)picker.Command).ChangeCanExecute();
            var items = newvalue as IList<T>;
            if (items != null)
                picker.PopupPage.SelectedItems = items;
        }
    }
}