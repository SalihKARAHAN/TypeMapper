# Map Definition

## MapperBuilder.cs

```csharp
MapperBuilder

   # MapDefinitions: List<MapDefinition>

   + MapperBuilder(): MapperBuilder;
   
   + DefineMapFor<TTargetType, TSourceType>(): MapperBuilder
   
   + DefineMapFor<TTargetType, TSourceType>(Action<MapSpecificationsDefinition>): MapperBuilder
   
   # DefineMap(targetType: Type, sourceType: Type, delegate 
   
   + Build(): IMapper
```

MapperBuilder create a IMapper instance with below code:
```csharp 
IMapper mapper = new MapperBuilder()
```
This class has two methods; it`s names are ``DefineMapFor`` and ``Build``. 

The `DefineMapFor` provides you can define type couple which will mapping with between 
two defined types. This method has two different usage. First overload, that has not 
arguments, maps your types as name convertion. Same named properties matches on this way. 

```charp
new MapperBuilder()
    .DefineMapFor<TTargetType, TSourceType>()
```

Second overload, that take `Action<MapSpecification>` as argument, provides you define 
which property of first type will be mapped with other property of second type. You can do this like a below code: 

```charp
MapperBuilder mapperBuilder = new MapperBuilder()
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
            .Map(source => source. * 0.18)

        specifications
            .For(target => target.Prop3)
            .Map(()=> ...)		

    });
```

The abowe code describe 


## MapDefinition.cs

```csharp
MapDefinition

    #TargetType: Type
    #SourceType: Type
    #Specifications: List<MapSpecifications>
    
```

## MapSpecificationsDefinition.cs

```csharp
MapSpecificationsDefinition<TTargetType, TSourceType>

    # Specifications: List<MapSpecification>

    + For<TTargetPropertyType>(Func<TTargetType, TTargetPropertyType>): MapSpecificationDefinition

```

## MapSpecification.cs

```csharp
MapSpecification

    # TargetPropertyInfo: PropertyInfo
    # SourcePropertyInfo: PropertyInfo 
    # GetSourcePropertyValue<T: Func<

```


## MapSpecificationDefinition.cs

```csharp
MapSpecificationDefinition<TTargetType, TSourceType>

    # MapSpecificationDefinition(MapSpecifications): MapSpecification

    + Map<TSourcePropertyType>(Func<TSourceType, TSourcePropertyType>): void

``` 

### BuildMapper.DefineMapFor<TTargetType, TSourceType>()

### BuildMapper.DefineMapFor<TTargetType, TSourceType>(Action<MapSpecification>)

## MapTable.cs

```csharp
MapTable

    - index: string[]
    - maps: Map[]
    
    # MapTable(Map[] maps): MapTable
    
    # CreateIndex<TTargetType, TSourceType>(): string
    
    # FindMap<TTargetType, TSourceType>(): Map
    
    - CreateMD5Hash(text: string): string
    
    - 

``` 

## Map.cs

```csharp
Map

    - 

``` 

## Mapper.cs

```csharp
Mapper

    - mapTable: MapTable

    + MapTo<TTargetType>(object source): TTargetType

``` 