# Changelog

## OneStreamWebUI.Mvvm.Toolkit
## 1.0.0.1

**2022-03-29**
This document will track all changes to the OneStreamWebUI.Mvvm.Toolkt. The latest revisions will be posted on the OneStream Confluence site as well.

* [x] Features
 [#103] - Commands. Added RelayCommand and AsyncRelayCommand as used by the Microsoft.Mvvm.Toolkit using the ICommand Interface.(https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/introduction)
 
  [#104] - Added ViewModelCollectionBase Classes to be used as a base class for collections. This is optional and ObservableCollections can be used, however this classes exposes collection properties to UI such as Count, IndexOf, CurrectItem, etc.
 
 [#105] - Message Aggregator. This class and it's associated interfaces help facilitate ViewModel-to-ViewModel communication.


## Previous Releases

## 1.0.0.0

**2022-03-02**
 [#100] - Component Base Classes.Components need to inherit the base class OneStreamWebUI.Mvvm.Toolkit.ComponentViewBase.
 
 If you want full support use the generic 
 OneStreamWebUI.Mvvm.Toolkit.ComponentViewBase< T > view model type as a generic argument. Your view model will get auto injected into the component and set as a binding context.

 [#101] - ViewModelBase Classes.View models need to inherit the base class OneStreamWebUI.Mvvm.Toolkit.ViewModelBase.
 
 [#102] - Binding Functionality. Bindings are achieved via the Bind method in the component. If the value of the bound property has changed the component will be told to rerender automatically.
