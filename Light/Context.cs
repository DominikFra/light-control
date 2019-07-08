using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Light
{
    public class Context
    {
        internal Dispatcher uiDispatcher;
        internal TaskScheduler uiThreadScheduler;
        internal Action RefreshLights;
        
        internal Context()
        {
            uiDispatcher = Dispatcher.CurrentDispatcher;
            uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

    }
}
