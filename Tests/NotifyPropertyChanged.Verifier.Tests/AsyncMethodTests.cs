using System.Threading.Tasks;
using NotifyPropertyChanged.Verifier.Tests.ViewModels;
using Xunit;
using Xunit.Sdk;

namespace NotifyPropertyChanged.Verifier.Tests
{
    public sealed class AsyncMethodTests
    {
        readonly ViewModel vm;

        public AsyncMethodTests() => vm = new ViewModel();

        [Fact]
        public void ShouldNotifyOn_PropertyDoesNotNotify_Fails()
        {
            Assert.Throws<TrueException>(() =>
                vm.ShouldNotifyOn(vm => vm.PropertyWithoutNotify).
                   When(vm => vm.UpdatePropertyUsingTask(42)));

            Assert.Throws<TrueException>(() =>
                vm.ShouldNotifyOn(vm => vm.PropertyWithoutNotify).
                   When(vm => vm.UpdatePropertyUsingValueTask(42)));
        }

        [Fact]
        public void ShouldNotifyOn_PropertyNotifies_Succeeds()
        {
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify).
               When(vm => vm.UpdatePropertyUsingTask(42));

            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify).
               When(vm => vm.UpdatePropertyUsingValueTask(42));
        }

        [Fact]
        public void ShouldNotNotifyOn_PropertyNotifies_Fails()
        {
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingTask(42)));

            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingValueTask(42)));
        }

        [Fact]
        public void ShouldNotNotifyOn_PropertyDoesNotNotify_Succeeds()
        {
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingTask(42));

            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingValueTask(42));
        }

        [Fact]
        public void ShouldNotNotifyOn_PropertyWithMultipleNotifies_Fails()
        {
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));

            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42"))));
        }

        [Fact]
        public void ShouldNotifyOn_PropertyWithMultipleNotifies_Succeeds()
        {
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42"));

            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));
        }

        [Fact]
        public void ShouldNotifyOn_PropertyWithSuperfluousNotifies_Succeeds()
        {
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => Task.Run(() => vm.PropertyWithSuperfluousNotifies = "42"));

            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => new ValueTask(Task.Run(() => vm.PropertyWithSuperfluousNotifies = "42")));
        }

        [Fact]
        public void BothNotifyAndNotWithSameViewModel_SingleNotify_Works()
        {
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingTask(42)));
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingTask(42));

            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingValueTask(42)));
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingValueTask(42));
        }

        [Fact]
        public void BothNotifyAndNotWithSameViewModel_SingleNotify_Works2()
        {
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingTask(42));
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingTask(42)));

            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify).
               When(vm => vm.UpdatePropertyUsingValueTask(42));
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify).
                   When(vm => vm.UpdatePropertyUsingValueTask(42)));
        }

        [Fact]
        public void BothNotifyAndNotWithSameViewModel_MultipleNotify_Works()
        {
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42"));

            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42"))));
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));
        }

        [Fact]
        public void BothNotifyAndNotWithSameViewModel_MultipleNotify_Works2()
        {
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42"));
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));

            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
               When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42")));
            Assert.Throws<FalseException>(() =>
                vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies).
                   When(vm => new ValueTask(Task.Run(() => vm.PropertyWithMultipleNotifies = "42"))));
        }
    }
}
