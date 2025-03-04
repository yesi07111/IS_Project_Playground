export interface PlotProps {
    labels: string[];  // Nombres de los recursos
    data: number[];    // Frecuencia de uso de los recursos
    label: string;
}

export interface LinesChartProps {
    resourceNames: string[]; // Nombres de los recursos
    days: string[][];          // Lista de días (labels)
    frequencies: number[][]; // Frecuencias de uso por recurso
}