# NotifyPropertyChanged.Verifier

Introducing [NotifyPropertyChanged.Verifier](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier/), a fluent extension of xUnit for testing implementations of INotifyPropertyChanged in ViewModels.

[![Build status](https://github.com/sankra/NotifyPropertyChanged.Verifier/workflows/CI/badge.svg)](https://github.com/Sankra/NotifyPropertyChanged.Verifier/actions) [![codecov](https://codecov.io/gh/Sankra/NotifyPropertyChanged.Verifier/branch/master/graph/badge.svg)](https://codecov.io/gh/Sankra/NotifyPropertyChanged.Verifier) [![codecov](https://img.shields.io/nuget/v/NotifyPropertyChanged.Verifier.svg)](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier) [![codecov](https://img.shields.io/nuget/dt/NotifyPropertyChanged.Verifier.svg)](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier)

![mvvm](/doc/mvvm.svg)

## tl;dr

```csharp
vm.ShouldNotifyOn(vm => vm.PropertyWithNotify)
  .When(vm => vm.PropertyWithNotify = 42);

vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify)
  .When(vm => vm.PropertyWithoutNotify = -1);
```

## Usage

Consider the following ViewModel:

```csharp
public class ViewModel : INotifyPropertyChanged {
  int backingField;
  string backingField2;

  public int PropertyWithoutNotify { get; set; }

  public int PropertyWithNotify {
      get => backingField;
      set {
        backingField = value;
        OnPropertyChanged();
      }
  }

  public string PropertyWithMultipleNotifies {
      get => backingField2;
      set {
          PropertyWithNotify = int.Parse(value);
          OnPropertyChanged();
      }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

To test it, create an xUnit-test project and add a NuGet reference to [NotifyPropertyChanged.Verifier](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier/). It's a .Net Standard 2.0 library and can be used both in .Net Core and the full .NET Framework. The preceding ViewModel can test its implementation of INotifyPropertyChanged doing:

```csharp
using NotifyPropertyChanged.Verifier;
using Xunit;

namespace Tests {
    public class UnitTests {
        readonly ViewModel vm;

        public UnitTests() => vm = new ViewModel();

        [Fact]
        public void PropertyWithNotify_WillRaiseNotifyEvent() =>
            vm.ShouldNotifyOn(vm => vm.PropertyWithNotify)
              .When(vm => vm.PropertyWithNotify = 42);

        [Fact]
        public void PropertyWithoutNotify_WillNotRaiseNotifyEvent() =>
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify)
              .When(vm => vm.PropertyWithoutNotify = -1);

        [Fact]
        public void PropertyWithMultipleNotifies_WillRaiseMultipleNotifyEvents() =>
            vm.ShouldNotNotifyOn(vm => vm.PropertyWithNotify,
                                 vm => vm.PropertyWithMultipleNotifies)
              .When(vm => vm.PropertyWithMultipleNotifies = "42");
    }
}
```

The library consists of two extension methods on INotifyPropertyChanged, `ShouldNotifyOn` and `ShouldNotNotifyOn` which takes 1 or more property expressions as input. These are the properties that should either receive or not receive a NotifyPropertyChanged-event when an `Action<T>` is called by the `When` method. This can anything, not only methods or properties on the ViewModel itself.


Inspired by this [blogpost](https://blog.ploeh.dk/2009/08/06/AFluentInterfaceForTestingINotifyPropertyChanged/).
