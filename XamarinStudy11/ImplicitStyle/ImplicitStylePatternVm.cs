using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Reactive.Bindings;

namespace XamarinStudy11
{
    class ResourceChangedArgs
    {
        internal string Resource { get; set; }
        internal object NewValue { get; set; }
    }

    public class ImplicitStylePatternVM
    {
        CommonModel model = new CommonModel();
        ResourceDictionary resources;

        public ObservableCollection<PickerItem> PickerSource { get; } = new ObservableCollection<PickerItem>();
        public ReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>(-1);

        public ICommand ToggleCommand { get; set; }

        public ImplicitStylePatternVM()
        {
            resources = Application.Current.Resources;
            InitBinding();

            ToggleCommand = new Command((p) => Toggle((string)p));
        }

        internal void Initialize()
        {
            resources.Add("pTextColor", GetXColor(model.TextColor));
            resources.Add("pBackgroundColor", GetXColor(model.BackgroundColor));
            resources.Add("pPlaceholderColor", GetXColor(model.PlaceholderColor));
            resources.Add("pBorder", model.Border);
            resources.Add("pBorderColor", GetXColor(model.BorderColor));
            resources.Add("pBorderWidth", model.BorderWidth);
            resources.Add("pBorderRadius", model.BorderRadius);
            resources.Add("pPickerColor", GetXColor(model.PickerColor));
            resources.Add("pPickerBarColor", GetXColor(model.PickerBarColor));
            resources.Add("pPickerTextColor", GetXColor(model.PickerTextColor));
            resources.Add("pPickerBarTextColor", GetXColor(model.PickerBarTextColor));
        }

        void InitBinding()
        {
            for (int i = 0; i < 10; i++)
            {
                PickerSource.Add(new PickerItem { Key = i, Value = "Item " + i });
            }
        }

        void Toggle(string propertyName)
        {
            switch (propertyName)
            {
                case "Clear":
                    SelectedIndex.Value = -1;
                    break;
                case "TextColor":
                    resources["pTextColor"] = GetXColor(model.ToggleTextColor());
                    break;
                case "PlaceholderColor":
                    resources["pPlaceholderColor"] = GetXColor(model.TogglePlaceholderColor());
                    break;
                case "BackgroundColor":
                    resources["pBackgroundColor"] = GetXColor(model.ToggleBackgroundColor());
                    break;
                case "Border":
                    resources["pBorder"] = model.ToggleBorder();
                    break;
                case "BorderColor":
                    resources["pBorderColor"] = GetXColor(model.ToggleBorderColor());
                    break;
                case "BorderWidth":
                    resources["pBorderWidth"] = model.ToggleBorderWidth();
                    break;
                case "BorderRadius":
                    resources["pBorderRadius"] = model.ToggleBorderRadius();
                    break;
                case "PickerColor":
                    resources["pPickerColor"] = GetXColor(model.TogglePickerColor());
                    break;
                case "PickerBarColor":
                    resources["pPickerBarColor"] = GetXColor(model.TogglePickerBarColor());
                    break;
                case "PickerTextColor":
                    resources["pPickerTextColor"] = GetXColor(model.TogglePickerTextColor());
                    break;
                case "PickerBarTextColor":
                    resources["pPickerBarTextColor"] = GetXColor(model.TogglePickerBarTextColor());
                    break;
            }
        }

        Color GetXColor(System.Drawing.Color color)
        {
            if (color == System.Drawing.Color.Transparent)
                return Color.Default;
            else
                return color;
        }
    }
}
