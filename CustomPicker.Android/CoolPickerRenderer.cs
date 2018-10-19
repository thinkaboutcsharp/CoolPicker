using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xColor = Xamarin.Forms.Color;
using aTextAlignment = Android.Views.TextAlignment;

[assembly: ExportRenderer(typeof(CoolPicker.CoolPicker), typeof(CoolPicker.Android.CoolPickerRenderer))]

namespace CoolPicker.Android
{
    public class CoolPickerRenderer : PickerRenderer
    {
        IElementController ElementController => Element as IElementController;

        AlertDialog pickerView;

        public CoolPickerRenderer(Context context) : base(context)
        {}

        protected override EditText CreateNativeControl()
        {
            var picker = Element as CoolPicker;
            var background = new GradientDrawable();
            background.SetColor(xColor.Transparent.ToAndroid());

            //for hook click
            var field = new CoolField(Context) { Focusable = true, Clickable = true, Tag = this };

            if (picker.Border)
            {
                background.SetStroke(2, picker.BorderColor.ToAndroid());
                field.SetPadding(40, 0, 0, 0);
            }

            field.SetBackground(background);
            return field;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            Control.InputType = InputTypes.Null; //necessary

            Control.SetHintTextColor(((CoolPicker)Element).PlaceholderColor.ToAndroid());
            Control.FocusChange += (s, a) => { if (a.HasFocus) OnClick(); };
        }

        void OnClick()
        {
            var type = Control.InputType;

            var picker = Element as CoolPicker;
            var dataPicker = new CoolDataPicker(Context, picker.PickerColor, picker.PickerTextColor);
            if (picker.Items != null && picker.Items.Count > 0)
            {
                dataPicker.MaxValue = picker.Items.Count - 1;
                dataPicker.MinValue = 0;
                dataPicker.SetDisplayedValues(picker.Items.ToArray());
                dataPicker.WrapSelectorWheel = false;
                dataPicker.DescendantFocusability = DescendantFocusability.BlockDescendants;
                dataPicker.Value = picker.SelectedIndex;
            }

            var layout = new LinearLayout(Context) { Orientation = Orientation.Vertical };
            layout.AddView(dataPicker);

            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);

            var builder = new AlertDialog.Builder(Context);
            builder.SetView(layout);
            builder.SetTitle("");
            builder.SetNegativeButton(global::Android.Resource.String.Cancel, (s, a) =>
            {
                ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                pickerView = null;
            });
            builder.SetPositiveButton(global::Android.Resource.String.Ok, (s, a) =>
            {
                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, dataPicker.Value);
                // It is possible for the Content of the Page to be changed on SelectedIndexChanged. 
                // In this case, the Element & Control will no longer exist.
                if (Element != null)
                {
                    if (picker.Items.Count > 0 && Element.SelectedIndex >= 0)
                        Control.Text = picker.Items[Element.SelectedIndex];
                    ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
                }
                pickerView = null;
            });

            pickerView = builder.Create();
            pickerView.DismissEvent += (sender, args) =>
            {
                ElementController?.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            };
            pickerView.Show();
        }

        class CoolField : EditText
        {
            public new event EventHandler<KeyEventArgs> KeyPress;

            internal CoolField(Context context) : base(context)
            {
                base.KeyPress += (s, a) => a.Handled = true;
            }

            public override void SetOnClickListener(IOnClickListener l)
            {
                //Replace
                base.SetOnClickListener(new CoolClickListener());
            }
        }

        class CoolClickListener : Java.Lang.Object, IOnClickListener //without nesting, IOnClickedListener not found. Why?
        {
            public void OnClick(global::Android.Views.View v)
            {
                var renderer = v.Tag as CoolPickerRenderer;
                renderer.OnClick();
            }
        }

        class CoolDataPicker : NumberPicker
        {
            xColor textColor;

            internal CoolDataPicker(Context context, xColor background, xColor text) : base(context)
            {
                SetBackgroundColor(background.ToAndroid());
                this.textColor = text;
            }
        }
    }
}
