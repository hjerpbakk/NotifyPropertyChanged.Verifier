using System;
using System.Threading.Tasks;
using NotifyPropertyChanged.Verifier.Tests.ViewModels;
using Xunit;

namespace NotifyPropertyChanged.Verifier.AdditionalTests
{
    public sealed class AdditionalTests
    {
        [Fact]
        public void ShouldNotifyOn_ViewModelIsNull_Fails()
        {
            ViewModel nullVM = null;

            Assert.Throws<ArgumentNullException>(() =>
                nullVM.ShouldNotifyOn(vm => vm.PropertyWithNotify).
                    When(vm => vm.PropertyWithNotify = 42));
        }

        [Fact]
        public void ShouldNotNotifyOn_ViewModelIsNull_Fails()
        {
            ViewModel nullVM = null;

            Assert.Throws<ArgumentNullException>(() =>
                nullVM.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                    When(vm => vm.PropertyWithNotify = 42));
        }

        [Fact]
        public void ShouldNotNotifyOn_PropertiesNotSpecified_Fails()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ViewModel().ShouldNotNotifyOn(null).
                    When(vm => vm.PropertyWithoutNotify = 42));
        }

        [Fact]
        public void ShouldNotifyOn_PropertiesNotSpecified_Fails()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ViewModel().ShouldNotifyOn(null).
                    When(vm => vm.PropertyWithoutNotify = 42));
        }

        [Fact]
        public void When_ActionNotSpecified_Fails()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ViewModel().ShouldNotifyOn(vm => vm.PropertyWithoutNotify).
                    When((Func<ViewModel, Task>)null));
            Assert.Throws<ArgumentNullException>(() =>
                new ViewModel().ShouldNotifyOn(vm => vm.PropertyWithoutNotify).
                    When((Func<ViewModel, ValueTask>)null));
            Assert.Throws<ArgumentNullException>(() =>
                new ViewModel().ShouldNotifyOn(vm => vm.PropertyWithoutNotify).
                    When((Action<ViewModel>)null));
        }

        [Fact]
        public void Throws()
            => Assert.Throws<ArgumentException>(() => new ViewModel().ShouldNotifyOn(vm => new object()));
    }
}
