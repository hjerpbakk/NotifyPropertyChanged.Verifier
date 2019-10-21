using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace NotifyPropertyChanged.Verifier
{
    /// <summary>
    /// Contains expectations of <see cref="INotifyPropertyChanged"/> events
    /// to be fired or not fired by an instance a view model.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model implementing <see cref="INotifyPropertyChanged"/>.</typeparam>
    public sealed class NotifyExpectation<TViewModel> where TViewModel : notnull, INotifyPropertyChanged
    {
        private readonly TViewModel owner;
        private readonly string[] propertyNames;
        private readonly bool eventExpected;
        private readonly bool[] wasNotified;

        internal NotifyExpectation(TViewModel owner, IEnumerable<string> propertyNames, bool eventExpected)
        {
            this.owner = owner;
            this.propertyNames = propertyNames.ToArray();
            this.eventExpected = eventExpected;
            wasNotified = new bool[this.propertyNames.Length];
        }

        /// <summary>
        /// Runs the supplied <see cref="Action"/> and verifies that the specifed
        /// <see cref="INotifyPropertyChanged"/> events are either raised or not raised
        /// based on the earlier specified expectations.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> which triggers or not triggers the expected <see cref="INotifyPropertyChanged"/> events.</param>
        public void When(Action<TViewModel> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            
            owner.PropertyChanged += StoreNotifiedProperties;
            action(owner);
            owner.PropertyChanged -= StoreNotifiedProperties;
            
            for (int i = 0; i < wasNotified.Length; i++)
            {
                if (eventExpected)
                {
                    Assert.True(wasNotified[i], $"{propertyNames[i]} was not notified.");
                }
                else
                {
                    Assert.False(wasNotified[i], $"{propertyNames[i]} was notitied.");
                }
            }
        }

        private void StoreNotifiedProperties(object sender, PropertyChangedEventArgs e)
        {
            var index = Array.IndexOf(propertyNames, e.PropertyName);
            if (index == -1)
            {
                return;
            }

            wasNotified[index] = true;
        }
    }
}
