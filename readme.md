# Usage

```csharp
var generator = new IdGenerator(1);
var id = generator.NewId();
```

# 依赖注入
```csharp
 services.AddSnowflakeGenerator(2);


 var generator = serviceProvider.GetService<ISnowflakeGenerator<AnyType>>();
 var id = generator.NewId();
```
```csharp
 services.AddSnowflakeGenerator(2);


 var factory = serviceProvider.GetService<ISnowflakeGeneratorFactory>();
 var generator = factory.Create("g1");
 var id = generator.NewId();
```