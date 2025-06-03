using Dragon.Model.Configs;
using Dragon.Provider;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication(a => { a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(a => { a.RequireHttpsMetadata = false; a.TokenValidationParameters = new() { IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigProvider.EncryptionKey)), ValidateIssuer = false, ValidateLifetime = true, ClockSkew = TimeSpan.Zero, ValidateAudience = false }; });
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s => { s.AddSecurityDefinition("Bearer", new() { Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey, Description = "Enter JWT with Bearer into field" }); s.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } }); });
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDirectoryBrowser();
builder.Services.AddScoped(p => { Claim claim = ((ClaimsIdentity)p.GetService<IHttpContextAccessor>().HttpContext.User.Identity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData); return claim != null && !string.IsNullOrWhiteSpace(claim.Value) ? claim.Value.FromJson<User>() : null; });

WebApplication app = builder.Build();

app.UseSwagger().UseDeveloperExceptionPage().UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Dragon"); });
app.UseHttpsRedirection().UseRouting();
app.UseHealthChecks("/", new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(ConfigProvider.Provider.BaseDirectory), RequestPath = new PathString($"/{ConfigProvider.Settings.BaseFolderName}") });
app.UseAuthentication().UseAuthorization();
app.MapControllers();
app.Run();