// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionCommand.cs" company="dSPACE GmbH">
//   Copyright (c) dSPACE GmbH. All rights reserved.
// </copyright>
// <author>Marius Bürck, Jonas Schrader</author>
// --------------------------------------------------------------------------------------------------------------------

namespace UserClient.Helper
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A class to bind Commands and Events with EventArgs parsing.
    /// </summary>
    /// <typeparam name="T">The EventArgs type.</typeparam>
    [Serializable]
    public class ActionCommand<T> : ICommand
    {
        /// <summary>
        /// The Action to perform when the event is fired.
        /// </summary>
        [NonSerialized]
        private Action<T> action;

        /// <summary>
        /// Initializes a new instance of the ActionCommand class.
        /// </summary>
        /// <param name="action">The Action to perform when the event is fired.</param>
        public ActionCommand(Action<T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Forced by ICommand but no used.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines whether the Command can be executed - every time true.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        /// <returns>Every time true.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the action with the given EvenArgs parameter.
        /// </summary>
        /// <param name="parameter">The EventArgs.</param>
        public void Execute(object parameter)
        {
            if (this.action != null)
            {
                var castParameter = (T)Convert.ChangeType(parameter, typeof(T));
                this.action(castParameter);
            }
        }
    }
}
