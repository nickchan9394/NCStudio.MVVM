using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NCStudio.MVVM
{
    public class PropertyChangedExtendedEventArgs:PropertyChangedEventArgs
    {
        private Object oldValue;
        private Object newValue;

        public PropertyChangedExtendedEventArgs(String propertyName,Object oldValue,Object newValue)
            :base(propertyName)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public Object OldValue
        {
            get
            {
                return oldValue;
            }
        }

        public Object NewValue
        {
            get
            {
                return newValue;
            }
        }
    }
}
