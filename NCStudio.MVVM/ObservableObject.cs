using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NCStudio.MVVM
{
    public abstract class ObservableObject<TSource>:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<TProperty>(Expression<Func<TSource,TProperty>> propertyExpression)
        {
            String propertyName=GetPropertyName<TProperty>(propertyExpression);
            if(PropertyChanged!=null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnPropertyChanged(String propertyName)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnPropertyChanged<TProperty>(Expression<Func<TSource,TProperty>> propertyExpression,Object oldValue,Object newValue)
        {
            String propertyName = GetPropertyName<TProperty>(propertyExpression);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName,oldValue,newValue));
            }
        }

        public void OnPropertyChanged(String propertyName,Object oldValue,Object newValue)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName, oldValue, newValue));
            }
        }

        private string GetPropertyName<TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "expression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", "expression");
            }

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property.", "expression");
            }

            return memberExpression.Member.Name;
        }

        protected virtual bool SetField<TProperty>(ref TProperty field, TProperty value, Expression<Func<TSource, TProperty>> propertyExpression)
        {
            if(EqualityComparer<TProperty>.Default.Equals(field,value))return false;
            field = value;
            OnPropertyChanged(propertyExpression);
            return true;
        }
    }
}
