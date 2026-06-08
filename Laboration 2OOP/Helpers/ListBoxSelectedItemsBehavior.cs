using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Laboration_2OOP
{
    public static class ListBoxSelectedItemsBehavior
    {
        public static readonly DependencyProperty BoundSelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "BoundSelectedItems",
                typeof(IList),
                typeof(ListBoxSelectedItemsBehavior),
                new PropertyMetadata(null, OnBoundSelectedItemsChanged));

        private static readonly DependencyProperty IsHookedProperty =
            DependencyProperty.RegisterAttached(
                "IsHooked",
                typeof(bool),
                typeof(ListBoxSelectedItemsBehavior),
                new PropertyMetadata(false));

        public static void SetBoundSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(BoundSelectedItemsProperty, value);
        }

        public static IList GetBoundSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(BoundSelectedItemsProperty);
        }

        private static void OnBoundSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ListBox listBox)
                return;

            bool isHooked = (bool)listBox.GetValue(IsHookedProperty);
            if (!isHooked)
            {
                listBox.SelectionChanged += ListBox_SelectionChanged;
                listBox.SetValue(IsHookedProperty, true);
            }

            SyncFromListBox(listBox);
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                SyncFromListBox(listBox);
            }
        }

        private static void SyncFromListBox(ListBox listBox)
        {
            var boundList = GetBoundSelectedItems(listBox);
            if (boundList == null)
                return;

            boundList.Clear();

            foreach (var item in listBox.SelectedItems)
            {
                boundList.Add(item);
            }
        }
    }
}
