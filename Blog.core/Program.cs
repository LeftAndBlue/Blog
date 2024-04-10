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
#region JWT����
// ע��JWT����
builder.Services.AddSingleton(new JwtHelper(builder.Configuration));

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //�Ƿ���֤Issuer
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //������Issuer
        ValidateAudience = false, //�Ƿ���֤Audience      
        ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecKey"])), //SecurityKey
        ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
        ClockSkew = TimeSpan.FromSeconds(30), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
        RequireExpirationTime = true,
    };
}
);
builder.Services.AddAuthorization(options =>
{
/***    "Client" ����Ҫ���û�����ӵ�� "Client" ��ɫ���ܷ��������Դ��
"Admin" ����Ҫ���û�����ӵ�� "Admin" ��ɫ���ܷ��������Դ��
"SystemOrAdmin" ����Ҫ���û�����ӵ�� "Admin" ���� "System" ��ɫ֮һ���ܷ��������Դ��***/
    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
});
#endregion
#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=> {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
    //����ע��
    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
    // ���� JWT Bearer ��Ȩ
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
var servicesDllFile = Path.Combine(basePath, "Blog.Core.Services.dll"); //�����
var repositoryDllFile = Path.Combine(basePath, "Blog.Core.Repository.dll"); //�ִ���
if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
{
    throw new Exception("Repository.dll��service.dll ��ʧ����Ϊ��Ŀ�����ˣ�������Ҫ��F6���룬��F5���У����� bin �ļ��У���������");
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
    // ��ȡ Service.dll ���򼯷��񣬲�ע��
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    build.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray());//����������������б�����ע�ᡣ

    // ��ȡ Repository.dll ���򼯷��񣬲�ע��
    var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
    build.RegisterAssemblyTypes(assemblysRepository)
            .AsImplementedInterfaces()
            .InstancePerDependency();

   
});
#endregion
//����
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
//������֤�м��
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
