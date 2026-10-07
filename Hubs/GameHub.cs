using Microsoft.AspNetCore.SignalR;
using CircuitYard.Server.Data;
using CircuitYard.Server.Models;
using CircuitYard.Server.Dtos;
using Microsoft.EntityFrameworkCore;
using CircuitYard.Server.Constants;

namespace CircuitYard.Server.Hubs;

public class GameHub : Hub
{
    private readonly AppDbContext _dbContext;

    public GameHub(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Place(int x, int y)
    {
        _dbContext.PlacedObjects.Add(new PlacedObject() { X = x, Y = y });

        await _dbContext.SaveChangesAsync();
    }

    //TODO: Use constants for chunk and nearby chunks to load(render distance) and world side
    //Refactor the whole hub.
    public async Task<List<PlacedObject>> LoadNearbyObjects(int chunkX, int chunkY)
    {
        var maxCellCoordinate = WorldConstants.WorldSize - 1;
        var renderDistance = WorldConstants.RenderDistance;
        var chunkSize = WorldConstants.ChunkSize;

        var minX = Math.Max(0, (chunkX - renderDistance) * chunkSize);
        var maxX = Math.Min(maxCellCoordinate, (chunkX + renderDistance + 1) * chunkSize) - 1;

        var minY = Math.Max(0, (chunkY - renderDistance) * chunkSize);
        var maxY = Math.Min(maxCellCoordinate, (chunkY + renderDistance + 1) * chunkSize) - 1;

        return await _dbContext.PlacedObjects
            .Where(p =>
                    p.X >= minX &&
                    p.X <= maxX &&
                    p.Y >= minY &&
                    p.Y <= maxY)
            .AsNoTracking()
            .ToListAsync();
    }
}
