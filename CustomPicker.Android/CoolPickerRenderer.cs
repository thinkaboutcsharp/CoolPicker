using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using aColor = Android.Graphics.Color;
using Android.Views;
using aView = Android.Views.View;
using Android.Widget;
using wOrientation = Android.Widget.Orientation;
using Android.Content.Res;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xColor = Xamarin.Forms.Color;
using System.ComponentModel;
using Android.Graphics;
using Android.Runtime;

[assembly: ExportRenderer(typeof(CoolControl.CoolPicker), typeof(CoolControl.Android.CoolPickerRenderer))]

namespace CoolControl.Android
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
        AlertDialog confirmView;

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
            if (e.NewElement == null) return;

            defaultPlaceholderTextColor = ConvertArgbColor(Control.CurrentHintTextColor);

            var picker = Element as CoolPicker;
            if (picker.PlaceholderColor != xColor.Default)
                Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
            Control.FocusChange += (s, a) => { if (a.HasFocus) OnClick(); };
            Control.Gravity = ExchangeAlignmentFlag(picker.HorizontalTextAlignment);

            aColor ConvertArgbColor(int argb)
            {
                var alpha = (argb >> 24) & 0xFF;
                var red = (argb >> 16) & 0xFF;
                var green = (argb >> 8) & 0xFF;
                var blue = argb & 0xFF;

                return aColor.Argb(alpha, red, green, blue);
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
            else if (e.PropertyName == CoolPicker.HorizontalTextAlignmentProperty.PropertyName)
                Control.Gravity = ExchangeAlignmentFlag((Element as CoolPicker).HorizontalTextAlignment);
            else if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                var picker = Element as CoolPicker;
                if (picker.SelectedIndex >= 0) Control.Gravity = ExchangeAlignmentFlag(picker.HorizontalTextAlignment);
                else Control.Gravity = GravityFlags.Left;
            }
        }

        GravityFlags ExchangeAlignmentFlag(Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Start:
                    return GravityFlags.Left;
                case Xamarin.Forms.TextAlignment.Center:
                    return GravityFlags.Center;
                case Xamarin.Forms.TextAlignment.End:
                    return GravityFlags.Right;
            }
            return GravityFlags.Start;
        }

        void OnClick()
        {
            var picker = Element as CoolPicker;
            var layout = new LinearLayout(Context) { Orientation = wOrientation.Vertical };

            if (picker.Items == null || picker.Items.Count == 0)
            {
                var confirm = new AlertDialog.Builder(Context);
                layout.AddView(new EditText(Context) { Text = "リストが設定されていません。", Focusable = false });
                layout.SetBackgroundColor(picker.PickerColor.ToAndroid());
                confirm.SetTitle("！");
                confirm.SetView(layout);
                confirm.SetPositiveButton(global::Android.Resource.String.Ok, (s, e) =>
                {
                    confirmView = null;
                });
                confirmView = confirm.Create();
                confirm.Show();
                return;
            }

            var dataPicker = new CoolDataPicker(Context) { BackgroundColor = picker.PickerColor };
            dataPicker.MaxValue = picker.Items.Count - 1;
            dataPicker.MinValue = 0;
            dataPicker.SetDisplayedValues(picker.Items.ToArray());
            dataPicker.WrapSelectorWheel = false;
            dataPicker.DescendantFocusability = DescendantFocusability.BlockDescendants;
            dataPicker.Value = picker.SelectedIndex;

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
            //avoid parent level event binding
            public new event EventHandler<KeyEventArgs> KeyPress;

            internal CoolField(Context context) : base(context)
            {
                //original binding
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
            xColor backgroundColor = xColor.Default;
            internal xColor BackgroundColor { get => backgroundColor; set { SetBackgroundColor(value.ToAndroid()); backgroundColor = value; } }

            internal CoolDataPicker(Context context) : base(context) { }

        }
    }
}
