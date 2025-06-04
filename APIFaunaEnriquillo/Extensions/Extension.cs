namespace APIFaunaEnriquillo.Extensions
{
    public static class Extension
    {
        
        public static void UseExtensionPresentationSwagger(this IApplicationBuilder builder)
        {
           builder.UseSwagger();
            builder.UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fauna Enriquillo API V1");
              });



        }


    }
}
