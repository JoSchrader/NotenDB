// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToCommandBehavior.cs" company="dSPACE GmbH">
//   Copyright (c) dSPACE GmbH. All rights reserved.
// </copyright>
// <author>Marius Bürck, Jonas Schrader</author>
// --------------------------------------------------------------------------------------------------------------------

namespace UserClient.Helper
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// A class that will connect an UI event to a ViewModel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// DependencyProperty to DataBind the Event property.
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));
        
        /// <summary>
        /// DependencyProperty to DataBind the Command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));
     
        /// <summary>
        /// DependencyProperty to DataBind the PassArguments property.
        /// </summary>
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));

        /// <summary>
        /// The method to invoke when the event is raised.
        /// </summary>
        private Delegate handler;

        /// <summary>
        /// The last attached Event.
        /// Stored to detach in case the Event property changes.
        /// </summary>
        private EventInfo oldEvent;

        /// <summary>
        /// Gets or sets the Event.
        /// </summary>
        public string Event
        {
            get
            {
                return (string)GetValue(EventProperty);
            }

            set
            {
                this.SetValue(EventProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the EventArgs should be passed.
        /// </summary>
        public bool PassArguments
        {
            get
            {
                return (bool)GetValue(PassArgumentsProperty);
            }

            set
            {
                this.SetValue(PassArgumentsProperty, value);
            }
        }

        /// <summary>
        /// Attaches the Event to the handler.
        /// </summary>
        protected override void OnAttached()
        {
            this.AttachHandler(this.Event);
        }

        /// <summary>
        /// Attaches a Event to a Command.
        /// </summary>
        /// <param name="d">The Behavior to attach to.</param>
        /// <param name="e">The Event.</param>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (EventToCommandBehavior)d;

            if (behavior.AssociatedObject != null)
            {
                behavior.AttachHandler((string)e.NewValue);
            }
        }
              
        /// <summary>
        /// Attaches the handler to the event.
        /// </summary>
        /// <param name="eventName">The name of the Event to attach to.</param>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (this.oldEvent != null)
            {
                this.oldEvent.RemoveEventHandler(this.AssociatedObject, this.handler);
            }

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = this.AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    this.handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(this.AssociatedObject, this.handler);
                    this.oldEvent = ei;
                }
                else
                {
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'", eventName, this.AssociatedObject.GetType().Name));
                }
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs.</param>
        private void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = this.PassArguments ? e : null;
            if (this.Command != null)
            {
                if (this.Command.CanExecute(parameter))
                {
                    this.Command.Execute(parameter);
                }
            }
        }
    }
}
