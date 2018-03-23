using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace NCStudio.MVVM
{
    public class MvvmCommand:ICommand
    {
        private Action<object> execute = null;

        private Func<object, bool> canExecute = null;

        private GenericWeakEventManager<PropertyChangedEventArgs> weakPropertyChangedEventListener;

        private GenericWeakEventManager<NotifyCollectionChangedEventArgs> weakCollectionChangedEventListener;

        #region Implement ICommand
        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
            {
                return this.canExecute(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if (this.execute != null)
            {
                this.execute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {

            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Listner
       

        private void RequeryCanExecuteForProperty(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnCanExecuteChanged();
        }

        private void RequeryCanExecuteForCollection(object sender, NotifyCollectionChangedEventArgs collectionChangedEventArgs)
        {
            OnCanExecuteChanged();
        }

        public MvvmCommand AddListener<TEntity>(INotifyPropertyChanged source, Expression<Func<TEntity, object>> property)
        {
            string propertyName = GetPropertyName(property);
            PropertyChangedEventManager.AddListener(source, weakPropertyChangedEventListener, propertyName);
            return this;
        }

        public MvvmCommand AddListener<TEntity>(INotifyCollectionChanged source)
        {
            CollectionChangedEventManager.AddListener(source, weakCollectionChangedEventListener);
            return this;
        }
        #endregion

        public MvvmCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            
            weakPropertyChangedEventListener = new GenericWeakEventManager<PropertyChangedEventArgs>(RequeryCanExecuteForProperty);
            weakCollectionChangedEventListener = new GenericWeakEventManager<NotifyCollectionChangedEventArgs>(RequeryCanExecuteForCollection);
        }

        private String GetPropertyName<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }
            var constantExpression = memberExpression.Expression as ConstantExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            return propertyInfo.Name;

        }



        
        
    }
}
