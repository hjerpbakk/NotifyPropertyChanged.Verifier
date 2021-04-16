using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace NotifyPropertyChanged.Verifier
{
    /// <summary>
    /// Contains extension methods on <see cref="INotifyPropertyChanged"/> to
    /// set one or more expectations of <see cref="INotifyPropertyChanged"/> events
    /// to be fired or not fired.
    /// </summary>
    public static class NotifyPropertyChanged
    {
        /// <summary>
        /// Creates an excpectation that the view model should raise <see cref="INotifyPropertyChanged"/>
        /// events for the given properties. At least on property must be specified.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model being tested.</typeparam>
        /// <param name="owner">The instance of the view model being tested.</param>
        /// <param name="property">The first property to raise an <see cref="INotifyPropertyChanged"/> event.</param>
        /// <param name="additionalProperties">Optional additional properties to raise <see cref="INotifyPropertyChanged"/> events.</param>
        /// <returns>An expectation of the <see cref="INotifyPropertyChanged"/> events to be raised.</returns>
        public static NotifyExpectation<TViewModel> ShouldNotifyOn<TViewModel>(this TViewModel owner,
            Expression<Func<TViewModel, object>> property,
            params Expression<Func<TViewModel, object>>[] additionalProperties)
            where TViewModel : class, INotifyPropertyChanged
            => CreateExpectation(owner, property, additionalProperties, true);

        /// <summary>
        /// Creates an expectation that the view model should not raise <see cref="INotifyPropertyChanged"/>
        /// events for the given properties. At least on property must be specified.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model being tested.</typeparam>
        /// <param name="owner">The instance of the view model being tested.</param>
        /// <param name="property">The first property to not raise an <see cref="INotifyPropertyChanged"/> event.</param>
        /// <param name="additionalProperties">Optional additional properties to not raise <see cref="INotifyPropertyChanged"/> events.</param>
        /// <returns>An expectation of the <see cref="INotifyPropertyChanged"/> events that should not be raised.</returns>
        public static NotifyExpectation<TViewModel> ShouldNotNotifyOn<TViewModel>(this TViewModel owner,
            Expression<Func<TViewModel, object>> property,
            params Expression<Func<TViewModel, object>>[] additionalProperties)
            where TViewModel : class, INotifyPropertyChanged
            => CreateExpectation(owner, property, additionalProperties, false);

        private static NotifyExpectation<TViewModel> CreateExpectation<TViewModel>(TViewModel owner,
            Expression<Func<TViewModel, object>> property,
            Expression<Func<TViewModel, object>>[] additionalProperties,
            bool eventExpected)
            where TViewModel : class, INotifyPropertyChanged
        {
            var allProperties = new Expression<Func<TViewModel, object>>[additionalProperties.Length + 1];
            Array.Copy(additionalProperties, 0, allProperties, 1, additionalProperties.Length);
            allProperties[0] = property ?? throw new ArgumentNullException(nameof(property));
            var propertyNames = allProperties.Select(propertyPicker => GetPropertyName(propertyPicker.Body));
            return new NotifyExpectation<TViewModel>(owner ?? throw new ArgumentNullException(nameof(owner)), propertyNames, eventExpected);
        }

        private static string GetPropertyName(Expression expression)
            => expression switch
            {
                UnaryExpression u => ((MemberExpression)u.Operand).Member.Name,
                MemberExpression m => m.Member.Name,
                _ => throw new ArgumentException($"{expression.GetType().FullName} is an unsupported property expression.", nameof(expression))
            };
    }
}
