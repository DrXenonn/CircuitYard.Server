using Microsoft.AspNetCore.SignalR;
using CircuitYard.Server.Data;
using CircuitYard.Server.Models;
using CircuitYard.Server.Dtos;
using Microsoft.EntityFrameworkCore;

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
        var cell = await _dbContext.Cells.FindAsync(x, y);

        if (cell is null)
        {
            _dbContext.Cells.Add(new Cell
            {
                X = x,
                Y = y,
                IsPlaced = true
            });
        }

        await _dbContext.SaveChangesAsync();
    }

    //TODO: Use constants for chunk and nearby chunks to load(render distance) and world side
    //Refactor the whole hub.
    public async Task<List<PlacedObjectDto>> LoadNearbyObjects(int chunkX, int chunkY)
    {
        var minX = Math.Max(0, chunkX - 1);
        var maxX = Math.Min(7, chunkX + 1);
        var minY = Math.Max(0, chunkY - 1);
        var maxY = Math.Min(7, chunkY + 1);

        var chunks = await _dbContext.Chunks
            .AsNoTracking()
            .Include(c => c.Cells)
            .Where(c => c.X >= minX && c.X <= maxX &&
                        c.Y >= minY && c.Y <= maxY)
            .ToListAsync();

        return chunks
            .SelectMany(chunk => chunk.Cells)
            .Select(cell => new PlacedObjectDto(cell.X, cell.Y, true))
            .ToList();
    }
}
