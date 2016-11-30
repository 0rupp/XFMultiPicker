using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace GeoBis.Mobile.UI.CustomControls
{
    public class MultiPicker<T> : Button where T : class
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IList<T>), typeof(MultiPicker<T>),
                null, propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty SelectedItemsProperty =
            BindableProperty.Create("SelectedItems", typeof(IList<T>), typeof(MultiPicker<T>),
                null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

        public MultiPicker()
        {
            Command = new Command(() => { PopupNavigation.PushAsync(PopupPage); }, () => SelectedItems != null);
            PopupPage = new SelectMultipleBasePage<T>();
        }

        public SelectMultipleBasePage<T> PopupPage { get; set; }

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
            var picker = bindable as MultiPicker<T>;
            var items = newvalue as IList<T>;
            if (items != null)
                picker.PopupPage.Items = items;
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as MultiPicker<T>;
            ((Command)picker.Command).ChangeCanExecute();
            var items = newvalue as IList<T>;
            if (items != null)
                picker.PopupPage.SelectedItems = items;
        }
    }
}