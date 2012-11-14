XTween
======
XAML/C# Simple Tween Engine

Example Code
======
    //Simple
    var tween = new XTween(target, "Width", 100, 200, 1);
    tween.Play();
    
    //Cascade
    new XTween(target)
        .SetProperties("Width", "Height")
        .SetFrom(100)
        .SetTo(200)
        .SetDuration(1)
        .Play();
    
    //Serial
    XTween.SerialTweens(
        new XTween(target, "Width", 100, 200, 1),
        new XTween(target, "Height", 100, 200, 1)
    ).Play();
    
    //Parallel
    XTween.ParallelTweens(
        new XTween(target, "Width", 100, 200, 1),
        new XTween(target, "Height", 100, 200, 1)
    ).Play();