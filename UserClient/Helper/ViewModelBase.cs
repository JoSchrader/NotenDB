// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="dSPACE GmbH">
//   Copyright (c) dSPACE GmbH. All rights reserved.
// </copyright>
// <author>Marius Bürck, Jonas Schrader</author>
// --------------------------------------------------------------------------------------------------------------------

namespace UserClient.Helper
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    /// <summary>
    /// A class used as a base class for all ViewModels to keep GUI and ViewModel synchronous.
    /// </summary>
    [Serializable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is fired when a Property changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Fires a property changed event.
        /// </summary>
        /// <typeparam name="TProperty">The Type.</typeparam>
        /// <param name="property">The changed event args.</param>
        public void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }
    }
}