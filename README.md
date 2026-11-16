# SketchFabApi.Net
SketchFab exporter for any GLB model

See it live on https://elevationapi.com by exporting to SketchFab and sharing your models to the World!

## Setup

```csharp
// Registers SketchfabApi and its HttpClient (10 min timeout for large uploads).
// The returned IHttpClientBuilder can add handlers, change the timeout, etc.
services.AddSketchfabApi();
```

Every method takes an optional `CancellationToken`. JSON is handled with System.Text.Json (source-generated); the library no longer changes the global `JsonConvert.DefaultSettings`.

## Operations

### Model

```csharp
// Upload model (the file is streamed from disk)
Task<SketchfabUploadResponse> UploadModelAsync(UploadModelRequest request, string sketchFabToken)

// Update model tags and categories
Task UpdateModelAsync(string modelId, UpdateModelRequest request, string sketchFabToken, TokenType tokenType)

// Get model information
Task<Model> GetModelAsync(string modelId, string sketchFabToken, TokenType tokenType)

// Get all models as an enumerable (fetches though pages)
IAsyncEnumerable<Model> GetMyModelsAsync(string sketchFabToken, TokenType tokenType)

// Get model ready state
Task<bool> IsReadyAsync(string modelId, string token)
```

### Account

```csharp
// Retrieves account information
Task<Account> GetMyAccountAsync(string sketchFabToken, TokenType tokenType)

// Like a model
Task PostLikeAsync(string modelId, string sketchFabToken, TokenType tokenType)
```

### Collections

```csharp
// Get user's collections (all pages)
Task<List<Collection>> GetMyCollectionsAsync(string sketchFabToken, TokenType tokenType)

// Add model to collection
Task AddModelToCollectionAsync(string collectionId, string sketchFabToken, TokenType tokenType, params string[] modelIds)
```

**Work in progress, I'll let you know when it's ready**

- [x] Upload model
- [x] Update model
- [x] Add model to a collection
- [x] Get my models, (with generic paging)
