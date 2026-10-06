using Microsoft.EntityFrameworkCore;
using NodaTime;
using CircuitYard.Server.Data;
using CircuitYard.Server.Extensions;
using CircuitYard.Server.Models;
using CircuitYard.Server.Services;

namespace CircuitYard.Server.Endpoints.Handlers;

public class RefreshTokenHandler
{
    public static async Task<IResult> Handler(
        AppDbContext dbContext,
        TokenProvider tokenProvider,
        HttpContext httpContext)
    {
        var refreshToken = httpContext.Request.Cookies["refreshToken"];

        RefreshToken? storedRefreshToken = await dbContext.RefreshTokens
            .Include(rt => rt.ApplicationUser)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedRefreshToken is null || storedRefreshToken.Expires < SystemClock.Instance.GetCurrentInstant())
        {
            return Results.Unauthorized();
        }

        var accessToken = await tokenProvider.CreateAccessToken(storedRefreshToken.ApplicationUser);

        storedRefreshToken.Token = tokenProvider.GenerateRefreshToken();
        storedRefreshToken.Expires = SystemClock.Instance.GetCurrentInstant() + Duration.FromDays(7);

        httpContext.AppendAccessTokenCookie(accessToken);
        httpContext.AppendRefreshTokenCookie(storedRefreshToken.Token);

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }
}
