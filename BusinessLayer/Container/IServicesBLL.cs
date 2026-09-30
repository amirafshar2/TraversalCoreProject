using BusinessLayer.Abstract;
using BusinessLayer.Concrate;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFrameWork;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Container
{
    /// <summary>Registriert alle Services (BLL) und Repositories (DAL) im DI-Container.</summary>
    public static class IServicesBLL
    {
        public static void ContainerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAboutServic, AboutManager>();
            services.AddScoped<IAbouteDAL, EfAboutDAL>();

            services.AddScoped<IAbout2Service, Aboute2Manager>();
            services.AddScoped<IAbout2DAL, EfAbout2DAL>();

            services.AddScoped<ICommentService, CommentManager>();
            services.AddScoped<ICommentDAL, EfCommentDAL>();

            services.AddScoped<IContactServic, ContactManager>();
            services.AddScoped<IContactDAL, EfContactDAL>();

            services.AddScoped<IContactMessageService, ContactMessageManager>();
            services.AddScoped<IContactMessageDAL, EfContactMessageDAL>();

            services.AddScoped<IDestinitionServic, DestinitonsManager>();
            services.AddScoped<IDestinationDAL, EfDestinitionDAL>();

            services.AddScoped<IFeatureServic, FeatureManager>();
            services.AddScoped<IFeatureDAL, EfFeatureDAL>();

            services.AddScoped<IFeatur2Servic, Feature2Manager>();
            services.AddScoped<IFeature2DAL, EfFeature2DAL>();

            services.AddScoped<IGuideService, GuideManager>();
            services.AddScoped<IGuideDAL, EfGuideDAL>();

            services.AddScoped<INewsletterService, NewsletterManager>();
            services.AddScoped<INewsLetterDAL, EfNewsletterDAL>();

            services.AddScoped<IReservationService, ReservationManager>();
            services.AddScoped<IReservationDal, EfReservationDAL>();

            services.AddScoped<ISubAboutServic, SubAboutManager>();
            services.AddScoped<ISababoutDAL, EfSubAboutDAL>();

            services.AddScoped<ITestimonialServic, TestimonialManeger>();
            services.AddScoped<ITestimonialDAL, EfTestimonialDAL>();

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserDAL, EfUserDAL>();
        }
    }
}
