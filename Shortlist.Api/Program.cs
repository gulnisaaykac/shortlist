using Microsoft.AspNetCore.Identity;
using Shortlist.Api;

var builder = WebApplication.CreateBuilder(args);
//Web sunucusunu kuracak olan nesne henüz dinlenmiyor
builder.Services.AddEndpointsApiExplorer();
//minimal api uçlarıı (get/post) keşfet   Swaggerın hangi url var demesi için 
builder.Services.AddSwaggerGen();
//o keşiften /swagger sayfasının json unu üretecek olan nesne

var app = builder.Build();
//ayarlar bitti gerçek uygulamanın nesnesi oluşturuldu ve artık dinlenmeye hazır

if (app.Environment.IsDevelopment())
//sadece geliştirme . üretimde swagger açılmasın diye
{
    app.UseSwagger();
    app.UseSwaggerUI();//tarayıcıdaki yeşil try it out sayfası için 
}

app.MapGet("/applications", (HttpRequest request) =>
{
    var user = GetUserFromRequest(request);
    if (user is null)
        return Results.Unauthorized();

    var items = Store.Load().Where(x => x.UserId == user.Id).ToList();
    return Results.Ok(items);
});

app.MapGet("/applications/{id}", (string id, HttpRequest request) =>
{
    var user = GetUserFromRequest(request);
    if (user is null)
        return Results.Unauthorized();

    var item = Store.Load().Find(x => x.Id == id && x.UserId == user.Id);
    if (item is null)
        return Results.NotFound();
    return Results.Ok(item);
});

app.MapPost("/applications", (ApplicationRecord body, HttpRequest request) =>
{
    var user = GetUserFromRequest(request);
    if (user is null)
        return Results.Unauthorized();

    if (string.IsNullOrWhiteSpace(body.Company) || string.IsNullOrWhiteSpace(body.Role))
        return Results.BadRequest("Company and Role are required fields.");

    var items = Store.Load();
    var item = new ApplicationRecord
    {
        Id = Guid.NewGuid().ToString("N"),
        Company = body.Company.Trim(),
        Role = body.Role.Trim(),
        Status = "applied",
        CreatedAt = DateTime.UtcNow.ToString("o"),
        UserId = user.Id
    };

    items.Add(item);
    Store.Save(items);
    return Results.Created($"/applications/{item.Id}", item);
});

app.MapPost("/applications/{id}/status", (string id, StatusUpdate body, HttpRequest request) =>
{
    var user = GetUserFromRequest(request);
    if (user is null)
        return Results.Unauthorized();

    var to = body.To?.Trim().ToLowerInvariant();
    var allowed = new[] { "applied", "waiting", "interview", "offer", "rejected" };

    if (string.IsNullOrWhiteSpace(to) || Array.IndexOf(allowed, to) < 0)
        return Results.BadRequest("invalid to. use: applied, waiting, interview, offer, rejected");

    var items = Store.Load();
    var item = items.Find(x => x.Id == id && x.UserId == user.Id);

    if (item is null)
        return Results.NotFound();

    item.Status = to;
    Store.Save(items);
    return Results.Ok(item);
});

app.MapPost("/register", (RegisterRequest body) =>
{
    var email = body.Email?.Trim().ToLowerInvariant();
    var password = body.Password;

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.BadRequest("email and password are required");

    var users = UserStore.Load();
    if (users.Exists(u => u.Email == email))
        return Results.BadRequest("email already registered");

    var user = new UserRecord
    {
        Id = Guid.NewGuid().ToString("N"),
        Email = email
    };

    var hasher = new PasswordHasher<UserRecord>();
    user.PasswordHash = hasher.HashPassword(user, password);

    users.Add(user);
    UserStore.Save(users);

    return Results.Created($"/users/{user.Id}", new { user.Id, user.Email });
});

app.MapPost("/login", (RegisterRequest body) =>
{
    var email = body.Email?.Trim().ToLowerInvariant();
    var password = body.Password;

    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.BadRequest("email and password are required");

    var users = UserStore.Load();
    var user = users.Find(u => u.Email == email);
    if (user is null)
        return Results.Json(new { message = "invalid email or password" }, statusCode: 401);

    var hasher = new PasswordHasher<UserRecord>();
    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
    if (result == PasswordVerificationResult.Failed)
        return Results.Json(new { message = "invalid email or password" }, statusCode: 401);

    var token = Guid.NewGuid().ToString("N");
    var sessions = SessionStore.Load();
    sessions.Add(new SessionRecord
    {
        Token = token,
        UserId = user.Id,
        CreatedAt = DateTime.UtcNow.ToString("o")
    });
    SessionStore.Save(sessions);

    return Results.Ok(new { user.Id, user.Email, token });
});

static UserRecord? GetUserFromRequest(HttpRequest request)
{
    var header = request.Headers.Authorization.ToString();
    const string prefix = "Bearer ";
    if (!header.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        return null;

    var token = header[prefix.Length..].Trim();
    if (string.IsNullOrWhiteSpace(token))
        return null;

    var session = SessionStore.Load().Find(s => s.Token == token);
    if (session is null)
        return null;

    return UserStore.Load().Find(u => u.Id == session.UserId);
}

app.Run();
//dinlemeye başla buraya kadar gelmeden maplar kayıtlı olur program burda bekler ctrl+c ile kapanır

public sealed class StatusUpdate
{
    public string? To { get; set; }
}