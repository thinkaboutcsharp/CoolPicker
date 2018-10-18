using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinStudy11
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var list = new List<PickerItem>
            {
                new PickerItem { Text = "abc"},
                new PickerItem{ Text = "def"},
                new PickerItem{ Text = "ghi"}
            };
            pck_CoolPicker.ItemsSource = list;

            pck_NormalPicker.ItemsSource = list;
        }

        class PickerItem
        {
            public string Text { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
    }
}
