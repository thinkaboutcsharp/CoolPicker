using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace XamarinStudy11
{
    public partial class StartPage : ContentPage
    {
        NavigationPage navi;
        public StartPage()
        {
            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            navi = Parent as NavigationPage;
        }

        void OnBindingClicked(object sender, EventArgs args)
        {
            navi.PushAsync(new BindingPattern());
        }

        void OnStyleClicked(object sender, EventArgs args)
        {
            navi.PushAsync(new ImplicitStylePattern());
        }
    }
}
