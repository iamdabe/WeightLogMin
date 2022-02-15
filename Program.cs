using LiteDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLiteDb(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapGet("/", (LiteDbContext db) =>
{
    return Results.Content("ok");
});

/// <summary>
/// Endpoint for listing all weightLog for a user
/// </summary>
app.MapGet("/log/{userId}/", (LiteDbContext db, int userId) =>
{
    var weightLogs = db.Context.GetCollection<WeightLog>("WeightLog");
    return weightLogs.Find(x => x.userId == userId).ToList();
});

/// <summary>
/// Endpoint for retrieving individual weightLog id
/// </summary>
app.MapGet("/log/{userId}/{id}", (LiteDbContext db, int userId, int id) =>
{
    var weightLogs = db.Context.GetCollection<WeightLog>("WeightLog");
    return weightLogs.Find(x => x.userId == userId && x.Id == id).FirstOrDefault();
});

/// <summary>
/// Endpoint for posting a weightLog entry
/// </summary>
app.MapPost("/log/{userId}", (LiteDbContext db, int userId, WeightLogWrapperDto weightLogWrapperDto) =>
{
    if (weightLogWrapperDto.secret != builder.Configuration.GetSection("Secret").Value)
    {
        return Results.BadRequest();
    }
    try
    {
        var weightLog = new WeightLog
        {
            userId = userId,
            // Expected date format from Withings HealthMate is peculiar, for example February 14, 2022 at 09:36AM
            dateAndTime = DateTime.ParseExact(weightLogWrapperDto.weightLog.dateAndTime.Replace("at ", "").Replace(",", ""), "MMMM dd yyyy hh:mmtt", System.Globalization.CultureInfo.InvariantCulture),
            weight = weightLogWrapperDto.weightLog.weight,
            unit = weightLogWrapperDto.weightLog.unit,
            fatMass = weightLogWrapperDto.weightLog.fatMass,
            fatMassPercent = weightLogWrapperDto.weightLog.fatMassPercent,
            leanMass = weightLogWrapperDto.weightLog.leanMass
        };

        var weightLogs = db.Context.GetCollection<WeightLog>("WeightLog");
        weightLogs.Insert(weightLog);


        return Results.Ok(weightLog);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"WeightLog for {userId} not saved, maybe exception: " + ex.ToString());
    }
});

app.Run();

