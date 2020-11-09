using System;
using System.Diagnostics;
using Lottie.Forms;
using Xamarin.Forms;

namespace AvilaShellAppSample.Triggers
{
    public class StopLottieAnimationTriggerAction : TriggerAction<AnimationView>
    {
        protected override void Invoke(AnimationView sender)
        {
            Debug.WriteLine($"StopLottieAnimationTriggerAction()");
            sender.StopAnimation();
        }
    }
}
