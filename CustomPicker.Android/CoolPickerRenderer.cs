using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using aColor = Android.Graphics.Color;
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
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CoolPicker.CoolPicker), typeof(CoolPicker.Android.CoolPickerRenderer))]

namespace CoolPicker.Android
{
    public class CoolPickerRenderer : PickerRenderer
    {
        IElementController ElementController => Element as IElementController;

        aColor defaultPlaceholderTextColor;

        struct Padding
        {
            internal int Start;
            internal int End;
            internal int Top;
            internal int Bottom;
        }
        Padding defaultTextFieldPadding;
        Drawable defaultTextFieldDrawable;

        AlertDialog pickerView;

        public CoolPickerRenderer(Context context) : base(context)
        {
            defaultTextFieldPadding = new Padding();
        }

        protected override EditText CreateNativeControl()
        {
            var picker = Element as CoolPicker;

            //for hook click
            var field = new CoolField(Context) { Focusable = true, Clickable = true, Tag = this };
            defaultTextFieldPadding.Start = field.PaddingStart;
            defaultTextFieldPadding.End = field.PaddingEnd;
            defaultTextFieldPadding.Top = field.PaddingTop;
            defaultTextFieldPadding.Bottom = field.PaddingBottom;
            defaultTextFieldDrawable = field.Background;

            if (picker.Border)
            {
                var background = new GradientDrawable();
                background.SetStroke(2, picker.BorderColor.ToAndroid());
                field.SetPadding(40, 0, 0, 0);
                field.SetBackground(background);
            }

            return field;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            //Control.InputType = InputTypes.Null; //necessary

            defaultPlaceholderTextColor = ConvertArgbColor(Control.CurrentHintTextColor);

            var picker = Element as CoolPicker;
            if (picker.PlaceholderColor != xColor.Default)
                Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
            Control.FocusChange += (s, a) => { if (a.HasFocus) OnClick(); };

            aColor ConvertArgbColor(int argb)
            {
                var alpha = argb & 0xFF000000 >> 24;
                var red = argb & 0x00FF0000 >> 16;
                var green = argb & 0x0000FF00 >> 8;
                var blue = argb & 0x000000FF;

                return aColor.Argb((int)alpha, red, green, blue);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CoolPicker.BorderProperty.PropertyName)
                UpdateBorder();
            else if (e.PropertyName == CoolPicker.BorderColorProperty.PropertyName)
                UpdateBorder();
            else if (e.PropertyName == CoolPicker.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholder();
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

        void UpdateBorder()
        {
            var picker = Element as CoolPicker;
            var field = Control as CoolField;

            if (picker.Border)
            {
                var background = new GradientDrawable();
                background.SetStroke(2, picker.BorderColor.ToAndroid());
                field.SetPaddingRelative(
                    30,
                    defaultTextFieldPadding.End,
                    defaultTextFieldPadding.Top + 5,
                    defaultTextFieldPadding.Bottom
                );
                field.SetBackground(background);
            }
            else
            {
                field.SetBackground(defaultTextFieldDrawable);
                field.SetPaddingRelative(
                    defaultTextFieldPadding.Start,
                    defaultTextFieldPadding.End,
                    defaultTextFieldPadding.Top,
                    defaultTextFieldPadding.Bottom
                );
            }
        }

        void UpdatePlaceholder()
        {
            var picker = Element as CoolPicker;
            if (Control.CurrentHintTextColor != picker.PlaceholderColor.ToAndroid().ToArgb())
            {
                if (picker.PlaceholderColor != xColor.Default)
                    Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
                else
                    Control.SetHintTextColor(defaultPlaceholderTextColor);
            }
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
