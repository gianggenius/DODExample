# SOLID Principles Refactoring Plan for DODExample

## Current Architecture Analysis

The DODExample project is a Unity-based Data-Oriented Design (DOD) implementation with ECS-style architecture. However, it contains several SOLID principle violations that limit its maintainability, testability, and extensibility.

## Current SOLID Violations Identified

### 1. Single Responsibility Principle (SRP) Violations

#### DatabaseManager.cs
- **Problem**: Acts as singleton manager, contains all table instances, and handles initialization
- **Current Responsibilities**: 
  - Singleton pattern management
  - Table instance storage
  - Table initialization
  - MonoBehaviour lifecycle management

#### CharacterManager.cs
- **Problem**: Handles multiple unrelated responsibilities
- **Current Responsibilities**:
  - Player GameObject instantiation
  - Database record creation
  - Combat event triggering
  - Transform positioning

#### BaseTable.cs
- **Problem**: Complex class with multiple responsibilities
- **Current Responsibilities**:
  - Data structure management
  - Reflection-based operations
  - Array resizing and manipulation
  - CRUD operations

### 2. Open/Closed Principle (OCP) Violations

#### SystemManager.cs
- **Problem**: Hard-coded system registration in `InitializeSystems()`
- **Impact**: Requires code modification to add new systems

#### DatabaseManager.cs
- **Problem**: Hard-coded table initialization
- **Impact**: Requires code modification to add new table types

### 3. Liskov Substitution Principle (LSP) Violations

#### BaseSystem.cs
- **Problem**: All derived systems forced to depend on DatabaseManager
- **Impact**: Limits system substitutability and testing

### 4. Interface Segregation Principle (ISP) Violations

#### Empty Interfaces
- **Problem**: `ITableData` and `ITableRecordData` provide no meaningful contracts
- **Impact**: No behavioral guarantees for implementers

#### BaseTable.cs
- **Problem**: Exposes all methods to all clients
- **Impact**: Clients forced to depend on methods they don't use

### 5. Dependency Inversion Principle (DIP) Violations

#### System Dependencies
- **Problem**: Systems directly depend on concrete DatabaseManager singleton
- **Impact**: Tight coupling, difficult testing, inflexible architecture

## Proposed Refactoring Plan

### Phase 1: Interface Segregation and Abstraction (ISP & DIP)

#### 1.1 Create Meaningful Interfaces

```csharp
// Core interfaces for data access
public interface IReadOnlyTable<TRecord> where TRecord : ITableRecordData
{
    int Length { get; }
    TRecord GetRecordByIndex(int index);
    TRecord GetRecordByKey(string key);
}

public interface IWritableTable<TRecord> : IReadOnlyTable<TRecord> where TRecord : ITableRecordData
{
    void AddRecord(TRecord record);
    bool RemoveRecord(int index);
    bool UpdateRecordByIndex(TRecord record, int index);
}

// Query interfaces for specific table operations
public interface IPlayerHealthQueries
{
    int GetHealthByPlayerID(string playerID);
    bool GetDeadStatusByPlayerID(string playerID);
    void SetHealthByPlayerID(string playerID, int hp);
}

public interface IPlayerAttackQueries
{
    int GetDamageByPlayerID(string playerID);
}

// System interfaces
public interface ISystem
{
    void Update();
}

public interface IInitializableSystem : ISystem
{
    void Initialize();
}

// Data management interfaces
public interface IDataRepository
{
    IPlayerHealthQueries PlayerHealth { get; }
    IPlayerAttackQueries PlayerAttack { get; }
    IWritableTable<PlayerCombatRecord> PlayerCombat { get; }
    IWritableTable<PlayerPositionRecord> PlayerPosition { get; }
}
```

#### 1.2 Create Service Locator/DI Container

```csharp
public interface IServiceContainer
{
    void Register<TInterface, TImplementation>() where TImplementation : class, TInterface;
    void RegisterSingleton<TInterface, TImplementation>() where TImplementation : class, TInterface;
    TInterface Resolve<TInterface>();
}

public class UnityServiceContainer : MonoBehaviour, IServiceContainer
{
    // Implementation for Unity-specific DI container
}
```

### Phase 2: Single Responsibility Refactoring (SRP)

#### 2.1 Break Down DatabaseManager

```csharp
// Separate concerns into focused classes
public class TableRegistry : IDataRepository
{
    // Only manages table instances and provides access
}

public class DataInitializer
{
    // Only handles data initialization
    public void InitializeTables(IDataRepository repository);
}

public class DatabaseLifecycleManager : MonoBehaviour
{
    // Only handles Unity lifecycle and coordinates initialization
}
```

#### 2.2 Refactor CharacterManager

```csharp
public interface IPlayerFactory
{
    PlayerEntity CreatePlayer(Vector3 position, string uniqueId);
}

public interface IPlayerDataInitializer
{
    void InitializePlayerData(string playerId, Vector3 position);
}

public interface ICombatEventGenerator
{
    void GenerateCombatEvents(IReadOnlyList<PlayerEntity> players);
}

public class CharacterManager : MonoBehaviour
{
    // Only coordinates between services, doesn't implement business logic
    private readonly IPlayerFactory _playerFactory;
    private readonly IPlayerDataInitializer _dataInitializer;
    private readonly ICombatEventGenerator _combatGenerator;
}
```

#### 2.3 Refactor BaseTable

```csharp
// Separate data operations from reflection logic
public interface IDataArrayOperations<T>
{
    void AddToArray(ref T[] array, T item);
    bool RemoveFromArray(ref T[] array, int index);
    void UpdateInArray(ref T[] array, int index, T item);
}

public class ReflectionDataMapper<TStruct, TRecord>
{
    // Only handles reflection-based mapping
}

public abstract class BaseTable<TStruct, TRecord> : IWritableTable<TRecord>
    where TStruct : ITableData
    where TRecord : ITableRecordData
{
    // Focused only on table-level operations
    private readonly IDataArrayOperations<TRecord> _arrayOperations;
    private readonly IDataMapper<TStruct, TRecord> _dataMapper;
}
```

### Phase 3: Open/Closed Principle Implementation (OCP)

#### 3.1 System Registration Strategy

```csharp
public interface ISystemRegistry
{
    void RegisterSystem<T>() where T : ISystem;
    void RegisterSystem(Type systemType);
    IEnumerable<ISystem> GetSystems();
}

public class SystemManager : MonoBehaviour
{
    [SerializeField] private SystemConfiguration _systemConfig;
    
    private void InitializeSystems()
    {
        // Load system types from configuration rather than hard-coding
        foreach (var systemType in _systemConfig.SystemTypes)
        {
            _systemRegistry.RegisterSystem(systemType);
        }
    }
}

[CreateAssetMenu(fileName = "SystemConfiguration", menuName = "DOD/System Configuration")]
public class SystemConfiguration : ScriptableObject
{
    [SerializeField] private List<string> _systemTypeNames;
    public IEnumerable<Type> SystemTypes => _systemTypeNames.Select(Type.GetType);
}
```

#### 3.2 Table Registration Strategy

```csharp
public interface ITableFactory
{
    T CreateTable<T>() where T : class;
    object CreateTable(Type tableType);
}

public class ConfigurableTableRegistry : IDataRepository
{
    private readonly ITableFactory _tableFactory;
    private readonly TableConfiguration _config;
    
    public void InitializeFromConfiguration()
    {
        // Create tables based on configuration rather than hard-coding
    }
}
```

### Phase 4: Liskov Substitution Compliance (LSP)

#### 4.1 System Base Class Refactoring

```csharp
public abstract class BaseSystem : ISystem
{
    protected readonly IDataRepository _dataRepository;
    
    protected BaseSystem(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
    }
    
    public abstract void Update();
}

// Specialized base classes for different system types
public abstract class DataProcessingSystem : BaseSystem
{
    protected DataProcessingSystem(IDataRepository dataRepository) : base(dataRepository) { }
    
    public sealed override void Update()
    {
        if (ShouldProcess())
        {
            Process();
        }
    }
    
    protected abstract bool ShouldProcess();
    protected abstract void Process();
}
```

### Phase 5: Dependency Inversion Implementation (DIP)

#### 5.1 Remove Singleton Dependencies

```csharp
public class PlayerCombatSystem : DataProcessingSystem
{
    public PlayerCombatSystem(IDataRepository dataRepository) 
        : base(dataRepository) { }
    
    protected override bool ShouldProcess()
    {
        return _dataRepository.PlayerCombat.Length > 0;
    }
    
    protected override void Process()
    {
        // Use injected repository instead of singleton
        var combatTable = _dataRepository.PlayerCombat;
        var healthQueries = _dataRepository.PlayerHealth;
        var attackQueries = _dataRepository.PlayerAttack;
        
        // Process combat logic
    }
}
```

#### 5.2 Entity Refactoring

```csharp
public interface IEntityDataSynchronizer
{
    void SynchronizeEntity(IEntity entity);
}

public class PlayerEntity : MonoBehaviour, IEntity
{
    private IEntityDataSynchronizer _synchronizer;
    
    public void Initialize(IEntityDataSynchronizer synchronizer)
    {
        _synchronizer = synchronizer ?? throw new ArgumentNullException(nameof(synchronizer));
    }
    
    private void Update()
    {
        _synchronizer?.SynchronizeEntity(this);
    }
}
```

## Implementation Timeline

### Week 1: Foundation
- [ ] Create core interfaces (ISystem, IDataRepository, service interfaces)
- [ ] Implement basic service container
- [ ] Create configuration classes for systems and tables

### Week 2: Data Layer Refactoring
- [ ] Refactor BaseTable to separate concerns
- [ ] Implement specialized table interfaces
- [ ] Create TableRegistry and DataInitializer classes

### Week 3: System Layer Refactoring
- [ ] Refactor BaseSystem to remove singleton dependency
- [ ] Implement dependency injection for systems
- [ ] Update SystemManager to use configuration-based registration

### Week 4: Manager Refactoring
- [ ] Break down CharacterManager into focused services
- [ ] Implement factory patterns for entity creation
- [ ] Create data initialization services

### Week 5: Integration and Testing
- [ ] Wire up all components with dependency injection
- [ ] Create unit tests for refactored components
- [ ] Performance testing to ensure no DOD performance regression

### Week 6: Documentation and Cleanup
- [ ] Update documentation
- [ ] Remove obsolete code
- [ ] Code review and final adjustments

## Benefits After Refactoring

### Maintainability
- Each class has a single, clear responsibility
- Changes to one aspect don't affect unrelated code
- Easier to understand and modify individual components

### Testability
- Systems can be unit tested with mock dependencies
- Data operations can be tested independently
- No reliance on Unity singletons in business logic

### Extensibility
- New systems can be added through configuration
- New table types can be registered without code changes
- New data operations can be implemented without touching existing code

### Performance
- Maintains DOD performance characteristics
- Reduces unnecessary coupling and dependencies
- Enables better optimization opportunities

## Migration Strategy

1. **Parallel Implementation**: Implement new architecture alongside existing code
2. **Gradual Migration**: Move systems one by one to new architecture
3. **Feature Flags**: Use Unity scripting defines to switch between implementations
4. **Comprehensive Testing**: Ensure functional parity throughout migration
5. **Performance Monitoring**: Continuously monitor performance during migration

This refactoring plan transforms the codebase into a maintainable, testable, and extensible architecture while preserving the performance benefits of the Data-Oriented Design approach.