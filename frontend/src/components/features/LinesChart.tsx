import { Line } from "react-chartjs-2";
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
import { LinesChartProps } from "../../interfaces/PlotProps";
import { useState } from "react";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler
)

const LinesChart: React.FC<LinesChartProps> = ({ resourceNames, days, frequencies }) => {
    const [currentPage, setCurrentPage] = useState(0); // Página actual
    const itemsPerPage = 2; // Número de gráficos por página

    // Calcular el índice de inicio y fin para la página actual
    const startIndex = currentPage * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const paginatedResources = resourceNames.slice(startIndex, endIndex);
    const paginatedFrequencies = frequencies.slice(startIndex, endIndex);

    // Manejar cambio de página
    const handleNextPage = () => {
        if (endIndex < resourceNames.length) {
            setCurrentPage(currentPage + 1);
        }
    };

    const handlePrevPage = () => {
        if (currentPage > 0) {
            setCurrentPage(currentPage - 1);
        }
    };

    return (
        <div>
            {paginatedResources.map((name, index) => {
                // Construir los datos para el gráfico actual
                const data = {
                    labels: days,
                    datasets: [
                        {
                            label: `Frecuencia de uso - ${name}`,
                            data: paginatedFrequencies[index],
                            tension: 0.5,
                            fill: true,
                            borderColor: `rgba(${Math.floor(Math.random() * 256)}, ${Math.floor(
                                Math.random() * 256
                            )}, ${Math.floor(Math.random() * 256)}, 1)`,
                            backgroundColor: `rgba(${Math.floor(Math.random() * 256)}, ${Math.floor(
                                Math.random() * 256
                            )}, ${Math.floor(Math.random() * 256)}, 0.5)`,
                            pointRadius: 5,
                            pointBorderColor: "rgba(0,0,0,0.8)",
                            pointBackgroundColor: "rgba(255,255,255,1)",
                        },
                    ],
                };

                // Opciones del gráfico
                const options = {
                    responsive: true,
                    maintainAspectRatio: false,
                };

                return (
                    <div key={startIndex + index} style={{ marginBottom: "2rem", height: "300px", width: "100%" }}>
                        <Line data={data} options={options} />
                    </div>
                );
            })}

            {/* Controles de paginación */}
            <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginTop: "1rem" }}>
                <button onClick={handlePrevPage} disabled={currentPage === 0}>
                    Anterior
                </button>
                <button onClick={handleNextPage} disabled={endIndex >= resourceNames.length}>
                    Siguiente
                </button>
            </div>
        </div>
    );
};

export default LinesChart;