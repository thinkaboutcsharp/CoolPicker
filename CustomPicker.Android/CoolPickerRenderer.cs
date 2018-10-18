using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CoolPicker.CoolPicker), typeof(CoolPicker.Android.CoolPickerRenderer))]

namespace CoolPicker.Android
{
    public class CoolPickerRenderer : PickerRenderer
    {
        public CoolPickerRenderer(Context context) : base(context)
        {
        }
    }
}
