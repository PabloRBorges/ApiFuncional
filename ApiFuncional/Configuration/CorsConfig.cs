namespace ApiFuncional.Configuration
{
    public static class CorsConfig
    {
        public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("Development", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                );

                option.AddPolicy("Production", builder =>
                    builder.WithOrigins("http://localhost:9000") //Origens aceitas
                        .WithMethods("POST")  // métodos permitidos
                        .AllowAnyHeader()); //cabe
            });

            return builder;
        }

    }
}
