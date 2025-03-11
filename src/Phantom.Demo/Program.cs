// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Phantom.Core;
using Phantom.Input.Events;
using Phantom.Windows;

using PhantomEngine engine = PhantomEngine.Init(SubSystem.Video);

using Window window = new("Hello, Phantom!", 1080, 720, WindowOptions.Resizable);

while (window.IsOpen)
{
    while (window.Poll(out Event e))
    {
        if (e.RequestQuit())
        {
            window.Close();
            return;
        }
    }
}
