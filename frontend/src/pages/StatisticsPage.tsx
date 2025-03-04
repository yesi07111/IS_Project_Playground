import React, { useState } from "react";
import { usePDF } from 'react-to-pdf';
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
import { ListReservationResponse, ListReservationStatsResponse } from "../interfaces/Reservation";
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
    const { toPDF, targetRef } = usePDF({ filename: 'Estádisticas.pdf' });

    //plots
    const [useFrequencyPlot, setUseFrequencyPlot] = useState(false);
    const [conditionPlot, setConditionPlot] = useState(false);
    const [useFrequencyDatePlot, setUseFrequencyDatePlot] = useState(false);
    const [resPerActivityPlot, setResPerActivityPlot] = useState(false);
    const [resPerAgeRangePlot, setResPerAgeRangePlot] = useState(false);
    const [lastYearTop3, setLastYearTop3] = useState(false);

    //plots de recursos
    const [names, setNames] = useState<string[]>([]);
    const [frequencies, setFrequencies] = useState<number[]>([]);
    const [condition, setCondition] = useState<string[]>([]);
    const [frequenciesC, setFrequenciesC] = useState<number[]>([]);
    const [namesD, setNamesD] = useState<string[]>([]);
    const [dates, setDates] = useState<string[][]>([]);
    const [frequenciesD, setFrequenciesD] = useState<number[][]>([]);
    const [namesT1, setNamesT1] = useState<string[]>([]);
    const [frequenciesT1, setFrequenciesT1] = useState<number[]>([]);
    const [namesT2, setNamesT2] = useState<string[]>([]);
    const [frequenciesT2, setFrequenciesT2] = useState<number[]>([]);

    //plots de reservaciones
    const [activityDateInfo, setActivityDateInfo] = useState<string[]>([]);
    const [reservationCount, setReservationCount] = useState<number[]>([]);
    const [state, setState] = useState<string[]>([]);
    const [ageGroup, setAgeGroup] = useState<string[][]>([]);
    const [count, setCount] = useState<number[][]>([]);

    const fetchAllNamesFreq = async () => {
        try {
            const response: ListResourceResponse = await resourceService.getAllResources([{ type: 'NameFreq' }]);
            const typedResult = response.result as { name: string; frequency: number }[];

            const a1 = typedResult.map(item => item.name);
            const a2 = typedResult.map(item => item.frequency);

            setNames(a1);
            setFrequencies(a2);
        }
        catch (error) {
            console.error('Error buscando los nombres y frecuencias de uso:', error);
        }
    }

    const fetchAllConditionsFreq = async () => {
        try {
            const response: ListResourceResponse = await resourceService.getAllResources([{ type: 'ConditionFreq' }]);
            const typedResult = response.result as { condition: string; frequency: number }[];

            const a1 = typedResult.map(item => item.condition);
            const a2 = typedResult.map(item => item.frequency);

            setCondition(a1);
            setFrequenciesC(a2);
        }
        catch (error) {
            console.error('Error buscando los nombres y frecuencias de uso:', error);
        }
    }

    const fetchAllResourceDates = async (useCase: string) => {
        try {
            const response: ListResourceDateResponse = await resourceService.getAllResourceDates(useCase);

            if (useCase === 'NameDayFreq') {
                const typedResult = response.result as { name: string, frequencies: { date: string, useFrequency: number }[] }[];

                const a1 = typedResult.map(item => item.name);
                const a2 = typedResult.map(item => item.frequencies.map(item => item.date));
                const a3 = typedResult.map(item => item.frequencies.map(item => item.useFrequency));

                setNamesD(a1);
                setDates(a2);
                setFrequenciesD(a3);
            }
            else if (useCase == 'NameFreqMost' || useCase == 'NameFreqLess') {
                const typedResult = response.result as { name: string, totalFrequency: number }[];

                const a1 = typedResult.map(item => item.name);
                const a2 = typedResult.map(item => item.totalFrequency);

                if (useCase == 'NameFreqMost') {
                    setNamesT1(a1);
                    setFrequenciesT1(a2);
                }
                else if (useCase == 'NameFreqLess') {
                    setNamesT2(a1);
                    setFrequenciesT2(a2);
                }

            }
        }
        catch (error) {
            console.error('Error fetching resources:', error);
        }
    }

    //reservaciones
    const fetchReservationStats = async (useCase: string) => {
        try {
            const response: ListReservationStatsResponse = await reservationService.getAllReservationStats(useCase);

            if (useCase === 'ActDateResFreq') {

                const typedResult = response.result as { activityDateInfo: string, reservationCount: number }[];
                
                const a1 = typedResult.map(item => item.activityDateInfo);
                const a2 = typedResult.map(item => item.reservationCount);

                setActivityDateInfo(a1);
                setReservationCount(a2);

            }
            else if (useCase === 'ActDateResFreqAgeRangeWithState') {

                const typedResult = response.result as { state: string, ageRanges: { ageGroup: string, count: number }[] }[];

                const a1 = typedResult.map(item => item.state);
                const a2 = typedResult.map(item => item.ageRanges.map(item => item.ageGroup));
                const a3 = typedResult.map(item => item.ageRanges.map(item => item.count));

                setState(a1);
                setAgeGroup(a2);
                setCount(a3);

            }
        } catch (error) {
            console.error('Error obteniendo las estadisticas:', error);
        }
    };

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
                        fetchAllNamesFreq();
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
                        fetchAllConditionsFreq();
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
                        fetchAllResourceDates('NameDayFreq');
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
                        fetchAllResourceDates("NameFreqMost");
                        fetchAllResourceDates("NameFreqLess");
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
                        fetchReservationStats('ActDateResFreq');
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
                        fetchReservationStats('ActDateResFreqAgeRangeWithState');
                    }}>
                    Frecuencia de Estado de Reservas por Rangos de Edad
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
                {/* Agrega más botones aquí */}
            </Box>

            {/* Contenido principal */}
            <Box
                ref={targetRef}
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
                        <BarsChart labels={names} data={frequencies} label='Frecuencia de Uso' />
                    </Box>
                )}
                {conditionPlot && (
                    <Box sx={{ width: "80%", height: "400px" }}>
                        <PiesChart labels={condition} data={frequenciesC} label='Estado del recurso' />
                    </Box>
                )}
                {useFrequencyDatePlot && (
                    <Box sx={{ width: "80%", height: "400px" }}>
                        <LinesChart resourceNames={namesD} days={dates} frequencies={frequenciesD} />
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
                            {namesT1.map((name, index) => (
                                <Grid2 container spacing={4} key={name}>
                                    <Box sx={{ p: 2, bgcolor: "#e3f2fd", borderRadius: 2, textAlign: "center" }}>
                                        <Typography variant="h6">{name}</Typography>
                                        <Typography>Frecuencia: {frequenciesT1[index]}</Typography>
                                    </Box>
                                </Grid2>
                            ))}
                        </Grid2>

                        {/* Recursos Menos Utilizados */}
                        <Typography variant="h5" sx={{ mt: 4, mb: 2 }}>❄️ Recursos Menos Utilizados</Typography>
                        <Grid2 container spacing={3} justifyContent="center">
                            {namesT2.map((name, index) => (
                                <Grid2 container spacing={4} key={name}>
                                    <Box sx={{ p: 2, bgcolor: "#ffebee", borderRadius: 2, textAlign: "center" }}>
                                        <Typography variant="h6">{name}</Typography>
                                        <Typography>Frecuencia: {frequenciesT2[index]}</Typography>
                                    </Box>
                                </Grid2>
                            ))}
                        </Grid2>
                    </Box>
                )}
                {resPerActivityPlot && (
                    <Box sx={{ width: "80%", margin: "auto", padding: 4 }}>
                        <BarsChart labels={activityDateInfo} data={reservationCount} label="Reservaciones por actividad y fecha" />
                    </Box>
                )}
                {resPerAgeRangePlot && (
                    <Box sx={{ width: "80%", margin: "auto", padding: 4, display: "flex", flexDirection: "column", gap: 4 }}>
                        {state.map((state, index) => (
                            <Box key={state} sx={{ width: "100%", padding: 2, backgroundColor: "#f8f9fa", borderRadius: 2 }}>
                                <Typography variant="h6" align="center" gutterBottom>
                                    Reservas {state.charAt(0).toUpperCase() + state.slice(1)}
                                </Typography>
                                <BarsChart
                                    labels={ageGroup[index]}
                                    data={count[index]}
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
