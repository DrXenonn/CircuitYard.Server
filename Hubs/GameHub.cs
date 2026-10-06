using Microsoft.AspNetCore.SignalR;
using CircuitYard.Server.Data;
using CircuitYard.Server.Models;

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
}
