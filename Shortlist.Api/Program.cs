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

app.MapGet("/applications", () =>//boş query/body yok
{
    var items = Store.Load();
    //cli gibi json dosyasından liste yoksa boş liste döndürür
    return Results.Ok(items);//http 200 döndürür ve body de items json olarak döner
});

app.MapGet("/applications/{id}", (string id) =>
{
    var items = Store.Load();
    var item = items.Find(x => x.Id == id);
    if (item is null)
        return Results.NotFound();
    return Results.Ok(item);
});

app.MapPost("/applications", (ApplicationRecord body) =>//post ayni url ama body var
{
    if (string.IsNullOrWhiteSpace(body.Company) || string.IsNullOrWhiteSpace(body.Role))
        //şirket vey rol yok boşluk --->kaydetme
        return Results.BadRequest("Company and Role are required fields.");
    //http 400 döndürür  cli daki retur1 + hata yazisinin http hali

    var items = Store.Load();
    //mevcut kayıtları al üzerine ekle dosyayı ezme
    var item = new ApplicationRecord  
    //cli add ile ayni 
    {
        Id = Guid.NewGuid().ToString("N"),
        Company = body.Company.Trim(),
        Role = body.Role.Trim(),
        Status = "applied",
        CreatedAt = DateTime.UtcNow.ToString("o")
    };

    items.Add(item);
    //hafıza + disk cli store ile ayni dosya mantığı 
    Store.Save(items);
    return Results.Created($"/applications/{item.Id}", item);
});

app.MapPost("/applications/{id}/status", (string id, StatusUpdate body) =>
{
    var to = body.To?.Trim().ToLowerInvariant();
    var allowed = new[] { "applied", "waiting", "interview", "offer", "rejected" };

    if (string.IsNullOrWhiteSpace(to) || Array.IndexOf(allowed, to) < 0)
        return Results.BadRequest("invalid to. use: applied, waiting, interview, offer, rejected");

    var items = Store.Load();
    var item = items.Find(x => x.Id == id);

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

    return Results.Ok(new { user.Id, user.Email });
});

app.Run();
//dinlemeye başla buraya kadar gelmeden maplar kayıtlı olur program burda bekler ctrl+c ile kapanır

public sealed class StatusUpdate
{
    public string? To { get; set; }
}