using System;
using System.Drawing;

namespace XamarinStudy11
{
    class CommonModel
    {
        internal Color TextColor { get; private set; } = Color.Transparent;
        internal Color PlaceholderColor { get; private set; } = Color.Transparent;
        internal Color BackgroundColor { get; private set; } = Color.Transparent;

        internal bool Border { get; private set; } = false;
        internal Color BorderColor { get; private set; } = Color.Transparent;
        internal int BorderWidth { get; private set; } = 0;
        internal float BorderRadius { get; private set; } = 0f;

        internal Color PickerColor { get; private set; } = Color.Transparent;
        internal Color PickerBarColor { get; private set; } = Color.Transparent;
        internal Color PickerTextColor { get; private set; } = Color.Transparent;
        internal Color PickerBarTextColor { get; private set; } = Color.Transparent;

        internal Color ToggleTextColor()
        {
            if (TextColor == Color.Transparent)
                TextColor = Color.BlueViolet;
            else
                TextColor = Color.Transparent;
            return TextColor;
        }

        internal Color TogglePlaceholderColor()
        {
            if (PlaceholderColor == Color.Transparent)
                PlaceholderColor = Color.PaleGoldenrod;
            else
                PlaceholderColor = Color.Transparent;
            return PlaceholderColor;
        }

        internal Color ToggleBackgroundColor()
        {
            if (BackgroundColor == Color.Transparent)
                BackgroundColor = Color.Ivory;
            else
                BackgroundColor = Color.Transparent;
            return BackgroundColor;
        }

        internal bool ToggleBorder()
        {
            Border = !Border;
            return Border;
        }

        internal Color ToggleBorderColor()
        {
            if (BorderColor == Color.Transparent)
                BorderColor = Color.DarkGreen;
            else
                BorderColor = Color.Transparent;
            return BorderColor;
        }

        internal int ToggleBorderWidth()
        {
            if (BorderWidth == 0)
                BorderWidth = 2;
            else if (BorderWidth == 2)
                BorderWidth = 5;
            else
                BorderWidth = 0;
            return BorderWidth;
        }

        internal float ToggleBorderRadius()
        {
            if (BorderRadius < 10e-3)
                BorderRadius = 6.0f;
            else if (BorderRadius < 6.0f + 10e-3)
                BorderRadius = 10.0f;
            else
                BorderRadius = 0.0f;
            return BorderRadius;
        }

        internal Color TogglePickerColor()
        {
            if (PickerColor == Color.Transparent)
                PickerColor = Color.LemonChiffon;
            else
                PickerColor = Color.Transparent;
            return PickerColor;
        }

        internal Color TogglePickerBarColor()
        {
            if (PickerBarColor == Color.Transparent)
                PickerBarColor = Color.Thistle;
            else
                PickerBarColor = Color.Transparent;
            return PickerBarColor;
        }

        internal Color TogglePickerTextColor()
        {
            if (PickerTextColor == Color.Transparent)
                PickerTextColor = Color.Yellow;
            else
                PickerTextColor = Color.Transparent;
            return PickerTextColor;
        }

        internal Color TogglePickerBarTextColor()
        {
            if (PickerBarTextColor == Color.Transparent)
                PickerBarTextColor = Color.Salmon;
            else
                PickerBarTextColor = Color.Transparent;
            return PickerBarTextColor;
        }
    }
}
