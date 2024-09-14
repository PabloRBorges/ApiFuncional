using ApiFuncional.Data;
using ApiFuncional.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiFuncional.Configuration
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>();

            //Pegando o Token e gerando uma chave encodada
            var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(JwtSettingsSection);

            var jwtSettings = JwtSettingsSection.Get<JwtSettings>(); //pega informação populada pelo Configure<jwtSettings>

            //Gera a chave encodada
            var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //aponta qual será o esquema de autenticação no caso webtokens
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //desafio para validar se está certo ou não
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true; //durante o request o token estará na requisição daquele request dentro do http. content (apenas durante a requisição
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key), //chave de emissão do token usando a key, ele vai retornar uma instância
                    ValidateIssuer = true, // valida quem for o emissor
                    ValidateAudience = true, //se minha audiência é compatível com o que vem no token
                    ValidAudience = jwtSettings.Audiencia, //validando a audiência que está dentro do jwtsettnigs
                    ValidIssuer = jwtSettings.Emissor //validando o emissor
                };
            });

            return builder;
        }

    }
}
