using System.Collections.ObjectModel;
using System.Windows.Input;
using Reactive.Bindings;
using Xamarin.Forms;

namespace XamarinStudy11
{
    public class BindingPatternVM
    {
        CommonModel model = new CommonModel();

        public ObservableCollection<PickerItem> PickerSource { get; } = new ObservableCollection<PickerItem>();
        public ReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>(-1);
        public ReactiveProperty<Color> pTextColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<Color> pBackgroundColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<Color> pPlaceholderColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<bool> pBorder { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<Color> pBorderColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<int> pBorderWidth { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<float> pBorderRadius { get; } = new ReactiveProperty<float>(0.0f);
        public ReactiveProperty<Color> pPickerColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<Color> pPickerBarColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<Color> pPickerTextColor { get; } = new ReactiveProperty<Color>(Color.Default);
        public ReactiveProperty<Color> pPickerBarTextColor { get; } = new ReactiveProperty<Color>(Color.Default);

        public ICommand ToggleCommand { get; set; }

        public BindingPatternVM()
        {
            InitBinding();

            ToggleCommand = new Command((p) => Toggle((string)p));
        }

        void InitBinding()
        {
            for (int i = 0; i < 10; i++)
            {
                PickerSource.Add(new PickerItem { Key = i, Value = "Item " + i });
            }

            pTextColor.Value = GetXColor(model.TextColor);
            pBackgroundColor.Value = GetXColor(model.BackgroundColor);
            pPlaceholderColor.Value = GetXColor(model.PlaceholderColor);
            pBorder.Value = model.Border;
            pBorderColor.Value = GetXColor(model.BorderColor);
            pBorderWidth.Value = model.BorderWidth;
            pBorderRadius.Value = model.BorderRadius;
            pPickerColor.Value = GetXColor(model.PickerColor);
            pPickerBarColor.Value = GetXColor(model.PickerBarColor);
            pPickerTextColor.Value = GetXColor(model.PickerTextColor);
            pPickerBarTextColor.Value = GetXColor(model.PickerBarTextColor);
        }

        void Toggle(string propertyName)
        {
            switch (propertyName)
            {
                case "Clear":
                    SelectedIndex.Value = -1;
                    break;
                case "TextColor":
                    pTextColor.Value = GetXColor(model.ToggleTextColor());
                    break;
                case "PlaceholderColor":
                    pPlaceholderColor.Value = GetXColor(model.TogglePlaceholderColor());
                    break;
                case "BackgroundColor":
                    pBackgroundColor.Value = GetXColor(model.ToggleBackgroundColor());
                    break;
                case "Border":
                    pBorder.Value = model.ToggleBorder();
                    break;
                case "BorderColor":
                    pBorderColor.Value = GetXColor(model.ToggleBorderColor());
                    break;
                case "BorderWidth":
                    pBorderWidth.Value = model.ToggleBorderWidth();
                    break;
                case "BorderRadius":
                    pBorderRadius.Value = model.ToggleBorderRadius();
                    break;
                case "PickerColor":
                    pPickerColor.Value = GetXColor(model.TogglePickerColor());
                    break;
                case "PickerBarColor":
                    pPickerBarColor.Value = GetXColor(model.TogglePickerBarColor());
                    break;
                case "PickerTextColor":
                    pPickerTextColor.Value = GetXColor(model.TogglePickerTextColor());
                    break;
                case "PickerBarTextColor":
                    pPickerBarTextColor.Value = GetXColor(model.TogglePickerBarTextColor());
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
