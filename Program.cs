using Cairo;
using Gdk;
using Gtk;
using Color = Cairo.Color;

class FPSCounter
{
    public float FPS { get; set; }
    float[] elapseds;
    DateTime _last;
    int cur = 0;

    public void update()
    {
        var now = DateTime.Now;
        var elapsed = now - _last;
        elapseds[cur] = (float)elapsed.TotalSeconds;
        cur = (cur + 1) % elapseds.Length;
        float sum = 0;
        foreach (var i in elapseds)
        {
            sum += i;
        }
        FPS = elapseds.Length / sum;
        _last = now;
    }

    public FPSCounter(int n = 10)
    {
        _last = DateTime.Now;
        elapseds = new float[n];
    }
}

class Area : DrawingArea
{
    Color black = new Color(0, 0, 0),
        blue = new Color(0, 0, 1),
        light_green = new Color(0.56, 0.93, 0.56);

    int n;
    int i;
    int x,
        y;
    DateTime _last;
    Random rand = new Random();
    FPSCounter fps = new FPSCounter(100);
    List<(int x, int y, Color c)> points = new List<(int x, int y, Color c)>();

    public Area(int n)
    {
        Name = $"Area {n}";
        this.n = n;
        this.i = 0;
        _last = DateTime.Now;
    }

    protected override bool OnDrawn(Context c)
    {
        ++i;
        fps.update();
        x = rand.Next(800);
        y = rand.Next(600);
        Color color = new Color(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
        points.Add((x, y, color));

        c.SetSourceColor(light_green);
        c.LineWidth = 5;
        foreach (var p in points)
        {
            c.SetSourceColor(p.c);
            c.LineTo(p.x, p.y);
            c.Stroke();
            c.MoveTo(p.x, p.y);
        }

        // draw a rectangle
        c.SetSourceColor(black);
        c.Rectangle(x: 100, y: 200, width: 200, height: 100);
        c.Stroke();

        // draw text centered in the rectangle
        (int cx, int cy) = (200, 250); // center of rectangle
        string s = $"FPS:{fps.FPS:0.0} hello {i}.";
        c.SetFontSize(30);
        TextExtents te = c.TextExtents(s);
        c.MoveTo(cx - (te.Width / 2 + te.XBearing), cy - (te.Height / 2 + te.YBearing));
        c.ShowText(s);
        QueueDraw();

        return true;
    }
}

class MyWindow : Gtk.Window
{
    public MyWindow()
        : base("hello, world")
    {
        Resize(800, 600);
        Add(new Area(1)); // add an Area to the window
    }

    protected override bool OnDeleteEvent(Event e)
    {
        Application.Quit();
        return true;
    }
    protected override bool OnKeyPressEvent(EventKey e)
    {
        Console.WriteLine($"key pressed: {e.Key} - {e.HardwareKeycode} - {e.State} - {e.Time}  - {e.Window}");
        if (e.Key == Gdk.Key.Escape)
        {
            Application.Quit();
            return true;
        }
        return base.OnKeyPressEvent(e);
    }
    protected override bool OnButtonPressEvent(EventButton e)
    {
        Console.WriteLine($"button pressed: {e.Button} - {e.X} - {e.Y} - {e.Time} - {e.Window}");
        return base.OnButtonPressEvent(e);
    }
    protected override bool OnMotionNotifyEvent(EventMotion e)
    {
        Console.WriteLine($"motion: {e.X} - {e.Y} - {e.Time} - {e.Window}");
        return base.OnMotionNotifyEvent(e);
    }
    protected override bool OnScrollEvent(EventScroll e)
    {
        Console.WriteLine($"scroll: {e.DeltaX} - {e.DeltaY} - {e.Time} - {e.Window}");
        return base.OnScrollEvent(e);
    }
    protected override bool OnConfigureEvent(EventConfigure e)
    {
        Console.WriteLine($"configure: {e.Width} - {e.Height} -- {e.Window}");
        return base.OnConfigureEvent(e);
    }
    // protected override bool OnExposeEvent(EventExpose e)
    // {
    //     Console.WriteLine($"expose: {e.Area.X} - {e.Area.Y} - {e.Area.Width} - {e.Area.Height} - {e.Time} - {e.Window}");
    //     return base.OnExposeEvent(e);
    // }
    protected override bool OnVisibilityNotifyEvent(EventVisibility e)
    {
        Console.WriteLine($"visibility: {e.State} - {e.Window}");
        return base.OnVisibilityNotifyEvent(e);
    }
    protected override bool OnWindowStateEvent(EventWindowState e)
    {
        Console.WriteLine($"window state: {e.ChangedMask} - {e.NewWindowState} - {e.Window}");
        return base.OnWindowStateEvent(e);
    }
    protected override bool OnFocusInEvent(EventFocus e)
    {
        Console.WriteLine($"focus in: {e.Window}");
        return base.OnFocusInEvent(e);
    }
    protected override bool OnFocusOutEvent(EventFocus e)
    {
        Console.WriteLine($"focus out: {e.Window}");
        return base.OnFocusOutEvent(e);
    }
    protected override bool OnEnterNotifyEvent(EventCrossing e)
    {
        Console.WriteLine($"enter: {e.Mode} - {e.X} - {e.Y} - {e.Time} - {e.Window}");
        return base.OnEnterNotifyEvent(e);
    }
    protected override bool OnLeaveNotifyEvent(EventCrossing e)
    {
        Console.WriteLine($"leave: {e.Mode} - {e.X} - {e.Y} - {e.Time} - {e.Window}");
        return base.OnLeaveNotifyEvent(e);
    }
    // protected override bool OnSizeAllocate(Allocation a)
    // {
    //     Console.WriteLine($"size allocate: {a.Width} - {a.Height}");
    //     return base.OnSizeAllocate(a);
    // }
    // protected override bool OnSizeRequest(ref Requisition r)
    // {
    //     Console.WriteLine($"size request: {r.Width} - {r.Height}");
    //     return base.OnSizeRequest(ref r);
    // }
    // protected override bool OnRealize()
    // {
    //     Console.WriteLine("realize");
    //     return base.OnRealize();
    // }
    // protected override bool OnUnrealize()
    // {
    //     Console.WriteLine("unrealize");
    //     return base.OnUnrealize();
    // }
    protected override bool OnMapEvent(Event e)
    {
        Console.WriteLine("map");
        return base.OnMapEvent(e);
    }
    protected override bool OnUnmapEvent(Event e)
    {
        Console.WriteLine("unmap");
        return base.OnUnmapEvent(e);
    }
    protected override bool OnPropertyNotifyEvent(EventProperty e)
    {
        Console.WriteLine("property notify");
        return base.OnPropertyNotifyEvent(e);
    }
    protected override bool OnSelectionClearEvent(EventSelection e)
    {
        Console.WriteLine("selection clear");
        return base.OnSelectionClearEvent(e);
    }
    protected override bool OnSelectionNotifyEvent(EventSelection e)
    {
        Console.WriteLine("selection notify");
        return base.OnSelectionNotifyEvent(e);
    }
    protected override bool OnSelectionRequestEvent(EventSelection e)
    {
        Console.WriteLine("selection request");
        return base.OnSelectionRequestEvent(e);
    }
    protected override bool OnProximityInEvent(EventProximity e)
    {
        Console.WriteLine("proximity in");
        return base.OnProximityInEvent(e);
    }
    protected override bool OnProximityOutEvent(EventProximity e)
    {
        Console.WriteLine("proximity out");
        return base.OnProximityOutEvent(e);
    }
    // protected override bool OnDragBegin(DragContext dc)
    // {
    //     Console.WriteLine("drag begin");
    //     return base.OnDragBegin(dc);
    // }
    // protected override bool OnDragDataDelete(DragContext dc)
    // {
    //     Console.WriteLine("drag data delete");
    //     return base.OnDragDataDelete(dc);
    // }
    // protected override bool OnDragDataGet(DragContext dc, SelectionData sd, uint info, uint time)
    // {
    //     Console.WriteLine("drag data get");
    //     return base.OnDragDataGet(dc, sd, info, time);
    // }
    // protected override bool OnDragDataReceived(DragContext dc, int x, int y, SelectionData sd, uint info, uint time)
    // {
    //     Console.WriteLine("drag data received");
    //     return base.OnDragDataReceived(dc, x, y, sd, info, time);
    // }
    protected override bool OnDragDrop(DragContext dc, int x, int y, uint time)
    {
        Console.WriteLine("drag drop");
        return base.OnDragDrop(dc, x, y, time);
    }
    // protected override bool OnDragEnd(DragContext dc)
    // {
    //     Console.WriteLine("drag end");
    //     return base.OnDragEnd(dc);
    // }
    protected override bool OnDragFailed(DragContext dc, DragResult result)
    {
        Console.WriteLine("drag failed");
        return base.OnDragFailed(dc, result);
    }
    // protected override bool OnDragLeave(DragContext dc, uint time)
    // {
    //     Console.WriteLine("drag leave");
    //     return base.OnDragLeave(dc, time);
    // }
    protected override bool OnDragMotion(DragContext dc, int x, int y, uint time)
    {
        Console.WriteLine("drag motion");
        return base.OnDragMotion(dc, x, y, time);
    }
    // protected override bool OnDragOver(DragContext dc, DragAction da, uint time)
    // {
    //     Console.WriteLine("drag over");
    //     return base.OnDragOver(dc, da, time);
    // }
    protected override bool OnQueryTooltip(int x, int y, bool keyboard_mode, Tooltip tooltip)
    {
        Console.WriteLine("query tooltip");
        return base.OnQueryTooltip(x, y, keyboard_mode, tooltip);
    }
    // protected override bool OnScreenChanged(Screen screen, Window previous_screen)
    // {
    //     Console.WriteLine("screen changed");
    //     return base.OnScreenChanged(screen, previous_screen);
    // }
    // protected override bool OnStyleSet(Style previous_style)
    // {
    //     Console.WriteLine("style set");
    //     return base.OnStyleSet(previous_style);
    // }
    // protected override bool OnWindowStateChanged(WindowState state)
    // {
    //     Console.WriteLine("window state changed");
    //     return base.OnWindowStateChanged(state);
    // }   
    // protected override bool OnWindowStateEvent(EventWindowState e)
    // {
    //     Console.WriteLine("window state event $(e.NewWindowState)");
    //     return base.OnWindowStateChanged(e.NewWindowState);
    // }
}

class Hello
{
    static void Main()
    {
        Application.Init();
        MyWindow w = new MyWindow();
        w.ShowAll();
        Application.Run();
    }
}
