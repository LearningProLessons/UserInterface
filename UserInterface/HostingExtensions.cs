public static class HostingExtensions
{
    public static IApplicationBuilder UseCustomPipeline(this IApplicationBuilder app)
    {
        var isDevEnvironment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        if (isDevEnvironment)
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages().RequireAuthorization();
        });

        return app;
    }
}
