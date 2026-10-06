using Microsoft.EntityFrameworkCore;
using CircuitYard.Api.Data;
using CircuitYard.Api.Extensions;

namespace CircuitYard.Api.Endpoints.Handlers;

public class LogoutHandler
{
    public static async Task<IResult> Handler(HttpContext httpContext, AppDbContext dbContext)
    {
        var refreshToken = httpContext.GetRefreshToken();

        if (refreshToken is null)
        {
            httpContext.DeleteAuthCookies();
            return Results.Ok();
        }

        var storedRefreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedRefreshToken is not null)
        {
            dbContext.RefreshTokens.Remove(storedRefreshToken);
            await dbContext.SaveChangesAsync();
        }

        httpContext.DeleteAuthCookies();
        return Results.Ok();
    }
}
