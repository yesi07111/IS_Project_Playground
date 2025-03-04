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

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Filler
);

const LinesChart: React.FC<LinesChartProps> = ({ resourceNames, days, frequencies }) => {
    return (
        <div>
            {resourceNames.map((name, index) => {
                // Construir los datos para el gráfico actual
                const resourceDays = days[index];
                const data = {
                    labels: resourceDays,
                    datasets: [
                        {
                            label: `Frecuencia de uso - ${name}`,
                            data: frequencies[index],
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
                    <div key={index} style={{ marginBottom: "2rem", height: "300px", width: "100%" }}>
                        <Line data={data} options={options} />
                    </div>
                );
            })}
        </div>
    );
};

export default LinesChart;