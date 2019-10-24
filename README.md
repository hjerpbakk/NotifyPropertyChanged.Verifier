# NotifyPropertyChanged.Verifier

Fluent extension of xUnit for testing implementations of INotifyPropertyChanged in ViewModels.

[![Build status](https://github.com/sankra/NotifyPropertyChanged.Verifier/workflows/CI/badge.svg)](https://github.com/Sankra/NotifyPropertyChanged.Verifier/actions) [![codecov](https://codecov.io/gh/Sankra/NotifyPropertyChanged.Verifier/branch/master/graph/badge.svg)](https://codecov.io/gh/Sankra/NotifyPropertyChanged.Verifier) [![codecov](https://img.shields.io/nuget/v/NotifyPropertyChanged.Verifier.svg)](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier) [![codecov](https://img.shields.io/nuget/dt/NotifyPropertyChanged.Verifier.svg)](https://www.nuget.org/packages/NotifyPropertyChanged.Verifier)

```csharp
var vm = new ViewModel();

vm.ShouldNotifyOn(vm => vm.PropertyWithNotify)
  .When(vm => vm.PropertyWithNotify = 42);

vm.ShouldNotNotifyOn(vm => vm.PropertyWithoutNotify)
  .When(vm => vm.PropertyWithoutNotify = -1);

vm.ShouldNotifyOn(vm => vm.PropertyWithNotify, vm => vm.PropertyWithMultipleNotifies)
  .When(vm => vm.PropertyWithMultipleNotifies = "42");
```
