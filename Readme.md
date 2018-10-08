# TypeMapper 

TypeMapper is lightweight and definition oriented property mapper for dotnet projects. The TypeMapper don't force you to map all properties both of two types!

## Usage

First, you define your type maps; which types will be mapped with other types.

#### Default mapping:
```csharp

MapperBuilder
	.DefineMapFor<TTargetType, TSourceType>()
	.Build();

```

#### Definitions
```csharp

new MapperBuilder()
	.DefineMapFor<TTargetType, TSourceType>(specifications =>
	{
		specifications
			.For(target => target.Prop1)
			.Map(source => source.PropOne);

		specifications
			.For(target => target.Prop2)
			.Map(source => { return string.Format("", source.PropA, source.PropB)})

		specifications
			.For(target => target.Prop3)
			.Map(source => source * 0.18)

		specifications
			.For(target => target.Prop3)
			.Map(()=> ...)		

	})
	.Build();

```

After that, the mapper ignores any mapping actions for all undefined properties. This rule doesn't accept for the same named properties. because by the default same named properties will be mapped automatically.

## What is the background?

The Build method of MapperBuilder class return a instance from implemented as IMapper.

All your definitions processed by MapperBuilder and create a type definitions table. The definitions table includes which property of the target type will be mapped with other property of the source type and which action will be invoked for the mapping process.