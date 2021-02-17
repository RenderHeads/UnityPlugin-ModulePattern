
<p align="center">
  <img src="https://i1.wp.com/siliconcape.com/wp-content/uploads/2018/10/118-logo.png">
</p>
<p align="center">
  <b>©RenderHeads 2021</b>
</p>
<p align ="center"> Author / Maintainer: Ross Borchers </p>


# Module pattern for Unity

**Have you experienced one or more of the following symptoms?**

- _**Uncertainty**_ surrounding which components depend on one another?
- _**Disbelief**_ staring at a mess of duplicated game objects after project hand-over?
- _**Dread**_ while searching for a component in the hierarchy editor window?
- _**Trepidation**_ related to moving a component to a different GameObject?
- _**Concern**_ over linking components together in the hierarchy and whether they will randomly become unlinked?
- _**Anxiety**_ while debugging, caused by forgetting to assign a reference?
- _**Unease**_ building the transform hierarchy for systems that have no reason to be represented spatially?
- _**Confusion**_ when disabled GameObjects erroneously disable an important component?

![It's going to be ok.](https://media.giphy.com/media/3ohs81rDuEz9ioJzAA/giphy-downsized.gif)

## Fear not - **we have the solution for you!**

The module pattern is a framework for **building gameplay systems decoupled from the transform hierarchy.**
The framework provides a very light set of tools to create acyclical, decoupled caller hierarchies in code, and **allows you to access them from MonoBehaviour-land.**

[We mean module in a classical sense!](https://en.wikipedia.org/wiki/Modular_programming) A decoupled, cohesive and replaceable piece of functionality, hidden behind an interface. In this case, modules are plain C# classes that inherit from an interface and have factories to access them program-wide. But you could put anything behind that interface.

## Features
- Build your module hierarchy in code.
- Access modules anywhere, globally and on a per scene basis, without overusing singletons.
- Create modules as plain C# classes which open up the possibility for new design patterns:
   - Adhere to [RAII](https://en.wikipedia.org/wiki/Resource_acquisition_is_initialization) principles.
   - Use [Inversion of Control](https://en.wikipedia.org/wiki/Inversion_of_control) and DI.
   - Manual control of module update order.

### The Module Pattern is not

- It is **not** a replacement or in competition with DOTS, Jobs, or Burst. You can use them together.
- It is **not** a replacement or in competition with MonoBehaviours, MonoBehaviours are still useful for view related logic.

# Usage
There are only three useful files in this repository:
- [IModule](https://github.com/RenderHeads/UnityPlugin-ModulePattern/blob/master/Packages/RH.ModulePattern/Runtime/RuntimeIModule.cs) 
    - Interface for any module you want to create (A C# class that extends IModule), IModuleFactory requires modules inherit from this.
- [IModuleFactory](https://github.com/RenderHeads/UnityPlugin-ModulePattern/blob/master/Packages/RH.ModulePattern/Runtime/IModuleFactory.cs)
    - Interface responsible for storing modules. This is used to get Modules in your game, whether it be in MonoBehaviours, Systems, etc.
- [DefaultModuleFactory](https://github.com/RenderHeads/UnityPlugin-ModulePattern/blob/master/Packages/RH.ModulePattern/Runtime/DefaultModuleFactory.cs) is an implementation of [IModuleFactory](https://github.com/RenderHeads/UnityPlugin-ModulePattern/blob/master/Packages/RH.ModulePattern/Runtime/IModuleFactory.cs).
    - It provides a standard way to get modules across your game and should be appropriate for 90% of use cases.

Note that we use interfaces wherever possible as a matter of principle to maintain decoupling. This abstraction is enforced in DefaultModuleFactory, but it is not necessarily required if you define your own module factory. It's often faster to develop without abstraction.

### Example
You can use a MonoBehaviour to bootstrap the module system, but there are other ways to bootstrap the module system. You just need a way to create and update the module factory and modules. Using a GameManager/LevelManager's Awake/Start and Update functions suffice most of the time.
 
- Making a module factory for each scene you want to have a dedicated group of modules:
```
//We specify the scene this factory is created in so we can search for modules by scene.
IModuleFactory moduleFactory = new DefaultModuleFactory(gameObject.scene);
```

- Initializing modules and dependencies, adding them to the module factory:
```
IMyModule1 module1 = moduleFactory.AddModule(new MyModule1());
IMyModule2 module2 = moduleFactory.AddModule(new MyModule2(module1)); //Module2 depends on module 1.
```

- After initialization, accessing modules from a MonoBehaviour:
```
//Get factory by scene
if(DefaultModuleFactory.TryFindFactoryInScene(this.gameObject.scene, out IModuleFactory factory))
{
   //Get module from factory
   if(factory.TryGetModule(out IModule1 module1))
   {
      module1.FooBar();
   }
}

//Get the first module of type in all factories across all scenes.
if(DefaultModuleFactory.TryFindFirstInAll(out IModule1 module1))
{
   module1.FooBar();
}

//Get all modules of type in all factories across all scenes.
List<IModule1> modules = DefaultModuleFactory.FindInAll<IModule1>();

//Get all module factories for all scenes
List<IModuleFactory> factories = DefaultModuleFactory.GetAll();

```

# Rationale

Inter-class communication traditionally is achieved by a [singleton](https://en.wikipedia.org/wiki/Singleton_pattern), [FindObjectOfType](https://docs.unity3d.com/ScriptReference/Object.FindObjectOfType.html) or [GetComponent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html), [GetComponentInChildren](https://docs.unity3d.com/ScriptReference/Component.GetComponentsInChildren.html), [GetComponentInParent](https://docs.unity3d.com/ScriptReference/GameObject.GetComponentInParent.html) and related functions. Sometimes you may assign a reference in an inspector field.

All of these assume some relationship between the transform hierarchy and the code structure. This is not always the case. Systems can contain logic but do not handle rendering and have no reason to be spatial - they are not view components! The relationship does not need to be serialized in the scene or prefab. There may be little relationship between transform hierarchy order and the caller hierarchy. It's convoluted.

Script update is typically controlled by MonoBehaviour Start/Update/FixedUpdate/LateUpdate callbacks. This requires that scripts inherit from MonoBehaviour, which couples script update to the transform hierarchy. There is no reason this needs to be the case. Furthermore, MonoBehaviour update order is notoriously hard to control and understand and can lead to one-frame-late type bugs. [Not to mention the performance implications!](https://blogs.unity3d.com/2015/12/23/1k-update-calls/)

Based on these observations we chose to separate the caller hierarchy for non-view systems from the scene tree hierarchy. In our projects, it has helped us decouple, standardize and re-use code and helped developers jump between projects faster than they otherwise would have been able to.

# Usage and Contribution
## Usage License
The project is licensed under a GPL-3.0 license, which means you can use it for commerical and non commercial use, but the project you use it in also needs to apply the GPL-3.0 license and be open-sourced. If this license is not suitable for you, please contact us at southafrica@renderheads.com and we can discuss an appropriate commercial license.

## Contributors License
We are currently working on a Contributors License Agreement, which we will put up here when it's ready. In the meantime, if you would like to contribute, please reach out to us.
