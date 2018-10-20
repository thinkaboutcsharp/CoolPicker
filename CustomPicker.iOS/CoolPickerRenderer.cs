using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoolPicker.CoolPicker), typeof(CoolPicker.iOS.CoolPickerRenderer))]

namespace CoolPicker.iOS
{
    class ExtendedField : UITextField
    {
        const float MinimumSpace = 8.0f;

        internal nfloat Space { get; set; }

        readonly HashSet<string> enableActions;

        UIColor defaultTextColor;

        internal ExtendedField() : base(new CGRect())
        {
            string[] actions = { "copy:", "select:", "selectAll:" };
            enableActions = new HashSet<string>(actions);

            defaultTextColor = TextColor;
        }

        internal void ResetTextColor() => TextColor = defaultTextColor;

        public override NSAttributedString AttributedText
        {
            get
            {
                var text = new NSAttributedString(Text, foregroundColor: TextColor);
                base.AttributedText = text;
                return text;
            }
            set => base.AttributedText = value;
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return enableActions.Contains(action.Name);
        }

        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            return forBounds.Inset((nfloat)Math.Max(Space, MinimumSpace), 0);
        }

        public override CGRect PlaceholderRect(CGRect forBounds)
        {
            return forBounds.Inset((nfloat)Math.Max(Space, MinimumSpace), 0);
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            return forBounds.Inset((nfloat)Math.Max(Space, MinimumSpace), 0);
        }
    }

    public class CoolPickerRenderer : PickerRenderer
    {
        IElementController ElementController => Element as IElementController;

        int selectedIndex;

        double defaultHeight;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            ReplaceEntry((CoolPicker)e.NewElement);

            defaultHeight = e.NewElement.HeightRequest;

            Control.Superview.ClipsToBounds = true;

            UpdateBorder();
            UpdateBorderWidth();
            UpdateBorderColor();
            UpdateBorderRadius();
            UpdatePicker();

            Resize();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                // Before calling base, replace UpdatePicker process. Because Model is never PickerSource.
                UpdatePicker();
                return;
            }

            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CoolPicker.BorderColorProperty.PropertyName)
                UpdateBorderColor();
            else if (e.PropertyName == CoolPicker.BorderProperty.PropertyName)
            {
                UpdateBorder();
                Resize();
            }
            else if (e.PropertyName == CoolPicker.BorderWidthProperty.PropertyName)
            {
                UpdateBorderWidth();
                Resize();
            }
            else if (e.PropertyName == CoolPicker.BorderRadiusProperty.PropertyName)
            {
                UpdateBorderRadius();
                Resize();
            }
            else if (e.PropertyName == CoolPicker.PickerColorProperty.PropertyName)
                UpdatePickerColor();
            else if (e.PropertyName == CoolPicker.PickerBarColorProperty.PropertyName)
                UpdatePickerBarColor();
            else if (e.PropertyName == CoolPicker.PickerBarTextColorProperty.PropertyName)
                UpdatePickerBarTextColor();
            else if (e.PropertyName == CoolPicker.PlaceholderColorProperty.PropertyName)
                UpdatePlaceholder();
            else if (e.PropertyName == CoolPicker.PickerTextColorProperty.PropertyName)
            { /* Automatically change by Delegate */ }
        }

        void ReplaceEntry(CoolPicker newPicker)
        {
            var originalEntry = Control as UITextField;
            var customEntry = new ExtendedField { Space = 0 };

            customEntry.EditingDidBegin += OnStarted;
            customEntry.EditingChanged += OnEditing;
            customEntry.EditingDidEnd += OnEnded;

            customEntry.InputView = originalEntry.InputView;
            customEntry.InputAccessoryView = originalEntry.InputAccessoryView;

            var toolBar = (UIToolbar)customEntry.InputAccessoryView;
            if (newPicker.PickerBarColor != Color.Default)
            {
                toolBar.BackgroundColor = newPicker.PickerBarColor.ToUIColor();
            }
            var toolBarItems = toolBar.Items;
            toolBarItems[1] = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
                {
                    var picker = (UIPickerView)Control.InputView;
                    if (selectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
                    {
                        UpdatePickerSelectedIndex(0);
                    }
                    UpdateTextColor();
                    Control.Text = Element.Items[selectedIndex];
                    ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, selectedIndex);
                    customEntry.ResignFirstResponder();
                }
            );
            if (newPicker.PickerBarTextColor != Color.Default)
                toolBarItems[1].TintColor = newPicker.PickerBarTextColor.ToUIColor();
            toolBar.SetItems(toolBarItems, false);

            var pickerView = new UIPickerView();
            customEntry.InputView = pickerView;
            var model = new CoolModel(this);
            pickerView.Model = model;
            pickerView.WeakDelegate = model;
            if (newPicker.PickerColor != Color.Default)
                pickerView.BackgroundColor = newPicker.PickerColor.ToUIColor();

            originalEntry.RemoveFromSuperview();
            originalEntry.Dispose();
            SetNativeControl(customEntry);
        }

        void OnEditing(object sender, EventArgs eventArgs)
        {
            selectedIndex = Element.SelectedIndex;
            var items = Element.Items;
            Control.Text = selectedIndex == -1 || items == null ? "" : items[selectedIndex];
            Control.UndoManager.RemoveAllActions();
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            var picker = (UIPickerView)Control.InputView;
            var model = picker.Model;
            if (selectedIndex != -1 && selectedIndex != picker.SelectedRowInComponent(0))
            {
                picker.Select(selectedIndex, 0, false);
            }
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        void OnStarted(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        void UpdateBorder()
        {
            if (Element == null) return;

            GetSet(out var picker, out var entry, out var view);
            if (picker.Border && picker.BorderWidth > 0)
                entry.BorderStyle = UITextBorderStyle.None;
            else
                entry.BorderStyle = UITextBorderStyle.RoundedRect;
        }

        void UpdateBorderWidth()
        {
            if (Element == null) return;

            GetSet(out var picker, out var entry, out var view);
            if (picker.BorderWidth >= 0)
            {
                var oldWidth = view.Layer.BorderWidth;
                view.Layer.BorderWidth = picker.BorderWidth;
                picker.HeightRequest += (oldWidth - picker.BorderWidth) * 2.0;
            }
            else
            {
                view.Layer.BorderWidth = 0;
                picker.HeightRequest = defaultHeight;
            }
        }

        void UpdateBorderColor()
        {
            if (Element == null) return;

            GetSet(out var picker, out var entry, out var view);
            if (picker.BorderColor != Color.Default)
                view.Layer.BorderColor = picker.BorderColor.ToCGColor();
        }

        void UpdateBorderRadius()
        {
            if (Element == null) return;

            GetSet(out var picker, out var entry, out var view);
            view.Layer.CornerRadius = picker.BorderRadius;
        }

        void UpdatePlaceholder()
        {
            var picker = Element as CoolPicker;
            if (picker.PlaceholderColor != Color.Default)
            {
                Control.Placeholder = null;
                Control.AttributedPlaceholder = new NSAttributedString(picker.Title, foregroundColor: picker.PlaceholderColor.ToUIColor());
            }
            else
            {
                Control.AttributedPlaceholder = null;
                Control.Placeholder = picker.Title;
            }
        }

        void UpdatePickerColor()
        {
            var picker = Element as CoolPicker;
            var pickerView = Control.InputView as UIPickerView;
            if (picker.PickerColor != Color.Default || pickerView.BackgroundColor != picker.PickerColor.ToUIColor())
                pickerView.BackgroundColor = picker.PickerColor.ToUIColor();
        }

        void UpdatePickerBarColor()
        {
            var picker = Element as CoolPicker;
            var toolBar = Control.InputAccessoryView as UIToolbar;
            if (picker.PickerBarColor != Color.Default || toolBar.BackgroundColor != picker.PickerBarColor.ToUIColor())
                toolBar.BackgroundColor = picker.PickerBarColor.ToUIColor();
        }

        void UpdatePickerBarTextColor()
        {
            var picker = Element as CoolPicker;
            var toolBar = Control.InputAccessoryView as UIToolbar;
            var doneButton = toolBar.Items[1];
            if (picker.PickerBarTextColor != Color.Default || doneButton.TintColor != picker.PickerBarTextColor.ToUIColor())
                doneButton.TintColor = picker.PickerBarTextColor.ToUIColor();
        }

        void UpdatePicker()
        {
            var recentSelectedIndex = Element.SelectedIndex;
            var items = Element.Items;
            var pickerView = (UIPickerView)Control.InputView;
            UpdatePlaceholder();
            var oldText = Control.Text;
            Control.Text = recentSelectedIndex == -1 || items == null || recentSelectedIndex >= items.Count ? "" : items[recentSelectedIndex];
            if (oldText != Control.Text)
                ((IVisualElementController)Element).NativeSizeChanged();
            pickerView.ReloadAllComponents();
            if (items == null || items.Count == 0)
                return;

            UpdatePickerSelectedIndex(recentSelectedIndex);
        }

        void UpdatePickerSelectedIndex(int formsIndex)
        {
            if (Element.SelectedIndex == formsIndex) return;

            var picker = (UIPickerView)Control.InputView;
            picker.Select(Math.Max(formsIndex, 0), 0, true);
            UpdateTextColor();
            Control.Text = formsIndex == -1
                                ? null
                                : Element.Items.Count > 0
                                    ? Element.Items[(int)picker.SelectedRowInComponent(0)]
                                    : null;
            ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, formsIndex);
            selectedIndex = formsIndex;
        }

        void UpdateTextColor()
        {
            if (Element.TextColor != Color.Default)
                Control.TextColor = Element.TextColor.ToUIColor();
            else
                ((ExtendedField)Control).ResetTextColor();
        }

        void GetSet(out CoolPicker picker, out UITextField entry, out UIView view)
        {
            picker = Element as CoolPicker;
            entry = Control as UITextField;
            view = entry.Superview;
        }

        void Resize()
        {
            GetSet(out var picker, out var entry, out var view);
            var space = view.Layer.BorderWidth + 5.0f;
            ((ExtendedField)Control).Space = space;
        }

        bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (disposed) return;

            disposed = true;

            if (disposing)
            {
                Control.EditingDidBegin -= OnStarted;
                Control.EditingChanged -= OnEditing;
                Control.EditingDidEnd -= OnEnded;
            }

            base.Dispose(disposing);
        }

        class CoolModel : UIPickerViewModel
        {
            CoolPickerRenderer renderer;
            CoolPicker picker;
            Color currentColor;

            internal CoolModel(CoolPickerRenderer renderer)
            {
                this.renderer = renderer;
                picker = renderer.Element as CoolPicker;
                currentColor = picker.PickerTextColor == Color.Default ? Color.Black : picker.PickerTextColor;
                //iOS can not use Color.Default for UIPickerView item title. If use it we will get transparent text.
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return picker.Items.Count;
            }

            public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
            {
                if (currentColor != picker.PickerTextColor)
                {
                    currentColor = picker.PickerTextColor == Color.Default ? Color.Black : picker.PickerTextColor;
                }

                return new NSAttributedString(picker.Items[(int)row], foregroundColor: currentColor.ToUIColor());
            }

            public override void Selected(UIPickerView pickerView, nint row, nint component)
            {
                if (renderer.Element.Items.Count == 0)
                {
                    renderer.UpdatePickerSelectedIndex(-1);
                }
                else
                {
                    renderer.UpdatePickerSelectedIndex((int)row);
                }
            }
        }
    }

    public static class Initialize
    {
        public static void Init() { }
    }
}
