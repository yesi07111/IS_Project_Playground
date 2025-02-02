import React, { useState } from "react";
import { Box, Button, Grid2, Typography, Grid2 as Grid } from '@mui/material';
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler
} from 'chart.js';
import { ListResourceDateResponse, ListResourceResponse, Resource, ResourceDate } from "../interfaces/Resource";
import { resourceService } from "../services/resourceService";
import { cacheService } from "../services/cacheService";
import { BarsChart } from "../components/features/BarsChart";
import PiesChart from "../components/features/PiesChart";
import LinesChart from "../components/features/LinesChart";
import { ListReservationResponse } from "../interfaces/Reservation";
import { reservationService } from "../services/reservationService";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler
)


const StatisticsPage: React.FC = () => {
    //plots
    const [useFrequencyPlot, setUseFrequencyPlot] = useState(false);
    const [conditionPlot, setConditionPlot] = useState(false);
    const [useFrequencyDatePlot, setUseFrequencyDatePlot] = useState(false);
    const [resPerActivityPlot, setResPerActivityPlot] = useState(false);
    const [resPerAgeRangePlot, setResPerAgeRangePlot] = useState(false);
    const [lastYearTop3, setLastYearTop3] = useState(false);

    //plots de recursos
    const [resources, setResources] = useState<Resource[]>([]);
    const [resourceDates, setResourceDates] = useState<ResourceDate[]>([]);

    //plots de reservaciones
    const reservation: ListReservationResponse = {
        result: [
            {
                reservationId: '',
                firstName: '',
                lastName: '',
                userName: '',
                activityId: '',
                activityName: '',
                activityDate: '',
                comments: '',
                amount: 0,
                state: '',
                activityRecommendedAge: 0,
            }
        ]
    }
    const [reservations, setReservations] = useState<ListReservationResponse>(reservation);
    const states = ["pendiente", "confirmada", "completada", "cancelada"];

    // Extraer nombres de recursos y frecuencias de uso
    const resourceNames = getResourceNames(resources);
    const resourceUseFrequency = resources.map((resource) => resource.useFrequency);
    const resourceConditions = Array.from(new Set(resources.map(resource => resource.condition)));
    const frequency = fetchAllConditions(resources);
    const days: Date[] = Array.from(new Set(resourceDates.map(resourceDate => resourceDate.date)));
    days.sort((a, b) => new Date(a).getTime() - new Date(b).getTime());
    const resourceDateUseFrequency = processResourceData(resourceDates, days);

    const fetchAllResources = async () => {
        try {
            const response: ListResourceResponse = await resourceService.getAllResources([]);
            const resourcesArray: Resource[] = Array.isArray(response.result)
                ? response.result as Resource[]
                : [];

            console.table(resourcesArray);

            cacheService.saveResources(resourcesArray);
            setResources(resourcesArray);  // Guardamos los recursos en el estado

        } catch (error) {
            console.error('Error fetching resources:', error);
        }
    }

    const fetchAllResourceDates = async () => {
        try {
            const response: ListResourceDateResponse = await resourceService.getAllResourceDates()
            const resourcesArray: ResourceDate[] = Array.isArray(response.result)
                ? response.result as ResourceDate[]
                : [];

            console.table(resourcesArray);

            cacheService.saveResourceDates(resourcesArray);
            setResourceDates(resourcesArray);  // Guardamos los recursos en el est
        }
        catch (error) {
            console.error('Error fetching resources:', error);
        }
    }

    function getResourceNames(resources: Resource[]) {
        // 1. Agrupar recursos por ID
        const groupedResources: { [id: string]: { name: string } } = {};

        resources.forEach((resource) => {
            if (!groupedResources[resource.id]) {
                groupedResources[resource.id] = {
                    name: resource.name,
                };
            }
        });

        const resourceDateNames: string[] = [];

        Object.keys(groupedResources).forEach((id) => {
            const resource = groupedResources[id];
            resourceDateNames.push(resource.name);
        });

        return resourceDateNames;
    }

    function fetchAllConditions(resources: Resource[]) {
        const counts: { [key: string]: number } = {};

        resources.forEach(resource => {
            counts[resource.condition] = (counts[resource.condition] || 0) + 1;
        });

        const frequencies = Object.values(counts);

        return frequencies;
    }

    function processResourceData(resources: ResourceDate[], dates: Date[]): { resourceNames: string[]; resourceFrequencies: number[][] } {
        // 1. Agrupar recursos por ID
        const groupedResources: { [id: string]: { name: string; data: Map<string, number> } } = {};

        resources.forEach((resource) => {
            if (!groupedResources[resource.id]) {
                groupedResources[resource.id] = {
                    name: resource.name,
                    data: new Map(), // Usamos un Map para asociar fecha con frecuencia de uso
                };
            }
            // Guardamos la frecuencia de uso por fecha
            groupedResources[resource.id].data.set(resource.date.toString(), resource.useFrequency);
        });

        // 2. Construir el array de nombres y las frecuencias
        const resourceNames: string[] = [];
        const resourceFrequencies: number[][] = [];

        Object.keys(groupedResources).forEach((id) => {
            const resource = groupedResources[id];
            resourceNames.push(resource.name);

            // Crear un array de frecuencias basado en las fechas dadas
            const frequencies = dates.map((date) => {
                const dateKey = date.toString(); // Convertimos la fecha a string ISO
                return resource.data.get(dateKey) || 0; // Si no hay datos para esa fecha, asignar 0
            });

            resourceFrequencies.push(frequencies);
        });

        return { resourceNames, resourceFrequencies };
    }

    const lessAndMoreUsed = (resources: ResourceDate[]) => {
        const haceUnAño = new Date();
        haceUnAño.setFullYear(haceUnAño.getFullYear() - 1);

        // 1. Filtrar los registros del último año
        const resourcesLastYear = resources.filter(resource => new Date(resource.date) >= haceUnAño);
        console.log(resourcesLastYear);

        // 2. Agrupar por recurso y sumar frecuencia de uso
        const totalFreq = resourcesLastYear.reduce((acc, resource) => {
            acc[resource.name] = (acc[resource.name] || 0) + resource.useFrequency;
            return acc;
        }, {} as Record<string, number>);

        // 3. Convertir en array y ordenar de mayor a menor uso
        const resourcesSorted = Object.entries(totalFreq)
            .map(([name, totalFreq]) => ({ name, totalFreq }))
            .sort((a, b) => b.totalFreq - a.totalFreq);

        // 4. Seleccionar los más y menos utilizados
        const moreUsed = resourcesSorted.slice(0, 3); // Top 3
        const lessUsed = resourcesSorted.slice(-3); // Últimos 3

        return { moreUsed, lessUsed };
    };

    const { moreUsed, lessUsed } = lessAndMoreUsed(resourceDates);
    console.log("Recursos más utilizados:", moreUsed);
    console.log("Recursos menos utilizados:", lessUsed);

    //reservaciones
    const fetchReservations = async () => {
        try {
            const response = await reservationService.getAllReservations("");
            setReservations(response);
        } catch (error) {
            console.error('Error obteniendo las reservas:', error);
        }
    };

    const processReservationData = (reservations: ListReservationResponse) => {
        const groupedData = new Map<string, number>();

        reservations.result.forEach(res => {
            const key = `${res.activityName} - ${res.activityDate}`;
            groupedData.set(key, (groupedData.get(key) || 0) + res.amount);
        });

        const activityDates = Array.from(groupedData.keys()); // Nombre de actividad + fecha
        const reservFrequencies = Array.from(groupedData.values()); // Cantidad de reservas

        return { activityDates, reservFrequencies };
    };

    const { activityDates, reservFrequencies } = processReservationData(reservations);

    const getAgeRange = (age: number) => {
        if (age <= 3) return "Infantes (0-3)";
        if (age <= 6) return "Niños pequeños (4-6)";
        if (age <= 9) return "Niños grandes (7-9)";
        return "Pre-adolescentes (10-12)";
    };

    const processReservationData2 = (reservations: ListReservationResponse) => {
        const groupedData: Record<string, Map<string, number>> = {
            pendiente: new Map(),
            confirmada: new Map(),
            completada: new Map(),
            cancelada: new Map()
        };

        reservations.result.forEach(res => {
            const ageRange = getAgeRange(res.activityRecommendedAge);
            const state = res.state.toLowerCase(); // Normalizamos a minúsculas

            if (groupedData[state]) {
                groupedData[state].set(ageRange, (groupedData[state].get(ageRange) || 0) + res.amount);
            }
        });

        return groupedData;
    };

    const groupedReservations = processReservationData2(reservations);

    return (
        <Box
            sx={{
                minHeight: "100vh",
                minWidth: "100vw",
                display: "flex",
                backgroundColor: "#f8f9fa",
                overflowY: "hidden",
            }}
        >
            {/* Barra lateral */}
            <Box
                sx={{
                    width: "250px",
                    backgroundColor: "#ffffff",
                    boxShadow: "2px 0 5px rgba(0,0,0,0.1)",
                    display: "flex",
                    flexDirection: "column",
                    padding: 2,
                    gap: 2,
                    height: "100vh", // Hace que la barra lateral ocupe toda la altura
                    overflowY: "auto", // Permite el desplazamiento si hay muchos botones
                }}
            >
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setUseFrequencyPlot(!useFrequencyPlot);
                        setConditionPlot(false);
                        setUseFrequencyDatePlot(false);
                        setResPerActivityPlot(false);
                        setResPerAgeRangePlot(false);
                        setLastYearTop3(false);
                        fetchAllResources();
                    }}>
                    Frecuencia de uso de los recursos
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setConditionPlot(!conditionPlot);
                        setUseFrequencyPlot(false);
                        setUseFrequencyDatePlot(false);
                        setResPerActivityPlot(false);
                        setResPerAgeRangePlot(false);
                        setLastYearTop3(false);
                        fetchAllResources();
                    }}>
                    Condición de los recursos
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setUseFrequencyDatePlot(!useFrequencyDatePlot);
                        setConditionPlot(false);
                        setUseFrequencyPlot(false);
                        setResPerActivityPlot(false);
                        setResPerAgeRangePlot(false);
                        setLastYearTop3(false);
                        fetchAllResourceDates();
                    }}>
                    Frecuencia de uso de los recursos por fecha
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setLastYearTop3(!lastYearTop3);
                        setResPerAgeRangePlot(false);
                        setResPerActivityPlot(false);
                        setUseFrequencyDatePlot(false);
                        setConditionPlot(false);
                        setUseFrequencyPlot(false);
                        fetchAllResourceDates();
                    }}>
                    Top 3 Recursos más y menos usados en el ultimo año
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setResPerActivityPlot(!resPerActivityPlot);
                        setUseFrequencyDatePlot(false);
                        setConditionPlot(false);
                        setUseFrequencyPlot(false);
                        setResPerAgeRangePlot(false);
                        setLastYearTop3(false);
                        fetchReservations();
                    }}>
                    Frecuencia de Reservas por cada Actividad
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setResPerAgeRangePlot(!resPerAgeRangePlot);
                        setResPerActivityPlot(false);
                        setUseFrequencyDatePlot(false);
                        setConditionPlot(false);
                        setUseFrequencyPlot(false);
                        setLastYearTop3(false);
                        fetchReservations();
                    }}>
                    Frecuencia de Estado de Reservas por Rangos de Edad
                </Button>
                {/* Agrega más botones aquí */}
            </Box>

            {/* Contenido principal */}
            <Box
                sx={{
                    flex: 1,
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    gap: 4,
                    padding: 4,
                    overflowY: "auto", // Permite el desplazamiento si hay muchas gráficas
                }}
            >
                {/* Mostrar la gráfica solo si useFrequencyPlot es true */}
                {useFrequencyPlot && (
                    <Box sx={{ width: "80%", height: "400px" }}>
                        <BarsChart labels={resourceNames} data={resourceUseFrequency} label='Frecuencia de Uso' />
                    </Box>
                )}
                {conditionPlot && (
                    <Box sx={{ width: "80%", height: "400px" }}>
                        <PiesChart labels={resourceConditions} data={frequency} label='Estado del recurso' />
                    </Box>
                )}
                {useFrequencyDatePlot && (
                    <Box sx={{ width: "80%", height: "400px" }}>
                        <LinesChart resourceNames={resourceDateUseFrequency.resourceNames} days={days.map((date) => new Date(date).toLocaleDateString("es-ES"))} frequencies={resourceDateUseFrequency.resourceFrequencies} />
                    </Box>
                )}
                {lastYearTop3 && (
                    <Box sx={{ p: 4 }}>
                        <Typography variant="h4" sx={{ mb: 3, textAlign: "center" }}>
                            📊 Top 3:
                        </Typography>

                        {/* Recursos Más Utilizados */}
                        <Typography variant="h5" sx={{ mb: 2 }}>🔥 Recursos Más Utilizados</Typography>
                        <Grid2 container spacing={3} justifyContent="center">
                            {moreUsed.map((resource) => (
                                <Grid2 container spacing={4} key={resource.name}>
                                    <Box sx={{ p: 2, bgcolor: "#e3f2fd", borderRadius: 2, textAlign: "center" }}>
                                        <Typography variant="h6">{resource.name}</Typography>
                                        <Typography>Frecuencia: {resource.totalFreq}</Typography>
                                    </Box>
                                </Grid2>
                            ))}
                        </Grid2>

                        {/* Recursos Menos Utilizados */}
                        <Typography variant="h5" sx={{ mt: 4, mb: 2 }}>❄️ Recursos Menos Utilizados</Typography>
                        <Grid2 container spacing={3} justifyContent="center">
                            {lessUsed.map((resource) => (
                                <Grid2 container spacing={4} key={resource.name}>
                                    <Box sx={{ p: 2, bgcolor: "#ffebee", borderRadius: 2, textAlign: "center" }}>
                                        <Typography variant="h6">{resource.name}</Typography>
                                        <Typography>Frecuencia: {resource.totalFreq}</Typography>
                                    </Box>
                                </Grid2>
                            ))}
                        </Grid2>
                    </Box>
                )}
                {resPerActivityPlot && (
                    <Box sx={{ width: "80%", margin: "auto", padding: 4 }}>
                        <BarsChart labels={activityDates} data={reservFrequencies} label="Reservaciones por actividad y fecha" />
                    </Box>
                )}
                {resPerAgeRangePlot && (
                    <Box sx={{ width: "80%", margin: "auto", padding: 4, display: "flex", flexDirection: "column", gap: 4 }}>
                        {states.map((state) => (
                            <Box key={state} sx={{ width: "100%", padding: 2, backgroundColor: "#f8f9fa", borderRadius: 2 }}>
                                <Typography variant="h6" align="center" gutterBottom>
                                    Reservas {state.charAt(0).toUpperCase() + state.slice(1)}
                                </Typography>
                                <BarsChart
                                    labels={Array.from(groupedReservations[state].keys())}
                                    data={Array.from(groupedReservations[state].values())}
                                    label={`Cantidad de reservas ${state}`}
                                />
                            </Box>
                        ))}
                    </Box>
                )}

                {/*  Separador entre los graficos y el final  */}
                <Box sx={{ height: 16 }} />

            </Box>
        </Box>
    );
};

export default StatisticsPage;
