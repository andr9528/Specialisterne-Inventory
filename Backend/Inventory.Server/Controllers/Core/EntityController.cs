using Inventory.Abstraction.Interfaces.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Server.Controllers.Core;

// Todo: Add Authorization / Authentication

// Add the two lines below to any controller.
//[Route(Constants.ROUTE_TEMPLATE)]
//[ApiController]
public abstract class EntityController<TEntity, TSearchable, TController, TComplex> : ControllerBase
    where TEntity : class, IEntity
    where TSearchable : class, ISearchable, new()
    where TController : ControllerBase
    where TComplex : IComplexSearchable<TSearchable>
{
    protected readonly IEntityQueryService<TEntity, TSearchable> entityService;
    protected readonly ILogger<TController> logger;

    protected EntityController(IEntityQueryService<TEntity, TSearchable> entityService, ILogger<TController> logger)
    {
        this.entityService = entityService;
        this.logger = logger;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        try
        {
            var entities = await entityService.GetAllEntities();
            return Ok(entities);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception was caught while attempting to get all entities of the controllers type.");
            throw;
        }
    }

    [HttpGet("id")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        try
        {
            IEntity entity = await entityService.GetEntity(new TSearchable { Id = id });
            return Ok(entity);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to get an entity by id of the controllers type. Id: {Id}",
                id);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetByQuery([FromBody] TSearchable searchable)
    {
        try
        {
            IEntity entity = await entityService.GetEntity(searchable);
            return Ok(entity);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to get entity matching specified query of the controllers type. Query: {@Searchable}",
                searchable);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetByComplexQuery([FromBody] TComplex complex)
    {
        try
        {
            IEntity entity = await entityService.GetEntityComplex(complex);
            return Ok(entity);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to get entity matching specified query of the controllers type. Query: {@Searchable}",
                complex);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetAllByQuery([FromBody] TSearchable searchable)
    {
        try
        {
            var entities = await entityService.GetEntities(searchable);
            return Ok(entities);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to get entities matching specified query of the controllers type. Query: {@Searchable}",
                searchable);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetAllByComplexQuery([FromBody] TComplex complex)
    {
        try
        {
            var entities = await entityService.GetEntitiesComplex(complex);
            return Ok(entities);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to get entities matching specified query of the controllers type. Query: {@Searchable}",
                complex);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> AddSingle([FromBody] TEntity entity)
    {
        try
        {
            await entityService.AddEntity(entity);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to add a single entity of the controllers type. Entity: {@Entity}",
                entity);
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> AddMultiple([FromBody] IEnumerable<TEntity> entities)
    {
        try
        {
            await entityService.AddEntities(entities);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to add multiple entities of the controllers type. Entities: {@Entities}",
                entities);
            throw;
        }
    }

    [HttpPut]
    public virtual async Task<IActionResult> UpdateSingle([FromBody] TEntity entity)
    {
        try
        {
            await entityService.UpdateEntity(entity);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to update a specific entity of the controllers type. Entity: {@Entity}",
                entity);
            throw;
        }
    }

    [HttpPut]
    public virtual async Task<IActionResult> UpdateMultiple([FromBody] IEnumerable<TEntity> entities)
    {
        try
        {
            await entityService.UpdateEntities(entities);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to update multiple entities of the controllers type. Entities: {@Entities}",
                entities);
            throw;
        }
    }

    [HttpDelete]
    public virtual async Task<IActionResult> DeleteByQuery([FromBody] TSearchable searchable)
    {
        try
        {
            await entityService.DeleteEntity(searchable);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to delete a specific entity by specified query of the controllers type. Query: {@Searchable}",
                searchable);
            throw;
        }
    }

    [HttpDelete("id")]
    public virtual async Task<IActionResult> DeleteById(int id)
    {
        try
        {
            await entityService.DeleteEntityById(id);
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "An exception was caught while attempting to delete an entity by id of the controllers type. Id: {Id}",
                id);
            throw;
        }
    }

    protected IActionResult MethodNotSupported(string methodName)
    {
        return StatusCode(
            StatusCodes.Status405MethodNotAllowed,
            $"{GetType().Name} does not support endpoint '{methodName}'.");
    }
}
