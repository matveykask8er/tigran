using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;

namespace FingerPaint
{
    public class FingerPaintCanvasView : View
    {

        Dictionary<int, FingerPaintPolyline> inProgressPolylines = new Dictionary<int, FingerPaintPolyline>();
        List<FingerPaintPolyline> completedPolylines = new List<FingerPaintPolyline>();

        Paint paint = new Paint();

        public FingerPaintCanvasView(Context context) : base(context)
        {
            Initialize();
        }

        public FingerPaintCanvasView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        void Initialize()
        {
        }

        public Color StrokeColor { set; get; } = Color.Red;

        public float StrokeWidth { set; get; } = 2;

        public void ClearAll()
        {
            completedPolylines.Clear();
            Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent args)
        {
            int pointerIndex = args.ActionIndex;

            int id = args.GetPointerId(pointerIndex);

            switch (args.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:

                    FingerPaintPolyline polyline = new FingerPaintPolyline
                    {
                        Color = StrokeColor,
                        StrokeWidth = StrokeWidth
                    };

                    polyline.Path.MoveTo(args.GetX(pointerIndex),
                                         args.GetY(pointerIndex));

                    inProgressPolylines.Add(id, polyline);
                    break;

                case MotionEventActions.Move:

                    for (pointerIndex = 0; pointerIndex < args.PointerCount; pointerIndex++)
                    {
                        id = args.GetPointerId(pointerIndex);

                        inProgressPolylines[id].Path.LineTo(args.GetX(pointerIndex),
                                                            args.GetY(pointerIndex));
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:

                    inProgressPolylines[id].Path.LineTo(args.GetX(pointerIndex),
                                                        args.GetY(pointerIndex));

                    completedPolylines.Add(inProgressPolylines[id]);
                    inProgressPolylines.Remove(id);
                    break;

                case MotionEventActions.Cancel:
                    inProgressPolylines.Remove(id);
                    break;
            }
       
            Invalidate();

            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            paint.SetStyle(Paint.Style.Fill);
            paint.Color = Color.White;
            canvas.DrawPaint(paint);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeCap = Paint.Cap.Round;
            paint.StrokeJoin = Paint.Join.Round;

            foreach (FingerPaintPolyline polyline in completedPolylines)
            {
                paint.Color = polyline.Color;
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }

            foreach (FingerPaintPolyline polyline in inProgressPolylines.Values)
            {
                paint.Color = polyline.Color;
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }
        }
    }
}