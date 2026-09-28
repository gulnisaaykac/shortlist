using Shortlist;

if (args.Length == 0)
{
    Console.WriteLine("Usage: add --company <name> --role <title> | list");
    return 1;
}

var command = args[0];

if (command == "add")
{ 
    var company = GetOption(args , "--company");
    var role = GetOption(args , "--role");

    if (string.IsNullOrWhiteSpace(company) || string.IsNullOrWhiteSpace(role))
    {
        Console.WriteLine("add requires --company and --role");
        return 1;
    }   

    var items = Store.Load();
    items.Add(new ApplicationRecord
    {
        Id = Guid.NewGuid().ToString("N"),
        Company = company.Trim(),
        Role = role.Trim(),
        Status = "applied",
        CreatedAt = DateTime.UtcNow.ToString("o")//dateTime.now desek bilgisayarımdaki aktif saati veriridi ama utc now dediğimizde ortak saati verir türkiye utc+3 yani öyle düşün
    });
    Store.Save(items);
    Console.WriteLine("saved");
    return 0;

}
if (command == "list")
{
    var items = Store.Load();
    if (items.Count == 0)
    {
        Console.WriteLine("No applications found yet.");
        return 0;
    }

    foreach (var item in items)
        Console.WriteLine($"{item.Id}   {item.Company}   {item.Role}   {item.Status}");
    return 0;
}
if (command == "status")
{
    var id = GetOption(args, "--id");
    var to = GetOption(args, "--to");

    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(to))
    {
        Console.Error.WriteLine("status requires --id and --to");
        return 1;
    }

    to = to.Trim().ToLowerInvariant();
    var allowed = new[] { "applied", "waiting", "interview", "offer", "rejected" };
    if (Array.IndexOf(allowed, to) < 0)
    {
        Console.Error.WriteLine("invalid --to. use: applied, waiting, interview, offer, rejected");
        return 1;
    }

    var items = Store.Load();
    var item = items.Find(x => x.Id == id.Trim());
    if (item is null)
    {
        Console.Error.WriteLine("not found");
        return 1;
    }

    item.Status = to;
    Store.Save(items);
    Console.WriteLine("updated");
    return 0;
}

Console.Error.WriteLine($"unknown command: {command}");
return 1;

static string? GetOption(string[] args, string name)
{
    for (var i = 0; i < args.Length - 1; i++)
    {
        if (args[i] == name)
            return args[i + 1];
    }
    return null;
}