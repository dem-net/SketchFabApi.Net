# SketchFabApi.Net
SketchFab exporter for any GLB model

See it live on https://elevationapi.com by exporting to SketchFab and sharing your models to the World!

## Operations

### Model

```csharp
// Upload model
Task<SketchFabUploadResponse> UploadModelAsync(UploadModelRequest request, string sketchFabToken)

// Get model information
Task<Model> GetModelAsync(string modelId)

// Get all models as an enumerable (fetches though pages)
IAsyncEnumerable<Model> GetMyModelsAsync(string sketchFabToken, TokenType tokenType)

// Get model ready state
Task<bool> IsReadyAsync(string modelId)
```

### Account

```csharp
// Retrieves account information
Task<Account> GetMyAccountAsync(string sketchFabToken, TokenType tokenType)
```

### Collections

```csharp
// Get user's collections
Task<List<Collection>> GetMyCollectionsAsync(string sketchFabToken, TokenType tokenType)

// Add model to collection
Task AddModelToCollectionAsync(string collectionId, string sketchFabToken, TokenType tokenType, params string[] modelIds)
```

**Work in progress, I'll let you know when it's ready**

- [x] Upload model
- [x] Update model
- [x] Add model to a collection
- [x] Get my models, (with generic paging)
