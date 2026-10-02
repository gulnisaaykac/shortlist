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

app.Run();
//dinlemeye başla buraya kadar gelmeden maplar kayıtlı olur program burda bekler ctrl+c ile kapanır

public sealed class StatusUpdate
{
    public string? To { get; set; }
}