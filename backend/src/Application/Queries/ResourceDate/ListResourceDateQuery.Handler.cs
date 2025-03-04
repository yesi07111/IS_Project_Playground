using System.Runtime.Versioning;
using FastEndpoints;
using Playground.Application.Factories;
using Playground.Application.Queries.Dtos;
using Playground.Application.Queries.Responses;
using Playground.Application.Repositories;
using Playground.Application.Services;

namespace Playground.Application.Queries.ResourceDate;

public class ListResourceDateQueryHandler : CommandHandler<ListResourceDateQuery, ListResourceDateResponse>
{
    private readonly IRepositoryFactory _repositoryFactory;

    public ListResourceDateQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }
    public override async Task<ListResourceDateResponse> ExecuteAsync(ListResourceDateQuery query, CancellationToken ct = default)
    {
        var resourceDateRepository = _repositoryFactory.CreateRepository<Domain.Entities.ResourceDate>();
        var resourceDateDtos = new List<object>();
        IEnumerable<object> resourceDates = [];

        if (query.UseCase == "NameDayFreq")
        {
            resourceDates = await GetAllNameDayFreq(resourceDateRepository);
        }
        else if (query.UseCase == "NameFreqMost")
        {
            resourceDates = await GetThreeMostUsed(resourceDateRepository);
        }
        else if (query.UseCase == "NameFreqLess")
        {
            resourceDates = await GetThreeLessUsed(resourceDateRepository);
        }
        else
        {
            resourceDates = await resourceDateRepository.GetAllAsync(r => r.Resource);

            foreach (Domain.Entities.ResourceDate resource in resourceDates)
            {
                var resourceDto = new ResourceDateDto
                {
                    Id = resource.Resource.Id,
                    Name = resource.Resource.Name,
                    Date = resource.Date.ToDateTime(TimeOnly.MinValue),
                    UseFrequency = resource.UseFrequency,
                };
                resourceDateDtos.Add(resourceDto);
            }
            resourceDates = resourceDateDtos;
        }

        return new ListResourceDateResponse(resourceDates);
    }

    private async Task<IEnumerable<object>> GetAllNameDayFreq(IRepository<Domain.Entities.ResourceDate> resourceDateRepository)
    {
        var resourceDates = await resourceDateRepository.GetAllAsync(r => r.Resource);

        var result = resourceDates
        .GroupBy(r => r.Resource.Name)
        .Select(g => new
        {
            Name = g.Key, // Nombre del recurso
            Frequencies = g.OrderBy(r => r.Date) // Ordena por fecha ascendente
                .Select(r => new { r.Date, r.UseFrequency }) // Crea un objeto anónimo para (Date, UseFrequency)
        })
        .Cast<object>() // Convierte el resultado en IEnumerable<object>
        .ToList();

        return result;
    }

    private async Task<IEnumerable<object>> GetThreeMostUsed(IRepository<Domain.Entities.ResourceDate> resourceDateRepository)
    {
        var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));  // Fecha de hace un año

        var resourceDates = await resourceDateRepository.GetAllAsync(r => r.Resource);

        var result = resourceDates
            .Where(r => r.Date >= oneYearAgo) // Filtrar solo los registros del último año
            .GroupBy(r => r.Resource.Name)
            .Select(g => new
            {
                Name = g.Key, // Nombre del recurso
                TotalFrequency = g.Sum(r => r.UseFrequency) // Sumar la frecuencia de uso de todas las fechas dentro del último año
            })
            .OrderByDescending(r => r.TotalFrequency) // Ordenar por frecuencia total de mayor a menor
            .Take(3) // Tomar solo los tres recursos más utilizados
            .ToList(); // Convertir en lista

        return result.Select(r => new { r.Name, r.TotalFrequency }); // Seleccionar solo nombre y frecuencia total
    }

    private async Task<IEnumerable<object>> GetThreeLessUsed(IRepository<Domain.Entities.ResourceDate> resourceDateRepository)
    {
        var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));  // Fecha de hace un año

        var resourceDates = await resourceDateRepository.GetAllAsync(r => r.Resource);

        var result = resourceDates
            .Where(r => r.Date >= oneYearAgo) // Filtrar solo los registros del último año
            .GroupBy(r => r.Resource.Name)
            .Select(g => new
            {
                Name = g.Key, // Nombre del recurso
                TotalFrequency = g.Sum(r => r.UseFrequency) // Sumar la frecuencia de uso de todas las fechas dentro del último año
            })
            .OrderBy(r => r.TotalFrequency) // Ordenar por frecuencia total de mayor a menor
            .Take(3) // Tomar solo los tres recursos más utilizados
            .ToList(); // Convertir en lista

        return result.Select(r => new { r.Name, r.TotalFrequency }); // Seleccionar solo nombre y frecuencia total
    }
}