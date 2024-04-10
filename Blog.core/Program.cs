using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Blog.core.IRepository;
using Blog.Core.AOP;
using Blog.Core.Auth;
using Blog.Core.Helper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Repository;
using Blog.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
#region JWT服务
// 注册JWT服务
builder.Services.AddSingleton(new JwtHelper(builder.Configuration));

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //是否验证Issuer
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //发行人Issuer
        ValidateAudience = false, //是否验证Audience      
        ValidateIssuerSigningKey = true, //是否验证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecKey"])), //SecurityKey
        ValidateLifetime = true, //是否验证失效时间
        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
        RequireExpirationTime = true,
    };
}
);
builder.Services.AddAuthorization(options =>
{
/***    "Client" 策略要求用户必须拥有 "Client" 角色才能访问相关资源。
"Admin" 策略要求用户必须拥有 "Admin" 角色才能访问相关资源。
"SystemOrAdmin" 策略要求用户必须拥有 "Admin" 或者 "System" 角色之一才能访问相关资源。***/
    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
});
#endregion
#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=> {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
    //开启注释
    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
    // 配置 JWT Bearer 授权
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, new string[] { } } };
    c.AddSecurityRequirement(securityRequirement);
});
#endregion
//IOC
//builder.Services.AddTransient<IUserRepository, UserRepository>();
//builder.Services.AddTransient<IUserService, UserService>();
#region AOP

var basePath = AppContext.BaseDirectory;
var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll"); //服务层
var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll"); //仓储层
if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
{
    throw new Exception("Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。");
}
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(build =>
{
    // AOP 
    var cacheType = new List<Type>();
    build.RegisterType<BlogLogAOP>();    
    cacheType.Add(typeof(BlogLogAOP));
    build.RegisterType<BlogCacheAOP>();
    cacheType.Add(typeof(BlogCacheAOP));

    build.RegisterType<MemoryCaching>().As<ICaching>()
           .AsImplementedInterfaces()
           .InstancePerDependency();
    // 获取 Service.dll 程序集服务，并注册
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    build.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

    // 获取 Repository.dll 程序集服务，并注册
    var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
    build.RegisterAssemblyTypes(assemblysRepository)
            .AsImplementedInterfaces()
            .InstancePerDependency();

   
});
#endregion
//跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
});
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    }); 
}
//启用验证中间件
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
