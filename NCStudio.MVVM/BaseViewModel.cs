using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NCStudio.MVVM
{
    public abstract class BaseViewModel<T> : ObservableObject<T>
    {
        private string viewTitle;

        public string ViewTitle
        {
            get { return viewTitle; }
            set
            {
                SetField<String>(ref viewTitle, value, vm => ViewTitle);
            }
        }
    }
}
