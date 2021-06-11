using System;

using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.OS;

namespace FingerPaint
{
    [Activity(Label = "Finger Paint", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        FingerPaintCanvasView fingerPaintCanvasView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            fingerPaintCanvasView = FindViewById<FingerPaintCanvasView>(Resource.Id.canvasView);

            Spinner colorSpinner = FindViewById<Spinner>(Resource.Id.colorSpinner);
            colorSpinner.ItemSelected += OnColorSpinnerItemSelected;

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.colors_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            colorSpinner.Adapter = adapter;

            Spinner widthSpinner = FindViewById<Spinner>(Resource.Id.widthSpinner);
            widthSpinner.ItemSelected += OnWidthSpinnerItemSelected; 

            var widthsAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.widths_array, Android.Resource.Layout.SimpleSpinnerItem);
            widthsAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            widthSpinner.Adapter = widthsAdapter;

            Button clearButton = FindViewById<Button>(Resource.Id.clearButton);
            clearButton.Click += OnClearButtonClick;
        }

        void OnColorSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs args)
        {
            Spinner spinner = (Spinner)sender;
            string strColor = (string)spinner.GetItemAtPosition(args.Position);
            Color strokeColor = (Color)(typeof(Color).GetProperty(strColor).GetValue(null));
            fingerPaintCanvasView.StrokeColor = strokeColor;
        }

        void OnWidthSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs args)
        {
            Spinner spinner = (Spinner)sender;
            float strokeWidth = new float[] { 2, 5, 10, 20, 50 } [args.Position];
            fingerPaintCanvasView.StrokeWidth = strokeWidth;
        }

        void OnClearButtonClick(object sender, EventArgs args)
        {
            fingerPaintCanvasView.ClearAll();
        }
    }
}

