import React, { useState } from "react";
import { Box, Button } from "@mui/material";
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
    const [useFrequencyPlot, setUseFrequencyPlot] = useState(false);
    const [conditionPlot, setConditionPlot] = useState(false);
    const [useFrequencyDatePlot, setUseFrequencyDatePlot] = useState(false);

    const [resources, setResources] = useState<Resource[]>([]);
    const [resourceDates, setResourceDates] = useState<ResourceDate[]>([]);

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
        const groupedResources: { [id: string]: { name: string} } = {};

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

    return (
        <Box
            sx={{
                minHeight: "100vh",
                minWidth: "100vw",
                display: "flex",
                backgroundColor: "#f8f9fa",
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
                }}
            >
                <Button
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => {
                        setUseFrequencyPlot(!useFrequencyPlot);
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
                        fetchAllResourceDates();
                    }}>
                    Frecuencia de uso de los recursos por fecha
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
                        <LinesChart resourceNames={resourceDateUseFrequency.resourceNames} days={days.map((date) => date.toString())} frequencies={resourceDateUseFrequency.resourceFrequencies}/>
                    </Box>
                )}

            </Box>
        </Box>
    );
};

export default StatisticsPage;
