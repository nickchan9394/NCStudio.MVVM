using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NCStudio.MVVM
{
    public class GenericWeakEventManager<TEventArgs>
        :IWeakEventListener where TEventArgs:EventArgs
    {
        EventHandler<TEventArgs> realHandler;

        public GenericWeakEventManager(EventHandler<TEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            this.realHandler = handler;

        }


        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            TEventArgs realArgs = (TEventArgs)e;
            this.realHandler(sender, realArgs);
            return true;
        }
    }
}
