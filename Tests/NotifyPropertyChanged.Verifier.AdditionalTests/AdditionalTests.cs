using System;
using NotifyPropertyChanged.Verifier.Tests.ViewModels;
using Xunit;

namespace NotifyPropertyChanged.Verifier.AdditionalTests
{
    public class AdditionalTests
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
                    When(null));
        }
    }
}
