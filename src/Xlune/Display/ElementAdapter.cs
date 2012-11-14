using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Xlune.Display
{
    class ElementAdapter : DependencyObject
    {
        private const int TRANSFORM_SCALE_ = 0;
        private const int TRANSFORM_ROTATE_ = 1;
        private const int TRANSFORM_TRANSRATE_ = 2;
        private const int TRANSFORM_SKEW_ = 3;

        private FrameworkElement element_;
        private TransformGroup transformGroup_;

        public ElementAdapter(FrameworkElement el)
        {

            element_ = el;

            /**
             * copy default property 
             */
            if(!double.IsNaN(Element.Width)){
                Width = Element.Width;
            }
            if (!double.IsNaN(Element.Height))
            {
                Height = Element.Height;
            }
            X = Canvas.GetLeft(Element);
            Y = Canvas.GetTop(Element);
            Alpha = Element.Opacity;

            /**
             * Transform
             */
            ScaleTransform myScaleTransform = new ScaleTransform();
            myScaleTransform.ScaleX = 1.0;
            myScaleTransform.ScaleY = 1.0;

            RotateTransform myRotateTransform = new RotateTransform();
            myRotateTransform.Angle = 0;

            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = 0;
            myTranslate.X = 0;

            SkewTransform mySkew = new SkewTransform();
            mySkew.AngleX = 0;
            mySkew.AngleY = 0;

            transformGroup_ = new TransformGroup();
            transformGroup_.Children.Add(myScaleTransform);
            transformGroup_.Children.Add(myRotateTransform);
            transformGroup_.Children.Add(myTranslate);
            transformGroup_.Children.Add(mySkew);

            element_.RenderTransform = transformGroup_;
        }

        /*================================================
         * Instance Properties
         *===============================================*/

        //Wrap UIElement
        public FrameworkElement Element
        {
            get
            {
                return element_;
            }
        }

        //Element Width
        public double Width
        {
            set
            {
                SetValue(WidthProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(WidthProperty));
            }
        }
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(0, WidthChanged));
        private static void WidthChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            tg.Element.Width = Convert.ToDouble(e.NewValue);
        }

        //Element Height
        public double Height
        {
            set
            {
                SetValue(HeightProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(HeightProperty));
            }
        }
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            "Height",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(0, HeightChanged));
        private static void HeightChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            tg.Element.Height = Convert.ToDouble(e.NewValue);

        }

        //Position X
        public double X
        {
            set
            {
                SetValue(XProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(XProperty));
            }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(0, XChanged));
        private static void XChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            Canvas.SetLeft(tg.Element, Convert.ToDouble(e.NewValue));
        }

        //Position Y
        public double Y
        {
            set
            {
                SetValue(YProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(YProperty));
            }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(0, YChanged));
        private static void YChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            Canvas.SetTop(tg.Element, Convert.ToDouble(e.NewValue));
        }

        //Anchor Point
        public Point AnchorPoint
        {
            set
            {
                SetValue(AnchorPointProperty, value);
            }
            get
            {
                return (Point)GetValue(AnchorPointProperty);
            }
        }
        public static readonly DependencyProperty AnchorPointProperty = DependencyProperty.Register(
            "AnchorPoint",
            typeof(Point),
            typeof(ElementAdapter),
            new PropertyMetadata(new Point(0, 0), AnchorPointChanged));
        private static void AnchorPointChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            Point point = (Point)e.NewValue;
            tg.Element.RenderTransformOrigin = point;
            TranslateTransform t = (TranslateTransform)tg.transformGroup_.Children[TRANSFORM_TRANSRATE_];
            t.X = tg.Width * -point.X;
            t.Y = tg.Height * -point.Y;
            tg.transformGroup_.Children[TRANSFORM_TRANSRATE_] = t;
        }

        //Scale X
        public double ScaleX
        {
            set
            {
                SetValue(ScaleXProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(ScaleXProperty));
            }
        }
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(
            "ScaleX",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(1.0, ScaleXChanged));
        private static void ScaleXChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            ScaleTransform t = (ScaleTransform)tg.transformGroup_.Children[TRANSFORM_SCALE_];
            t.ScaleX = Convert.ToDouble(e.NewValue);
            tg.transformGroup_.Children[TRANSFORM_SCALE_] = t;
        }

        //Scale Y
        public double ScaleY
        {
            set
            {
                SetValue(ScaleYProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(ScaleYProperty));
            }
        }
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(
            "ScaleY",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(1.0, ScaleYChanged));
        private static void ScaleYChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            ScaleTransform t = (ScaleTransform)tg.transformGroup_.Children[TRANSFORM_SCALE_];
            t.ScaleY = Convert.ToDouble(e.NewValue);
            tg.transformGroup_.Children[TRANSFORM_SCALE_] = t;
        }

        //Rotation
        public double Rotate
        {
            set
            {
                SetValue(RotateProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(RotateProperty));
            }
        }
        public static readonly DependencyProperty RotateProperty = DependencyProperty.Register(
            "Rotate",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(0, RotateChanged));
        private static void RotateChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            RotateTransform t = (RotateTransform)tg.transformGroup_.Children[TRANSFORM_ROTATE_];
            t.Angle = Convert.ToDouble(e.NewValue);
            tg.transformGroup_.Children[TRANSFORM_ROTATE_] = t;
        }

        //Z-Index
        public int ZIndex
        {
            set
            {
                SetValue(ZIndexProperty, value);
            }
            get
            {
                return Convert.ToInt32(GetValue(ZIndexProperty));
            }
        }
        public static readonly DependencyProperty ZIndexProperty = DependencyProperty.Register(
            "ZIndex",
            typeof(int),
            typeof(ElementAdapter),
            new PropertyMetadata(0, ZIndexChanged));
        private static void ZIndexChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            Canvas.SetZIndex(tg.Element, Convert.ToInt32(e.NewValue));
        }

        //Alpha
        public double Alpha
        {
            set
            {
                SetValue(AlphaProperty, value);
            }
            get
            {
                return Convert.ToDouble(GetValue(AlphaProperty));
            }
        }
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            "Alpha",
            typeof(Double),
            typeof(ElementAdapter),
            new PropertyMetadata(1, AlphaChanged));
        private static void AlphaChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ElementAdapter tg = (ElementAdapter)target;
            tg.Element.Opacity = Convert.ToDouble(e.NewValue);
        }
    }
}
