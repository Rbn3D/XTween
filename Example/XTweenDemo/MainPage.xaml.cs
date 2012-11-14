using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xlune.Animation;
using Xlune.Display;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace XTweenDemo
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Rect winRect_;
        private List<ElementAdapter> targets_;
        private double cellWidth_;

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResetPosition();
        }

        private void ResetPosition()
        {
            winRect_ = Window.Current.Bounds;
            targets_ = new List<ElementAdapter>();
            targets_.Add(new ElementAdapter(Target1));
            targets_.Add(new ElementAdapter(Target2));
            targets_.Add(new ElementAdapter(Target3));
            targets_.Add(new ElementAdapter(Target4));

            cellWidth_ = winRect_.Width / 4;
            for (int i = 0; i < targets_.Count; ++i )
            {
                targets_[i].AnchorPoint = new Point(0.5, 0.5);
                targets_[i].X = cellWidth_ * i + targets_[i].Width/2 + 10;
                targets_[i].Y = targets_[i].Height / 2 + 10;
            }
        }

        private void PlayAnimation1(object sender, TappedRoutedEventArgs e)
        {
            Button bt = (Button)sender;
            bt.IsEnabled = false;
            double originWidth = Target1.Width;

            var tween = new XTween(Target1, "Width", originWidth, cellWidth_ - 20, 2, XTween.Easing.EaseExpoInOut);
            tween.OnCompleted += (sender_, e_) => {
                Target1.Width = originWidth;
                bt.IsEnabled = true;
            };
            tween.Play();
        }

        private void PlayAnimation2(object sender, TappedRoutedEventArgs e)
        {
            Button bt = (Button)sender;
            bt.IsEnabled = false;

            var tween = XTween.SerialTweens(
                new XTween(Target2, "Width", 100, cellWidth_ - 20, 1, XTween.Easing.EaseElasticOut),
                new XTween(Target2, "Height", 100, cellWidth_ - 20, 1, XTween.Easing.EaseElasticOut),
                new XTween(Target2, "Width", cellWidth_ - 20, 100, 1, XTween.Easing.EaseBounceOut),
                new XTween(Target2, "Height", cellWidth_ - 20, 100, 1, XTween.Easing.EaseBounceOut)
            );
            tween.OnCompleted += (sender_, e_) =>
            {
                bt.IsEnabled = true;
            };
            tween.Play();
        }

        private void PlayAnimation3(object sender, TappedRoutedEventArgs e)
        {
            Button bt = (Button)sender;
            bt.IsEnabled = false;

            double originX = targets_[2].X;
            double originY = targets_[2].Y;

            var tween = XTween.ParallelTweens(
                new XTween(targets_[2], "X", originX, cellWidth_ * 2, 2, XTween.Easing.EaseElasticOut),
                new XTween(targets_[2], "Y", originY, winRect_.Height / 2 - 100, 2, XTween.Easing.EaseElasticOut),
                new XTween(targets_[2], "ScaleX", 1, 5, 2, XTween.Easing.EaseElasticOut),
                new XTween(targets_[2], "ScaleY", 1, 5, 2, XTween.Easing.EaseElasticOut),
                new XTween(targets_[2], "Rotate", 0, 180, 2, XTween.Easing.EaseCubicOut)
            );
            tween.OnCompleted += (sender_, e_) =>
            {
                targets_[2].X = originX;
                targets_[2].Y = originY;
                targets_[2].ScaleX = 1;
                targets_[2].ScaleY = 1;
                targets_[2].Rotate = 0;
                bt.IsEnabled = true;
            };
            tween.Play();

        }

        private void PlayAnimation4(object sender, TappedRoutedEventArgs e)
        {
            Button bt = (Button)sender;
            bt.IsEnabled = false;

            double originX = targets_[3].X;
            double originY = targets_[3].Y;

            XTween.SerialTweens(
                XTween.ParallelTweens(
                    new XTween(targets_[3], null, 1, 5, 1, XTween.Easing.EaseBounceOut).SetProperties("ScaleX", "ScaleY"),
                    new XTween(targets_[3], "X", originX, cellWidth_ * 2, 2, XTween.Easing.EaseElasticOut),
                    new XTween(targets_[3], "Y", originY, winRect_.Height / 2 - 100, 2, XTween.Easing.EaseElasticOut)
                ),
                XTween.ParallelTweens(
                    new XTween(targets_[3], null, 5, 1, 1, XTween.Easing.EaseBounceOut).SetProperties("ScaleX", "ScaleY"),
                    new XTween(targets_[3], "Rotate", 0, 180, 1, XTween.Easing.EaseElasticOut)
                ).SetDelay(0.5),
                XTween.ParallelTweens(
                    new XTween(targets_[3], "X", originX, 1, XTween.Easing.EaseBackOut),
                    new XTween(targets_[3], "Y", originY, 1, XTween.Easing.EaseBackOut)
                ).SetDelay(0.5)
            )
            .SetComplete((sender_, e_) => { bt.IsEnabled = true; })
            .Play();
        }
    }
}
