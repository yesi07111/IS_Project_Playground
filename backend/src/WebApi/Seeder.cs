using Microsoft.AspNetCore.Identity;
using Playground.Domain.Entities.Auth;
using Playground.Infraestructure.Data.DbContexts;
using Playground.Domain.Entities;
using Playground.Application.Factories;
using Playground.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Playground.WebApi;

public static class Seeder
{

    /// <summary>
    /// Inicializa los roles, usuarios, instalaciones, actividades, reseñas, reservas y recursos necesarios en la aplicación si no existen.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios de la aplicación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public static async Task AddSeeds(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
        var repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var facilityRepository = repositoryFactory.CreateRepository<Facility>();
        var activityRepository = repositoryFactory.CreateRepository<Activity>();
        var activityDateRepository = repositoryFactory.CreateRepository<ActivityDate>();
        var reservationRepository = repositoryFactory.CreateRepository<Reservation>();
        var reviewRepository = repositoryFactory.CreateRepository<Review>();
        var resourceRepository = repositoryFactory.CreateRepository<Resource>();
        var resourceDateRepository = repositoryFactory.CreateRepository<ResourceDate>();

        // Crear una instancia de Random para generar números aleatorios
        var random = new Random();

        // Vaciar todas las tablas
        context.Set<Review>().RemoveRange(context.Set<Review>());
        context.Set<Reservation>().RemoveRange(context.Set<Reservation>());
        context.Set<ActivityDate>().RemoveRange(context.Set<ActivityDate>());
        context.Set<Activity>().RemoveRange(context.Set<Activity>());
        context.Set<Facility>().RemoveRange(context.Set<Facility>());
        context.Set<Resource>().RemoveRange(context.Set<Resource>());
        context.Users.RemoveRange(context.Users);

        await unitOfWork.CommitAsync();


        #region Roles Manual
        // Crear roles personalizados
        var roles = new List<Rol>
        {
            new() { Name = "Admin" },
            new() { Name = "Educator" },
            new() { Name = "Parent" }
        };

        foreach (var role in roles)
        {
            if (!await context.Rol.AnyAsync(r => r.Name == role.Name))
            {
                await context.Rol.AddAsync(role);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Admin/Educador
        // Obtener roles para asignar
        var adminRole = await context.Rol.FirstOrDefaultAsync(r => r.Name == "Admin");
        var educatorRole = await context.Rol.FirstOrDefaultAsync(r => r.Name == "Educator");
        var parentRole = await context.Rol.FirstOrDefaultAsync(r => r.Name == "Parent");

        // Crear administrador
        var adminUser = new User
        {
            UserName = "admin1",
            Email = "admin1@ejemplo.com",
            FirstName = "Administrador",
            LastName = "Usuario",
            EmailConfirmed = true,
            Rol = adminRole!
        };

        if (userManager.Users.All(u => u.UserName != adminUser.UserName))
        {
            await userManager.CreateAsync(adminUser, "AdminPassword123!");
        }
        await unitOfWork.CommitAsync();

        // Crear educadores
        var educators = new List<User>
        {
            new() { UserName = "educador1", Email = "educador1@ejemplo.com", FirstName = "Juan", LastName = "Pérez", EmailConfirmed = true, Rol = educatorRole! },
            new() { UserName = "educador2", Email = "educador2@ejemplo.com", FirstName = "Ana", LastName = "García", EmailConfirmed = true, Rol = educatorRole! },
            new() { UserName = "educador3", Email = "educador3@ejemplo.com", FirstName = "María", LastName = "Rodríguez", EmailConfirmed = true, Rol = educatorRole! },
            new() { UserName = "educador4", Email = "educador4@ejemplo.com", FirstName = "Miguel", LastName = "López", EmailConfirmed = true, Rol = educatorRole! },
            new() { UserName = "educador5", Email = "educador5@ejemplo.com", FirstName = "Sara", LastName = "Martínez", EmailConfirmed = true, Rol = educatorRole! },
            new() { UserName = "educador6", Email = "educador6@ejemplo.com", FirstName = "David", LastName = "Fernández", EmailConfirmed = true, Rol = educatorRole! }
        };

        foreach (var educator in educators)
        {
            if (userManager.Users.All(u => u.UserName != educator.UserName))
            {
                await userManager.CreateAsync(educator, "Contraseña123!");
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Instalaciones
        // Crear instalaciones
        var facilities = new List<Facility>
        {
            new() { Name = "Taller de Arte", Location = "Calle Arte 123", Type = "Taller", MaximumCapacity = 20, UsagePolicy = "Abierto para todos." },
            new() { Name = "Piscina", Location = "Avenida Agua 456", Type = "Piscina", MaximumCapacity = 30, UsagePolicy = "Se requiere traje de baño." },
            new() { Name = "Carrusel", Location = "Avenida Diversión 789", Type = "Atracción", MaximumCapacity = 15, UsagePolicy = "Solo para niños." },
            new() { Name = "Laboratorio de Ciencias", Location = "Bulevar Ciencia 101", Type = "Laboratorio", MaximumCapacity = 35, UsagePolicy = "Se requiere equipo de seguridad." },
            new() { Name = "Estudio de Danza", Location = "Calle Danza 202", Type = "Estudio", MaximumCapacity = 20, UsagePolicy = "Se requieren zapatos de baile." },
            new() { Name = "Sala de Música", Location = "Avenida Música 303", Type = "Sala", MaximumCapacity = 15, UsagePolicy = "Instrumentos proporcionados." },
            new() { Name = "Gimnasio", Location = "Calle Salud 404", Type = "Gimnasio", MaximumCapacity = 50, UsagePolicy = "Ropa deportiva requerida." },
            new() { Name = "Biblioteca", Location = "Avenida Conocimiento 505", Type = "Biblioteca", MaximumCapacity = 40, UsagePolicy = "Silencio requerido." },
            new() { Name = "Cafetería", Location = "Plaza Comida 606", Type = "Cafetería", MaximumCapacity = 60, UsagePolicy = "Prohibido llevar comida fuera." },
            new() { Name = "Zona de Juegos Acuáticos", Location = "Calle Agua 707", Type = "Juegos Acuáticos", MaximumCapacity = 25, UsagePolicy = "Ropa de baño requerida." },
            new() { Name = "Parque de Aventuras", Location = "Avenida Aventura 808", Type = "Parque", MaximumCapacity = 30, UsagePolicy = "Supervisión de adultos requerida." },
            new() { Name = "Mini Zoológico", Location = "Bulevar Animal 909", Type = "Zoológico", MaximumCapacity = 20, UsagePolicy = "No alimentar a los animales." }
        };

        foreach (var facility in facilities)
        {
            if (!await context.Facility.AnyAsync(f => f.Name == facility.Name))
            {
                await facilityRepository.AddAsync(facility);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Actividades
        // Crear actividades
        var educador1 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador1");
        var educador2 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador2");
        var educador3 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador3");
        var educador4 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador4");
        var educador5 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador5");
        var educador6 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "educador6");
        var tallerArte = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Taller de Arte");
        var piscina = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Piscina");
        var carrusel = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Carrusel");
        var laboratorioCiencias = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Laboratorio de Ciencias");
        var estudioDanza = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Estudio de Danza");
        var salaMusica = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Sala de Música");
        var gimnasio = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Gimnasio");
        var biblioteca = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Biblioteca");
        var cafeteria = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Cafetería");
        var zonaJuegosAcuaticos = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Zona de Juegos Acuáticos");
        var parqueAventuras = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Parque de Aventuras");
        var miniZoologico = await context.Facility.FirstOrDefaultAsync(f => f.Name == "Mini Zoológico");

        var activities = new List<Activity>
        {
            // Taller de Arte
            new() { Name = "Clase de Arte", Description = "Clase creativa de arte.", Educator = educador1!, Type = "Arte", RecommendedAge = 7, ItsPrivate = false, Facility = tallerArte! },
            new() { Name = "Taller de Pintura", Description = "Aprender técnicas de pintura.", Educator = educador1!, Type = "Arte", RecommendedAge = 6, ItsPrivate = false, Facility = tallerArte! },
            new() { Name = "Clase de Fotografía", Description = "Aprender técnicas de fotografía.", Educator = educador3!, Type = "Arte", RecommendedAge = 15, ItsPrivate = false, Facility = tallerArte! },

            // Piscina
            new() { Name = "Lecciones de Natación", Description = "Aprender a nadar.", Educator = educador2!, Type = "Deporte", RecommendedAge = 5, ItsPrivate = false, Facility = piscina! },
            new() { Name = "Clases de Buceo", Description = "Introducción al buceo.", Educator = educador2!, Type = "Deporte", RecommendedAge = 12, ItsPrivate = false, Facility = piscina! },
            new() { Name = "Aqua Aeróbic", Description = "Ejercicios aeróbicos en el agua.", Educator = educador3!, Type = "Deporte", RecommendedAge = 10, ItsPrivate = false, Facility = piscina! },

            // Carrusel
            new() { Name = "Diversión en el Carrusel", Description = "Disfruta del paseo.", Educator = educador3!, Type = "Diversión", RecommendedAge = 4, ItsPrivate = false, Facility = carrusel! },
            new() { Name = "Carrusel Nocturno", Description = "Paseo en carrusel bajo las estrellas.", Educator = educador4!, Type = "Diversión", RecommendedAge = 6, ItsPrivate = false, Facility = carrusel! },
            new() { Name = "Carrusel Temático", Description = "Paseo en carrusel con temas especiales.", Educator = educador5!, Type = "Diversión", RecommendedAge = 5, ItsPrivate = false, Facility = carrusel! },

            // Laboratorio de Ciencias
            new() { Name = "Experimento Científico", Description = "Experimentos prácticos de ciencia.", Educator = educador4!, Type = "Ciencia", RecommendedAge = 10, ItsPrivate = false, Facility = laboratorioCiencias! },
            new() { Name = "Ciencia Divertida", Description = "Experimentos para niños.", Educator = educador4!, Type = "Ciencia", RecommendedAge = 8, ItsPrivate = false, Facility = laboratorioCiencias! },
            new() { Name = "Taller de Robótica", Description = "Construir y programar robots.", Educator = educador5!, Type = "Tecnología", RecommendedAge = 12, ItsPrivate = false, Facility = laboratorioCiencias! },

            // Estudio de Danza
            new() { Name = "Clase de Danza", Description = "Aprender nuevos pasos de baile.", Educator = educador5!, Type = "Danza", RecommendedAge = 8, ItsPrivate = false, Facility = estudioDanza! },
            new() { Name = "Taller de Ballet", Description = "Técnicas de ballet para principiantes.", Educator = educador6!, Type = "Danza", RecommendedAge = 10, ItsPrivate = false, Facility = estudioDanza! },
            new() { Name = "Clase de Hip-Hop", Description = "Aprender movimientos de hip-hop.", Educator = educador1!, Type = "Danza", RecommendedAge = 12, ItsPrivate = false, Facility = estudioDanza! },

            // Sala de Música
            new() { Name = "Jam de Música", Description = "Tocar y aprender música.", Educator = educador6!, Type = "Música", RecommendedAge = 9, ItsPrivate = false, Facility = salaMusica! },
            new() { Name = "Clase de Guitarra", Description = "Aprender a tocar la guitarra.", Educator = educador2!, Type = "Música", RecommendedAge = 11, ItsPrivate = false, Facility = salaMusica! },
            new() { Name = "Taller de Percusión", Description = "Aprender ritmos de percusión.", Educator = educador3!, Type = "Música", RecommendedAge = 10, ItsPrivate = false, Facility = salaMusica! },

            // Gimnasio
            new() { Name = "Clase de Gimnasia", Description = "Ejercicios y entrenamiento.", Educator = educador1!, Type = "Deporte", RecommendedAge = 15, ItsPrivate = false, Facility = gimnasio! },
            new() { Name = "Clase de Yoga", Description = "Ejercicios de yoga.", Educator = educador6!, Type = "Deporte", RecommendedAge = 16, ItsPrivate = false, Facility = gimnasio! },
            new() { Name = "Clase de Zumba", Description = "Ejercicios de baile y fitness.", Educator = educador6!, Type = "Deporte", RecommendedAge = 18, ItsPrivate = false, Facility = gimnasio! },

            // Biblioteca
            new() { Name = "Club de Lectura", Description = "Discusión de libros.", Educator = educador2!, Type = "Educación", RecommendedAge = 12, ItsPrivate = false, Facility = biblioteca! },
            new() { Name = "Taller de Escritura", Description = "Aprender a escribir.", Educator = educador3!, Type = "Educación", RecommendedAge = 14, ItsPrivate = false, Facility = biblioteca! },
            new() { Name = "Torneo de Ajedrez", Description = "Competencia de ajedrez.", Educator = educador5!, Type = "Juego", RecommendedAge = 8, ItsPrivate = false, Facility = biblioteca! },

            // Cafetería
            new() { Name = "Clase de Cocina", Description = "Aprender a cocinar.", Educator = educador4!, Type = "Cocina", RecommendedAge = 10, ItsPrivate = false, Facility = cafeteria! },
            new() { Name = "Taller de Repostería", Description = "Hornear y decorar pasteles.", Educator = educador5!, Type = "Cocina", RecommendedAge = 12, ItsPrivate = false, Facility = cafeteria! },
            new() { Name = "Clase de Barismo", Description = "Aprender a preparar café.", Educator = educador6!, Type = "Cocina", RecommendedAge = 14, ItsPrivate = false, Facility = cafeteria! },

            // Zona de Juegos Acuáticos
            new() { Name = "Juegos de Agua", Description = "Diversión con juegos acuáticos.", Educator = educador1!, Type = "Diversión", RecommendedAge = 7, ItsPrivate = false, Facility = zonaJuegosAcuaticos! },
            new() { Name = "Competencia de Natación", Description = "Carreras de natación para niños.", Educator = educador2!, Type = "Deporte", RecommendedAge = 10, ItsPrivate = false, Facility = zonaJuegosAcuaticos! },
            new() { Name = "Fiesta de Espuma", Description = "Diversión con espuma y agua.", Educator = educador3!, Type = "Diversión", RecommendedAge = 8, ItsPrivate = false, Facility = zonaJuegosAcuaticos! },

            // Parque de Aventuras
            new() { Name = "Escalada en Roca", Description = "Aprender a escalar en roca.", Educator = educador4!, Type = "Aventura", RecommendedAge = 12, ItsPrivate = false, Facility = parqueAventuras! },
            new() { Name = "Tirolesa", Description = "Deslizarse por la tirolesa.", Educator = educador5!, Type = "Aventura", RecommendedAge = 10, ItsPrivate = false, Facility = parqueAventuras! },
            new() { Name = "Circuito de Cuerdas", Description = "Desafíos en cuerdas y puentes.", Educator = educador6!, Type = "Aventura", RecommendedAge = 11, ItsPrivate = false, Facility = parqueAventuras! },

            // Mini Zoológico
            new() { Name = "Visita Guiada", Description = "Recorrido por el mini zoológico.", Educator = educador1!, Type = "Educación", RecommendedAge = 6, ItsPrivate = false, Facility = miniZoologico! },
            new() { Name = "Alimentación de Animales", Description = "Aprender a alimentar a los animales.", Educator = educador2!, Type = "Educación", RecommendedAge = 7, ItsPrivate = false, Facility = miniZoologico! },
            new() { Name = "Taller de Cuidado Animal", Description = "Aprender sobre el cuidado de los animales.", Educator = educador3!, Type = "Educación", RecommendedAge = 8, ItsPrivate = false, Facility = miniZoologico! }
        };

        foreach (var activity in activities)
        {
            if (!await context.Activity.AnyAsync(a => a.Name == activity.Name))
            {
                await activityRepository.AddAsync(activity);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Fechas Activ.
        // Crear fechas de actividades
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        var threeDaysLater = today.AddDays(3);
        var fourDaysLater = today.AddDays(4);
        var tenDaysAgo = today.AddDays(-10);
        var fifteenDaysAgo = today.AddDays(-15);
        var twentyDaysAgo = today.AddDays(-20);

        // Fechas pasadas
        var fiveDaysAgo = today.AddDays(-5);
        var eightDaysAgo = today.AddDays(-8);
        var twelveDaysAgo = today.AddDays(-12);
        var eighteenDaysAgo = today.AddDays(-18);
        var twentyFiveDaysAgo = today.AddDays(-25);

        // Fechas futuras (próxima semana)
        var nextMonday = today.AddDays((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7);
        var nextTuesday = nextMonday.AddDays(1);
        var nextWednesday = nextMonday.AddDays(2);
        var nextThursday = nextMonday.AddDays(3);
        var nextFriday = nextMonday.AddDays(4);
        var nextSaturday = nextMonday.AddDays(5);
        var nextSunday = nextMonday.AddDays(6);
        var nextMondayPlusOneWeek = nextMonday.AddDays(7);

        var activityDates = new List<ActivityDate>
        {
            // Taller de Arte (Capacidad Máxima: 20)
            new() { Activity = activities[0], DateTime = AddRandomTime(fiveDaysAgo), ReservedPlaces = 20, Pending = false },
            new() { Activity = activities[0], DateTime = AddRandomTime(nextMonday), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[1], DateTime = AddRandomTime(eightDaysAgo), ReservedPlaces = 17, Pending = false },
            new() { Activity = activities[1], DateTime = AddRandomTime(nextTuesday), ReservedPlaces = 14, Pending = false},
            new() { Activity = activities[2], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 9, Pending = false},
            new() { Activity = activities[2], DateTime = AddRandomTime(nextWednesday), ReservedPlaces = 11, Pending = false },

            // Piscina (Capacidad Máxima: 30)
            new() { Activity = activities[3], DateTime = AddRandomTime(eighteenDaysAgo), ReservedPlaces = 30, Pending = false },
            new() { Activity = activities[3], DateTime = AddRandomTime(nextThursday), ReservedPlaces = 18, Pending = false },
            new() { Activity = activities[4], DateTime = AddRandomTime(twentyFiveDaysAgo), ReservedPlaces = 28, Pending = false },
            new() { Activity = activities[4], DateTime = AddRandomTime(nextFriday), ReservedPlaces = 19, Pending = false },
            new() { Activity = activities[5], DateTime = AddRandomTime(tenDaysAgo), ReservedPlaces = 26, Pending = false },
            new() { Activity = activities[5], DateTime = AddRandomTime(nextSaturday), ReservedPlaces = 17, Pending = false },

            // Carrusel (Capacidad Máxima: 15)
            new() { Activity = activities[6], DateTime = AddRandomTime(fifteenDaysAgo), ReservedPlaces = 13, Pending = false },
            new() { Activity = activities[6], DateTime = AddRandomTime(nextSunday), ReservedPlaces = 15, Pending = false },
            new() { Activity = activities[7], DateTime = AddRandomTime(twentyDaysAgo), ReservedPlaces = 15, Pending = false },
            new() { Activity = activities[7], DateTime = AddRandomTime(nextMondayPlusOneWeek), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[8], DateTime = AddRandomTime(eightDaysAgo), ReservedPlaces = 10, Pending = false },
            new() { Activity = activities[8], DateTime = AddRandomTime(nextTuesday), ReservedPlaces = 11, Pending = false },

            // Laboratorio de Ciencias (Capacidad Máxima: 25)
            new() { Activity = activities[9], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 18, Pending = false },
            new() { Activity = activities[9], DateTime = AddRandomTime(nextWednesday), ReservedPlaces = 20, Pending = false },
            new() { Activity = activities[10], DateTime = AddRandomTime(fiveDaysAgo), ReservedPlaces = 22, Pending = false },
            new() { Activity = activities[10], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 23, Pending = false },
            new() { Activity = activities[10], DateTime = AddRandomTime(nextThursday), ReservedPlaces = 21, Pending = false },
            new() { Activity = activities[11], DateTime = AddRandomTime(eighteenDaysAgo), ReservedPlaces = 19, Pending = false },
            new() { Activity = activities[11], DateTime = AddRandomTime(nextFriday), ReservedPlaces = 17, Pending = false },

            // Estudio de Danza (Capacidad Máxima: 20)
            new() { Activity = activities[12], DateTime = AddRandomTime(twentyFiveDaysAgo), ReservedPlaces = 14, Pending = false },
            new() { Activity = activities[12], DateTime = AddRandomTime(nextSaturday), ReservedPlaces = 16, Pending = false },
            new() { Activity = activities[13], DateTime = AddRandomTime(tenDaysAgo), ReservedPlaces = 15, Pending = false },
            new() { Activity = activities[13], DateTime = AddRandomTime(tomorrow), ReservedPlaces = 13, Pending = false },
            new() { Activity = activities[14], DateTime = AddRandomTime(fifteenDaysAgo), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[14], DateTime = AddRandomTime(nextMondayPlusOneWeek), ReservedPlaces = 14, Pending = false },

            // Sala de Música (Capacidad Máxima: 15)
            new() { Activity = activities[15], DateTime = AddRandomTime(twentyDaysAgo), ReservedPlaces = 11, Pending = false },
            new() { Activity = activities[15], DateTime = AddRandomTime(nextMonday), ReservedPlaces = 13, Pending = false },
            new() { Activity = activities[16], DateTime = AddRandomTime(eightDaysAgo), ReservedPlaces = 10, Pending = false },
            new() { Activity = activities[16], DateTime = AddRandomTime(nextTuesday), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[17], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 9, Pending = false },
            new() { Activity = activities[17], DateTime = AddRandomTime(nextWednesday), ReservedPlaces = 11, Pending = false },

            // Gimnasio (Capacidad Máxima: 50)
            new() { Activity = activities[18], DateTime = AddRandomTime(fiveDaysAgo), ReservedPlaces = 30, Pending = true },
            new() { Activity = activities[18], DateTime = AddRandomTime(nextThursday), ReservedPlaces = 38, Pending = true },
            new() { Activity = activities[19], DateTime = AddRandomTime(eighteenDaysAgo), ReservedPlaces = 32, Pending = true },
            new() { Activity = activities[19], DateTime = AddRandomTime(nextFriday), ReservedPlaces = 19, Pending = true },
            new() { Activity = activities[20], DateTime = AddRandomTime(twentyFiveDaysAgo), ReservedPlaces = 49, Pending = true },
            new() { Activity = activities[20], DateTime = AddRandomTime(nextSaturday), ReservedPlaces = 17, Pending = true },

            // Biblioteca (Capacidad Máxima: 40)
            new() { Activity = activities[21], DateTime = AddRandomTime(tenDaysAgo), ReservedPlaces = 33, Pending = false },
            new() { Activity = activities[21], DateTime = AddRandomTime(nextSunday), ReservedPlaces = 15, Pending = false },
            new() { Activity = activities[22], DateTime = AddRandomTime(fifteenDaysAgo), ReservedPlaces = 34, Pending = false },
            new() { Activity = activities[22], DateTime = AddRandomTime(nextMondayPlusOneWeek), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[23], DateTime = AddRandomTime(twentyDaysAgo), ReservedPlaces = 40, Pending = false },
            new() { Activity = activities[23], DateTime = AddRandomTime(nextMonday), ReservedPlaces = 11, Pending = false },

            // Cafetería (Capacidad Máxima: 60)
            new() { Activity = activities[24], DateTime = AddRandomTime(eightDaysAgo), ReservedPlaces = 58, Pending = false },
            new() { Activity = activities[24], DateTime = AddRandomTime(nextTuesday), ReservedPlaces = 20, Pending = false },
            new() { Activity = activities[25], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 52, Pending = false },
            new() { Activity = activities[25], DateTime = AddRandomTime(nextWednesday), ReservedPlaces = 21, Pending = false },
            new() { Activity = activities[26], DateTime = AddRandomTime(fiveDaysAgo), ReservedPlaces = 59, Pending = false },
            new() { Activity = activities[26], DateTime = AddRandomTime(nextThursday), ReservedPlaces = 17, Pending = false },

            // Zona de Juegos Acuáticos (Capacidad Máxima: 25)
            new() { Activity = activities[27], DateTime = AddRandomTime(eighteenDaysAgo), ReservedPlaces = 19, Pending = false },
            new() { Activity = activities[27], DateTime = AddRandomTime(nextFriday), ReservedPlaces = 16, Pending = false },
            new() { Activity = activities[28], DateTime = AddRandomTime(twentyFiveDaysAgo), ReservedPlaces = 25, Pending = false },
            new() { Activity = activities[28], DateTime = AddRandomTime(nextSaturday), ReservedPlaces = 13, Pending = false },
            new() { Activity = activities[29], DateTime = AddRandomTime(tenDaysAgo), ReservedPlaces = 17, Pending = false },
            new() { Activity = activities[29], DateTime = AddRandomTime(nextSunday), ReservedPlaces = 14, Pending = false },

            // Parque de Aventuras (Capacidad Máxima: 30)
            new() { Activity = activities[30], DateTime = AddRandomTime(fifteenDaysAgo), ReservedPlaces = 30, Pending = false },
            new() { Activity = activities[30], DateTime = AddRandomTime(nextMondayPlusOneWeek), ReservedPlaces = 13, Pending = false },
            new() { Activity = activities[31], DateTime = AddRandomTime(twentyDaysAgo), ReservedPlaces = 27, Pending = false },
            new() { Activity = activities[31], DateTime = AddRandomTime(nextMonday), ReservedPlaces = 12, Pending = false },
            new() { Activity = activities[32], DateTime = AddRandomTime(eightDaysAgo), ReservedPlaces = 29, Pending = false },
            new() { Activity = activities[32], DateTime = AddRandomTime(nextTuesday), ReservedPlaces = 7, Pending = false },

            // Mini Zoológico (Capacidad Máxima: 20)
            new() { Activity = activities[33], DateTime = AddRandomTime(twelveDaysAgo), ReservedPlaces = 20, Pending = false },
            new() { Activity = activities[33], DateTime = AddRandomTime(nextWednesday), ReservedPlaces = 18, Pending = false },
            new() { Activity = activities[34], DateTime = AddRandomTime(fiveDaysAgo), ReservedPlaces = 20, Pending = false },
            new() { Activity = activities[34], DateTime = AddRandomTime(nextThursday), ReservedPlaces = 7, Pending = false },
            new() { Activity = activities[35], DateTime = AddRandomTime(eighteenDaysAgo), ReservedPlaces = 19, Pending = false },
            new() { Activity = activities[35], DateTime = AddRandomTime(nextFriday), ReservedPlaces = 17, Pending = false }
        };

        foreach (var activityDate in activityDates)
        {
            var existingActivityDate = await context.Set<ActivityDate>()
                .FirstOrDefaultAsync(ad => ad.Activity.Id == activityDate.Activity.Id);

            if (existingActivityDate == null)
            {
                // Si no existe, agregarlo
                await activityDateRepository.AddAsync(activityDate);
            }
            else if (existingActivityDate.DateTime != activityDate.DateTime)
            {
                // Si existe pero la fecha es diferente, actualizar la fecha
                existingActivityDate.DateTime = activityDate.DateTime;
                context.Set<ActivityDate>().Update(existingActivityDate);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Fechas Activ. P
        // Obtener todas las actividades
        var allActivities = await context.Activity.ToListAsync();
        var allActivityDates = new List<ActivityDate>();

        foreach (var activity in allActivities)
        {
            // Generar un número aleatorio entre 1 y 20 para restar a la fecha actual
            int daysToSubtract = random.Next(1, 21);
            // Generar un número aleatorio entre 10 y 15 para los puestos reservados
            int reservedPlaces = random.Next(10, 16);

            // Crear una nueva instancia de ActivityDate con valores aleatorios
            allActivityDates.Add(new ActivityDate
            {
                Activity = activity,
                DateTime = AddRandomTime(today.AddDays(-daysToSubtract)),
                ReservedPlaces = reservedPlaces
            });
        }

        foreach (var activityDate in allActivityDates)
        {
            if (!await context.Set<ActivityDate>().AnyAsync(ad => ad.Activity.Id == activityDate.Activity.Id && ad.DateTime == activityDate.DateTime))
            {
                await activityDateRepository.AddAsync(activityDate);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Padres Manual
        // Crear usuarios normales
        var parents = new List<User>
        {
            //Personas normales
            new() { UserName = "AliciaM_01", Email = "alicia.moreno01@gmail.com", FirstName = "Alicia", LastName = "Moreno", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "RVerde_2023", Email = "roberto.verde2023@outlook.com", FirstName = "Roberto", LastName = "Verde", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "CBlanco_77", Email = "carlos.blanco77@yahoo.com", FirstName = "Carlos", LastName = "Blanco", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LauraNegro", Email = "laura.negro@live.com", FirstName = "Laura", LastName = "Negro", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LuisAzul_5", Email = "luis.azul5@gmail.com", FirstName = "Luis", LastName = "Azul", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "ElenaR_88", Email = "elena.rojo88@hotmail.com", FirstName = "Elena", LastName = "Rojo", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "F.Amarillo", Email = "fernando.amarillo@outlook.com", FirstName = "Fernando", LastName = "Amarillo", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "GGris_09", Email = "gabriela.gris09@gmail.com", FirstName = "Gabriela", LastName = "Gris", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "HugoCastaño", Email = "hugo.castano@live.com", FirstName = "Hugo", LastName = "Castaño", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "IsabelV_123", Email = "isabel.violeta123@yahoo.com", FirstName = "Isabel", LastName = "Violeta", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "JavierT_01", Email = "javier.turquesa01@gmail.com", FirstName = "Javier", LastName = "Turquesa", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "KarlaLila", Email = "karla.lila@outlook.com", FirstName = "Karla", LastName = "Lila", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LeoDorado", Email = "leonardo.dorado@hotmail.com", FirstName = "Leonardo", LastName = "Dorado", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "MartaPlata", Email = "marta.plata@gmail.com", FirstName = "Marta", LastName = "Plata", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "NicoBronce", Email = "nicolas.bronce@outlook.com", FirstName = "Nicolás", LastName = "Bronce", EmailConfirmed = true, Rol = parentRole! },

            //Personajes de Arcane
            new() { UserName = "Vi_Zaun", Email = "vi_zaun@leagueoflegends.com", FirstName = "Vi", LastName = "Zaun", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Jinx.LoL", Email = "jinx@leagueoflegends.com", FirstName = "Jinx", LastName = "Zaun", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "CaitPilt", Email = "caitlyn@piltover.com", FirstName = "Caitlyn", LastName = "Piltover", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "JaycePilt0ver", Email = "jaycep@leagueoflegends.com", FirstName = "Jayce", LastName = "Piltover", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "ViktorHex", Email = "viktor.hex@league.com", FirstName = "Viktor", LastName = "Hex", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "EkkoBoy", Email = "ekko@piltover.com", FirstName = "Ekko", LastName = "Zaun", EmailConfirmed = true, Rol = parentRole! },

             // Personajes de Hazbin Hotel y Helluva Boss
            new() { UserName = "CharlieMorn", Email = "charlie.morningstar@hazbin.com", FirstName = "Charlie", LastName = "Morningstar", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LuciferMS", Email = "lucifer.morningstar@hell.com", FirstName = "Lucifer", LastName = "Morningstar", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "VaggieAngel", Email = "vaggie@hazbin.com", FirstName = "Vaggie", LastName = "Angel", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Husk_Bar", Email = "husk.bar@demonmail.com", FirstName = "Husk", LastName = "Bartender", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Niffty_Maid", Email = "niffty.maid@hellmail.com", FirstName = "Niffty", LastName = "Demon", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "SirPent_SL", Email = "sir.pentious@demonmail.com", FirstName = "Sir Pentious", LastName = "Snake", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "CherriBBomb", Email = "cherri.bomb@hellmail.com", FirstName = "Cherri", LastName = "Bomb", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "KatieKill", Email = "katie.killjoy@newshell.com", FirstName = "Katie", LastName = "Killjoy", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Tom_Trench", Email = "tom.trench@newshell.com", FirstName = "Tom", LastName = "Trench", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Blitzo.Imp", Email = "blitzo@helluvaboss.com", FirstName = "Blitzo", LastName = "Imp", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Stolas.Arc", Email = "stoliz@helluvabosstopship.com", FirstName = "Stolas", LastName = "Arc Goetia", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Angel.Dust", Email = "angel.dust@hazbin.com", FirstName = "Angel", LastName = "Dust", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "AlastorRadio", Email = "alastor@hazbin.com", FirstName = "Alastor", LastName = "Radio Demon", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "Moxxie.Hell", Email = "moxxie@helluvaboss.com", FirstName = "Moxxie", LastName = "Imp", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LoonaHound", Email = "loona.hound@live.com", FirstName = "Loona", LastName = "HellHound", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "ViaDaddyIssues", Email = "via.arcgoetia@hell.com", FirstName = "Octavia", LastName = "Arc Goetia", EmailConfirmed = true, Rol = parentRole! },

            // 7 Pecados Capitales de Helluva Boss 
            new() { UserName = "SatanWrathSin", Email = "SatanWrath.sin@hell.com", FirstName = "Satan", LastName = "Wrath", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "MammonGreedLord", Email = "MammonGreed.sin@hell.com", FirstName = "Mammon", LastName = "Greed", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "SlothDreamer", Email = "BelphegorSloth.dreamer@limbo.com", FirstName = "Belphegor", LastName = "Sloth", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "LeviathanEnvySnake", Email = "LeviathanEnvy.snake@hell.com", FirstName = "Leviathan", LastName = "Envy", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "BeelzebubGluttonyDevour", Email = "BeelzebubGluttony.devour@hell.com", FirstName = "Beelzebub", LastName = "Gluttony", EmailConfirmed = true, Rol = parentRole! },
            new() { UserName = "AsmodeusLustDesire", Email = "asmodeusxfizzarolli@ship.com", FirstName = "Asmodeus", LastName = "Lust", EmailConfirmed = true, Rol = parentRole! }
        };

        foreach (var parent in parents)
        {
            if (userManager.Users.All(u => u.UserName != parent.UserName))
            {
                await userManager.CreateAsync(parent, "Contraseña123!");
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Obtener Padres
        // Personas normales
        var padre1 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "AliciaM_01");
        var padre2 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "RVerde_2023");
        var padre3 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "CBlanco_77");
        var padre4 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LauraNegro");
        var padre5 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LuisAzul_5");
        var padre6 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "ElenaR_88");
        var padre7 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "F.Amarillo");
        var padre8 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "GGris_09");
        var padre9 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "HugoCastaño");
        var padre10 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "IsabelV_123");
        var padre11 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "JavierT_01");
        var padre12 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "KarlaLila");
        var padre13 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LeoDorado");
        var padre14 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "MartaPlata");
        var padre15 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "NicoBronce");

        // Personajes de Arcane
        var padre16 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Vi_Zaun");
        var padre17 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Jinx.LoL");
        var padre18 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "CaitPilt");
        var padre19 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "JaycePilt0ver");
        var padre20 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "ViktorHex");
        var padre21 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "EkkoBoy");

        // Personajes de Hazbin Hotel y Helluva Boss
        var padre22 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "CharlieMorn");
        var padre23 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LuciferMS");
        var padre24 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "VaggieAngel");
        var padre25 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Husk_Bar");
        var padre26 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Niffty_Maid");
        var padre27 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "SirPent_SL");
        var padre28 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "CherriBBomb");
        var padre29 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "KatieKill");
        var padre30 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Tom_Trench");
        var padre31 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Blitzo.Imp");
        var padre32 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Stolas.Arc");
        var padre33 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Angel.Dust");
        var padre34 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "AlastorRadio");
        var padre35 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "Moxxie.Hell");
        var padre36 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LoonaHound");
        var padre37 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "ViaDaddyIssues");

        // 7 Pecados Capitales de Helluva Boss
        var padre38 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "SatanWrathSin");
        var padre39 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "MammonGreedLord");
        var padre40 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "SlothDreamer");
        var padre41 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "LeviathanEnvySnake");
        var padre42 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "BeelzebubGluttonyDevour");
        var padre43 = await context.Users.FirstOrDefaultAsync(u => u.UserName == "AsmodeusLustDesire");
        #endregion

        #region Activ. nuevas
        // Crear nuevas actividades que nunca hayan ocurrido
        var newActivities = new List<Activity>
        {
            new() { Name = "Clase de Cerámica", Description = "Aprender a moldear cerámica.", Educator = educador1!, Type = "Arte", RecommendedAge = 10, ItsPrivate = false, Facility = tallerArte! },
            new() { Name = "Taller de Escultura", Description = "Crear esculturas con diferentes materiales.", Educator = educador2!, Type = "Arte", RecommendedAge = 12, ItsPrivate = false, Facility = tallerArte! },
            new() { Name = "Clase de Natación Avanzada", Description = "Mejorar técnicas de natación.", Educator = educador3!, Type = "Deporte", RecommendedAge = 15, ItsPrivate = false, Facility = piscina! },
            new() { Name = "Taller de Astronomía", Description = "Explorar el universo.", Educator = educador4!, Type = "Ciencia", RecommendedAge = 14, ItsPrivate = false, Facility = laboratorioCiencias! },
            new() { Name = "Clase de Teatro", Description = "Aprender actuación y expresión.", Educator = educador5!, Type = "Arte", RecommendedAge = 13, ItsPrivate = false, Facility = estudioDanza! },
            new() { Name = "Taller de Cocina Internacional", Description = "Cocinar platos de todo el mundo.", Educator = educador6!, Type = "Cocina", RecommendedAge = 16, ItsPrivate = false, Facility = cafeteria! }
        };

        // Guardar las nuevas actividades en la base de datos
        foreach (var activity in newActivities)
        {
            await context.Set<Activity>().AddAsync(activity);
        }
        await unitOfWork.CommitAsync();

        // Fechas futuras para las nuevas actividades
        var newActivityDates = new List<ActivityDate>
        {
            new() { Activity = newActivities[0], DateTime = AddRandomTime(today.AddDays(2)), ReservedPlaces = 10 },
            new() { Activity = newActivities[1], DateTime = AddRandomTime(today.AddDays(5)), ReservedPlaces = 12 },
            new() { Activity = newActivities[2], DateTime = AddRandomTime(today.AddDays(6)), ReservedPlaces = 15 },
            new() { Activity = newActivities[3], DateTime = AddRandomTime(today.AddDays(3)), ReservedPlaces = 8 },
            new() { Activity = newActivities[4], DateTime = AddRandomTime(today.AddDays(4)), ReservedPlaces = 14 },
            new() { Activity = newActivities[5], DateTime = AddRandomTime(today.AddDays(7)), ReservedPlaces = 20 }
        };

        // Aquí nos aseguramos que la fecha siempre sea futura
        foreach (var newActivityDate in newActivityDates)
        {
            var existingActivityDate = await context.Set<ActivityDate>()
                .FirstOrDefaultAsync(ad => ad.Activity.Id == newActivityDate.Activity.Id && ad.DateTime == newActivityDate.DateTime);

            if (existingActivityDate == null)
            {
                // Si no existe, agregarlo
                await activityDateRepository.AddAsync(newActivityDate);
            }
            else if (existingActivityDate.DateTime != newActivityDate.DateTime)
            {
                // Si existe pero la fecha es diferente, actualizar la fecha
                existingActivityDate.DateTime = newActivityDate.DateTime;
                context.Set<ActivityDate>().Update(existingActivityDate);
            }
        }

        await unitOfWork.CommitAsync();
        #endregion

        #region Activ. Privadas
        // Crear actividades privadas
        var privateActivities = new List<Activity>
        {
            new() { Name = "Fiesta de Cumpleaños", Description = "Celebración de cumpleaños privada.", Educator = educador1!, Type = "Celebración", RecommendedAge = 10, ItsPrivate = true, Facility = cafeteria! },
            new() { Name = "Fiesta de 15 Años", Description = "Celebración de quinceañera privada.", Educator = educador2!, Type = "Celebración", RecommendedAge = 15, ItsPrivate = true, Facility = estudioDanza! },
            new() { Name = "Fiesta de Fin de Curso", Description = "Celebración de fin de curso escolar.", Educator = educador3!, Type = "Celebración", RecommendedAge = 12, ItsPrivate = true, Facility = gimnasio! },
            new() { Name = "Reunión Familiar", Description = "Reunión familiar privada.", Educator = educador4!, Type = "Reunión", RecommendedAge = 1, ItsPrivate = true, Facility = parqueAventuras! },
            new() { Name = "Fiesta de Graduación", Description = "Celebración de graduación privada.", Educator = educador5!, Type = "Celebración", RecommendedAge = 15, ItsPrivate = true, Facility = salaMusica! }
        };

        // Guardar las nuevas actividades privadas en la base de datos
        foreach (var activity in privateActivities)
        {
            await context.Set<Activity>().AddAsync(activity);
        }
        await unitOfWork.CommitAsync();

        // Crear fechas para actividades privadas
        var privateActivityDates = new List<ActivityDate>
        {
            // Fechas pasadas
            new() { Activity = privateActivities[0], DateTime = AddRandomTime(today.AddDays(-22)), ReservedPlaces = 30 },
            new() { Activity = privateActivities[1], DateTime = AddRandomTime(today.AddDays(-17)), ReservedPlaces = 20 },
            new() { Activity = privateActivities[2], DateTime = AddRandomTime(today.AddDays(-8)), ReservedPlaces = 40 },
            new() { Activity = privateActivities[3], DateTime = AddRandomTime(today.AddDays(-8)), ReservedPlaces = 35 },

            // Fechas futuras
            new() { Activity = privateActivities[4], DateTime = AddRandomTime(today.AddDays(1)), ReservedPlaces = 15 },
            new() { Activity = privateActivities[0], DateTime = AddRandomTime(today.AddDays(7)), ReservedPlaces = 30 },
            new() { Activity = privateActivities[1], DateTime = AddRandomTime(today.AddDays(9)), ReservedPlaces = 20 }
        };

        // Asegurarse de que las fechas sean correctas
        foreach (var privateActivityDate in privateActivityDates)
        {
            var existingActivityDate = await context.Set<ActivityDate>()
                .FirstOrDefaultAsync(ad => ad.Activity.Id == privateActivityDate.Activity.Id && ad.DateTime == privateActivityDate.DateTime);

            if (existingActivityDate == null)
            {
                // Si no existe, agregarlo
                await activityDateRepository.AddAsync(privateActivityDate);
            }
            else if (existingActivityDate.DateTime != privateActivityDate.DateTime)
            {
                // Si existe pero la fecha es diferente, actualizar la fecha
                existingActivityDate.DateTime = privateActivityDate.DateTime;
                context.Set<ActivityDate>().Update(existingActivityDate);
            }
        }

        await unitOfWork.CommitAsync();
        #endregion

        #region Reservas Auto
        // Crear reservas ajustadas
        var allParents = await context.Users.Where(u => u.Rol == parentRole).ToListAsync();
        var reservations = new List<Reservation>();

        // Array de comentarios adicionales
        var additionalComments = new[]
        {
            "¡Esperamos con ansias esta actividad!",
            "Mis hijos están muy emocionados.",
            "¡No podemos esperar para participar!",
            "Espero que sea una gran experiencia.",
            "Mis hijos han estado hablando de esto durante semanas.",
            "¡Qué gran oportunidad para aprender!",
            "Estamos muy emocionados por esta actividad.",
            "Espero que sea tan divertido como parece.",
            "Mis hijos están listos para la aventura.",
            "¡Gracias por organizar esto!",

            // Nuevos comentarios
            "Esta actividad suena increíble.",
            "Mis hijos no pueden dejar de hablar de esto.",
            "¡Estamos contando los días!",
            "Espero que sea una experiencia inolvidable.",
            "Mis hijos están ansiosos por comenzar.",
            "¡Qué oportunidad tan maravillosa!",
            "Estamos muy entusiasmados con esta actividad.",
            "Espero que sea tan educativo como parece.",
            "Mis hijos están preparados para la diversión.",
            "¡Gracias por hacer esto posible!",

            // Más comentarios
            "No podemos esperar para ver qué nos espera.",
            "Mis hijos están llenos de energía para esto.",
            "¡Estamos muy emocionados por participar!",
            "Espero que sea una experiencia enriquecedora.",
            "Mis hijos han estado esperando esto con ansias.",
            "¡Qué excelente oportunidad para crecer!",
            "Estamos muy emocionados por esta nueva aventura.",
            "Espero que sea tan emocionante como parece.",
            "Mis hijos están listos para aprender y divertirse.",
            "¡Gracias por brindarnos esta oportunidad!",

            // Comentarios adicionales
            "Mis hijos están contando los días.",
            "¡Estamos muy emocionados por esta experiencia!",
            "Espero que sea tan increíble como parece.",
            "Mis hijos están listos para la acción.",
            "¡Gracias por organizar una actividad tan genial!",
            "Estamos muy emocionados por lo que viene.",
            "Espero que sea una experiencia memorable.",
            "Mis hijos están ansiosos por participar.",
            "¡Qué gran oportunidad para explorar!",
            "Estamos muy emocionados por esta oportunidad.",
            "Espero que sea tan divertido como lo imaginamos.",
            "Mis hijos están listos para la aventura.",
            "¡Gracias por hacer esto posible para nosotros!",
            "Estamos muy emocionados por lo que nos espera.",
            "Espero que sea una experiencia educativa.",
            "Mis hijos están ansiosos por aprender.",
            "¡Qué gran oportunidad para descubrir!",
            "Estamos muy emocionados por esta nueva experiencia.",
            "Espero que sea tan emocionante como lo parece.",
            "Mis hijos están listos para disfrutar."
        };
        foreach (var activityDate in activityDates.Concat(newActivityDates).Concat(privateActivityDates))
        {
            int totalReserved = reservations.Where(r => r.ActivityDate.Id == activityDate.Id).Sum(r => r.AmmountOfChildren);

            while (totalReserved < activityDate.ReservedPlaces)
            {
                int children = random.Next(1, Math.Min(6, activityDate.ReservedPlaces - totalReserved) + 1);
                var parent = allParents[random.Next(allParents.Count)];
                var reservationState = activityDate.DateTime < today ? "Completada" : "Confirmada";
                var comment = additionalComments[random.Next(additionalComments.Length)];

                reservations.Add(new Reservation
                {
                    Parent = parent,
                    ActivityDate = activityDate,
                    AdditionalComments = comment,
                    AmmountOfChildren = children,
                    ReservationState = reservationState
                });

                totalReserved += children;
            }
        }

        foreach (var reservation in reservations)
        {
            if (!await context.Reservation.AnyAsync(r => r.AdditionalComments == reservation.AdditionalComments && r.ActivityDate.Id == reservation.ActivityDate.Id))
            {
                await reservationRepository.AddAsync(reservation);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion

        #region Reseñas Auto
        //Crear reseñas automáticas

        // Listas de comentarios
        var goodComments = new List<string>
        {
            "Una experiencia increíble.",
            "Muy educativo y divertido.",
            "Mis hijos lo disfrutaron mucho.",
            "Excelente organización.",
            "Aprendimos mucho y nos divertimos.",
            "El personal fue muy amable y servicial.",
            "Las actividades fueron muy bien planificadas.",
            "Un ambiente seguro y acogedor.",
            "Definitivamente volveremos.",
            "Los niños aprendieron nuevas habilidades.",
            "Fue un día lleno de risas y aprendizaje.",
            "La atención al detalle fue impresionante.",
            "Superó nuestras expectativas.",
            "Un evento bien coordinado.",
            "Los instructores fueron muy profesionales.",

            // Nuevos comentarios positivos
            "Una experiencia que recordaremos siempre.",
            "Todo estuvo perfectamente organizado.",
            "Mis hijos hicieron nuevos amigos.",
            "El lugar estaba impecable.",
            "Las actividades fueron innovadoras y creativas.",
            "Nos sentimos muy bienvenidos.",
            "La seguridad fue una prioridad evidente.",
            "Los niños estaban encantados con todo.",
            "Fue un día lleno de descubrimientos.",
            "La calidad del evento fue excepcional.",
            "Nos encantó cada momento.",
            "Los niños no querían irse.",
            "Fue una experiencia enriquecedora.",
            "El personal fue excepcionalmente atento.",
            "Definitivamente lo recomendaría a otros."
        };

        var badComments = new List<string>
        {
            "No fue lo que esperaba.",
            "Podría haber sido mejor.",
            "Mis hijos no se divirtieron.",
            "La organización fue deficiente.",
            "No aprendimos mucho.",
            "El personal no fue muy atento.",
            "Las actividades no estaban bien planificadas.",
            "El ambiente no era muy acogedor.",
            "No creo que volvamos.",
            "Los niños no aprendieron nada nuevo.",
            "Fue un día aburrido y sin aprendizaje.",
            "La atención al detalle fue pobre.",
            "No cumplió con nuestras expectativas.",
            "El evento fue mal coordinado.",
            "Los instructores no parecían profesionales.",

            // Nuevos comentarios negativos
            "El lugar estaba desordenado.",
            "Las actividades fueron repetitivas.",
            "No fue divertido para los niños.",
            "El evento comenzó tarde.",
            "No había suficiente espacio para todos.",
            "La comida no fue buena.",
            "El equipo no estaba bien mantenido.",
            "No había suficiente información disponible.",
            "El sonido era demasiado alto.",
            "Las instrucciones no fueron claras.",
            "El personal parecía desinteresado.",
            "No había suficientes actividades para los niños.",
            "El lugar estaba demasiado lleno.",
            "No había suficiente sombra en el área exterior.",
            "El evento terminó antes de lo esperado."
        };

        // Crear reseñas para cada ActivityDate pasada
        var pastActivityDates = await context.Set<ActivityDate>().Where(ad => ad.DateTime < today).ToListAsync();

        // Obtener todas las reservas para las fechas de actividades pasadas
        var reservationsDone = await context.Set<Reservation>()
            .Where(r => pastActivityDates.Select(ad => ad.Id).Contains(r.ActivityDate.Id))
            .ToListAsync();

        // Crear reseñas para cada ActivityDate pasada
        var Reviews = new List<(User? Parent, string Comments, int Score, int ActivityIndex)>();


        foreach (var activityDate in pastActivityDates)
        {
            // Filtrar padres que hicieron reserva para esta actividad
            var parentsWithReservations = reservationsDone
                .Where(r => r.ActivityDate.Id == activityDate.Id)
                .Select(r => r.Parent)
                .Distinct()
                .ToList();

            for (int i = 0; i < 12; i++)
            {
                if (parentsWithReservations.Count == 0)
                    break; // Si no hay padres con reservas, salir del bucle

                // Seleccionar un padre aleatoriamente de los que hicieron reserva
                var parent = parentsWithReservations[random.Next(parentsWithReservations.Count)];

                // Verificar si ya existe una reseña de este padre para esta actividad
                if (Reviews.Any(ar => ar.Parent == parent && ar.ActivityIndex == pastActivityDates.IndexOf(activityDate)))
                    continue; // Si ya existe, continuar con el siguiente

                // Decidir si usar comentarios buenos o malos
                bool useGoodComments = i % 2 == 0 || random.Next(2) == 0;

                // Seleccionar un comentario aleatoriamente
                string comment = useGoodComments
                    ? goodComments[random.Next(goodComments.Count)]
                    : badComments[random.Next(badComments.Count)];

                // Asignar un puntaje basado en el tipo de comentario
                int score = useGoodComments ? random.Next(4, 6) : random.Next(1, 4);

                // Agregar la reseña a la lista
                Reviews.Add((parent, comment, score, pastActivityDates.IndexOf(activityDate)));

                // Remover el padre de la lista para evitar duplicados
                parentsWithReservations.Remove(parent);
            }
        }

        foreach (var review in Reviews)
        {
            // Verificar si ya existe una reseña en la base de datos
            if (!await context.Set<Review>().AnyAsync(r => r.Parent == review.Parent && r.ActivityDate.Id == pastActivityDates[review.ActivityIndex].Id))
            {
                var newReview = new Review
                {
                    Parent = review.Parent!,
                    Comments = review.Comments,
                    Score = review.Score,
                    ActivityDate = pastActivityDates[review.ActivityIndex]
                };

                await context.Set<Review>().AddAsync(newReview);
            }
        }

        await unitOfWork.CommitAsync();

        #endregion

        #region Recursos Manual
        // Crear recursos
        var resources = new List<Resource>
        {
            new() { Name = "Silla", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = tallerArte! },
            new() { Name = "Mesa", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = tallerArte! },
            new() { Name = "Vasos", Type = "Utensilios", ResourceCondition = "Roto", Facility = piscina! },
            new() { Name = "Jugo", Type = "Bebida", ResourceCondition = "Bueno", Facility = piscina! },
            new() { Name = "Pinturas", Type = "Material", ResourceCondition = "Roto", Facility = tallerArte! },
            new() { Name = "Salvavidas", Type = "Equipo de seguridad", ResourceCondition = "Bueno", Facility = piscina! },
            new() { Name = "Microscopio", Type = "Equipo", ResourceCondition = "Bueno", Facility = laboratorioCiencias! },
            new() { Name = "Pipetas", Type = "Material", ResourceCondition = "Roto", Facility = laboratorioCiencias! },
            new() { Name = "Lienzos", Type = "Material", ResourceCondition = "Roto", Facility = tallerArte! },
            new() { Name = "Bocinas", Type = "Equipo", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Piano", Type = "Instrumento", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Guitarra", Type = "Instrumento", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Batería", Type = "Instrumento", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Pesas", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Cinta de correr", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Bicicleta estática", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Libros", Type = "Material de lectura", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Computadora", Type = "Equipo", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Proyector", Type = "Equipo", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Cafetera", Type = "Electrodoméstico", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Horno", Type = "Electrodoméstico", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Refrigerador", Type = "Electrodoméstico", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Tobogán", Type = "Equipo de Juego", ResourceCondition = "Bueno", Facility = zonaJuegosAcuaticos! },
            new() { Name = "Cubo de Agua", Type = "Equipo de Juego", ResourceCondition = "Bueno", Facility = zonaJuegosAcuaticos! },
            new() { Name = "Rociadores", Type = "Equipo de Juego", ResourceCondition = "Bueno", Facility = zonaJuegosAcuaticos! },
            new() { Name = "Arnés de Seguridad", Type = "Equipo de Seguridad", ResourceCondition = "Roto", Facility = parqueAventuras! },
            new() { Name = "Cuerda de Escalada", Type = "Equipo de Juego", ResourceCondition = "Bueno", Facility = parqueAventuras! },
            new() { Name = "Puente Colgante", Type = "Equipo de Juego", ResourceCondition = "Bueno", Facility = parqueAventuras! },
            new() { Name = "Jaula de Conejos", Type = "Habitat", ResourceCondition = "Bueno", Facility = miniZoologico! },
            new() { Name = "Jaula de Aves", Type = "Habitat", ResourceCondition = "Bueno", Facility = miniZoologico! },
            new() { Name = "Jaula de Reptiles", Type = "Habitat", ResourceCondition = "Bueno", Facility = miniZoologico! },
            new() { Name = "Caballetes", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = tallerArte! },
            new() { Name = "Pinceles", Type = "Material", ResourceCondition = "Roto", Facility = tallerArte! },
            new() { Name = "Paletas de Pintura", Type = "Material", ResourceCondition = "Roto", Facility = tallerArte! },
            new() { Name = "Flotadores", Type = "Equipo de seguridad", ResourceCondition = "Bueno", Facility = piscina! },
            new() { Name = "Gorros de Natación", Type = "Accesorio", ResourceCondition = "Roto", Facility = piscina! },
            new() { Name = "Gafas de Natación", Type = "Accesorio", ResourceCondition = "Roto", Facility = piscina! },
            new() { Name = "Tizas", Type = "Material", ResourceCondition = "Roto", Facility = laboratorioCiencias! },
            new() { Name = "Probetas", Type = "Material", ResourceCondition = "Roto", Facility = laboratorioCiencias! },
            new() { Name = "Tableros de Dibujo", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = laboratorioCiencias! },
            new() { Name = "Espejos", Type = "Equipo", ResourceCondition = "Bueno", Facility = estudioDanza! },
            new() { Name = "Barras de Ballet", Type = "Equipo", ResourceCondition = "Bueno", Facility = estudioDanza! },
            new() { Name = "Vestuario", Type = "Ropa", ResourceCondition = "Bueno", Facility = estudioDanza! },
            new() { Name = "Altavoces", Type = "Equipo", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Micrófonos", Type = "Equipo", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Atriles", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = salaMusica! },
            new() { Name = "Colchonetas", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Balones de Yoga", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Bandas Elásticas", Type = "Equipo de ejercicio", ResourceCondition = "Bueno", Facility = gimnasio! },
            new() { Name = "Sillas de Lectura", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Mesas de Estudio", Type = "Mobiliario", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Lámparas de Lectura", Type = "Equipo", ResourceCondition = "Bueno", Facility = biblioteca! },
            new() { Name = "Tazas", Type = "Utensilios", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Platos", Type = "Utensilios", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Cubiertos", Type = "Utensilios", ResourceCondition = "Bueno", Facility = cafeteria! },
            new() { Name = "Caballetes Viejos", Type = "Mobiliario", ResourceCondition = "Deteriorado", Facility = tallerArte! },
            new() { Name = "Pinceles Usados", Type = "Material", ResourceCondition = "Deteriorado", Facility = tallerArte! },
            new() { Name = "Flotadores Desgastados", Type = "Equipo de seguridad", ResourceCondition = "Deteriorado", Facility = piscina! },
            new() { Name = "Gorros de Natación Viejos", Type = "Accesorio", ResourceCondition = "Deteriorado", Facility = piscina! },
            new() { Name = "Microscopios Antiguos", Type = "Equipo", ResourceCondition = "Deteriorado", Facility = laboratorioCiencias! },
            new() { Name = "Probetas Ralladas", Type = "Material", ResourceCondition = "Deteriorado", Facility = laboratorioCiencias! },
            new() { Name = "Espejos Rotos", Type = "Equipo", ResourceCondition = "Deteriorado", Facility = estudioDanza! },
            new() { Name = "Barras de Ballet Oxidadas", Type = "Equipo", ResourceCondition = "Deteriorado", Facility = estudioDanza! },
            new() { Name = "Altavoces Antiguos", Type = "Equipo", ResourceCondition = "Deteriorado", Facility = salaMusica! },
            new() { Name = "Micrófonos Viejos", Type = "Equipo", ResourceCondition = "Deteriorado", Facility = salaMusica! },
            new() { Name = "Colchonetas Desgastadas", Type = "Equipo de ejercicio", ResourceCondition = "Deteriorado", Facility = gimnasio! },
            new() { Name = "Balones de Yoga Viejos", Type = "Equipo de ejercicio", ResourceCondition = "Deteriorado", Facility = gimnasio! },
            new() { Name = "Sillas de Lectura Viejas", Type = "Mobiliario", ResourceCondition = "Deteriorado", Facility = biblioteca! },
            new() { Name = "Mesas de Estudio Desgastadas", Type = "Mobiliario", ResourceCondition = "Deteriorado", Facility = biblioteca! },
            new() { Name = "Tazas Ralladas", Type = "Utensilios", ResourceCondition = "Deteriorado", Facility = cafeteria! },
            new() { Name = "Platos Viejos", Type = "Utensilios", ResourceCondition = "Deteriorado", Facility = cafeteria! },
            new() { Name = "Cubiertos Desgastados", Type = "Utensilios", ResourceCondition = "Deteriorado", Facility = cafeteria! },
            new() { Name = "Tobogán Desgastado", Type = "Equipo de Juego", ResourceCondition = "Deteriorado", Facility = zonaJuegosAcuaticos! },
            new() { Name = "Cuerda de Escalada Vieja", Type = "Equipo de Juego", ResourceCondition = "Deteriorado", Facility = parqueAventuras! },
            new() { Name = "Jaula de Conejos Deteriorada", Type = "Habitat", ResourceCondition = "Deteriorado", Facility = miniZoologico! }
        };


        foreach (var resource in resources)
        {
            if (!await context.Set<Resource>().AnyAsync(r => r.Name == resource.Name))
            {
                await resourceRepository.AddAsync(resource);
            }
        }
        await unitOfWork.CommitAsync();
        #endregion


        #region Frec. Uso Recurso
        var resourcesDate = new List<ResourceDate>();

        foreach (var resource in resources)
        {
            var randomDate = randomDateGenerator();
            var randomUseFrequency = random.Next(1, 101); // Genera un número entre 1 y 100
            
            resourcesDate.Add(new ResourceDate
            {
                Resource = resource,
                Date = randomDate,
                UseFrequency = randomUseFrequency
            });
        }

        foreach(var resourceDate in resourcesDate)
        {
            if(!await context.Set<ResourceDate>().AnyAsync(rd => rd.Id == resourceDate.Id))
            {
                await resourceDateRepository.AddAsync(resourceDate);
            }
        }

        var useFrequency = 0;
        //guardar en Resources las frecuencias de uso
        foreach(var resource in resources)
        {
            useFrequency = resource.UseFrequency;
            foreach(var resourceDate in resourcesDate)
            {
                if(resourceDate.Resource.Id == resource.Id)
                {
                    useFrequency += resourceDate.UseFrequency;
                }
            }
            resource.UseFrequency = useFrequency;
            resourceRepository.Update(resource);
        }

        await unitOfWork.CommitAsync();

        // Método auxiliar para generar fechas aleatorias
        DateOnly randomDateGenerator()
        {
            var start = new DateOnly(2020, 1, 1);
            var end = DateOnly.FromDateTime(DateTime.Now);
            
            // Calcular la diferencia en días
            int daysBetween = end.DayNumber - start.DayNumber;
            
            // Generar una fecha aleatoria dentro del rango
            return start.AddDays(random.Next(daysBetween + 1));
        }

        #endregion
    }

    private static DateTime AddRandomTime(DateTime date)
    {
        var random = new Random();
        int hour = random.Next(9, 18); // Horas entre 9 AM y 5 PM
        int[] possibleMinutes = [0, 15, 30, 45];
        int minute = possibleMinutes[random.Next(possibleMinutes.Length)];
        // Establecer la hora y los minutos en el DateTime y asegurarse de que sea UTC
        return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0, DateTimeKind.Utc);
    }

}
