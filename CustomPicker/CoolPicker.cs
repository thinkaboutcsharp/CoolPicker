using System;
using Xamarin.Forms;

namespace CoolPicker
{
    public class CoolPicker : Picker
    {
        public static readonly BindableProperty BorderProperty = BindableProperty.Create("Border", typeof(bool), typeof(CoolPicker), false);
        public bool Border
        {
            get => (bool)GetValue(BorderProperty);
            set { SetValue(BorderProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create("BorderWidth", typeof(int), typeof(CoolPicker), 0);
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(CoolPicker), Color.Default);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderRadiusProperty = BindableProperty.Create("BorderRadius", typeof(int), typeof(CoolPicker), 0);
        public int BorderRadius
        {
            get => (int)GetValue(BorderRadiusProperty);
            set { SetValue(BorderRadiusProperty, value); }
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create("PlaceholderColor", typeof(Color), typeof(CoolPicker), Color.Default);
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public static readonly BindableProperty PickerColorProperty = BindableProperty.Create("PickerColor", typeof(Color), typeof(CoolPicker), Color.Default);
        public Color PickerColor
        {
            get => (Color)GetValue(PickerColorProperty);
            set { SetValue(PickerColorProperty, value); }
        }

        public static readonly BindableProperty PickerBarColorProperty = BindableProperty.Create("PickerBarColor", typeof(Color), typeof(CoolPicker), Color.Default);
        public Color PickerBarColor
        {
            get => (Color)GetValue(PickerBarColorProperty);
            set { SetValue(PickerBarColorProperty, value); }
        }

        public static readonly BindableProperty PickerTextColorProperty = BindableProperty.Create("PickerTextColor", typeof(Color), typeof(CoolPicker), Color.Black);
        public Color PickerTextColor
        {
            get => (Color)GetValue(PickerTextColorProperty);
            set { SetValue(PickerTextColorProperty, value); }
        }

        public static readonly BindableProperty PickerBarTextColorProperty = BindableProperty.Create("PickerBarTextColor", typeof(Color), typeof(CoolPicker), Color.Default);
        public Color PickerBarTextColor
        {
            get => (Color)GetValue(PickerBarTextColorProperty);
            set { SetValue(PickerBarTextColorProperty, value); }
        }
    }
}
