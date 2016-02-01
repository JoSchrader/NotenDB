// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="dSPACE GmbH">
//   Copyright (c) dSPACE GmbH. All rights reserved.
// </copyright>
// <author>Marius Bürck, Jonas Schrader</author>
// --------------------------------------------------------------------------------------------------------------------

namespace UserClient.Helper
{
    using System;
    using System.Windows.Input;

    /// <summary>
    ///  A class to bind Commands and Events.
    /// </summary>
    [Serializable]
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The Method to execute when the Command is executed.
        /// </summary>
        [NonSerialized]
        private readonly Action<object> execute;

        /// <summary>
        /// The Method indicating whether the Command can be executed.
        /// </summary>
        [NonSerialized]
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execute method.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execute method.</param>
        /// <param name="canExecute">The can execute method.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }
        
        /// <summary>
        /// Forced by ICommand but no used.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add 
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            { 
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// New instance of the RelayCommand class.
        /// </summary>
        /// <param name="parameter">The parameter to check the can execute with.</param>
        /// <returns>True when the command can be executed, false when the command can not be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter to execute the command with.</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
