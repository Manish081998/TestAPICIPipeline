var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISalaryService,        SalaryService>();
builder.Services.AddScoped<ISalaryRepository,     SalaryRepository>();
builder.Services.AddScoped<IDepartmentService,    DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITicketService,        TicketService>();
builder.Services.AddScoped<ITicketRepository,     TicketRepository>();
builder.Services.AddScoped<IJobOrderService,      JobOrderService>();
builder.Services.AddScoped<IJobOrderRepository,   JobOrderRepository>();
builder.Services.AddScoped<IPrStatusService,      PrStatusService>();
builder.Services.AddScoped<IPrStatusRepository,   PrStatusRepository>();
builder.Services.AddScoped<ICaseService,          CaseService>();
builder.Services.AddScoped<ICaseRepository,       CaseRepository>();
builder.Services.AddScoped<IDbHelper,             DbHelper>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
